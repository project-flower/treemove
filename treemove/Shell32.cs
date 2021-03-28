using System.Runtime.InteropServices;

namespace Win32Api
{
    public static class Shell32
    {
        /// <summary>
        /// shell32.dllのファイル名
        /// </summary>
        public const string AssemblyName = "shell32.dll";

        [DllImport(AssemblyName, CharSet = CharSet.Unicode)]
        public extern static int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
    }
}
