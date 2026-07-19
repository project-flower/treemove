using System;
using System.IO;
using System.Runtime.InteropServices;
using NativeMethods;

namespace treemove
{
    public class FileOperation : ComSupporter
    {
        public static bool Operate(FO func, string[] fileNames, string pTo, FOF flags, IntPtr handle)
        {
            if (fileNames == null || fileNames.Length == 0) return true;

            Type foType = Type.GetTypeFromCLSID(CLSID.FileOperation);
            object coclass = null;
            IntPtr iUnknown = IntPtr.Zero;
            IntPtr iFileOpPtr = IntPtr.Zero;
            IFileOperation fileOp = null;
            IntPtr destPtr = IntPtr.Zero;
            IShellItem destShellItem = null;

            try
            {
                coclass = Activator.CreateInstance(foType);

                // Get IUnknown for coclass and QI for IFileOperation
                iUnknown = Marshal.GetIUnknownForObject(coclass);
                Guid iidFileOp = IID.IFileOperation;
                int qiHr = Marshal.QueryInterface(iUnknown, ref iidFileOp, out iFileOpPtr);
                ThrowIfFailed(qiHr);

                // Get managed wrapper for IFileOperation
                fileOp = GetObjectForIUnknown<IFileOperation>(iFileOpPtr);

                // Set operation flags
                int hr = fileOp.SetOperationFlags((uint)flags);
                ThrowIfFailed(hr);

                // Create destination IShellItem
                Guid iidShell = IID.IShellItem;
                hr = Shell32.SHCreateItemFromParsingName(pTo, IntPtr.Zero, ref iidShell, out destPtr);
                ThrowIfFailed(hr);

                destShellItem = GetObjectForIUnknown<IShellItem>(destPtr);
                // release raw pointer, RCW holds reference
                destPtr.Release();

                foreach (string sourcePath in fileNames)
                {
                    if (string.IsNullOrEmpty(sourcePath)) continue;

                    IntPtr srcPtr = IntPtr.Zero;
                    IShellItem srcShellItem = null;

                    try
                    {
                        Guid iidSrc = IID.IShellItem;
                        hr = Shell32.SHCreateItemFromParsingName(sourcePath, IntPtr.Zero, ref iidSrc, out srcPtr);
                        ThrowIfFailed(hr);
                        srcShellItem = GetObjectForIUnknown<IShellItem>(srcPtr);
                        srcPtr.Release();

                        switch (func)
                        {
                            case FO.COPY:
                                hr = fileOp.CopyItem(srcShellItem, destShellItem, null, IntPtr.Zero);
                                break;
                            case FO.MOVE:
                                hr = fileOp.MoveItem(srcShellItem, destShellItem, null, IntPtr.Zero);
                                break;
                            default:
                                throw new NotSupportedException("指定された操作はサポートされていません。");
                        }

                        ThrowIfFailed(hr);
                    }
                    finally
                    {
                        srcShellItem.Release();
                    }
                }

                hr = fileOp.PerformOperations();
                ThrowIfFailed(hr);
                hr = fileOp.GetAnyOperationsAborted(out bool aborted);
                ThrowIfFailed(hr);

                if (!aborted) return true;

                throw new IOException("ファイル操作はユーザーによって中止されました。");
            }
            finally
            {
                destShellItem.Release();
                fileOp.Release();
                iFileOpPtr.Release();
                iUnknown.Release();
                coclass.Release();
            }
        }

        public static bool Copy(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return Operate(FO.COPY, fileNames, destDirectory, (FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }

        public static bool Move(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return Operate(FO.MOVE, fileNames, destDirectory, (FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }
    }
}
