using System;
using Win32Api;

namespace treemove
{
    static class FileOperation
    {
        static bool operate(FO func, string[] fileNames, string pTo, FOF flags, IntPtr handle)
        {
            SHFILEOPSTRUCT shell = new SHFILEOPSTRUCT();
            shell.hwnd = handle;
            shell.wFunc = func;
            shell.pFrom = string.Empty;

            foreach (string fileName in fileNames)
            {
                shell.pFrom += fileName + '\0';
            }

            shell.pFrom += '\0';
            shell.pTo = pTo;
            shell.fFlags = flags;
            shell.fAnyOperationsAborted = false;
            shell.hNameMappings = IntPtr.Zero;
            shell.lpszProgressTitle = string.Empty;
            return (Shell32.SHFileOperation(ref shell) == 0);
        }

        public static bool Copy(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return operate(FO.COPY, fileNames, destDirectory, (FOF.SILENT | FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }

        public static bool Move(string[] fileNames, string destDirectory, IntPtr handle)
        {
            return operate(FO.MOVE, fileNames, destDirectory, (FOF.SILENT | FOF.ALLOWUNDO | FOF.NOCONFIRMMKDIR), handle);
        }
    }
}
