using System.Collections.Generic;
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

        public static bool GetSHFileOperationErrorMessage(int result, out string message)
        {
            return SHFileOperationResults.TryGetValue(result, out message);
        }

        /// <summary>
        /// SHFileOperation の戻り値の意味を表します。
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/ja-jp/windows/win32/api/shellapi/nf-shellapi-shfileoperationa"/>
        private static readonly Dictionary<int, string> SHFileOperationResults = new Dictionary<int, string>()
        {
            { 0x71, "ソースファイルと宛先ファイルは同じファイルです。" },
            { 0x72, "ソースバッファに複数のファイルパスが指定されましたが、宛先ファイルパスは1つだけです。" },
            { 0x73, "名前の変更操作が指定されましたが、宛先パスが別のディレクトリです。代わりに移動操作を使用してください。" },
            { 0x74, "ソースはルートディレクトリであり、移動したり名前を変更したりすることはできません。" },
            { 0x75, "操作はユーザーによってキャンセルされました。適切なフラグがSHFileOperationに指定されている場合は、サイレントにキャンセルされました。" },
            { 0x76, "宛先はソースのサブツリーです。" },
            { 0x78, "セキュリティ設定により、ソースへのアクセスが拒否されました。" },
            { 0x79, "ソースパスまたは宛先パスがMAX_PATHを超えたか、超える可能性があります。" },
            { 0x7A, "操作には複数の宛先パスが含まれ、移動操作の場合は失敗する可能性があります。" },
            { 0x7C, "ソースまたは宛先、あるいはその両方のパスが無効でした。" },
            { 0x7D, "ソースと宛先は同じ親フォルダーを持っています。" },
            { 0x7E, "宛先パスは既存のファイルです。" },
            { 0x80, "宛先パスは既存のフォルダーです。" },
            { 0x81, "ファイル名がMAX_PATHを超えています。" },
            { 0x82, "宛先は読み取り専用のCD-ROMであり、フォーマットされていない可能性があります。" },
            { 0x83, "宛先は読み取り専用DVDで、フォーマットされていない可能性があります。" },
            { 0x84, "宛先は書き込み可能なCD-ROMであり、フォーマットされていない可能性があります。" },
            { 0x85, " 	操作に関係するファイルが、宛先メディアまたはファイルシステムに対して大きすぎます。" },
            { 0x86, "ソースは読み取り専用のCD-ROMであり、フォーマットされていない可能性があります。" },
            { 0x87, "ソースは読み取り専用DVDであり、フォーマットされていない可能性があります。" },
            { 0x88, "ソースは書き込み可能なCD-ROMであり、フォーマットされていない可能性があります。" },
            { 0xB7, "操作中にMAX_PATHを超えました。" },
            { 0x402, "不明なエラーが発生しました。これは通常、送信元または宛先のパスが無効であることが原因です。" },
            { 0x10000, "宛先で不特定のエラーが発生しました。" },
            { 0x10074, "宛先はルートディレクトリであり、名前を変更することはできません。" }
        };
    }
}
