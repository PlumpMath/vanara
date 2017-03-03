using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Vanara.Extensions;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Kernel32
	{
		public const int MAX_PATH = 260;
		public const int INVALID_FILE_SIZE = unchecked((int)0xFFFFFFFF);
		public const int INVALID_SET_FILE_POINTER = -1;
		public const int INVALID_FILE_ATTRIBUTES = -2;

		[UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		public delegate bool EnumResNameProc(IntPtr hModule, string lpszType, string lpszName, IntPtr lParam);

		[UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		private delegate bool EnumResNameProcManaged(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, ResList lParam);

		[Flags]
		public enum FileFlagsAndAttributes : uint
		{
			FILE_ATTRIBUTE_READONLY            = 0x00000001, 
			FILE_ATTRIBUTE_HIDDEN              = 0x00000002, 
			FILE_ATTRIBUTE_SYSTEM              = 0x00000004, 
			FILE_ATTRIBUTE_DIRECTORY           = 0x00000010, 
			FILE_ATTRIBUTE_ARCHIVE             = 0x00000020, 
			FILE_ATTRIBUTE_DEVICE              = 0x00000040, 
			FILE_ATTRIBUTE_NORMAL              = 0x00000080, 
			FILE_ATTRIBUTE_TEMPORARY           = 0x00000100, 
			FILE_ATTRIBUTE_SPARSE_FILE         = 0x00000200, 
			FILE_ATTRIBUTE_REPARSE_POINT       = 0x00000400, 
			FILE_ATTRIBUTE_COMPRESSED          = 0x00000800, 
			FILE_ATTRIBUTE_OFFLINE             = 0x00001000, 
			FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000, 
			FILE_ATTRIBUTE_ENCRYPTED           = 0x00004000, 
			FILE_ATTRIBUTE_INTEGRITY_STREAM    = 0x00008000, 
			FILE_ATTRIBUTE_VIRTUAL             = 0x00010000, 
			FILE_ATTRIBUTE_NO_SCRUB_DATA       = 0x00020000, 
			FILE_ATTRIBUTE_EA                  = 0x00040000, 
			FILE_FLAG_WRITE_THROUGH            = 0x80000000,
			FILE_FLAG_OVERLAPPED               = 0x40000000,
			FILE_FLAG_NO_BUFFERING             = 0x20000000,
			FILE_FLAG_RANDOM_ACCESS            = 0x10000000,
			FILE_FLAG_SEQUENTIAL_SCAN          = 0x08000000,
			FILE_FLAG_DELETE_ON_CLOSE          = 0x04000000,
			FILE_FLAG_BACKUP_SEMANTICS         = 0x02000000,
			FILE_FLAG_POSIX_SEMANTICS          = 0x01000000,
			FILE_FLAG_SESSION_AWARE            = 0x00800000,
			FILE_FLAG_OPEN_REPARSE_POINT       = 0x00200000,
			FILE_FLAG_OPEN_NO_RECALL           = 0x00100000,
			FILE_FLAG_FIRST_PIPE_INSTANCE      = 0x00080000,
			SECURITY_ANONYMOUS                 = 0x00000000,
			SECURITY_IDENTIFICATION            = 0x00010000,
			SECURITY_IMPERSONATION             = 0x00020000,
			SECURITY_DELEGATION                = 0x00030000,
			SECURITY_CONTEXT_TRACKING          = 0x00040000,
			SECURITY_EFFECTIVE_ONLY            = 0x00080000,
			SECURITY_SQOS_PRESENT              = 0x00100000,
			SECURITY_VALID_SQOS_FLAGS          = 0x001F0000,
		}

		/// <summary>Flags that may be passed to the <see cref="Kernel32.GetVolumeInformation(string,System.Text.StringBuilder,int,ref int,ref int,ref FileSystemFlags,System.Text.StringBuilder,int)"/> function.</summary>
		[Flags]
		public enum FileSystemFlags
		{
			/// <summary>The specified volume supports case-sensitive file names.</summary>
			FILE_CASE_SENSITIVE_SEARCH = 0x00000001,

			/// <summary>The specified volume supports preserved case of file names when it places a name on disk.</summary>
			FILE_CASE_PRESERVED_NAMES = 0x00000002,

			/// <summary>The specified volume supports Unicode in file names as they appear on disk.</summary>
			FILE_UNICODE_ON_DISK = 0x00000004,

			/// <summary>
			/// The specified volume preserves and enforces access control lists (ACL). For example, the NTFS file system preserves and enforces ACLs, and the
			/// FAT file system does not.
			/// </summary>
			FILE_PERSISTENT_ACLS = 0x00000008,

			/// <summary>The specified volume supports file-based compression.</summary>
			FILE_FILE_COMPRESSION = 0x00000010,

			/// <summary>The specified volume supports disk quotas.</summary>
			FILE_VOLUME_QUOTAS = 0x00000020,

			/// <summary>The specified volume supports sparse files.</summary>
			FILE_SUPPORTS_SPARSE_FILES = 0x00000040,

			/// <summary>The specified volume supports reparse points.</summary>
			FILE_SUPPORTS_REPARSE_POINTS = 0x00000080,

			/// <summary>The specified volume supports remote storage.</summary>
			FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100,

			/// <summary>The specified volume is a compressed volume, for example, a DoubleSpace volume.</summary>
			FILE_VOLUME_IS_COMPRESSED = 0x00008000,

			/// <summary>The specified volume supports object identifiers.</summary>
			FILE_SUPPORTS_OBJECT_IDS = 0x00010000,

			/// <summary>
			/// The specified volume supports the Encrypted File System (EFS). For more information, see <a
			/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa364223(v=vs.85).aspx">File Encryption</a>.
			/// </summary>
			FILE_SUPPORTS_ENCRYPTION = 0x00020000,

			/// <summary>The specified volume supports named streams.</summary>
			FILE_NAMED_STREAMS = 0x00040000,

			/// <summary>The specified volume is read-only.</summary>
			FILE_READ_ONLY_VOLUME = 0x00080000,

			/// <summary>The specified volume supports a single sequential write.</summary>
			FILE_SEQUENTIAL_WRITE_ONCE = 0x00100000,

			/// <summary>
			/// The specified volume supports transactions. For more information, see <a
			/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365993(v=vs.85).aspx">About KTM</a>.
			/// </summary>
			FILE_SUPPORTS_TRANSACTIONS = 0x00200000,

			/// <summary>
			/// The specified volume supports hard links. For more information, see <a
			/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365006(v=vs.85).aspx">Hard Links and Junctions.</a>
			/// <para>
			/// <c>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c> This value is not supported until Windows Server 2008 R2 and
			/// Windows 7.
			/// </para>
			/// </summary>
			FILE_SUPPORTS_HARD_LINKS = 0x00400000,

			/// <summary>
			/// The specified volume supports extended attributes. An extended attribute is a piece of application-specific metadata that an application can
			/// associate with a file and is not part of the file's data.
			/// <para>
			/// <c>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c> This value is not supported until Windows Server 2008 R2 and
			/// Windows 7.
			/// </para>
			/// </summary>
			FILE_SUPPORTS_EXTENDED_ATTRIBUTES = 0x00800000,

			/// <summary>
			/// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
			/// <para>
			/// <c>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c> This value is not supported until Windows Server 2008 R2 and
			/// Windows 7.
			/// </para>
			/// </summary>
			FILE_SUPPORTS_OPEN_BY_FILE_ID = 0x01000000,

			/// <summary>
			/// The specified volume supports update sequence number (USN) journals. For more information, see <a
			/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa363803(v=vs.85).aspx">Change Journal Records</a> .
			/// <para>
			/// <c>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c> This value is not supported until Windows Server 2008 R2 and
			/// Windows 7.
			/// </para>
			/// </summary>
			FILE_SUPPORTS_USN_JOURNAL = 0x02000000,

			/// <summary>The specified volume supports integrity streams.</summary>
			FILE_SUPPORTS_INTEGRITY_STREAMS = 0x04000000,

			/// <summary>The specified volume supports block refcounting.</summary>
			FILE_SUPPORTS_BLOCK_REFCOUNTING = 0x08000000,

			/// <summary>The specified volume supports sparse VDL.</summary>
			FILE_SUPPORTS_SPARSE_VDL = 0x10000000,

			/// <summary>
			/// The specified volume is a direct access (DAX) volume.
			/// <para><c>Note</c> This flag was introduced in Windows 10, version 1607.</para>
			/// </summary>
			FILE_DAX_VOLUME = 0x20000000,

			/// <summary>The specified volume supports ghosting.</summary>
			FILE_SUPPORTS_GHOSTING = 0x40000000
		}

		/// <summary>Flags passed to the <see cref="Kernel32.FormatMessage(FormatMessageFlags,SafeLibraryHandle,int,int,System.IntPtr,int,string[])"/> method.</summary>
		[Flags]
		public enum FormatMessageFlags
		{
			/// <summary>
			/// The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified
			/// by lpBuffer. The nSize parameter specifies the minimum number of TCHARs to allocate for an output message buffer. The caller should use the
			/// LocalFree function to free the buffer when it is no longer needed.
			/// <para>
			/// If the length of the formatted message exceeds 128K bytes, then FormatMessage will fail and a subsequent call to GetLastError will return ERROR_MORE_DATA.
			/// </para>
			/// <para>
			/// In previous versions of Windows, this value was not available for use when compiling Windows Store apps. As of Windows 10 this value can be used.
			/// </para>
			/// <para>
			/// Windows Server 2003 and Windows XP: If the length of the formatted message exceeds 128K bytes, then FormatMessage will not automatically fail
			/// with an error of ERROR_MORE_DATA.
			/// </para>
			/// <para>
			/// Windows 10: LocalFree is not in the modern SDK, so it cannot be used to free the result buffer. Instead, use HeapFree (GetProcessHeap(),
			/// allocatedMessage). In this case, this is the same as calling LocalFree on memory.
			/// </para>
			/// <para>
			/// Important: LocalAlloc() has different options: LMEM_FIXED, and LMEM_MOVABLE. FormatMessage() uses LMEM_FIXED, so HeapFree can be used. If
			///            LMEM_MOVABLE is used, HeapFree cannot be used.
			/// </para>
			/// </summary>
			FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100,

			/// <summary>
			/// The Arguments parameter is not a va_list structure, but is a pointer to an array of values that represent the arguments. This flag cannot be used
			/// with 64-bit integer values. If you are using a 64-bit integer, you must use the va_list structure.
			/// </summary>
			FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x2000,

			/// <summary>
			/// The lpSource parameter is a module handle containing the message-table resource(s) to search. If this lpSource handle is NULL, the current
			/// process's application image file will be searched. This flag cannot be used with <see cref="FORMAT_MESSAGE_FROM_STRING"/>.
			/// <para>If the module has no message table resource, the function fails with ERROR_RESOURCE_TYPE_NOT_FOUND.</para>
			/// </summary>
			FORMAT_MESSAGE_FROM_HMODULE = 0x800,

			/// <summary>
			/// The lpSource parameter is a pointer to a null-terminated string that contains a message definition. The message definition may contain insert
			/// sequences, just as the message text in a message table resource may. This flag cannot be used with <see cref="FORMAT_MESSAGE_FROM_HMODULE"/> or
			/// <see cref="FORMAT_MESSAGE_FROM_SYSTEM"/>.
			/// </summary>
			FORMAT_MESSAGE_FROM_STRING = 0x400,

			/// <summary>
			/// The function should search the system message-table resource(s) for the requested message. If this flag is specified with <see
			/// cref="FORMAT_MESSAGE_FROM_HMODULE"/>, the function searches the system message table if the message is not found in the module specified by
			/// lpSource. This flag cannot be used with <see cref="FORMAT_MESSAGE_FROM_STRING"/>.
			/// <para>
			/// If this flag is specified, an application can pass the result of the GetLastError function to retrieve the message text for a system-defined error.
			/// </para>
			/// </summary>
			FORMAT_MESSAGE_FROM_SYSTEM = 0x1000,

			/// <summary>
			/// Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged. This flag is useful for fetching
			/// a message for later formatting. If this flag is set, the Arguments parameter is ignored.
			/// </summary>
			FORMAT_MESSAGE_IGNORE_INSERTS = 0x200,

			/// <summary>
			/// The function ignores regular line breaks in the message definition text. The function stores hard-coded line breaks in the message definition
			/// text into the output buffer. The function generates no new line breaks.
			/// <para>
			/// Without this flag set: There are no output line width restrictions. The function stores line breaks that are in the message definition text into
			/// the output buffer. It specifies the maximum number of characters in an output line. The function ignores regular line breaks in the message
			/// definition text. The function never splits a string delimited by white space across a line break. The function stores hard-coded line breaks in
			/// the message definition text into the output buffer. Hard-coded line breaks are coded with the %n escape sequence.
			/// </para>
			/// </summary>
			FORMAT_MESSAGE_MAX_WIDTH_MASK = 0xff
		}

		/// <summary>Flags that may be passed to the <see cref="LoadLibraryEx"/> function.</summary>
		[Flags]
		public enum LoadLibraryExFlags
		{
			/// <summary>Define no flags, the function will behave as <see cref="LoadLibrary"/> does.</summary>
			None = 0,

			/// <summary>
			/// If this value is used, and the executable module is a DLL, the system does not call DllMain for process and thread initialization and
			/// termination. Also, the system does not load additional executable modules that are referenced by the specified module.
			/// </summary>
			/// <remarks>
			/// Do not use this value; it is provided only for backward compatibility. If you are planning to access only data or resources in the DLL, use <see
			/// cref="LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE"/> or <see cref="LOAD_LIBRARY_AS_IMAGE_RESOURCE"/> or both. Otherwise, load the library as a DLL or
			/// executable module using the LoadLibrary function.
			/// </remarks>
			DONT_RESOLVE_DLL_REFERENCES = 0x00000001,

			/// <summary>
			/// If this value is used, the system does not check AppLocker rules or apply Software Restriction Policies for the DLL. This action applies only to
			/// the DLL being loaded and not to its dependencies. This value is recommended for use in setup programs that must run extracted DLLs during installation.
			/// </summary>
			/// <remarks>
			/// <para>
			/// Windows Server 2008 R2 and Windows 7: On systems with KB2532445 installed, the caller must be running as "LocalSystem" or "TrustedInstaller";
			/// otherwise the system ignores this flag. For more information, see "You can circumvent AppLocker rules by using an Office macro on a computer that
			/// is running Windows 7 or Windows Server 2008 R2" in the Help and Support Knowledge Base at http://support.microsoft.com/kb/2532445.
			/// </para>
			/// <para>
			/// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP: AppLocker was introduced in Windows 7 and Windows Server 2008 R2.
			/// </para>
			/// </remarks>
			LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,

			/// <summary>
			/// If this value is used, the system maps the file into the calling process's virtual address space as if it were a data file. Nothing is done to
			/// execute or prepare to execute the mapped file. Therefore, you cannot call functions like GetModuleFileName, GetModuleHandle or GetProcAddress
			/// with this DLL. Using this value causes writes to read-only memory to raise an access violation. Use this flag when you want to load a DLL only to
			/// extract messages or resources from it.
			/// <para>This value can be used with <see cref="LOAD_LIBRARY_AS_IMAGE_RESOURCE"/>.</para>
			/// </summary>
			LOAD_LIBRARY_AS_DATAFILE = 0x00000002,

			/// <summary>
			/// Similar to <see cref="LOAD_LIBRARY_AS_DATAFILE"/>, except that the DLL file is opened with exclusive write access for the calling process. Other
			/// processes cannot open the DLL file for write access while it is in use. However, the DLL can still be opened by other processes.
			/// <para>This value can be used with <see cref="LOAD_LIBRARY_AS_IMAGE_RESOURCE"/>.</para>
			/// </summary>
			/// <remarks>Windows Server 2003 and Windows XP: This value is not supported until Windows Vista.</remarks>
			LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,

			/// <summary>
			/// If this value is used, the system maps the file into the process's virtual address space as an image file. However, the loader does not load the
			/// static imports or perform the other usual initialization steps. Use this flag when you want to load a DLL only to extract messages or resources
			/// from it.
			/// <para>
			/// Unless the application depends on the file having the in-memory layout of an image, this value should be used with either <see
			/// cref="LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE"/> or <see cref="LOAD_LIBRARY_AS_DATAFILE"/>. For more information, see the Remarks section.
			/// </para>
			/// </summary>
			LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,

			/// <summary>
			/// If this value is used, the application's installation directory is searched for the DLL and its dependencies. Directories in the standard search
			/// path are not searched. This value cannot be combined with <see cref="LOAD_WITH_ALTERED_SEARCH_PATH"/>.
			/// </summary>
			/// <remarks>
			/// Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: This value requires KB2533623 to be installed.
			/// <para>Windows Server 2003 and Windows XP: This value is not supported.</para>
			/// </remarks>
			LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,

			/// <summary>
			/// This value is a combination of <see cref="LOAD_LIBRARY_SEARCH_APPLICATION_DIR"/>, <see cref="LOAD_LIBRARY_SEARCH_SYSTEM32"/>, and <see
			/// cref="LOAD_LIBRARY_SEARCH_USER_DIRS"/>. Directories in the standard search path are not searched. This value cannot be combined with <see cref="LOAD_WITH_ALTERED_SEARCH_PATH"/>.
			/// <para>This value represents the recommended maximum number of directories an application should include in its DLL search path.</para>
			/// </summary>
			/// <remarks>
			/// Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: This value requires KB2533623 to be installed.
			/// <para>Windows Server 2003 and Windows XP: This value is not supported.</para>
			/// </remarks>
			LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,

			/// <summary>
			/// If this value is used, the directory that contains the DLL is temporarily added to the beginning of the list of directories that are searched for
			/// the DLL's dependencies. Directories in the standard search path are not searched.
			/// <para>The lpFileName parameter must specify a fully qualified path. This value cannot be combined with <see cref="LOAD_WITH_ALTERED_SEARCH_PATH"/>.</para>
			/// <para>
			/// For example, if Lib2.dll is a dependency of C:\Dir1\Lib1.dll, loading Lib1.dll with this value causes the system to search for Lib2.dll only in
			/// C:\Dir1. To search for Lib2.dll in C:\Dir1 and all of the directories in the DLL search path, combine this value with <see cref="LOAD_LIBRARY_SEARCH_DEFAULT_DIRS"/>.
			/// </para>
			/// </summary>
			/// <remarks>
			/// Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: This value requires KB2533623 to be installed.
			/// <para>Windows Server 2003 and Windows XP: This value is not supported.</para>
			/// </remarks>
			LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,

			/// <summary>
			/// If this value is used, %windows%\system32 is searched for the DLL and its dependencies. Directories in the standard search path are not searched.
			/// This value cannot be combined with <see cref="LOAD_WITH_ALTERED_SEARCH_PATH"/>.
			/// </summary>
			/// <remarks>
			/// Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: This value requires KB2533623 to be installed.
			/// <para>Windows Server 2003 and Windows XP: This value is not supported.</para>
			/// </remarks>
			LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,

			/// <summary>
			/// If this value is used, directories added using the AddDllDirectory or the SetDllDirectory function are searched for the DLL and its dependencies.
			/// If more than one directory has been added, the order in which the directories are searched is unspecified. Directories in the standard search
			/// path are not searched. This value cannot be combined with <see cref="LOAD_WITH_ALTERED_SEARCH_PATH"/>.
			/// </summary>
			/// <remarks>
			/// Windows 7, Windows Server 2008 R2, Windows Vista, and Windows Server 2008: This value requires KB2533623 to be installed.
			/// <para>Windows Server 2003 and Windows XP: This value is not supported.</para>
			/// </remarks>
			LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,

			/// <summary>
			/// If this value is used and lpFileName specifies an absolute path, the system uses the alternate file search strategy discussed in the Remarks
			/// section to find associated executable modules that the specified module causes to be loaded. If this value is used and lpFileName specifies a
			/// relative path, the behavior is undefined.
			/// <para>
			/// If this value is not used, or if lpFileName does not specify a path, the system uses the standard search strategy discussed in the Remarks
			/// section to find associated executable modules that the specified module causes to be loaded.
			/// </para>
			/// <para>This value cannot be combined with any LOAD_LIBRARY_SEARCH flag.</para>
			/// </summary>
			LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
		}

		/// <summary>Closes an open object handle.</summary>
		/// <param name="hObject">A valid handle to an open object.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.
		/// </returns>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport(nameof(Kernel32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		/// <summary>
		/// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe. The function returns a handle that can be used to access the file or device for various types of I/O depending on the file or device and the flags and attributes specified.
		/// </summary>
		/// <param name="lpFileName">The name of the file or device to be created or opened. You may use either forward slashes (/) or backslashes (\) in this name.
		/// <para>In the ANSI version of this function, the name is limited to MAX_PATH characters. To extend this limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path. For more information, see Naming Files, Paths, and Namespaces.</para>
		/// <para>For information on special device names, see Defining an MS-DOS Device Name.</para>
		/// <para>To create a file stream, specify the name of the file, a colon, and then the name of the stream. For more information, see File Streams.</para>
		/// <note type="tip">Starting with Windows 10, version 1607, for the unicode version of this function (CreateFileW), you can opt-in to remove the MAX_PATH limitation without prepending "\\?\". See the "Maximum Path Length Limitation" section of Naming Files, Paths, and Namespaces for details.</note></param>
		/// <param name="dwDesiredAccess">The requested access to the file or device, which can be summarized as read, write, both or neither zero).
		/// <para>The most commonly used values are GENERIC_READ, GENERIC_WRITE, or both (GENERIC_READ | GENERIC_WRITE). For more information, see Generic Access Rights, File Security and Access Rights, File Access Rights Constants, and ACCESS_MASK.</para>
		/// <para>If this parameter is zero, the application can query certain metadata such as file, directory, or device attributes without accessing that file or device, even if GENERIC_READ access would have been denied.</para>
		/// <para>You cannot request an access mode that conflicts with the sharing mode that is specified by the dwShareMode parameter in an open request that already has an open handle.</para></param>
		/// <param name="dwShareMode">The requested sharing mode of the file or device, which can be read, write, both, delete, all of these, or none (refer to the following table). Access requests to attributes or extended attributes are not affected by this flag.
		/// <para>If this parameter is zero and CreateFile succeeds, the file or device cannot be shared and cannot be opened again until the handle to the file or device is closed. For more information, see the Remarks section.</para>
		/// <para>You cannot request a sharing mode that conflicts with the access mode that is specified in an existing request that has an open handle. CreateFile would fail and the GetLastError function would return ERROR_SHARING_VIOLATION.</para>
		/// <para>To enable a process to share a file or device while another process has the file or device open, use a compatible combination of one or more of the following values. For more information about valid combinations of this parameter with the dwDesiredAccess parameter, see Creating and Opening Files.</para>
		/// <note>The sharing options for each open handle remain in effect until that handle is closed, regardless of process context.</note></param>
		/// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes.
		/// <para>This parameter can be NULL.</para>
		/// <para>If this parameter is NULL, the handle returned by CreateFile cannot be inherited by any child processes the application may create and the file or device associated with the returned handle gets a default security descriptor.</para>
		/// <para>The lpSecurityDescriptor member of the structure specifies a SECURITY_DESCRIPTOR for a file or device. If this member is NULL, the file or device associated with the returned handle is assigned a default security descriptor.</para>
		/// <para>CreateFile ignores the lpSecurityDescriptor member when opening an existing file or device, but continues to use the bInheritHandle member.</para>
		/// <para>The bInheritHandlemember of the structure specifies whether the returned handle can be inherited.</para></param>
		/// <param name="dwCreationDisposition">An action to take on a file or device that exists or does not exist.
		/// <para>For devices other than files, this parameter is usually set to OPEN_EXISTING.</para></param>
		/// <param name="dwFlagsAndAttributes">The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files.
		/// <para>This parameter can include any combination of the available file attributes (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL.</para>
		/// <para>This parameter can also contain combinations of flags (FILE_FLAG_*) for control of file or device caching behavior, access modes, and other special-purpose flags. These combine with any FILE_ATTRIBUTE_* values.</para>
		/// <para>This parameter can also contain Security Quality of Service (SQOS) information by specifying the SECURITY_SQOS_PRESENT flag. Additional SQOS-related flags information is presented in the table following the attributes and flags tables.</para>
		/// <note>When CreateFile opens an existing file, it generally combines the file flags with the file attributes of the existing file, and ignores any file attributes supplied as part of dwFlagsAndAttributes. Special cases are detailed in Creating and Opening Files.</note>
		/// <para>Some of the following file attributes and flags may only apply to files and not necessarily all other types of devices that CreateFile can open. For additional information, see the Remarks section of this topic and Creating and Opening Files.</para>
		/// <para>For more advanced access to file attributes, see SetFileAttributes. For a complete list of all file attributes with their values and descriptions, see File Attribute Constants.</para></param>
		/// <param name="hTemplateFile">A valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created.
		/// <para>This parameter can be NULL.</para>
		/// <para>When opening an existing file, CreateFile ignores this parameter.</para>
		/// <para>When opening a new encrypted file, the file inherits the discretionary access control list from its parent directory. For additional information, see File Encryption.</para></param>
		/// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.
		/// <para>If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</para></returns>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode,
			SECURITY_ATTRIBUTES lpSecurityAttributes, FileMode dwCreationDisposition, FileFlagsAndAttributes dwFlagsAndAttributes,
			SafeFileHandle hTemplateFile);

		/// <summary>
		/// Enumerates resources of a specified type within a binary module. For Windows Vista and later, this is typically a language-neutral Portable
		/// Executable (LN file), and the enumeration will also include resources from the corresponding language-specific resource files (.mui files) that
		/// contain localizable language resources. It is also possible for hModule to specify an .mui file, in which case only that file is searched for resources.
		/// </summary>
		/// <param name="hModule">
		/// A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate .mui files (if any exist) are included in
		/// the search.
		/// <para>If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the current process.</para>
		/// </param>
		/// <param name="lpszType">
		/// The type of the resource for which the name is being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		/// ID is an integer value representing a predefined resource type.
		/// </param>
		/// <param name="lpEnumFunc">A pointer to the callback function to be called for each enumerated resource name or ID.</param>
		/// <param name="lParam">An application-defined value passed to the callback function. This parameter can be used in error checking.</param>
		/// <returns>
		/// The return value is TRUE if the function succeeds or FALSE if the function does not find a resource of the type specified, or if the function fails
		/// for another reason. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumResourceNames(SafeLibraryHandle hModule, string lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

		/// <summary>
		/// Enumerates resources of a specified type within a binary module. For Windows Vista and later, this is typically a language-neutral Portable
		/// Executable (LN file), and the enumeration will also include resources from the corresponding language-specific resource files (.mui files) that
		/// contain localizable language resources. It is also possible for hModule to specify an .mui file, in which case only that file is searched for resources.
		/// </summary>
		/// <param name="hModule">
		/// A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate .mui files (if any exist) are included in
		/// the search.
		/// <para>If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the current process.</para>
		/// </param>
		/// <param name="type">
		/// The type of the resource for which the name is being enumerated. Alternately, rather than a string, this parameter can be MAKEINTRESOURCE(ID), where
		/// ID is an integer value representing a predefined resource type.
		/// </param>
		/// <returns>A list of strings for each of the resources matching <paramref name="type"/>.</returns>
		public static IList<ResourceName> EnumResourceNames(SafeLibraryHandle hModule, ResourceName type)
		{
			var list = new ResList();
			EnumResNameProcManaged ep = (h, t, n, p) => { p.L.Add(new ResourceName(n)); return true; };
			if (!EnumResourceNames(hModule, type, ep, list))
				throw new Win32Exception();
			return list.L;
		}

		/// <summary>
		/// Converts a file time to system time format. System time is based on Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="lpFileTime">A pointer to a FILETIME structure containing the file time to be converted to system (UTC) date and time format. This value must be less than 0x8000000000000000. Otherwise, the function fails.</param>
		/// <param name="lpSystemTime">A pointer to a SYSTEMTIME structure to receive the converted file time.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Kernel32), SetLastError = true), SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FileTimeToSystemTime(ref FILETIME lpFileTime, ref SYSTEMTIME lpSystemTime);

		/// <summary>
		/// Determines the location of a resource with the specified type and name in the specified module.
		/// <para>To specify a language, use the FindResourceEx function.</para>
		/// </summary>
		/// <param name="hModule">
		/// A handle to the module whose portable executable file or an accompanying MUI file contains the resource. If this parameter is Null, the function
		/// searches the module used to create the current process.
		/// </param>
		/// <param name="lpName">
		/// The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE, where wInteger is the integer identifier of the resource.
		/// </param>
		/// <param name="lpType">
		/// The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE, where wInteger is the integer identifier of the given
		/// resource type.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the specified resource's information block. To obtain a handle to the resource, pass this
		/// handle to the <see cref="LoadResource"/> function.
		/// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr FindResource(SafeLibraryHandle hModule, ResourceName lpName, ResourceName lpType);

		/// <summary>
		/// Formats a message string. The function requires a message definition as input. The message definition can come from a buffer passed into the
		/// function. It can come from a message table resource in an already-loaded module. Or the caller can ask the function to search the system's message
		/// table resource(s) for the message definition. The function finds the message definition in a message table resource based on a message identifier and
		/// a language identifier. The function copies the formatted message text to an output buffer, processing any embedded insert sequences if requested.
		/// </summary>
		/// <param name="dwFlags">
		/// The formatting options, and how to interpret the lpSource parameter. The low-order byte of dwFlags specifies how the function handles line breaks in
		/// the output buffer. The low-order byte can also specify the maximum width of a formatted output line.
		/// </param>
		/// <param name="lpSource">
		/// The location of the message definition. The type of this parameter depends upon the settings in the <paramref name="dwFlags"/> parameter. If <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_HMODULE"/>: A handle to the module that contains the message table to search. If <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>: Pointer to a string that consists of unformatted message text. It will be scanned for inserts
		/// and formatted accordingly. If neither of these flags is set in dwFlags, then lpSource is ignored.
		/// </param>
		/// <param name="dwMessageId">The message identifier for the requested message. This parameter is ignored if dwFlags includes <see cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>.</param>
		/// <param name="dwLanguageId">
		/// The language identifier for the requested message. This parameter is ignored if dwFlags includes <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>. If you pass a specific LANGID in this parameter, FormatMessage will return a message for that
		/// LANGID only. If the function cannot find a message for that LANGID, it sets Last-Error to ERROR_RESOURCE_LANG_NOT_FOUND. If you pass in zero,
		/// FormatMessage looks for a message for LANGIDs in the following order: Language neutral Thread LANGID, based on the thread's locale value User default
		/// LANGID, based on the user's default locale value System default LANGID, based on the system default locale value US English If FormatMessage does not
		/// locate a message for any of the preceding LANGIDs, it returns any language message string that is present. If that fails, it returns ERROR_RESOURCE_LANG_NOT_FOUND.
		/// </param>
		/// <param name="lpBuffer">
		/// A pointer to a buffer that receives the null-terminated string that specifies the formatted message. If dwFlags includes <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/>, the function allocates a buffer using the LocalAlloc function, and places the pointer to
		/// the buffer at the address specified in lpBuffer. This buffer cannot be larger than 64K bytes.
		/// </param>
		/// <param name="nSize">
		/// If the <see cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/> flag is not set, this parameter specifies the size of the output buffer, in
		/// TCHARs. If <see cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/> is set, this parameter specifies the minimum number of TCHARs to allocate
		/// for an output buffer. The output buffer cannot be larger than 64K bytes.
		/// </param>
		/// <param name="arguments">
		/// An array of values that are used as insert values in the formatted message. A %1 in the format string indicates the first value in the Arguments
		/// array; a %2 indicates the second argument; and so on. The interpretation of each value depends on the formatting information associated with the
		/// insert in the message definition.The default is to treat each value as a pointer to a null-terminated string. By default, the Arguments parameter is
		/// of type va_list*, which is a language- and implementation-specific data type for describing a variable number of arguments.The state of the va_list
		/// argument is undefined upon return from the function.To use the va_list again, destroy the variable argument list pointer using va_end and
		/// reinitialize it with va_start. If you do not have a pointer of type va_list*, then specify the FORMAT_MESSAGE_ARGUMENT_ARRAY flag and pass a pointer
		/// to an array of DWORD_PTR values; those values are input to the message formatted as the insert values.Each insert must have a corresponding element
		/// in the array.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is the number of TCHARs stored in the output buffer, excluding the terminating null character. If the
		/// function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(FormatMessageFlags dwFlags, SafeLibraryHandle lpSource, int dwMessageId, int dwLanguageId, [Out] IntPtr lpBuffer,
			int nSize, string[] arguments);

		/// <summary>
		/// Formats a message string. The function requires a message definition as input. The message definition can come from a buffer passed into the
		/// function. It can come from a message table resource in an already-loaded module. Or the caller can ask the function to search the system's message
		/// table resource(s) for the message definition. The function finds the message definition in a message table resource based on a message identifier and
		/// a language identifier. The function copies the formatted message text to an output buffer, processing any embedded insert sequences if requested.
		/// </summary>
		/// <param name="dwFlags">
		/// The formatting options, and how to interpret the lpSource parameter. The low-order byte of dwFlags specifies how the function handles line breaks in
		/// the output buffer. The low-order byte can also specify the maximum width of a formatted output line.
		/// </param>
		/// <param name="lpSource">
		/// The location of the message definition. The type of this parameter depends upon the settings in the <paramref name="dwFlags"/> parameter. If <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_HMODULE"/>: A handle to the module that contains the message table to search. If <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>: Pointer to a string that consists of unformatted message text. It will be scanned for inserts
		/// and formatted accordingly. If neither of these flags is set in dwFlags, then lpSource is ignored.
		/// </param>
		/// <param name="dwMessageId">The message identifier for the requested message. This parameter is ignored if dwFlags includes <see cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>.</param>
		/// <param name="dwLanguageId">
		/// The language identifier for the requested message. This parameter is ignored if dwFlags includes <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING"/>. If you pass a specific LANGID in this parameter, FormatMessage will return a message for that
		/// LANGID only. If the function cannot find a message for that LANGID, it sets Last-Error to ERROR_RESOURCE_LANG_NOT_FOUND. If you pass in zero,
		/// FormatMessage looks for a message for LANGIDs in the following order: Language neutral Thread LANGID, based on the thread's locale value User default
		/// LANGID, based on the user's default locale value System default LANGID, based on the system default locale value US English If FormatMessage does not
		/// locate a message for any of the preceding LANGIDs, it returns any language message string that is present. If that fails, it returns ERROR_RESOURCE_LANG_NOT_FOUND.
		/// </param>
		/// <param name="lpBuffer">
		/// A pointer to a buffer that receives the null-terminated string that specifies the formatted message. If dwFlags includes <see
		/// cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/>, the function allocates a buffer using the LocalAlloc function, and places the pointer to
		/// the buffer at the address specified in lpBuffer. This buffer cannot be larger than 64K bytes.
		/// </param>
		/// <param name="nSize">
		/// If the <see cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/> flag is not set, this parameter specifies the size of the output buffer, in
		/// TCHARs. If <see cref="FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER"/> is set, this parameter specifies the minimum number of TCHARs to allocate
		/// for an output buffer. The output buffer cannot be larger than 64K bytes.
		/// </param>
		/// <param name="arguments">
		/// An array of values that are used as insert values in the formatted message. A %1 in the format string indicates the first value in the Arguments
		/// array; a %2 indicates the second argument; and so on. The interpretation of each value depends on the formatting information associated with the
		/// insert in the message definition.The default is to treat each value as a pointer to a null-terminated string. By default, the Arguments parameter is
		/// of type va_list*, which is a language- and implementation-specific data type for describing a variable number of arguments.The state of the va_list
		/// argument is undefined upon return from the function.To use the va_list again, destroy the variable argument list pointer using va_end and
		/// reinitialize it with va_start. If you do not have a pointer of type va_list*, then specify the FORMAT_MESSAGE_ARGUMENT_ARRAY flag and pass a pointer
		/// to an array of DWORD_PTR values; those values are input to the message formatted as the insert values.Each insert must have a corresponding element
		/// in the array.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is the number of TCHARs stored in the output buffer, excluding the terminating null character. If the
		/// function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, [Out] IntPtr lpBuffer,
			int nSize, IntPtr arguments);

		/// <summary>
		/// Formats a message string. The function requires a message definition as input. The message definition can come from a message table resource in an
		/// already-loaded module. Or the caller can ask the function to search the system's message table resource(s) for the message definition. The function
		/// finds the message definition in a message table resource based on a message identifier and a language identifier. The function returns the formatted
		/// message text, processing any embedded insert sequences if requested.
		/// </summary>
		/// <param name="id">The message identifier for the requested message.</param>
		/// <param name="args">
		/// An array of values that are used as insert values in the formatted message. A %1 in the format string indicates the first value in the Arguments
		/// array; a %2 indicates the second argument; and so on. The interpretation of each value depends on the formatting information associated with the
		/// insert in the message definition. Each insert must have a corresponding element in the array.
		/// </param>
		/// <param name="hLib">A handle to the module that contains the message table to search.</param>
		/// <param name="flags">
		/// The formatting options, and how to interpret the lpSource parameter. The low-order byte of dwFlags specifies how the function handles line breaks in
		/// the output buffer. The low-order byte can also specify the maximum width of a formatted output line.
		/// </param>
		/// <param name="langId">
		/// The language identifier for the requested message. If you pass a specific LANGID in this parameter, FormatMessage will return a message for that
		/// LANGID only. If the function cannot find a message for that LANGID, it sets Last-Error to ERROR_RESOURCE_LANG_NOT_FOUND. If you pass in zero,
		/// FormatMessage looks for a message for LANGIDs in the following order: Language neutral Thread LANGID, based on the thread's locale value User default
		/// LANGID, based on the user's default locale value System default LANGID, based on the system default locale value US English If FormatMessage does not
		/// locate a message for any of the preceding LANGIDs, it returns any language message string that is present. If that fails, it returns ERROR_RESOURCE_LANG_NOT_FOUND.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is the string that specifies the formatted message. To get extended error information, call GetLastError.
		/// </returns>
		public static string FormatMessage(int id, string[] args, SafeLibraryHandle hLib = null, FormatMessageFlags flags = 0, int langId = 0)
		{
			flags &= ~FormatMessageFlags.FORMAT_MESSAGE_FROM_STRING;
			flags |= FormatMessageFlags.FORMAT_MESSAGE_ALLOCATE_BUFFER | FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM;
			if (hLib != null) flags |= FormatMessageFlags.FORMAT_MESSAGE_FROM_HMODULE;
			if (args != null && args.Length > 0) flags |= FormatMessageFlags.FORMAT_MESSAGE_ARGUMENT_ARRAY;
			var mem = SafeHGlobalHandle.Null;
			var ret = FormatMessage(flags, hLib ?? SafeLibraryHandle.Null, id, langId, (IntPtr)mem, 1024, args);
			if (ret == 0) throw new Win32Exception();
			return mem.ToString(-1);
		}

		/// <summary>
		/// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the
		/// module is unloaded from the address space of the calling process and the handle is no longer valid.
		/// </summary>
		/// <param name="hModule">
		/// A handle to the loaded library module. The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a nonzero value.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		/// <summary>
		/// Retrieves the actual number of bytes of disk storage used to store a specified file. If the file is located on a volume that supports compression and
		/// the file is compressed, the value obtained is the compressed size of the specified file. If the file is located on a volume that supports sparse
		/// files and the file is a sparse file, the value obtained is the sparse size of the specified file.
		/// </summary>
		/// <param name="lpFileName">
		/// The name of the file.
		/// <para>Do not specify the name of a file on a nonseeking device, such as a pipe or a communications device, as its file size has no meaning.</para>
		/// <para>
		/// This parameter may include the path. In the ANSI version of this function, the name is limited to <see cref="MAX_PATH"/> characters. To extend this
		/// limit to 32,767 wide characters, call the Unicode version of the function and prepend "\\?\" to the path. For more information, see <a
		/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx">Naming a File</a>.
		/// </para>
		/// <para>
		/// <c>Tip</c> Starting with Windows 10, version 1607, for the unicode version of this function ( <c>GetCompressedFileSizeW</c>), you can opt-in to
		/// remove the <see cref="MAX_PATH"/> limitation without prepending "\\?\". See the "Maximum Path Length Limitation" section of <a
		/// href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247(v=vs.85).aspx">Naming Files, Paths, and Namespaces</a> for details.
		/// </para>
		/// </param>
		/// <param name="lpFileSizeHigh">
		/// The high-order DWORD of the compressed file size. The function's return value is the low-order DWORD of the compressed file size.
		/// <para>
		/// This parameter can be NULL if the high-order DWORD of the compressed file size is not needed.Files less than 4 gigabytes in size do not need the
		/// high-order DWORD.
		/// </para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is the low-order DWORD of the actual number of bytes of disk storage used to store the specified file, and
		/// if <paramref name="lpFileSizeHigh"/> is non-NULL, the function puts the high-order DWORD of that actual value into the DWORD pointed to by that
		/// parameter. This is the compressed file size for compressed files, the actual file size for noncompressed files.
		/// <para>
		/// If the function fails, and <paramref name="lpFileSizeHigh"/> is NULL, the return value is INVALID_FILE_SIZE. To get extended error information, call GetLastError.
		/// </para>
		/// <para>
		/// If the return value is INVALID_FILE_SIZE and <paramref name="lpFileSizeHigh"/> is non-NULL, an application must call GetLastError to determine
		/// whether the function has succeeded (value is NO_ERROR) or failed (value is other than NO_ERROR).
		/// </para>
		/// </returns>
		/// <remarks>
		/// An application can determine whether a volume is compressed by calling <see cref="GetVolumeInformation(string,System.Text.StringBuilder,int,ref int,ref int,ref FileSystemFlags,System.Text.StringBuilder,int)"/>, then checking the status of the
		/// FS_VOL_IS_COMPRESSED flag in the DWORD value pointed to by that function's lpFileSystemFlags parameter.
		/// <para>
		/// If the file is not located on a volume that supports compression or sparse files, or if the file is not compressed or a sparse file, the value
		/// obtained is the actual file size, the same as the value returned by a call to GetFileSize.
		/// </para>
		/// <para>Symbolic link behavior—If the path points to a symbolic link, the function returns the file size of the target.</para>
		/// </remarks>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetCompressedFileSize(string lpFileName, ref int lpFileSizeHigh);

		/// <summary>Retrieves a pseudo handle for the current process.</summary>
		/// <returns>The return value is a pseudo handle to the current process.</returns>
		/// <remarks>
		/// A pseudo handle is a special constant, currently (HANDLE)-1, that is interpreted as the current process handle. For compatibility with future
		/// operating systems, it is best to call GetCurrentProcess instead of hard-coding this constant value. The calling process can use a pseudo handle to
		/// specify its own process whenever a process handle is required. Pseudo handles are not inherited by child processes.
		/// <para>This handle has the PROCESS_ALL_ACCESS access right to the process object.</para>
		/// <para>
		/// Windows Server 2003 and Windows XP: This handle has the maximum access allowed by the security descriptor of the process to the primary token of the process.
		/// </para>
		/// <para>
		/// A process can create a "real" handle to itself that is valid in the context of other processes, or that can be inherited by other processes, by
		/// specifying the pseudo handle as the source handle in a call to the DuplicateHandle function. A process can also use the OpenProcess function to open
		/// a real handle to itself.
		/// </para>
		/// <para>
		/// The pseudo handle need not be closed when it is no longer needed. Calling the <see cref="CloseHandle"/> function with a pseudo handle has no
		/// effect.If the pseudo handle is duplicated by DuplicateHandle, the duplicate handle must be closed.
		/// </para>
		/// </remarks>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		[DllImport(nameof(Kernel32), SetLastError = true)]
		public static extern IntPtr GetCurrentThread();

		[DllImport(nameof(Kernel32), SetLastError = true)]
		public static extern uint GetCurrentThreadId();

		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetDiskFreeSpace(string lpRootPathName, out int lpSectorsPerCluster, out int lpBytesPerSector, out int lpNumberOfFreeClusters, out int lpTotalNumberOfClusters);

		[SecurityCritical, SuppressUnmanagedCodeSecurity, DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleFileName(SafeLibraryHandle hModule, [Out] StringBuilder buffer, int length);

		[SecurityCritical]
		public static string GetModuleFileName(SafeLibraryHandle hModule)
		{
			var buffer = new StringBuilder(MAX_PATH);
			Label_000B:
			var num1 = GetModuleFileName(hModule, buffer, buffer.Capacity);
			if (num1 == 0)
				throw new Win32Exception();
			if (num1 == buffer.Capacity && Marshal.GetLastWin32Error() == Win32Error.ERROR_INSUFFICIENT_BUFFER)
			{
				buffer.EnsureCapacity(buffer.Capacity * 2);
				goto Label_000B;
			}
			return buffer.ToString();
		}

		/// <summary>
		/// The GetProcAddress function retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
		/// </summary>
		/// <param name="hModule">Handle to the DLL module that contains the function or variable. The LoadLibrary or GetModuleHandle function returns this handle.</param>
		/// <param name="lpProcName">Pointer to a null-terminated string containing the function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
		/// <returns>If the function succeeds, the return value is the address of the exported function or variable.<br></br><br>If the function fails, the return value is NULL. To get extended error information, call Marshal.GetLastWin32Error.</br></returns>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern IntPtr GetProcAddress(SafeLibraryHandle hModule, string lpProcName);

		/// <summary>Retrieves information about the file system and volume associated with the specified root directory.</summary>
		/// <param name="lpRootPathName">
		/// A pointer to a string that contains the root directory of the volume to be described.
		/// <para>
		/// If this parameter is NULL, the root of the current directory is used. A trailing backslash is required. For example, you specify \\MyServer\MyShare
		/// as "\\MyServer\MyShare\", or the C drive as "C:\".
		/// </para>
		/// </param>
		/// <param name="lpVolumeNameBuffer">
		/// A pointer to a buffer that receives the name of a specified volume. The buffer size is specified by the <paramref name="nVolumeNameSize"/> parameter.
		/// </param>
		/// <param name="nVolumeNameSize">
		/// The length of a volume name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1.
		/// <para>This parameter is ignored if the volume name buffer is not supplied.</para>
		/// </param>
		/// <param name="lpVolumeSerialNumber">
		/// A pointer to a variable that receives the volume serial number.
		/// <para>This parameter can be NULL if the serial number is not required.</para>
		/// <para>
		/// This function returns the volume serial number that the operating system assigns when a hard disk is formatted. To programmatically obtain the hard
		/// disk's serial number that the manufacturer assigns, use the Windows Management Instrumentation (WMI) Win32_PhysicalMedia property SerialNumber.
		/// </para>
		/// </param>
		/// <param name="lpMaximumComponentLength">
		/// A pointer to a variable that receives the maximum length, in TCHARs, of a file name component that a specified file system supports.
		/// <para>A file name component is the portion of a file name between backslashes.</para>
		/// <para>
		/// The value that is stored in the variable that <paramref name="lpMaximumComponentLength"/> points to is used to indicate that a specified file system
		/// supports long names. For example, for a FAT file system that supports long names, the function stores the value 255, rather than the previous 8.3
		/// indicator. Long names can also be supported on systems that use the NTFS file system.
		/// </para>
		/// </param>
		/// <param name="lpFileSystemFlags">
		/// A pointer to a variable that receives flags associated with the specified file system.
		/// <para>
		/// This parameter can be one or more of the <see cref="FileSystemFlags"/> values. However, FILE_FILE_COMPRESSION and FILE_VOL_IS_COMPRESSED are mutually exclusive.
		/// </para>
		/// </param>
		/// <param name="lpFileSystemNameBuffer">
		/// A pointer to a buffer that receives the name of the file system, for example, the FAT file system or the NTFS file system. The buffer size is
		/// specified by the <paramref name="nFileSystemNameSize"/> parameter.
		/// </param>
		/// <param name="nFileSystemNameSize">
		/// The length of the file system name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1.
		/// <para>This parameter is ignored if the file system name buffer is not supplied.</para>
		/// </param>
		/// <returns>
		/// If all the requested information is retrieved, the return value is nonzero.
		/// <para>If not all the requested information is retrieved, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>
		/// When a user attempts to get information about a floppy drive that does not have a floppy disk, or a CD-ROM drive that does not have a compact disc,
		/// the system displays a message box for the user to insert a floppy disk or a compact disc, respectively. To prevent the system from displaying this
		/// message box, call the SetErrorMode function with SEM_FAILCRITICALERRORS.
		/// <para>
		/// The FILE_VOL_IS_COMPRESSED flag is the only indicator of volume-based compression. The file system name is not altered to indicate compression, for
		/// example, this flag is returned set on a DoubleSpace volume. When compression is volume-based, an entire volume is compressed or not compressed.
		/// </para>
		/// <para>
		/// The FILE_FILE_COMPRESSION flag indicates whether a file system supports file-based compression. When compression is file-based, individual files can
		/// be compressed or not compressed.
		/// </para>
		/// <para>The FILE_FILE_COMPRESSION and FILE_VOL_IS_COMPRESSED flags are mutually exclusive. Both bits cannot be returned set.</para>
		/// <para>
		/// The maximum component length value that is stored in lpMaximumComponentLength is the only indicator that a volume supports longer-than-normal FAT
		/// file system (or other file system) file names. The file system name is not altered to indicate support for long file names.
		/// </para>
		/// <para>
		/// The GetCompressedFileSize function obtains the compressed size of a file. The GetFileAttributes function can determine whether an individual file is compressed.
		/// </para>
		/// <para>Symbolic link behavior—</para>
		/// <para>If the path points to a symbolic link, the function returns volume information for the target.</para>
		/// </remarks>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetVolumeInformation(string lpRootPathName, StringBuilder lpVolumeNameBuffer, int nVolumeNameSize, ref int lpVolumeSerialNumber, ref int lpMaximumComponentLength, ref FileSystemFlags lpFileSystemFlags, StringBuilder lpFileSystemNameBuffer, int nFileSystemNameSize);

		/// <summary>Retrieves information about the file system and volume associated with the specified root directory.</summary>
		/// <param name="rootPathName">
		/// A string that contains the root directory of the volume to be described.
		/// <para>
		/// If this parameter is NULL, the root of the current directory is used. A trailing backslash is required. For example, you specify \\MyServer\MyShare
		/// as "\\MyServer\MyShare\", or the C drive as "C:\".
		/// </para>
		/// </param>
		/// <param name="volumeName">Receives the name of a specified volume.</param>
		/// <param name="volumeSerialNumber">
		/// Receives the volume serial number.
		/// <para>
		/// This function returns the volume serial number that the operating system assigns when a hard disk is formatted. To programmatically obtain the hard
		/// disk's serial number that the manufacturer assigns, use the Windows Management Instrumentation (WMI) Win32_PhysicalMedia property SerialNumber.
		/// </para>
		/// </param>
		/// <param name="maximumComponentLength">
		/// Receives the maximum length, in characters, of a file name component that a specified file system supports.
		/// <para>A file name component is the portion of a file name between backslashes.</para>
		/// <para>
		/// The value that is stored in the variable that <paramref name="maximumComponentLength"/> returns is used to indicate that a specified file system
		/// supports long names. For example, for a FAT file system that supports long names, the function stores the value 255, rather than the previous 8.3
		/// indicator. Long names can also be supported on systems that use the NTFS file system.
		/// </para>
		/// </param>
		/// <param name="fileSystemFlags">
		/// Receives the flags associated with the specified file system.
		/// <para>
		/// This parameter can be one or more of the <see cref="FileSystemFlags"/> values. However, FILE_FILE_COMPRESSION and FILE_VOL_IS_COMPRESSED are mutually exclusive.
		/// </para>
		/// </param>
		/// <param name="fileSystemName">Receives the name of the file system, for example, the FAT file system or the NTFS file system.</param>
		/// <returns>
		/// If all the requested information is retrieved, the return value is nonzero.
		/// <para>If not all the requested information is retrieved, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>
		/// When a user attempts to get information about a floppy drive that does not have a floppy disk, or a CD-ROM drive that does not have a compact disc,
		/// the system displays a message box for the user to insert a floppy disk or a compact disc, respectively. To prevent the system from displaying this
		/// message box, call the SetErrorMode function with SEM_FAILCRITICALERRORS.
		/// <para>
		/// The FILE_VOL_IS_COMPRESSED flag is the only indicator of volume-based compression. The file system name is not altered to indicate compression, for
		/// example, this flag is returned set on a DoubleSpace volume. When compression is volume-based, an entire volume is compressed or not compressed.
		/// </para>
		/// <para>
		/// The FILE_FILE_COMPRESSION flag indicates whether a file system supports file-based compression. When compression is file-based, individual files can
		/// be compressed or not compressed.
		/// </para>
		/// <para>The FILE_FILE_COMPRESSION and FILE_VOL_IS_COMPRESSED flags are mutually exclusive. Both bits cannot be returned set.</para>
		/// <para>
		/// The maximum component length value that is stored in lpMaximumComponentLength is the only indicator that a volume supports longer-than-normal FAT
		/// file system (or other file system) file names. The file system name is not altered to indicate support for long file names.
		/// </para>
		/// <para>
		/// The GetCompressedFileSize function obtains the compressed size of a file. The GetFileAttributes function can determine whether an individual file is compressed.
		/// </para>
		/// <para>Symbolic link behavior—</para>
		/// <para>If the path points to a symbolic link, the function returns volume information for the target.</para>
		/// </remarks>
		public static bool GetVolumeInformation(string rootPathName, out string volumeName, out int volumeSerialNumber,
			out int maximumComponentLength, out FileSystemFlags fileSystemFlags, out string fileSystemName)
		{
			var sb1 = new StringBuilder(MAX_PATH + 1);
			var sn = 0;
			var cl = 0;
			FileSystemFlags flags = 0;
			var sb2 = new StringBuilder(MAX_PATH + 1);
			var ret = GetVolumeInformation(rootPathName, sb1, sb1.Capacity, ref sn, ref cl, ref flags, sb2, sb2.Capacity);
			volumeName = sb1.ToString();
			volumeSerialNumber = sn;
			maximumComponentLength = cl;
			fileSystemFlags = flags;
			fileSystemName = sb2.ToString();
			return ret;
		}

		/// <summary>
		/// The GlobalLock function locks a global memory object and returns a pointer to the first byte of the object's memory block. GlobalLock function
		/// increments the lock count by one. Needed for the clipboard functions when getting the data from IDataObject
		/// </summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		/// <summary>The GlobalUnlock function decrements the lock count associated with a memory object.</summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GlobalUnlock(IntPtr hMem);

		public static bool IsIntResource(IntPtr ptr) => ptr.ToInt64() >> 16 == 0;

		/// <summary>
		/// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
		/// <para>For additional load options, use the LoadLibraryEx function.</para>
		/// </summary>
		/// <param name="lpFileName">
		/// The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name
		/// of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file.
		/// <para>If the string specifies a full path, the function searches only that path for the module.</para>
		/// <para>
		/// If the string specifies a relative path or a module name without a path, the function uses a standard search strategy to find the module.
		/// </para>
		/// <para>
		/// If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (\), not forward slashes (/).
		/// </para>
		/// <para>
		/// If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll
		/// to the module name. To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.
		/// </para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the loaded module.
		/// <para>If the function fails, the return value is an invalid handle. To get extended error information, call GetLastError.</para>
		/// </returns>
		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		public static extern IntPtr LoadLibrary([In, MarshalAs(UnmanagedType.LPTStr)] string lpFileName);

		/// <summary>Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.</summary>
		/// <param name="lpFileName">
		/// <para>
		/// A string that specifies the file name of the module to load. This name is not related to the name stored in a library module itself, as specified by
		/// the LIBRARY keyword in the module-definition (.def) file.
		/// </para>
		/// <para>
		/// The module can be a library module (a .dll file) or an executable module (an .exe file). If the specified module is an executable module, static
		/// imports are not loaded; instead, the module is loaded as if <see cref="LoadLibraryExFlags.DONT_RESOLVE_DLL_REFERENCES"/> was specified. See the
		/// <paramref name="dwFlags"/> parameter for more information.
		/// </para>
		/// <para>
		/// If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll
		/// to the module name. To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.
		/// </para>
		/// <para>
		/// If the string specifies a fully qualified path, the function searches only that path for the module. When specifying a path, be sure to use
		/// backslashes (\), not forward slashes (/). For more information about paths, see Naming Files, Paths, and Namespaces.
		/// </para>
		/// <para>
		/// If the string specifies a module name without a path and more than one loaded module has the same base name and extension, the function returns a
		/// handle to the module that was loaded first.
		/// </para>
		/// <para>
		/// If the string specifies a module name without a path and a module of the same name is not already loaded, or if the string specifies a module name
		/// with a relative path, the function searches for the specified module. The function also searches for modules if loading the specified module causes
		/// the system to load other associated modules (that is, if the module has dependencies). The directories that are searched and the order in which they
		/// are searched depend on the specified path and the dwFlags parameter.
		/// </para>
		/// <para>If the function cannot find the module or one of its dependencies, the function fails.</para>
		/// </param>
		/// <param name="hFile">This parameter is reserved for future use. It must be <see langword="null"/>.</param>
		/// <param name="dwFlags">
		/// The action to be taken when loading the module. If <see cref="LoadLibraryExFlags.None"/> is specified, the behavior of this function is identical to
		/// that of the LoadLibrary function.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the loaded module.
		/// <para>If the function fails, the return value is an invalid handle. To get extended error information, call GetLastError.</para>
		/// </returns>
		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LoadLibraryEx([In, MarshalAs(UnmanagedType.LPTStr)] string lpFileName, IntPtr hFile, LoadLibraryExFlags dwFlags);

		/// <summary>Retrieves a handle that can be used to obtain a pointer to the first byte of the specified resource in memory.</summary>
		/// <param name="hModule">
		/// A handle to the module whose executable file contains the resource. If hModule is <see cref="SafeLibraryHandle.Null"/>, the system loads the resource
		/// from the module that was used to create the current process.
		/// </param>
		/// <param name="hResInfo">A handle to the resource to be loaded. This handle is returned by the FindResource or FindResourceEx function.</param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the data associated with the resource.
		/// <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LoadResource(SafeLibraryHandle hModule, IntPtr hResInfo);

		/// <summary>Retrieves a pointer to the specified resource in memory.</summary>
		/// <param name="hResData">A handle to the resource to be accessed. The <see cref="LoadResource"/> function returns this handle.</param>
		/// <returns>If the loaded resource is available, the return value is a pointer to the first byte of the resource; otherwise, it is NULL.</returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern IntPtr LockResource(IntPtr hResData);

		/// <summary>
		/// Retrieves information about MS-DOS device names. The function can obtain the current mapping for a particular MS-DOS device name. The function can
		/// also obtain a list of all existing MS-DOS device names.
		/// <para>
		/// MS-DOS device names are stored as junctions in the object namespace. The code that converts an MS-DOS path into a corresponding path uses these
		/// junctions to map MS-DOS devices and drive letters. The QueryDosDevice function enables an application to query the names of the junctions used to
		/// implement the MS-DOS device namespace as well as the value of each specific junction.
		/// </para>
		/// </summary>
		/// <param name="lpDeviceName">
		/// An MS-DOS device name string specifying the target of the query. The device name cannot have a trailing backslash; for example, use "C:", not "C:\".
		/// <para>
		/// This parameter can be NULL. In that case, the QueryDosDevice function will store a list of all existing MS-DOS device names into the buffer pointed
		/// to by <paramref name="lpTargetPath"/>.
		/// </para>
		/// </param>
		/// <param name="lpTargetPath">
		/// A pointer to a buffer that will receive the result of the query. The function fills this buffer with one or more null-terminated strings. The final
		/// null-terminated string is followed by an additional NULL.
		/// <para>
		/// If <paramref name="lpDeviceName"/> is non-NULL, the function retrieves information about the particular MS-DOS device specified by <paramref
		/// name="lpDeviceName"/>. The first null-terminated string stored into the buffer is the current mapping for the device. The other null-terminated
		/// strings represent undeleted prior mappings for the device.
		/// </para>
		/// <para>
		/// If <paramref name="lpDeviceName"/> is NULL, the function retrieves a list of all existing MS-DOS device names. Each null-terminated string stored
		/// into the buffer is the name of an existing MS-DOS device, for example, \Device\HarddiskVolume1 or \Device\Floppy0.
		/// </para>
		/// </param>
		/// <param name="ucchMax">The maximum number of TCHARs that can be stored into the buffer pointed to by <paramref name="lpTargetPath"/>.</param>
		/// <returns>
		/// If the function succeeds, the return value is the number of TCHARs stored into the buffer pointed to by <paramref name="lpTargetPath"/>.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// <para>If the buffer is too small, the function fails and the last error code is ERROR_INSUFFICIENT_BUFFER.</para>
		/// </returns>
		/// <remarks>
		/// The DefineDosDevice function enables an application to create and modify the junctions used to implement the MS-DOS device namespace.
		/// <para>
		/// <c>Windows Server 2003 and Windows XP:</c><c>QueryDosDevice</c> first searches the Local MS-DOS Device namespace for the specified device name. If
		/// the device name is not found, the function will then search the Global MS-DOS Device namespace.
		/// </para>
		/// <para>
		/// When all existing MS-DOS device names are queried, the list of device names that are returned is dependent on whether it is running in the
		/// "LocalSystem" context. If so, only the device names included in the Global MS-DOS Device namespace will be returned. If not, a concatenation of the
		/// device names in the Global and Local MS-DOS Device namespaces will be returned. If a device name exists in both namespaces, <c>QueryDosDevice</c>
		/// will return the entry in the Local MS-DOS Device namespace.
		/// </para>
		/// </remarks>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int QueryDosDevice(string lpDeviceName, IntPtr lpTargetPath, int ucchMax);

		public static IEnumerable<string> QueryDosDevice(string deviceName)
		{
			SafeHGlobalHandle mem = null;
			deviceName = deviceName?.TrimEnd('\\');
			try
			{
				var bytes = 1024;
				mem = new SafeHGlobalHandle(bytes);
				var retLen = QueryDosDevice(deviceName, (IntPtr)mem, mem.Size / Marshal.SystemDefaultCharSize);
				while (retLen == 0 && Marshal.GetLastWin32Error() == Win32Error.ERROR_INSUFFICIENT_BUFFER)
				{
					mem.Dispose();
					mem = new SafeHGlobalHandle(bytes *= 4);
					retLen = QueryDosDevice(deviceName, (IntPtr)mem, mem.Size / Marshal.SystemDefaultCharSize);
				}
				if (retLen == 0) throw new Win32Exception();
				return mem.DangerousGetHandle().GetStrings(retLen);
			}
			finally
			{
				mem?.Dispose();
			}
		}

		/// <summary>
		/// Sets the last-error code for the calling thread.
		/// </summary>
		/// <param name="dwErrCode">The last-error code for the thread.</param>
		[DllImport(nameof(Kernel32), ExactSpelling = true)]
		public static extern void SetLastError(uint dwErrCode);

		/// <summary>Retrieves the size, in bytes, of the specified resource.</summary>
		/// <param name="hModule">A handle to the module whose executable file contains the resource.</param>
		/// <param name="hResInfo">A handle to the resource. This handle must be created by using the FindResource or FindResourceEx function.</param>
		/// <returns>
		/// If the function succeeds, the return value is the number of bytes in the resource. If the function fails, the return value is zero. To get extended
		/// error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		public static extern int SizeofResource(SafeLibraryHandle hModule, IntPtr hResInfo);

		/// <summary>
		/// Converts a system time to file time format. System time is based on Coordinated Universal Time (UTC).
		/// </summary>
		/// <param name="lpSystemTime">A pointer to a SYSTEMTIME structure that contains the system time to be converted from UTC to file time format. The wDayOfWeek member of the SYSTEMTIME structure is ignored.</param>
		/// <param name="lpFileTime">A pointer to a FILETIME structure to receive the converted system time.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Kernel32), SetLastError = true), SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SystemTimeToFileTime(ref SYSTEMTIME lpSystemTime, ref FILETIME lpFileTime);

		/// <summary>
		/// Enumerates resources of a specified type within a binary module. For Windows Vista and later, this is typically a language-neutral Portable
		/// Executable (LN file), and the enumeration will also include resources from the corresponding language-specific resource files (.mui files) that
		/// contain localizable language resources. It is also possible for hModule to specify an .mui file, in which case only that file is searched for resources.
		/// </summary>
		/// <param name="hModule">
		/// A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate .mui files (if any exist) are included in
		/// the search.
		/// <para>If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the current process.</para>
		/// </param>
		/// <param name="lpszType">
		/// The type of the resource for which the name is being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where
		/// ID is an integer value representing a predefined resource type.
		/// </param>
		/// <param name="lpEnumFunc">A pointer to the callback function to be called for each enumerated resource name or ID.</param>
		/// <param name="lParam">An application-defined value passed to the callback function. This parameter can be used in error checking.</param>
		/// <returns>
		/// The return value is TRUE if the function succeeds or FALSE if the function does not find a resource of the type specified, or if the function fails
		/// for another reason. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumResourceNames(SafeLibraryHandle hModule, IntPtr lpszType, EnumResNameProcManaged lpEnumFunc, ResList lParam);

		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		private static extern int lstrlen([In, MarshalAs(UnmanagedType.LPTStr)] string s);

		public class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			/// <summary>A handle that may be used in place of <see cref="IntPtr.Zero"/>.</summary>
			public static readonly SafeLibraryHandle Null = new SafeLibraryHandle(IntPtr.Zero);

			public SafeLibraryHandle(string fileName, LoadLibraryExFlags flags = 0) : base(true)
			{
				var hLib = LoadLibraryEx(fileName, IntPtr.Zero, flags);
				if (hLib == IntPtr.Zero)
					throw new Win32Exception();
				SetHandle(hLib);
			}

			public SafeLibraryHandle(IntPtr handle, bool own = true) : base(own)
			{
				SetHandle(handle);
			}

			public static implicit operator IntPtr(SafeLibraryHandle h) => h.DangerousGetHandle();

			protected override bool ReleaseHandle() => FreeLibrary(handle);
		}

		private class ResList { public List<ResourceName> L { get; } = new List<ResourceName>(); }
	}
}