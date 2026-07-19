using System;
using System.Runtime.InteropServices;

namespace NativeMethods
{
    #region Public Classes

    public static partial class CLSID
    {
        /// <summary>指定したコピー先にコピーする 1 つの項目を宣言します。</summary>
        public static readonly Guid FileOperation = new Guid("3ad05575-8857-4850-9277-11b85bdb8e09");
    }

    /// <summary>
    /// シェル項目のコピー、移動、名前変更、作成、削除を行うメソッドと、進行状況とエラー ダイアログを提供するメソッドを公開します。
    /// このインターフェイスは 、SHFileOperation 関数を置き換えます。
    /// </summary>
    [ComImport]
    [Guid("947aab5f-0a5c-4c13-b4d6-4bf7836fc9f8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileOperation
    {
        /// <summary>
        /// ハンドラーがすべての操作の状態とエラー情報を提供できるようにします。
        /// </summary>
        /// <param name="pfops">進行状況とエラー通知に使用する IFileOperationProgressSink オブジェクトへのポインター。</param>
        /// <param name="pdwCookie">
        /// このメソッドが返されると、このパラメーターは、この接続を一意に識別する返されたトークンを指します。
        /// 呼び出元のアプリケーションは、後でこのトークンを使用して、
        /// 接続を IFileOperation::Unadvise に渡すことで接続を削除します。
        /// Advise の呼び出しが失敗した場合、この値は意味がありません。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int Advise(IntPtr pfops, out uint pdwCookie);


        /// <summary>
        /// 以前に IFileOperation::Advise によって確立されたアドバイザリ接続を終了します。
        /// </summary>
        /// <param name="dwCookie">
        /// 削除する接続を識別する接続トークン。
        /// この値は、接続が確立されたときに、
        /// 最初に Advise によって取得されました。
        /// </param>
        /// <returns>
        /// ここに記載されている値以外の値は、エラーを示します。
        /// <para>S_OK : 接続が正常に終了しました。</para>
        /// <para>CONNECT_E_NOCONNECTION : dwCookie の値は、有効な接続を表していません。</para>
        /// </returns>
        [PreserveSig]
        int Unadvise(uint dwCookie);

        /// <summary>
        /// 現在の操作のパラメーターを設定します。
        /// </summary>
        /// <param name="dwOperationFlags">
        /// ファイル操作を制御するフラグ。
        /// このメンバーは、次のフラグの組み合わせにすることができます。
        /// FOF フラグは Shellapi.h で定義され、FOFX フラグは Shobjidl.h で定義されます。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int SetOperationFlags(uint dwOperationFlags);

        /// <summary>
        /// 実装されていません。
        /// </summary>
        /// <param name="pszMessage">ウィンドウ タイトルへのポインター。 これは null で終わる Unicode 文字列です。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int SetProgressMessage([MarshalAs(UnmanagedType.LPWStr)] string pszMessage);

        /// <summary>
        /// 操作の進行状況を表示するために使用するダイアログ ボックスを指定します。
        /// </summary>
        /// <param name="popd">ダイアログ ボックスを表す IOperationsProgressDialog オブジェクトへのポインター。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int SetProgressDialog(IntPtr popd);

        /// <summary>
        /// アイテムまたはアイテムに設定するプロパティと値のセットを宣言します。
        /// </summary>
        /// <param name="pproparray">設定するプロパティとその新しい値を指定する IPropertyChange オブジェクトのコレクションにアクセスする IPropertyChangeArray へのポインター。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int SetProperties(IntPtr pproparray);

        /// <summary>
        /// 進行状況ウィンドウとダイアログ ウィンドウの親ウィンドウまたは所有者ウィンドウを設定します。
        /// </summary>
        /// <param name="hwndOwner">
        /// 操作の所有者ウィンドウへのハンドル。
        /// このウィンドウには、エラー メッセージが表示されます。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int SetOwnerWindow(IntPtr hwndOwner);

        /// <summary>
        /// プロパティ値を設定する 1 つの項目を宣言します。
        /// </summary>
        /// <param name="psiItem">新しいプロパティ値を受け取る項目へのポインター。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int ApplyPropertiesToItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiItem);

        /// <summary>
        /// 共通のプロパティ値のセットを適用する項目のセットを宣言します。
        /// </summary>
        /// <param name="punkItems">
        /// 項目のグループを表す IShellItemArray、IDataObject、または IEnumShellItems オブジェクトの IUnknown へのポインター。
        /// IPersistIDList オブジェクトをポイントして 1 つの項目を表すこともできます。IFileOperation::ApplyPropertiesToItem と同じ関数を効果的に実行できます。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int ApplyPropertiesToItems(IntPtr punkItems);

        /// <summary>
        /// 新しい表示名を指定する 1 つの項目を宣言します。
        /// </summary>
        /// <param name="psiItem">ソース アイテムを指定する IShellItem へのポインター。</param>
        /// <param name="pszNewName">
        /// 項目の新しい 表示名 へのポインター。
        /// これは null で終わる Unicode 文字列です。
        /// </param>
        /// <param name="pfopsItem">
        /// この特定の移動操作の進行状況とエラー通知に使用される IFileOperationProgressSink オブジェクトへのポインター。
        /// 操作全体に対して IFileOperation::Advise を呼び出すと、移動操作の進行状況とエラー通知がそこに含まれるため、このパラメーターを NULL に設定します。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int RenameItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, IntPtr pfopsItem);

        /// <summary>
        /// 新しい表示名を指定する項目のセットを宣言します。 すべての項目に同じ名前が付けられます。
        /// </summary>
        /// <param name="pUnkItems">
        /// 名前を変更する項目のグループを表す IShellItemArray、IDataObject、または IEnumShellItems オブジェクトの IUnknown へのポインター。
        /// また、IPersistIDList オブジェクトをポイントして 1 つの項目を表し、IFileOperation::RenameItem と同じ関数を効果的に実行することもできます。
        /// </param>
        /// <param name="pszNewName">項目の新しい表示名へのポインター。 これは null で終わる Unicode 文字列です。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int RenameItems(IntPtr pUnkItems, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);

        /// <summary>
        /// 指定した宛先に移動する 1 つの項目を宣言します。
        /// </summary>
        /// <param name="psiItem">ソース項目を指定する IShellItem へのポインター。</param>
        /// <param name="psiDestinationFolder">移動されたアイテムを格納する移動先フォルダーを指定する IShellItem へのポインター。</param>
        /// <param name="pszNewName">
        /// 新しい場所にある項目の新しい名前へのポインター。
        /// これは null で終わる Unicode 文字列であり、 NULL にすることができます。
        /// NULL の場合、コピー先アイテムの名前はソースと同じです。
        /// </param>
        /// <param name="pfopsItem">
        /// この特定の移動操作の進行状況とエラー通知に使用される IFileOperationProgressSink オブジェクトへのポインター。
        /// 操作全体に対して IFileOperation::Advise を呼び出すと、移動操作の進行状況とエラー通知がそこに含まれるため、このパラメーターを NULL に設定します。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int MoveItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiItem, [MarshalAs(UnmanagedType.Interface)] IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, IntPtr pfopsItem);

        /// <summary>
        /// 指定した宛先に移動する項目のセットを宣言します。
        /// </summary>
        /// <param name="punkItems">
        /// 移動する項目のグループを表す IShellItemArray、IDataObject、または IEnumShellItems オブジェクトの IUnknown へのポインター。
        /// また、IPersistIDList オブジェクトをポイントして 1 つの項目を表し、IFileOperation::MoveItem と同じ関数を効果的に実行することもできます。
        /// </param>
        /// <param name="psiDestinationFolder">移動されたアイテムを格納する移動先フォルダーを指定する IShellItem へのポインター。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int MoveItems(IntPtr punkItems, [MarshalAs(UnmanagedType.Interface)] IShellItem psiDestinationFolder);

        /// <summary>
        /// 指定したコピー先にコピーする 1 つの項目を宣言します。
        /// </summary>
        /// <param name="psiItem">ソース アイテムを指定する IShellItem へのポインター。</param>
        /// <param name="psiDestinationFolder">アイテムのコピーを格納する宛先フォルダーを指定する IShellItem へのポインター。</param>
        /// <param name="pszCopyName">
        /// コピー後の項目の新しい名前へのポインター。
        /// これは null で終わる Unicode 文字列であり、 NULL にすることができます。
        /// NULL の場合、変換先アイテムの名前はソースと同じです。
        /// </param>
        /// <param name="pfopsItem">
        /// この特定の移動操作の進行状況とエラー通知に使用される IFileOperationProgressSink オブジェクトへのポインター。
        /// 操作全体に対して IFileOperation::Advise を呼び出すと、移動操作の進行状況とエラー通知がそこに含まれるため、このパラメーターを NULL に設定します。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int CopyItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiItem, [MarshalAs(UnmanagedType.Interface)] IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszCopyName, IntPtr pfopsItem);

        /// <summary>
        /// 指定したコピー先にコピーする項目のセットを宣言します。
        /// </summary>
        /// <param name="punkItems">
        /// コピーする項目のグループを表す IShellItemArray、IDataObject、または IEnumShellItems オブジェクトの IUnknown へのポインター。
        /// また、IPersistIDList オブジェクトをポイントして 1 つの項目を表し、IFileOperation::CopyItem と同じ関数を効果的に実行することもできます。
        /// </param>
        /// <param name="psiDestinationFolder">アイテムのコピーを格納する宛先フォルダーを指定する IShellItem へのポインター。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int CopyItems(IntPtr punkItems, [MarshalAs(UnmanagedType.Interface)] IShellItem psiDestinationFolder);

        /// <summary>
        /// 削除する 1 つの項目を宣言します。
        /// </summary>
        /// <param name="psiItem">削除する項目を指定する IShellItem へのポインター。</param>
        /// <param name="pfopsItem">
        /// この特定の削除操作の進行状況とエラー通知に使用される IFileOperationProgressSink オブジェクトへのポインター。
        /// 操作全体に対して IFileOperation::Advise を呼び出すと、削除操作の進行状況とエラー通知がそこに含まれるため、このパラメーターを NULL に設定します。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int DeleteItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiItem, IntPtr pfopsItem);

        /// <summary>
        /// 削除する項目のセットを宣言します。
        /// </summary>
        /// <param name="punkItems">
        /// 削除する項目のグループを表す IShellItemArray、IDataObject、または IEnumShellItems オブジェクトの IUnknown へのポインター。
        /// IPersistIDList オブジェクトをポイントして 1 つの項目を表すこともできます。IFileOperation::D eleteItem と同じ関数を効果的に実行できます。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int DeleteItems(IntPtr punkItems);

        /// <summary>
        /// 指定した場所に作成される新しい項目を宣言します。
        /// </summary>
        /// <param name="psiDestinationFolder">新しい項目を含む移動先フォルダーを指定する IShellItem へのポインター。</param>
        /// <param name="dwFileAttributes">ファイルまたはフォルダーのファイル システム属性を指定するビットごとの値。 使用可能な値については、「 GetFileAttributes 」を参照してください。</param>
        /// <param name="pszName">新しい項目のファイル名へのポインター (例: Newfile.txt)。 これは null で終わる Unicode 文字列です。</param>
        /// <param name="pszTemplateName">
        /// 新しい項目の基になっているテンプレート ファイルの名前 ( Excel9.xlsなど) へのポインターは、次のいずれかの場所に格納されます。
        /// <para>CSIDL_COMMON_TEMPLATES。 このフォルダーの既定のパスは %ALLUSERSPROFILE%\Templates です。</para>
        /// <para>CSIDL_TEMPLATES。 このフォルダーの既定のパスは %USERPROFILE%\Templates です。</para>
        /// <para>%SystemRoot%\shellnew</para>
        /// これは、アプリケーションが新しいファイルに含める最小限のコンテンツを含む、新しいファイルと同じ種類の既存のファイルを指定するために使用される、null で終わる Unicode 文字列です。
        /// このパラメーターは通常、新しい空のファイルを指定する 場合は NULL です 。
        /// </param>
        /// <param name="pfopsItem">
        /// 状態と失敗の通知に使用する IFileOperationProgressSink オブジェクトへのポインター。
        /// 操作全体に対して IFileOperation::Advise を呼び出すと、作成操作の進行状況とエラー通知がそこに含まれるため、このパラメーターを NULL に設定します。
        /// </param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int NewItem([MarshalAs(UnmanagedType.Interface)] IShellItem psiDestinationFolder, uint dwFileAttributes, [MarshalAs(UnmanagedType.LPWStr)] string pszName, [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName, IntPtr pfopsItem);

        /// <summary>
        /// 選択したすべての操作を実行します。
        /// </summary>
        /// <returns>
        /// 成功した場合はS_OK、それ以外の場合はエラー値を返します。
        /// 操作がユーザーによって取り消された場合でも、このメソッドは成功コードを返すことができることに注意してください。
        /// GetAnyOperationsAborted メソッドを使用して、これが該当するかどうかを判断します。
        /// </returns>
        [PreserveSig]
        int PerformOperations();

        /// <summary>
        /// IFileOperation::PerformOperations の呼び出しによって開始されたファイル操作が完了前に停止されたかどうかを示す値を取得します。
        /// 操作は、ユーザーアクションによって停止することも、システムによってサイレントに停止することもできます。
        /// </summary>
        /// <param name="pfAnyOperationsAborted">このメソッドが戻るときに、ファイル操作が完了する前に中止された場合は TRUE を ポイントします。それ以外の場合は FALSE。</param>
        /// <returns>
        /// このメソッドは、成功すると S_OK を返します。
        /// そうでない場合は、HRESULT エラー コードを返します。
        /// </returns>
        [PreserveSig]
        int GetAnyOperationsAborted([MarshalAs(UnmanagedType.Bool)] out bool pfAnyOperationsAborted);
    }

    public static partial class IID
    {
        public static readonly Guid IFileOperation = new Guid("947aab5f-0a5c-4c13-b4d6-4bf7836fc9f8");
        public static readonly Guid IShellItem = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");
    }

    /// <summary>
    /// シェル項目に関する情報を取得するメソッドを公開します。
    /// IShellItem と IShellItem2 は、新しいコード内の項目の優先表現です。
    /// </summary>
    [ComImport]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
    }

    public static partial class Shell32
    {
        /// <summary>
        /// 解析名からシェル項目オブジェクトを作成および初期化します。
        /// </summary>
        /// <param name="pszPath">表示名へのポインター。</param>
        /// <param name="pbc">
        /// Optional. パラメーターを入力および出力として解析関数に渡すために使用されるバインド コンテキストへのポインター。
        /// これらの渡されたパラメーターは、多くの場合、データ ソースに固有であり、データ ソースの所有者によって文書化されます。
        /// たとえば、ファイル システム データ ソースは、STR_FILE_SYS_BIND_DATA バインド コンテキスト パラメーターを使用して、解析される名前を (WIN32_FIND_DATA構造体として) 受け入れます。
        /// STR_PARSE_PREFER_FOLDER_BROWSING を渡して、可能な場合はファイル システム データ ソースを使用して URL が解析されることを示すことができます。
        /// CreateBindCtx を使用してバインド コンテキスト オブジェクトを構築し、IBindCtx::RegisterObjectParam を使用して値を設定します。
        /// これらの完全な一覧については、「 バインド コンテキスト文字列キー 」を参照してください。
        /// このパラメーターの使用例については、 パラメーターによる解析のサンプル を参照してください。
        /// 解析関数との間でデータが渡されない場合、または解析関数からデータを受け取っていない場合、この値は NULL にすることができます。
        /// </param>
        /// <param name="riid">
        /// ppv を介して取得するインターフェイスの IID への参照。
        /// 通常は、IID_IShellItemまたはIID_IShellItem2。
        /// </param>
        /// <param name="ppv">
        /// このメソッドが正常に返されると、 riid で要求されたインターフェイス ポインターが含まれます。
        /// これは通常、 IShellItem または IShellItem2 です。
        /// </param>
        /// <returns>
        /// この関数が成功すると、S_OKが返 されます。
        /// それ以外の場合は、 HRESULT エラー コードが返されます。
        /// </returns>
        [DllImport(AssemblyName, CharSet = CharSet.Unicode, PreserveSig = true)]
        public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, out IntPtr ppv);
    }

    #endregion
}
