using System;
using System.Runtime.InteropServices;

namespace Win32Api
{
    /// <summary>
    /// Shell File Operations
    /// </summary>
    public enum FO
    {
        MOVE = 0x0001,
        COPY = 0x0002,
        DELETE = 0x0003,
        RENAME = 0x0004
    }

    /// <summary>
    /// SHFILEOPSTRUCT.fFlags and IFileOperation::SetOperationFlags() flag values
    /// </summary>
    public enum FOF
    {
        MULTIDESTFILES = 0x0001,
        CONFIRMMOUSE = 0x0002,
        /// <summary>
        /// don't display progress UI (confirm prompts may be displayed still)
        /// </summary>
        SILENT = 0x0004,
        /// <summary>
        /// automatically rename the source files to avoid the collisions
        /// </summary>
        RENAMEONCOLLISION = 0x0008,
        /// <summary>
        /// don't display confirmation UI, assume "yes" for cases that can be bypassed, "no" for those that can not
        /// </summary>
        NOCONFIRMATION = 0x0010,
        /// <summary>
        /// Fill in SHFILEOPSTRUCT.hNameMappings
        /// <para>Must be freed using SHFreeNameMappings</para>
        /// </summary>
        WANTMAPPINGHANDLE = 0x0020,
        /// <summary>
        /// enable undo including Recycle behavior for IFileOperation::Delete()
        /// </summary>
        ALLOWUNDO = 0x0040,
        /// <summary>
        /// only operate on the files (non folders), both files and folders are assumed without this
        /// </summary>
        FILESONLY = 0x0080,
        /// <summary>
        /// means don't show names of files
        /// </summary>
        SIMPLEPROGRESS = 0x0100,
        /// <summary>
        /// don't dispplay confirmatino UI before making any needed directories, assume "Yes" in these cases
        /// </summary>
        NOCONFIRMMKDIR = 0x0200,
        /// <summary>
        /// don't put up error UI, other UI may be displayed, progress, confirmations
        /// </summary>
        NOERRORUI = 0x0400,
        /// <summary>
        /// dont copy file security attributes (ACLs)
        /// </summary>
        NOCOPYSECURITYATTRIBS = 0x0800,
        /// <summary>
        /// don't recurse into directories for operations that would recurse
        /// </summary>
        NORECURSION = 0x1000,
        /// <summary>
        /// don't operate on connected elements ("xxx_files" folders that go with .htm files)
        /// </summary>
        NO_CONNECTED_ELEMENTS = 0x2000,
        /// <summary>
        /// during delete operation, warn if object is being permanently destroyed instead of recycling (partially overrides FOF_NOCONFIRMATION)
        /// </summary>
        WANTNUKEWARNING = 0x4000,
        /// <summary>
        /// deprecated; the operations engine always does the right thing on FolderLink objects (symlinks, reparse points, folder shortcuts)
        /// </summary>
        NORECURSEREPARSE = 0x8000,
        NO_UI = (SILENT | NOCONFIRMATION | NOERRORUI | NOCONFIRMMKDIR)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public FO wFunc;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pFrom;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pTo;
        public FOF fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszProgressTitle;
    }
}
