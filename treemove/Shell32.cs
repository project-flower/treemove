using System.Runtime.InteropServices;

namespace Win32Api
{
    static class Shell32
    {
        /// <summary>
        /// shell32.dllのファイル名
        /// </summary>
        const string DLLNAME = "shell32.dll";

        [DllImport(DLLNAME, CharSet = CharSet.Unicode)]
        extern public static int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
    }
}
