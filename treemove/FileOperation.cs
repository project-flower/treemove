using System;
using System.IO;
using System.Text;
using Win32Api;

namespace treemove
{
    public static class FileOperation
    {
        public static bool Operate(FO func, string[] fileNames, string pTo, FOF flags, IntPtr handle)
        {
            SHFILEOPSTRUCT shell = new SHFILEOPSTRUCT
            {
                hwnd = handle,
                wFunc = func,
                pFrom = null
            };

            var builder = new StringBuilder();

            foreach (string fileName in fileNames)
            {
                builder.AppendFormat("{0}{1}", fileName, '\0');
            }

            builder.Append('\0');
            shell.pFrom = builder.ToString();
            shell.pTo = pTo;
            shell.fFlags = flags;
            shell.fAnyOperationsAborted = false;
            shell.hNameMappings = IntPtr.Zero;
            shell.lpszProgressTitle = string.Empty;
            int result = Shell32.SHFileOperation(ref shell);

            if (result == 0) return true;

            if (Shell32.GetSHFileOperationErrorMessage(result, out string message))
            {
                throw new IOException(message);
            }

            throw new IOException($"SHFileOperation エラーコード: {result:X2}");
        }

        public static bool Copy(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return Operate(FO.COPY, fileNames, destDirectory, (FOF.SILENT | FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }

        public static bool Move(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return Operate(FO.MOVE, fileNames, destDirectory, (FOF.SILENT | FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }
    }
}
