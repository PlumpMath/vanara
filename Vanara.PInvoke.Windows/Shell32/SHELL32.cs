using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	[SuppressUnmanagedCodeSecurity]
	public static partial class Shell32
	{
		/// <summary>Specify special retrieval options for known folders. These values supersede CSIDL values, which have parallel meanings.</summary>
		[Flags]
		public enum KNOWN_FOLDER_FLAG : uint
		{
			/// <summary>No flags.</summary>
			KF_FLAG_DEFAULT = 0x00000000,
			/// <summary>
			/// Build a simple IDList (PIDL) This value can be used when you want to retrieve the file system path but do not specify this value if you are
			/// retrieving the localized display name of the folder because it might not resolve correctly.
			/// </summary>
			KF_FLAG_SIMPLE_IDLIST = 0x00000100,
			/// <summary>Gets the folder's default path independent of the current location of its parent. KF_FLAG_DEFAULT_PATH must also be set.</summary>
			KF_FLAG_NOT_PARENT_RELATIVE = 0x00000200,
			/// <summary>
			/// Gets the default path for a known folder. If this flag is not set, the function retrieves the current—and possibly redirected—path of the folder.
			/// The execution of this flag includes a verification of the folder's existence unless KF_FLAG_DONT_VERIFY is set.
			/// </summary>
			KF_FLAG_DEFAULT_PATH = 0x00000400,
			/// <summary>
			/// Initializes the folder using its Desktop.ini settings. If the folder cannot be initialized, the function returns a failure code and no path is
			/// returned. This flag should always be combined with KF_FLAG_CREATE. If the folder is located on a network, the function might take a longer time
			/// to execute.
			/// </summary>
			KF_FLAG_INIT = 0x00000800,
			/// <summary>
			/// Gets the true system path for the folder, free of any aliased placeholders such as %USERPROFILE%, returned by SHGetKnownFolderIDList and
			/// IKnownFolder::GetIDList. This flag has no effect on paths returned by SHGetKnownFolderPath and IKnownFolder::GetPath. By default, known folder
			/// retrieval functions and methods return the aliased path if an alias exists.
			/// </summary>
			KF_FLAG_NO_ALIAS = 0x00001000,
			/// <summary>
			/// Stores the full path in the registry without using environment strings. If this flag is not set, portions of the path may be represented by
			/// environment strings such as %USERPROFILE%. This flag can only be used with SHSetKnownFolderPath and IKnownFolder::SetPath.
			/// </summary>
			KF_FLAG_DONT_UNEXPAND = 0x00002000,
			/// <summary>
			/// Do not verify the folder's existence before attempting to retrieve the path or IDList. If this flag is not set, an attempt is made to verify that
			/// the folder is truly present at the path. If that verification fails due to the folder being absent or inaccessible, the function returns a
			/// failure code and no path is returned. If the folder is located on a network, the function might take a longer time to execute. Setting this flag
			/// can reduce that lag time.
			/// </summary>
			KF_FLAG_DONT_VERIFY = 0x00004000,
			/// <summary>
			/// Forces the creation of the specified folder if that folder does not already exist. The security provisions predefined for that folder are
			/// applied. If the folder does not exist and cannot be created, the function returns a failure code and no path is returned. This value can be used
			/// only with the following functions and methods: SHGetKnownFolderPath, SHGetKnownFolderIDList, IKnownFolder::GetIDList, IKnownFolder::GetPath, and IKnownFolder::GetShellItem.
			/// </summary>
			KF_FLAG_CREATE = 0x00008000,
			/// <summary>
			/// Introduced in Windows 7: When running inside an app container, or when providing an app container token, this flag prevents redirection to app
			/// container folders. Instead, it retrieves the path that would be returned where it not running inside an app container.
			/// </summary>
			KF_FLAG_NO_APPCONTAINER_REDIRECTION = 0x00010000,
			/// <summary>Introduced in Windows 7. Return only aliased PIDLs. Do not use the file system path.</summary>
			KF_FLAG_ALIAS_ONLY = 0x80000000
		}

		public enum OFASI
		{
			OFASI_NONE = 0,
			OFASI_EDIT = 1,
			OFASI_OPENDESKTOP = 2
		}

		public enum SHARD
		{
			SHARD_APPIDINFO = 4,
			SHARD_APPIDINFOIDLIST = 5,
			SHARD_APPIDINFOLINK = 7,
			SHARD_LINK = 6,
			SHARD_PATHA = 2,
			SHARD_PATHW = 3,
			SHARD_PIDL = 1,
			SHARD_SHELLITEM = 8
		}

		[Flags]
		public enum ShellExecuteMaskFlags : uint
		{
			SEE_MASK_DEFAULT = 0x00000000,
			SEE_MASK_CLASSNAME = 0x00000001,
			SEE_MASK_CLASSKEY = 0x00000003,
			SEE_MASK_IDLIST = 0x00000004,
			SEE_MASK_INVOKEIDLIST = 0x0000000c, // Note SEE_MASK_INVOKEIDLIST(0xC) implies SEE_MASK_IDLIST(0x04)
			SEE_MASK_HOTKEY = 0x00000020,
			SEE_MASK_NOCLOSEPROCESS = 0x00000040,
			SEE_MASK_CONNECTNETDRV = 0x00000080,
			SEE_MASK_NOASYNC = 0x00000100,
			SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC,
			SEE_MASK_DOENVSUBST = 0x00000200,
			SEE_MASK_FLAG_NO_UI = 0x00000400,
			SEE_MASK_UNICODE = 0x00004000,
			SEE_MASK_NO_CONSOLE = 0x00008000,
			SEE_MASK_ASYNCOK = 0x00100000,
			SEE_MASK_HMONITOR = 0x00200000,
			SEE_MASK_NOZONECHECKS = 0x00800000,
			SEE_MASK_NOQUERYCLASSSTORE = 0x01000000,
			SEE_MASK_WAITFORINPUTIDLE = 0x02000000,
			SEE_MASK_FLAG_LOG_USAGE = 0x04000000
		}

		/// <summary>The flags that specify the file information to retrieve from <see cref="Shell32.SHGetFileInfo(string,System.IO.FileAttributes,ref SHFILEINFO,int,SHGFI)"/>.</summary>
		[Flags]
		public enum SHGFI
		{
			/// <summary>
			/// Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the
			/// hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
			/// </summary>
			SHGFI_ICON = 0x000000100,

			/// <summary>
			/// Retrieve the display name for the file, which is the name as it appears in Windows Explorer. The name is copied to the szDisplayName member of
			/// the structure specified in psfi. The returned display name uses the long file name, if there is one, rather than the 8.3 form of the file name.
			/// Note that the display name can be affected by settings such as whether extensions are shown.
			/// </summary>
			SHGFI_DISPLAYNAME = 0x000000200,

			/// <summary>
			/// Retrieve the string that describes the file's type. The string is copied to the szTypeName member of the structure specified in psfi.
			/// </summary>
			SHGFI_TYPENAME = 0x000000400,

			/// <summary>
			/// Retrieve the item attributes. The attributes are copied to the dwAttributes member of the structure specified in the psfi parameter. These are
			/// the same attributes that are obtained from IShellFolder::GetAttributesOf.
			/// </summary>
			SHGFI_ATTRIBUTES = 0x000000800,

			/// <summary>
			/// Retrieve the name of the file that contains the icon representing the file specified by pszPath, as returned by the IExtractIcon::GetIconLocation
			/// method of the file's icon handler. Also retrieve the icon index within that file. The name of the file containing the icon is copied to the
			/// szDisplayName member of the structure specified by psfi. The icon's index is copied to that structure's iIcon member.
			/// </summary>
			SHGFI_ICONLOCATION = 0x000001000,

			/// <summary>
			/// Retrieve the type of the executable file if pszPath identifies an executable file. The information is packed into the return value. This flag
			/// cannot be specified with any other flags.
			/// </summary>
			SHGFI_EXETYPE = 0x000002000,

			/// <summary>
			/// Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon member of psfi. The return value is a handle to
			/// the system image list. Only those images whose indices are successfully copied to iIcon are valid. Attempting to access other images in the
			/// system image list will result in undefined behavior.
			/// </summary>
			SHGFI_SYSICONINDEX = 0x000004000,

			/// <summary>Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The SHGFI_ICON flag must also be set.</summary>
			SHGFI_LINKOVERLAY = 0x000008000,

			/// <summary>
			/// Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight color. The SHGFI_ICON flag must also be set.
			/// </summary>
			SHGFI_SELECTED = 0x000010000,

			/// <summary>
			/// Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure at psfi contains the specific attributes that are
			/// desired. These attributes are passed to IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to
			/// IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with the SHGFI_ICON flag.
			/// </summary>
			SHGFI_ATTR_SPECIFIED = 0x000020000,

			/// <summary>Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.</summary>
			SHGFI_LARGEICON = 0x000000000,

			/// <summary>
			/// Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return
			/// the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
			/// </summary>
			SHGFI_SMALLICON = 0x000000001,

			/// <summary>
			/// Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return
			/// the handle to the system image list that contains the file's small open icon. A container object displays an open icon to indicate that the
			/// container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
			/// </summary>
			SHGFI_OPENICON = 0x000000002,

			/// <summary>
			/// Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not specified the function sizes the icon according to
			/// the system metric values. The SHGFI_ICON flag must also be set.
			/// </summary>
			SHGFI_SHELLICONSIZE = 0x000000004,

			/// <summary>Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.</summary>
			SHGFI_PIDL = 0x000000008,

			/// <summary>
			/// Indicates that the function should not attempt to access the file specified by pszPath. Rather, it should act as if the file specified by pszPath
			/// exists with the file attributes passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.
			/// </summary>
			SHGFI_USEFILEATTRIBUTES = 0x000000010,

			/// <summary>Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also be set.</summary>
			SHGFI_ADDOVERLAYS = 0x000000020,

			/// <summary>
			/// Return the index of the overlay icon. The value of the overlay index is returned in the upper eight bits of the iIcon member of the structure
			/// specified by psfi. This flag requires that the SHGFI_ICON be set as well.
			/// </summary>
			SHGFI_OVERLAYINDEX = 0x000000040
		}

		public enum ShellExecuteShowCommands
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11
		}

		public enum SHGetDataFormat
		{
			/// <summary>
			/// Format used for file system objects. The pv parameter is the address of a WIN32_FIND_DATA structure.
			/// </summary>
			SHGDFIL_FINDDATA = 1,
			/// <summary>
			/// Format used for network resources. The pv parameter is the address of a NETRESOURCE structure.
			/// </summary>
			SHGDFIL_NETRESOURCE = 2,
			/// <summary>
			/// Version 4.71. Format used for network resources. The pv parameter is the address of an SHDESCRIPTIONID structure.
			/// </summary>
			SHGDFIL_DESCRIPTIONID = 3
		}

		public enum SHIL
		{
			SHIL_LARGE      = 0,   // normally 32x32
			SHIL_SMALL      = 1,   // normally 16x16
			SHIL_EXTRALARGE = 2,
			SHIL_SYSSMALL   = 3,   // like SHIL_SMALL, but tracks system small icon metric correctly
			SHIL_JUMBO      = 4,   // normally 256x256
		}

		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		public static extern int ExtractIconEx([MarshalAs(UnmanagedType.LPTStr)] string lpszFile, int nIconIndex,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] IntPtr[] phIconLarge,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] IntPtr[] phIconSmall, int nIcons);

		/// <summary>Retrieves the User Model AppID that has been explicitly set for the current process via SetCurrentProcessExplicitAppUserModelID</summary>
		/// <param name="AppID"></param>
		[DllImport(nameof(Shell32))]
		public static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.BStr)] out string AppID);

		/// <summary>
		/// Tests whether two ITEMIDLIST structures are equal in a binary comparison.
		/// </summary>
		/// <param name="pidl1">The first ITEMIDLIST structure.</param>
		/// <param name="pidl2">The second ITEMIDLIST structure.</param>
		/// <returns>Returns TRUE if the two structures are equal, FALSE otherwise.</returns>
		[DllImport(nameof(Shell32), ExactSpelling = true, SetLastError = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);

		/// <summary> Sets the User Model AppID for the current process, enabling Windows to retrieve this ID </summary>
		/// <param name="AppID"></param>
		[DllImport(nameof(Shell32))]
		public static extern HRESULT SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item><definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition></item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern void SHAddToRecentDocs(SHARD uFlags, IShellLinkW pv);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item><definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition></item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern void SHAddToRecentDocs(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		/// <summary>
		/// Notifies the system that an item has been accessed, for the purposes of tracking those items used most recently and most frequently. This function can also be used to clear all usage data.
		/// </summary>
		/// <param name="uFlags">A value from the SHARD enumeration that indicates the form of the information pointed to by the pv parameter.</param>
		/// <param name="pv">A pointer to data that identifies the item that has been accessed. The item can be specified in this parameter in one of the following forms:
		/// <list type="bullet">
		/// <item><definition>A null-terminated string that contains the path and file name of the item.</definition></item>
		/// <item><definition>A PIDL that identifies the item's file object.</definition></item>
		/// <item><definition>Windows 7 and later only. A SHARDAPPIDINFO, SHARDAPPIDINFOIDLIST, or SHARDAPPIDINFOLINK structure that identifies the item through an AppUserModelID. See Application User Model IDs (AppUserModelIDs) for more information.</definition></item>
		/// <item><definition>Windows 7 and later only. An IShellLink object that identifies the item through a shortcut.</definition></item>
		/// </list>
		/// <para>Set this parameter to NULL to clear all usage data on all items.</para>
		/// </param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern void SHAddToRecentDocs(SHARD uFlags, PIDL pv);

		/// <summary>Displays a dialog box that enables the user to select a Shell folder.</summary>
		/// <param name="lpbi">A pointer to a BROWSEINFO structure that contains information used to display the dialog box.</param>
		/// <returns>
		/// Returns a PIDL that specifies the location of the selected folder relative to the root of the namespace. If the user chooses the Cancel button in the
		/// dialog box, the return value is NULL.
		/// </returns>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		public static extern PIDL SHBrowseForFolder(ref BROWSEINFO lpbi);

		/// <summary>
		/// Creates and initializes a Shell item object from a pointer to an item identifier list (PIDL). The resulting shell item object supports the IShellItem interface.
		/// </summary>
		/// <param name="pidl">The source PIDL.</param>
		/// <param name="riid">A reference to the IID of the requested interface.</param>
		/// <param name="ppv">When this function returns, contains the interface pointer requested in riid. This will typically be IShellItem or IShellItem2.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern void SHCreateItemFromIDList(PIDL pidl, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Creates and initializes a Shell item object from a parsing name.
		/// </summary>
		/// <param name="pszPath">A pointer to a display name.</param>
		/// <param name="pbc">Optional. A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function. These passed parameters are often specific to the data source and are documented by the data source owners. For example, the file system data source accepts the name being parsed (as a WIN32_FIND_DATA structure), using the STR_FILE_SYS_BIND_DATA bind context parameter.
		/// <para>STR_PARSE_PREFER_FOLDER_BROWSING can be passed to indicate that URLs are parsed using the file system data source when possible. Construct a bind context object using CreateBindCtx and populate the values using IBindCtx::RegisterObjectParam. See Bind Context String Keys for a complete list of these. See the Parsing With Parameters Sample for an example of the use of this parameter.</para>
		/// <para>If no data is being passed to or received from the parsing function, this value can be NULL.</para></param>
		/// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItem or IID_IShellItem2.</param>
		/// <param name="ppv">When this method returns successfully, contains the interface pointer requested in riid. This is typically IShellItem or IShellItem2.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath,
			[MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
			[MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Creates and initializes a Shell item object from a relative parsing name.
		/// </summary>
		/// <param name="psiParent">A pointer to the parent Shell item.</param>
		/// <param name="pszName">A pointer to a null-terminated, Unicode string that specifies a display name that is relative to the psiParent.</param>
		/// <param name="pbc">A pointer to a bind context that controls the parsing operation. This parameter can be NULL.</param>
		/// <param name="riid">A reference to an interface ID.</param>
		/// <param name="ppv">When this function returns, contains the interface pointer requested in riid. This will usually be IShellItem or IShellItem2.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateItemFromRelativeName([In, MarshalAs(UnmanagedType.Interface)] IShellItem psiParent, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName,
			[In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Creates a Shell item object for a single file that exists inside a known folder.
		/// </summary>
		/// <param name="kfid">A reference to the KNOWNFOLDERID, a GUID that identifies the folder that contains the item.</param>
		/// <param name="dwKFFlags">Flags that specify special options in the object retrieval. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.</param>
		/// <param name="pszItem">A pointer to a null-terminated buffer that contains the file name of the new item as a Unicode string. This parameter can also be NULL. In this case, an IShellItem that represents the known folder itself is created.</param>
		/// <param name="riid">A reference to an interface ID.</param>
		/// <param name="ppv">When this function returns, contains the interface pointer requested in riid. This will usually be IShellItem or IShellItem2.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateItemInKnownFolder([In, MarshalAs(UnmanagedType.LPStruct)] Guid kfid, [In] KNOWN_FOLDER_FLAG dwKFFlags,
			[In, Optional, MarshalAs(UnmanagedType.LPWStr)] string pszItem, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Create a Shell item, given a parent folder and a child item ID.
		/// </summary>
		/// <param name="pidlParent">The IDList of the parent folder of the item being created; the IDList of psfParent. This parameter can be NULL, if psfParent is specified.</param>
		/// <param name="psfParent">A pointer to IShellFolder interface that specifies the shell data source of the child item specified by the pidl.This parameter can be NULL, if pidlParent is specified.</param>
		/// <param name="pidl">A child item ID relative to its parent folder specified by psfParent or pidlParent.</param>
		/// <param name="riid">A reference to an interface ID.</param>
		/// <param name="ppvItem">When this function returns, contains the interface pointer requested in riid. This will usually be IShellItem or IShellItem2.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateItemWithParent([In] PIDL pidlParent, [In, MarshalAs(UnmanagedType.Interface)] IShellFolder psfParent,
			[In] PIDL pidl, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvItem);

		// [DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		// [SecurityCritical, SuppressUnmanagedCodeSecurity]
		// public static extern HRESULT SHCreateShellFolderView([In] ref SFV_CREATE pcsfv, [MarshalAs(UnmanagedType.Interface)] out object ppvItem);

		/// <summary>
		/// Creates a Shell item array object.
		/// </summary>
		/// <param name="pidlParent">The ID list of the parent folder of the items specified in ppidl. If psf is specified, this parameter can be NULL. If this pidlParent is not specified, it is computed from the psf parameter using IPersistFolder2.</param>
		/// <param name="psf">The Shell data source object that is the parent of the child items specified in ppidl. If pidlParent is specified, this parameter can be NULL.</param>
		/// <param name="cidl">The number of elements in the array specified by ppidl.</param>
		/// <param name="ppidl">The list of child item IDs for which the array is being created. This value can be NULL.</param>
		/// <param name="ppsiItemArray">When this function returns, contains the address of an <see cref="IShellItemArray"/> interface pointer.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateShellItemArray([In] PIDL pidlParent, [In, MarshalAs(UnmanagedType.Interface)] IShellFolder psf,
			uint cidl, [In] PIDL ppidl, [MarshalAs(UnmanagedType.Interface)] out object ppsiItemArray);

		/// <summary>
		/// Creates a Shell item array object from a list of ITEMIDLIST structures.
		/// </summary>
		/// <param name="cidl">The number of elements in the array.</param>
		/// <param name="rgpidl">A list of cidl constant pointers to ITEMIDLIST structures.</param>
		/// <param name="ppsiItemArray">When this function returns, contains an IShellItemArray interface pointer.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateShellItemArrayFromIDLists(uint cidl, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] PIDL[] rgpidl, [MarshalAs(UnmanagedType.Interface)] out object ppsiItemArray);

		/// <summary>
		/// Creates an array of one element from a single Shell item.
		/// </summary>
		/// <param name="psi">Pointer to IShellItem object that represents the item.</param>
		/// <param name="riid">A reference to the IID of the interface to retrieve through ppv, typically IID_IShellItemArray.</param>
		/// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically a pointer to an IShellItemArray.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHCreateShellItemArrayFromShellItem([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Provides a default handler to extract an icon from a file.
		/// </summary>
		/// <param name="pszIconFile">A pointer to a null-terminated buffer that contains the path and name of the file from which the icon is extracted.</param>
		/// <param name="iIndex">The location of the icon within the file named in pszIconFile. If this is a positive number, it refers to the zero-based position of the icon in the file. For instance, 0 refers to the 1st icon in the resource file and 2 refers to the 3rd. If this is a negative number, it refers to the icon's resource ID.</param>
		/// <param name="uFlags">A flag that controls the icon extraction.</param>
		/// <param name="phiconLarge">A pointer to an HICON that, when this function returns successfully, receives the handle of the large version of the icon specified in the LOWORD of nIconSize. This value can be NULL.</param>
		/// <param name="phiconSmall">A pointer to an HICON that, when this function returns successfully, receives the handle of the small version of the icon specified in the HIWORD of nIconSize.</param>
		/// <param name="nIconSize">A value that contains the large icon size in its LOWORD and the small icon size in its HIWORD. Size is measured in pixels. Pass 0 to specify default large and small sizes.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		public static extern HRESULT SHDefExtractIcon(string pszIconFile, int iIndex, uint uFlags, ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIconSize);

		/// <summary>
		/// Provides a default handler to extract an icon from a file.
		/// </summary>
		/// <param name="pszIconFile">A pointer to a null-terminated buffer that contains the path and name of the file from which the icon is extracted.</param>
		/// <param name="iIndex">The location of the icon within the file named in pszIconFile. If this is a positive number, it refers to the zero-based position of the icon in the file. For instance, 0 refers to the 1st icon in the resource file and 2 refers to the 3rd. If this is a negative number, it refers to the icon's resource ID.</param>
		/// <param name="uFlags">A flag that controls the icon extraction.</param>
		/// <param name="phiconLarge">A pointer to an HICON that, when this function returns successfully, receives the handle of the large version of the icon specified in the LOWORD of nIconSize. This value can be NULL.</param>
		/// <param name="phiconSmall">A pointer to an HICON that, when this function returns successfully, receives the handle of the small version of the icon specified in the HIWORD of nIconSize.</param>
		/// <param name="nIconSize">A value that contains the large icon size in its LOWORD and the small icon size in its HIWORD. Size is measured in pixels. Pass 0 to specify default large and small sizes.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		public static extern HRESULT SHDefExtractIcon(string pszIconFile, int iIndex, uint uFlags, IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIconSize);

		/// <summary>
		/// Performs an operation on a specified file.
		/// </summary>
		/// <param name="lpExecInfo">A pointer to a SHELLEXECUTEINFO structure that contains and receives information about the application being executed.</param>
		/// <returns>Returns TRUE if successful; otherwise, FALSE. Call GetLastError for extended error information.</returns>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

		/// <summary>
		/// Retrieves extended property data from a relative identifier list.
		/// </summary>
		/// <param name="psf">The address of the parent IShellFolder interface. This must be the immediate parent of the ITEMIDLIST structure referenced by the pidl parameter.</param>
		/// <param name="pidl">A pointer to an ITEMIDLIST structure that identifies the object relative to the folder specified in psf.</param>
		/// <param name="nFormat">The format in which the data is being requested. </param>
		/// <param name="pv">A pointer to a buffer that, when this function returns successfully, receives the requested data. The format of this buffer is determined by nFormat.
		/// <para>If nFormat is SHGDFIL_NETRESOURCE, there are two possible cases. If the buffer is large enough, the net resource's string information (fields for the network name, local name, provider, and comments) will be placed into the buffer. If the buffer is not large enough, only the net resource structure will be placed into the buffer and the string information pointers will be NULL.</para></param>
		/// <param name="cb">Size of the buffer at pv, in bytes.</param>
		/// <remarks>This function extracts only information that is present in the pointer to an item identifier list (PIDL). Since the content of a PIDL depends on the folder object that created the PIDL, there is no guarantee that all requested information will be available. In addition, the information that is returned reflects the state of the object at the time the PIDL was created. The current state of the object could be different. For example, if you set nFormat to SHGDFIL_FINDDATA, the function might assign meaningful values to only some of the members of the WIN32_FIND_DATA structure. The remaining members will be set to zero. To retrieve complete current information on a file system file or folder, use standard file system functions such as GetFileTime or FindFirstFile.
		/// <para>E_INVALIDARG is returned if the psf, pidl, pv, or cb parameter does not match the nFormat parameter, or if nFormat is not one of the specific SHGDFIL_ values shown above.</para></remarks>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHGetDataFromIDList([In, MarshalAs(UnmanagedType.Interface)] IShellFolder psf, [In] PIDL pidl, SHGetDataFormat nFormat, [In, Out] IntPtr pv, int cb);

		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

		/// <summary>
		/// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
		/// </summary>
		/// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.
		/// <para>If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. Relative PIDLs are not allowed.</para>
		/// <para>If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.</para>
		/// <para>This string can use either short (the 8.3 form) or long file names.</para></param>
		/// <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
		/// <param name="psfi">Pointer to a SHFILEINFO structure to receive the file information.</param>
		/// <param name="cbFileInfo">The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.</param>
		/// <param name="uFlags">The flags that specify the file information to retrieve.</param>
		/// <returns>Returns a value whose meaning depends on the uFlags parameter.
		/// <para>If uFlags does not contain SHGFI_EXETYPE or SHGFI_SYSICONINDEX, the return value is nonzero if successful, or zero otherwise.</para>
		/// <para>If uFlags contains the SHGFI_EXETYPE flag, the return value specifies the type of the executable file. It will be one of the following values.</para></returns>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI uFlags);

		/// <summary>
		/// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
		/// </summary>
		/// <param name="itemIdList">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.
		/// <para>If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. Relative PIDLs are not allowed.</para>
		/// <para>If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.</para>
		/// <para>This string can use either short (the 8.3 form) or long file names.</para></param>
		/// <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
		/// <param name="psfi">Pointer to a SHFILEINFO structure to receive the file information.</param>
		/// <param name="cbFileInfo">The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.</param>
		/// <param name="uFlags">The flags that specify the file information to retrieve.</param>
		/// <returns>Returns a value whose meaning depends on the uFlags parameter.
		/// <para>If uFlags does not contain SHGFI_EXETYPE or SHGFI_SYSICONINDEX, the return value is nonzero if successful, or zero otherwise.</para>
		/// <para>If uFlags contains the SHGFI_EXETYPE flag, the return value specifies the type of the executable file. It will be one of the following values.</para></returns>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SHGetFileInfo(PIDL itemIdList, FileAttributes dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, SHGFI uFlags);

		/// <summary>
		/// Flags used by <see cref="SHGetFolderPath"/>.
		/// </summary>
		public enum SHGFP
		{
			/// <summary>Retrieve the folder's current path.</summary>
			SHGFP_TYPE_CURRENT = 0,
			/// <summary>Retrieve the folder's default path.</summary>
			SHGFP_TYPE_DEFAULT = 1
		}

		/// <summary>
		/// Deprecated. Gets the path of a folder identified by a CSIDL value.
		/// <note>As of Windows Vista, this function is merely a wrapper for SHGetKnownFolderPath. The CSIDL value is translated to its associated KNOWNFOLDERID and then SHGetKnownFolderPath is called. New applications should use the known folder system rather than the older CSIDL system, which is supported only for backward compatibility.</note>
		/// </summary>
		/// <param name="hwndOwner">Reserved.</param>
		/// <param name="nFolder">A CSIDL value that identifies the folder whose path is to be retrieved. Only real folders are valid. If a virtual folder is specified, this function fails. You can force creation of a folder by combining the folder's CSIDL with CSIDL_FLAG_CREATE.</param>
		/// <param name="hToken">An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder for the current user.
		/// <para>Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of that specific user must be mounted. See Access Control for further discussion of access control issues.</para>
		/// <para>Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the Default User folders requires administrator privileges.</para></param>
		/// <param name="dwFlags">Flags that specify the path to be returned. This value is used in cases where the folder associated with a KNOWNFOLDERID (or CSIDL) can be moved, renamed, redirected, or roamed across languages by a user or administrator.
		/// <para>The known folder system that underlies SHGetFolderPath allows users or administrators to redirect a known folder to a location that suits their needs. This is achieved by calling IKnownFolderManager::Redirect, which sets the "current" value of the folder associated with the SHGFP_TYPE_CURRENT flag.</para>
		/// <para>The default value of the folder, which is the location of the folder if a user or administrator had not redirected it elsewhere, is retrieved by specifying the SHGFP_TYPE_DEFAULT flag. This value can be used to implement a "restore defaults" feature for a known folder.</para>
		/// <para>For example, the default value (SHGFP_TYPE_DEFAULT) for FOLDERID_Music (CSIDL_MYMUSIC) is "C:\Users\user name\Music". If the folder was redirected, the current value (SHGFP_TYPE_CURRENT) might be "D:\Music". If the folder has not been redirected, then SHGFP_TYPE_DEFAULT and SHGFP_TYPE_CURRENT retrieve the same path.</para></param>
		/// <param name="pszPath">A pointer to a null-terminated string of length MAX_PATH which will receive the path. If an error occurs or S_FALSE is returned, this string will be empty. The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHGetFolderPath(IntPtr hwndOwner, int nFolder, [In, Optional] AdvApi32.SafeTokenHandle hToken, SHGFP dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath);

		/// <summary>
		/// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID. This extends SHGetKnownFolderPath by allowing you to set the initial size of the string buffer.
		/// </summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.</param>
		/// <param name="hToken">An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder for the current user.
		/// <para>Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of that specific user must be mounted. See Access Control for further discussion of access control issues.</para>
		/// <para>Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the Default User folders requires administrator privileges.</para></param>
		/// <param name="pszPath">A null-terminated, Unicode string. This buffer must be of size cchPath. When SHGetFolderPathEx returns successfully, this parameter contains the path for the known folder.</param>
		/// <param name="cchPath">The size of the ppszPath buffer, in characters.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		public static extern HRESULT SHGetFolderPathEx([In, MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] AdvApi32.SafeTokenHandle hToken, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath);

		/// <summary>Retrieves the pointer to an item identifier list (PIDL) of an object.</summary>
		/// <param name="iUnknown">A pointer to the IUnknown of the object from which to get the PIDL.</param>
		/// <param name="ppidl">When this function returns, contains a pointer to the PIDL of the given object.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown, out PIDL ppidl);

		/// <summary>Retrieves an image list.</summary>
		/// <param name="iImageList">The image type contained in the list.</param>
		/// <param name="riid">Reference to the image list interface identifier, normally IID_IImageList.</param>
		/// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IImageList.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetImageList(SHIL iImageList, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out ComCtl32.IImageList ppv);

		/// <summary>Retrieves the path of a known folder as an ITEMIDLIST structure.</summary>
		/// <param name="rfid">
		/// A reference to the KNOWNFOLDERID that identifies the folder. The folders associated with the known folder IDs might not exist on a particular system.
		/// </param>
		/// <param name="dwFlags">
		/// Flags that specify special retrieval options. This value can be 0; otherwise, it is one or more of the KNOWN_FOLDER_FLAG values.
		/// </param>
		/// <param name="hToken">
		/// An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to access the current
		/// user's instance of the folder. However, you may need to assign a value to hToken for those folders that can have multiple users but are treated as
		/// belonging to a single user. The most commonly used folder of this type is Documents.
		/// <para>
		/// The calling application is responsible for correct impersonation when hToken is non-null. It must have appropriate security privileges for the
		/// particular user, including TOKEN_QUERY and TOKEN_IMPERSONATE, and the user's registry hive must be currently mounted. See Access Control for further
		/// discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderIDList to find folder locations (such
		/// as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special
		/// folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the
		/// Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="ppidl">
		/// When this method returns, contains a pointer to the PIDL of the folder. This parameter is passed uninitialized. The caller is responsible for freeing
		/// the returned PIDL when it is no longer needed by calling ILFree.
		/// </param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetKnownFolderIDList([In, MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] AdvApi32.SafeTokenHandle hToken, out PIDL ppidl);

		/// <summary>
		/// Retrieves an IShellItem object that represents a known folder.
		/// </summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID, a GUID that identifies the folder that contains the item.</param>
		/// <param name="dwFlags">Flags that specify special options used in the retrieval of the known folder IShellItem. This value can be KF_FLAG_DEFAULT; otherwise, one or more of the KNOWN_FOLDER_FLAG values.</param>
		/// <param name="hToken">
		/// An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to access the current
		/// user's instance of the folder. However, you may need to assign a value to hToken for those folders that can have multiple users but are treated as
		/// belonging to a single user. The most commonly used folder of this type is Documents.
		/// <para>
		/// The calling application is responsible for correct impersonation when hToken is non-null. It must have appropriate security privileges for the
		/// particular user, including TOKEN_QUERY and TOKEN_IMPERSONATE, and the user's registry hive must be currently mounted. See Access Control for further
		/// discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderIDList to find folder locations (such
		/// as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special
		/// folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the
		/// Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <param name="riid">A reference to the IID of the interface that represents the item, usually IID_IShellItem or IID_IShellItem2.</param>
		/// <param name="ppv">When this method returns, contains the interface pointer requested in riid.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetKnownFolderItem([In, MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, [In, Optional] AdvApi32.SafeTokenHandle hToken,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		/// <summary>
		/// Retrieves the display name of an item identified by its IDList.
		/// </summary>
		/// <param name="pidl">A PIDL that identifies the item.</param>
		/// <param name="sigdnName">A value from the SIGDN enumeration that specifies the type of display name to retrieve.</param>
		/// <param name="ppszName">A value that, when this function returns successfully, receives the address of a pointer to the retrieved display name.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetNameFromIDList(PIDL pidl, SIGDN sigdnName, out SafeCoTaskMemHandle ppszName);

		/// <summary>
		/// Converts an item identifier list to a file system path.
		/// </summary>
		/// <param name="pidl">The address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
		/// <param name="pszPath">The address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
		/// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
		[DllImport(nameof(Shell32), CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHGetPathFromIDList(PIDL pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

		/// <summary>
		/// Deprecated. Retrieves the path of a folder as an ITEMIDLIST structure.
		/// </summary>
		/// <param name="hwndOwner">Reserved.</param>
		/// <param name="nFolder">A CSIDL value that identifies the folder to be located. The folders associated with the CSIDLs might not exist on a particular system.</param>
		/// <param name="hToken">An access token that can be used to represent a particular user. It is usually set to NULL, but it may be needed when there are multiple users for those folders that are treated as belonging to a single user. The most commonly used folder of this type is My Documents. The calling application is responsible for correct impersonation when hToken is non-NULL. It must have appropriate security privileges for the particular user, and the user's registry hive must be currently mounted.
		/// <para>Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetFolderLocation to find folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special folders such as My Documents and Desktop. Any items added to the Default User folder also appear in any new user account.</para></param>
		/// <param name="dwReserved">Reserved.</param>
		/// <param name="ppidl">The address of a pointer to an item identifier list structure that specifies the folder's location relative to the root of the namespace (the desktop). The ppidl parameter is set to NULL on failure. The calling application is responsible for freeing this resource by calling CoTaskMemFree.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetFolderLocation(IntPtr hwndOwner, int nFolder, AdvApi32.SafeTokenHandle hToken, int dwReserved, out PIDL ppidl);

		/// <summary>
		/// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.
		/// </summary>
		/// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.</param>
		/// <param name="hToken">An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder for the current user.
		/// <para>Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has sufficient privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE rights. In some cases, you also need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of that specific user must be mounted. See Access Control for further discussion of access control issues.</para>
		/// <para>Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find folder locations (such as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the Default User folders requires administrator privileges.</para></param>
		/// <param name="pszPath">When this method returns, contains the address of a pointer to a null-terminated Unicode string that specifies the path of the known folder. The calling process is responsible for freeing this resource once it is no longer needed by calling CoTaskMemFree. The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
		/// <remarks>This function replaces SHGetFolderPath. That older function is now simply a wrapper for SHGetKnownFolderPath.</remarks>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, AdvApi32.SafeTokenHandle hToken, out SafeCoTaskMemHandle pszPath);

		/// <summary>Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.</summary>
		/// <param name="id">A reference to the KNOWNFOLDERID that identifies the folder.</param>
		/// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, one or more of the KNOWN_FOLDER_FLAG values.</param>
		/// <param name="hToken">
		/// An access token that represents a particular user. If this parameter is NULL, which is the most common usage, the function requests the known folder
		/// for the current user.
		/// <para>
		/// Request a specific user's folder by passing the hToken of that user. This is typically done in the context of a service that has sufficient
		/// privileges to retrieve the token of a given user. That token must be opened with TOKEN_QUERY and TOKEN_IMPERSONATE rights. In some cases, you also
		/// need to include TOKEN_DUPLICATE. In addition to passing the user's hToken, the registry hive of that specific user must be mounted. See Access
		/// Control for further discussion of access control issues.
		/// </para>
		/// <para>
		/// Assigning the hToken parameter a value of -1 indicates the Default User. This allows clients of SHGetKnownFolderPath to find folder locations (such
		/// as the Desktop folder) for the Default User. The Default User user profile is duplicated when any new user account is created, and includes special
		/// folders such as Documents and Desktop. Any items added to the Default User folder also appear in any new user account. Note that access to the
		/// Default User folders requires administrator privileges.
		/// </para>
		/// </param>
		/// <returns>String that specifies the path of the known folder.</returns>
		/// <remarks>This function replaces SHGetFolderPath. That older function is now simply a wrapper for SHGetKnownFolderPath.</remarks>
		public static string SHGetKnownFolderPath(KNOWNFOLDERID_Enum id, KNOWN_FOLDER_FLAG dwFlags, AdvApi32.SafeTokenHandle hToken = null)
		{
			SafeCoTaskMemHandle path;
			SHGetKnownFolderPath(id.Guid(), dwFlags, hToken ?? AdvApi32.SafeTokenHandle.Null, out path);
			return path.ToString(-1);
		}

		/// <summary>
		/// Opens a Windows Explorer window with specified items in a particular folder selected.
		/// </summary>
		/// <param name="pidlFolder">A pointer to a fully qualified item ID list that specifies the folder.</param>
		/// <param name="cidl">A count of items in the selection array, apidl. If cidl is zero, then pidlFolder must point to a fully specified ITEMIDLIST describing a single item to select. This function opens the parent folder and selects that item.</param>
		/// <param name="apidl">A pointer to an array of PIDL structures, each of which is an item to select in the target folder referenced by pidlFolder.</param>
		/// <param name="dwFlags">The optional flags. Under Windows XP this parameter is ignored. In Windows Vista, the following flags are defined.</param>
		[DllImport(nameof(Shell32), ExactSpelling = true)]
		public static extern HRESULT SHOpenFolderAndSelectItems(PIDL pidlFolder, uint cidl, [In, Optional, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] PIDL[] apidl, OFASI dwFlags);

		/// <summary>
		/// Translates a Shell namespace object's display name into an item identifier list and returns the attributes of the object. This function is the preferred method to convert a string to a pointer to an item identifier list (PIDL).
		/// </summary>
		/// <param name="pszName">A pointer to a zero-terminated wide string that contains the display name to parse.</param>
		/// <param name="pbc">A bind context that controls the parsing operation. This parameter is normally set to NULL.</param>
		/// <param name="ppidl">The address of a pointer to a variable of type ITEMIDLIST that receives the item identifier list for the object. If an error occurs, then this parameter is set to NULL.</param>
		/// <param name="sfgaoIn">A ULONG value that specifies the attributes to query. To query for one or more attributes, initialize this parameter with the flags that represent the attributes of interest. For a list of available SFGAO flags, see IShellFolder::GetAttributesOf.</param>
		/// <param name="psfgaoOut">A pointer to a ULONG. On return, those attributes that are true for the object and were requested in sfgaoIn are set. An object's attribute flags can be zero or a combination of SFGAO flags. For a list of available SFGAO flags, see IShellFolder::GetAttributesOf.</param>
		[DllImport(nameof(Shell32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern HRESULT SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszName, [In, Optional] IntPtr pbc, out PIDL ppidl, SFGAO sfgaoIn, out SFGAO psfgaoOut);

		[StructLayout(LayoutKind.Sequential)]
		public struct ITEMIDLIST
		{
			/// <summary>A list of item identifiers.</summary>
			[MarshalAs(UnmanagedType.Struct)] public SHITEMID mkid;
		}

		/*[StructLayout(LayoutKind.Sequential)]
		public struct SFV_CREATE
		{
			public uint cbSize;
			[MarshalAs(UnmanagedType.Interface)] public IShellFolder pshf;
			[MarshalAs(UnmanagedType.Interface)] public IShellView psvOuter;
			[MarshalAs(UnmanagedType.Interface)] public IShellFolderViewCB psfbcb;
		}*/

		[StructLayout(LayoutKind.Sequential)]
		public struct SHELLEXECUTEINFO
		{
			public int cbSize;
			public ShellExecuteMaskFlags fMask;
			public IntPtr hwnd;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpVerb;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpFile;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpParameters;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpDirectory;
			public ShellExecuteShowCommands nShellExecuteShow;
			public IntPtr hInstApp;
			public IntPtr lpIDList;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpClass;
			public IntPtr hkeyClass;
			public uint dwHotKey;
			public IntPtr hIcon;
			public IntPtr hProcess;

			public SHELLEXECUTEINFO(string fileName, string parameters = null) : this()
			{
				cbSize = Marshal.SizeOf(this);
				lpFile = fileName;
				lpParameters = parameters;
				nShellExecuteShow = ShellExecuteShowCommands.SW_NORMAL;
			}
		}

		/// <summary>Contains information about a file object.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct SHFILEINFO
		{
			/// <summary>
			/// A handle to the icon that represents the file. You are responsible for destroying this handle with DestroyIcon when you no longer need it.
			/// </summary>
			public IntPtr hIcon;
			/// <summary>The index of the icon image within the system image list.</summary>
			public int iIcon;
			/// <summary>
			/// An array of values that indicates the attributes of the file object. For information about these values, see the IShellFolder::GetAttributesOf method.
			/// </summary>
			public int dwAttributes;
			/// <summary>
			/// A string that contains the name of the file as it appears in the Windows Shell, or the path and file name of the file that contains the icon
			/// representing the file.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;
			/// <summary>A string that describes the type of file.</summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;

			public static int Size => Marshal.SizeOf(typeof(SHFILEINFO));
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SHITEMID
		{
			/// <summary>The size of identifier, in bytes, including <see cref="cb" /> itself.</summary>
			public ushort cb;
			/// <summary>A variable-length item identifier.</summary>
			public byte[] abID;
		}

		public enum STRRET_TYPE : uint
		{
			STRRET_WSTR = 0x0000,          // Use STRRET.pOleStr
			STRRET_OFFSET = 0x0001,          // Use STRRET.uOffset to Ansi
			STRRET_CSTR = 0x0002,          // Use STRRET.cStr
		}

		[StructLayout(LayoutKind.Explicit, Size = 264)]
		public struct STRRET
		{
			/// <summary>
			/// A value that specifies the desired format of the string.
			/// </summary>
			[FieldOffset(0)]
			public STRRET_TYPE uType;

			/// <summary>
			/// A pointer to the string. This memory must be allocated with CoTaskMemAlloc. It is the calling application's responsibility to free this memory with CoTaskMemFree when it is no longer needed.
			/// </summary>
			[FieldOffset(4), MarshalAs(UnmanagedType.BStr)]
			public string pOleStr;    // must be freed by caller of GetDisplayNameOf

			/// <summary>
			/// The offset into the item identifier list.
			/// </summary>
			[FieldOffset(4)]
			public uint uOffset;    // Offset into SHITEMID

			/// <summary>
			/// The buffer to receive the display name. CHAR[MAX_PATH]
			/// </summary>
			[FieldOffset(4), MarshalAs(UnmanagedType.LPStr, SizeConst = Kernel32.MAX_PATH)]
			public string cStr;        // Buffer to fill in (ANSI)

			public override string ToString() => uType == STRRET_TYPE.STRRET_CSTR ? cStr : (uType == STRRET_TYPE.STRRET_WSTR ? pOleStr : string.Empty);
		}
	}
}