using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class AdvApi32
	{
		public const uint MAX_SHUTDOWN_TIMEOUT = 10 * 365 * 24 * 60 * 60;

		public static readonly byte[] SECURITY_NULL_SID_AUTHORITY         = {0,0,0,0,0,0};
		public static readonly byte[] SECURITY_WORLD_SID_AUTHORITY        = {0,0,0,0,0,1};
		public static readonly byte[] SECURITY_LOCAL_SID_AUTHORITY        = {0,0,0,0,0,2};
		public static readonly byte[] SECURITY_CREATOR_SID_AUTHORITY      = {0,0,0,0,0,3};
		public static readonly byte[] SECURITY_NON_UNIQUE_AUTHORITY       = {0,0,0,0,0,4};
		public static readonly byte[] SECURITY_NT_AUTHORITY               = {0,0,0,0,0,5};
		public static readonly byte[] SECURITY_RESOURCE_MANAGER_AUTHORITY = {0,0,0,0,0,9};
		public static readonly byte[] SECURITY_APP_PACKAGE_AUTHORITY      = {0,0,0,0,0,15};
		public static readonly byte[] SECURITY_MANDATORY_LABEL_AUTHORITY  = {0,0,0,0,0,16};
		public static readonly byte[] SECURITY_SCOPED_POLICY_ID_AUTHORITY = {0,0,0,0,0,17};
		public static readonly byte[] SECURITY_AUTHENTICATION_AUTHORITY   = {0,0,0,0,0,18};
		public static readonly byte[] SECURITY_PROCESS_TRUST_AUTHORITY    = {0,0,0,0,0,19};

		[Flags]
		public enum AccessTypes : uint
		{
			TOKEN_ASSIGN_PRIMARY = 0x0001,
			TOKEN_DUPLICATE = 0x0002,
			TOKEN_IMPERSONATE = 0x0004,
			TOKEN_QUERY = 0x0008,
			TOKEN_QUERY_SOURCE = 0x0010,
			TOKEN_ADJUST_PRIVILEGES = 0x0020,
			TOKEN_ADJUST_GROUPS = 0x0040,
			TOKEN_ADJUST_DEFAULT = 0x0080,
			TOKEN_ADJUST_SESSIONID = 0x0100,
			TOKEN_ALL_ACCESS_P = 0x000F00FF,
			TOKEN_ALL_ACCESS = 0x000F01FF,
			TOKEN_READ = 0x00020008,
			TOKEN_WRITE = 0x000200E0,
			TOKEN_EXECUTE = 0x00020000,

			DELETE = 0x00010000,
			READ_CONTROL = 0x00020000,
			WRITE_DAC = 0x00040000,
			WRITE_OWNER = 0x00080000,
			SYNCHRONIZE = 0x00100000,
			STANDARD_RIGHTS_REQUIRED = 0x000F0000,
			STANDARD_RIGHTS_READ = 0x00020000,
			STANDARD_RIGHTS_WRITE = 0x00020000,
			STANDARD_RIGHTS_EXECUTE = 0x00020000,
			STANDARD_RIGHTS_ALL = 0x001F0000,
			SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
			ACCESS_SYSTEM_SECURITY = 0x01000000,
			MAXIMUM_ALLOWED = 0x02000000,
			GENERIC_READ = 0x80000000,
			GENERIC_WRITE = 0x40000000,
			GENERIC_EXECUTE = 0x20000000,
			GENERIC_ALL = 0x10000000
		}

		/// <summary>Specifies the logon provider.</summary>
		public enum LogonUserProvider
		{
			/// <summary>
			/// Use the standard logon provider for the system. The default security provider is negotiate, unless you pass NULL for the domain name and the user
			/// name is not in UPN format. In this case, the default provider is NTLM.
			/// </summary>
			LOGON32_PROVIDER_DEFAULT = 0,

			/// <summary>Use the Windows NT 3.5 logon provider.</summary>
			LOGON32_PROVIDER_WINNT35 = 1,

			/// <summary>Use the NTLM logon provider.</summary>
			LOGON32_PROVIDER_WINNT40 = 2,

			/// <summary>Use the negotiate logon provider.</summary>
			LOGON32_PROVIDER_WINNT50 = 3,

			/// <summary>Use the virtual logon provider.</summary>
			LOGON32_PROVIDER_VIRTUAL = 4
		}

		/// <summary>The type of logon operation to perform.</summary>
		public enum LogonUserType
		{
			/// <summary>
			/// This logon type is intended for users who will be interactively using the computer, such as a user being logged on by a terminal server, remote
			/// shell, or similar process. This logon type has the additional expense of caching logon information for disconnected operations; therefore, it is
			/// inappropriate for some client/server applications, such as a mail server.
			/// </summary>
			LOGON32_LOGON_INTERACTIVE = 2,

			/// <summary>
			/// This logon type is intended for high performance servers to authenticate plaintext passwords. The LogonUser function does not cache credentials
			/// for this logon type.
			/// </summary>
			LOGON32_LOGON_NETWORK = 3,

			/// <summary>
			/// This logon type is intended for batch servers, where processes may be executing on behalf of a user without their direct intervention. This type
			/// is also for higher performance servers that process many plaintext authentication attempts at a time, such as mail or web servers.
			/// </summary>
			LOGON32_LOGON_BATCH = 4,

			/// <summary>Indicates a service-type logon. The account provided must have the service privilege enabled.</summary>
			LOGON32_LOGON_SERVICE = 5,

			/// <summary>
			/// GINAs are no longer supported.
			/// <para>
			/// <c>Windows Server 2003 and Windows XP:</c> This logon type is for GINA DLLs that log on users who will be interactively using the computer. This
			/// logon type can generate a unique audit record that shows when the workstation was unlocked.
			/// </para>
			/// </summary>
			LOGON32_LOGON_UNLOCK = 7,

			/// <summary>
			/// This logon type preserves the name and password in the authentication package, which allows the server to make connections to other network
			/// servers while impersonating the client. A server can accept plain-text credentials from a client, call LogonUser, verify that the user can access
			/// the system across the network, and still communicate with other servers.
			/// </summary>
			LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

			/// <summary>
			/// This logon type allows the caller to clone its current token and specify new credentials for outbound connections. The new logon session has the
			/// same local identifier but uses different credentials for other network connections. This logon type is supported only by the
			/// LOGON32_PROVIDER_WINNT50 logon provider.
			/// </summary>
			LOGON32_LOGON_NEW_CREDENTIALS = 9
		}

		/// <summary>The MULTIPLE_TRUSTEE_OPERATION enumeration contains values that indicate whether a TRUSTEE structure is an impersonation trustee.</summary>
		public enum MULTIPLE_TRUSTEE_OPERATION
		{
			/// <summary>The trustee is not an impersonation trustee.</summary>
			NO_MULTIPLE_TRUSTEE,
			/// <summary>
			/// The trustee is an impersonation trustee. The pMultipleTrustee member of the TRUSTEE structure points to a trustee for a server that can
			/// impersonate the client trustee.
			/// </summary>
			TRUSTEE_IS_IMPERSONATE
		}

		/// <summary>
		/// The SECURITY_INFORMATION data type identifies the object-related security information being set or queried. This security information includes:
		/// <list type="bullet">
		/// <itemItem><para>The owner of an object</para></itemItem>
		/// <itemItem><para>The primary group of an object</para></itemItem>
		/// <itemItem><para>The discretionary access control list (DACL) of an object</para></itemItem>
		/// <itemItem><para>The system access control list (SACL) of an object</para></itemItem>
		/// </list>
		/// </summary>
		[Flags]
		public enum SECURITY_INFORMATION : uint
		{
			/// <summary>The owner identifier of the object is being referenced.</summary>
			OWNER_SECURITY_INFORMATION = 0x00000001,
			/// <summary>The primary group identifier of the object is being referenced.</summary>
			GROUP_SECURITY_INFORMATION = 0x00000002,
			/// <summary>The DACL of the object is being referenced.</summary>
			DACL_SECURITY_INFORMATION = 0x00000004,
			/// <summary>The SACL of the object is being referenced.</summary>
			SACL_SECURITY_INFORMATION = 0x00000008,
			/// <summary>The mandatory integrity label is being referenced. The mandatory integrity label is an ACE in the SACL of the object.</summary>
			LABEL_SECURITY_INFORMATION = 0x00000010,
			/// <summary>The resource properties of the object being referenced. The resource properties are stored in SYSTEM_RESOURCE_ATTRIBUTE_ACE types in the SACL of the security descriptor.</summary>
			ATTRIBUTE_SECURITY_INFORMATION = 0x00000020,
			/// <summary>The Central Access Policy (CAP) identifier applicable on the object that is being referenced. Each CAP identifier is stored in a SYSTEM_SCOPED_POLICY_ID_ACE type in the SACL of the SD.</summary>
			SCOPE_SECURITY_INFORMATION = 0x00000040,
			/// <summary>Reserved.</summary>
			PROCESS_TRUST_LABEL_SECURITY_INFORMATION = 0x00000080,
			/// <summary>All parts of the security descriptor. This is useful for backup and restore software that needs to preserve the entire security descriptor.</summary>
			BACKUP_SECURITY_INFORMATION = 0x00010000,
			/// <summary>The DACL cannot inherit access control entries (ACEs).</summary>
			PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000,
			/// <summary>The SACL cannot inherit ACEs.</summary>
			PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
			/// <summary>The DACL inherits ACEs from the parent object.</summary>
			UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
			/// <summary>The SACL inherits ACEs from the parent object.</summary>
			UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000
		}

		[Flags]
		public enum SecurityDescriptorControl : ushort
		{
			OwnerDefaulted = 0x0001,
			GroupDefaulted = 0x0002,
			DaclPresent = 0x0004,
			DaclDefaulted = 0x0008,
			SaclPresent = 0x0010,
			SaclDefaulted = 0x0020,
			DaclAutoInheritreq = 0x0100,
			SaclAutoInheritreq = 0x0200,
			DaclAutoInherited = 0x0400,
			SaclAutoInherited = 0x0800,
			DaclProtected = 0x1000,
			SaclProtected = 0x2000,
			RMControlValid = 0x4000,
			SelfRelative = 0x8000
		}

		/// <summary>
		/// The SE_OBJECT_TYPE enumeration contains values that correspond to the types of Windows objects that support security. The functions, such as GetSecurityInfo and SetSecurityInfo, that set and retrieve the security information of an object, use these values to indicate the type of object.
		/// </summary>
		public enum SE_OBJECT_TYPE
		{
			/// <summary>Unknown object type.</summary>
			SE_UNKNOWN_OBJECT_TYPE = 0,
			/// <summary>Indicates a file or directory. The name string that identifies a file or directory object can be in one of the following formats:
			/// <list type="bullet">
			/// <listItem><para>A relative path, such as FileName.dat or ..\FileName</para></listItem>
			/// <listItem><para>An absolute path, such as FileName.dat, C:\DirectoryName\FileName.dat, or G:\RemoteDirectoryName\FileName.dat.</para></listItem>
			/// <listItem><para>A UNC name, such as \\ComputerName\ShareName\FileName.dat.</para></listItem>
			/// </list>
			///</summary>
			SE_FILE_OBJECT,
			/// <summary>Indicates a Windows service. A service object can be a local service, such as ServiceName, or a remote service, such as \\ComputerName\ServiceName.</summary>
			SE_SERVICE,
			/// <summary>Indicates a printer. A printer object can be a local printer, such as PrinterName, or a remote printer, such as \\ComputerName\PrinterName.</summary>
			SE_PRINTER,
			/// <summary>Indicates a registry key. A registry key object can be in the local registry, such as CLASSES_ROOT\SomePath or in a remote registry, such as \\ComputerName\CLASSES_ROOT\SomePath.
			/// <para>The names of registry keys must use the following literal strings to identify the predefined registry keys: "CLASSES_ROOT", "CURRENT_USER", "MACHINE", and "USERS".</para></summary>
			SE_REGISTRY_KEY,
			/// <summary>Indicates a network share. A share object can be local, such as ShareName, or remote, such as \\ComputerName\ShareName.</summary>
			SE_LMSHARE,
			/// <summary>Indicates a local kernel object. The GetSecurityInfo and SetSecurityInfo functions support all types of kernel objects. The GetNamedSecurityInfo and SetNamedSecurityInfo functions work only with the following kernel objects: semaphore, event, mutex, waitable timer, and file mapping.</summary>
			SE_KERNEL_OBJECT,
			/// <summary>Indicates a window station or desktop object on the local computer. You cannot use GetNamedSecurityInfo and SetNamedSecurityInfo with these objects because the names of window stations or desktops are not unique.</summary>
			SE_WINDOW_OBJECT,
			/// <summary>Indicates a directory service object or a property set or property of a directory service object. The name string for a directory service object must be in X.500 form, for example:
			/// <para>CN=SomeObject,OU=ou2,OU=ou1,DC=DomainName,DC=CompanyName,DC=com,O=internet</para></summary>
			SE_DS_OBJECT,
			/// <summary>Indicates a directory service object and all of its property sets and properties.</summary>
			SE_DS_OBJECT_ALL,
			/// <summary>Indicates a provider-defined object.</summary>
			SE_PROVIDER_DEFINED_OBJECT,
			/// <summary>Indicates a WMI object.</summary>
			SE_WMIGUID_OBJECT,
			/// <summary>Indicates an object for a registry entry under WOW64.</summary>
			SE_REGISTRY_WOW64_32KEY
		}

		public enum ServiceStartType : uint
		{
			/// <summary>Makes no change for this setting.</summary>
			SERVICE_NO_CHANGE = 0xFFFFFFFF,
			/// <summary>A device driver started by the system loader. This value is valid only for driver services.</summary>
			SERVICE_BOOT_START = 0x00000000,
			/// <summary>A device driver started by the IoInitSystem function. This value is valid only for driver services.</summary>
			SERVICE_SYSTEM_START = 0x00000001,
			/// <summary>A service started automatically by the service control manager during system startup.</summary>
			SERVICE_AUTO_START = 0x00000002,
			/// <summary>A service started by the service control manager when a process calls the StartService function.</summary>
			SERVICE_DEMAND_START = 0x00000003,
			/// <summary>A service that cannot be started. Attempts to start the service result in the error code ERROR_SERVICE_DISABLED.</summary>
			SERVICE_DISABLED = 0x00000004
		}

		public enum ServiceErrorControlType : uint
		{
			/// <summary>Makes no change for this setting.</summary>
			SERVICE_NO_CHANGE = 0xFFFFFFFF,
			/// <summary>The startup program ignores the error and continues the startup operation.</summary>
			SERVICE_ERROR_IGNORE = 0x00000000,
			/// <summary>The startup program logs the error in the event log but continues the startup operation.</summary>
			SERVICE_ERROR_NORMAL = 0x00000001,
			/// <summary>
			/// The startup program logs the error in the event log. If the last-known-good configuration is being started, the startup operation continues.
			/// Otherwise, the system is restarted with the last-known-good configuration.
			/// </summary>
			SERVICE_ERROR_SEVERE = 0x00000002,
			/// <summary>
			/// The startup program logs the error in the event log, if possible. If the last-known-good configuration is being started, the startup operation
			/// fails. Otherwise, the system is restarted with the last-known good configuration.
			/// </summary>
			SERVICE_ERROR_CRITICAL = 0x00000003
		}

		[Flags]
		public enum ServiceTypes : uint
		{
			/// <summary>Makes no change for this setting.</summary>
			SERVICE_NO_CHANGE = 0xFFFFFFFF,
			/// <summary>Driver service.</summary>
			SERVICE_KERNEL_DRIVER = 0x00000001,
			/// <summary>File system driver service.</summary>
			SERVICE_FILE_SYSTEM_DRIVER = 0x00000002,
			/// <summary>Reserved</summary>
			SERVICE_ADAPTER = 0x00000004,
			/// <summary>Reserved</summary>
			SERVICE_RECOGNIZER_DRIVER = 0x00000008,
			/// <summary>Combination of SERVICE_KERNEL_DRIVER | SERVICE_FILE_SYSTEM_DRIVER | SERVICE_RECOGNIZER_DRIVER</summary>
			SERVICE_DRIVER = SERVICE_KERNEL_DRIVER | SERVICE_FILE_SYSTEM_DRIVER | SERVICE_RECOGNIZER_DRIVER,
			/// <summary>Service that runs in its own process.</summary>
			SERVICE_WIN32_OWN_PROCESS = 0x00000010,
			/// <summary>Service that shares a process with other services.</summary>
			SERVICE_WIN32_SHARE_PROCESS = 0x00000020,
			/// <summary>Combination of SERVICE_WIN32_OWN_PROCESS | SERVICE_WIN32_SHARE_PROCESS</summary>
			SERVICE_WIN32 = SERVICE_WIN32_OWN_PROCESS | SERVICE_WIN32_SHARE_PROCESS,
			/// <summary>The service user service</summary>
			SERVICE_USER_SERVICE = 0x00000040,
			/// <summary>The service userservice instance</summary>
			SERVICE_USERSERVICE_INSTANCE = 0x00000080,
			/// <summary>Combination of SERVICE_USER_SERVICE | SERVICE_WIN32_SHARE_PROCESS</summary>
			SERVICE_USER_SHARE_PROCESS = SERVICE_USER_SERVICE | SERVICE_WIN32_SHARE_PROCESS,
			/// <summary>Combination of SERVICE_USER_SERVICE | SERVICE_WIN32_OWN_PROCESS</summary>
			SERVICE_USER_OWN_PROCESS = SERVICE_USER_SERVICE | SERVICE_WIN32_OWN_PROCESS,
			/// <summary>The service can interact with the desktop.</summary>
			SERVICE_INTERACTIVE_PROCESS = 0x00000100,
			/// <summary>The service PKG service</summary>
			SERVICE_PKG_SERVICE = 0x00000200,
			/// <summary>Combination of all service types</summary>
			SERVICE_TYPE_ALL = SERVICE_WIN32 | SERVICE_ADAPTER | SERVICE_DRIVER | SERVICE_INTERACTIVE_PROCESS | SERVICE_USER_SERVICE | SERVICE_USERSERVICE_INSTANCE | SERVICE_PKG_SERVICE
		}

		/// <summary>The SID_NAME_USE enumeration contains values that specify the type of a security identifier (SID).</summary>
		public enum SID_NAME_USE
		{
			/// <summary>A user SID.</summary>
			SidTypeUser = 1,
			/// <summary>A group SID</summary>
			SidTypeGroup,
			/// <summary>A domain SID.</summary>
			SidTypeDomain,
			/// <summary>An alias SID.</summary>
			SidTypeAlias,
			/// <summary>A SID for a well-known group.</summary>
			SidTypeWellKnownGroup,
			/// <summary>A SID for a deleted account.</summary>
			SidTypeDeletedAccount,
			/// <summary>A SID that is not valid.</summary>
			SidTypeInvalid,
			/// <summary>A SID of unknown type/.</summary>
			SidTypeUnknown,
			/// <summary>A SID for a computer.</summary>
			SidTypeComputer,
			/// <summary>A mandatory integrity label SID.</summary>
			SidTypeLabel
		}

		/// <summary>
		/// Flags used in the <see cref="AdvApi32.InitiateShutdown"/> function.
		/// </summary>
		[Flags]
		public enum ShutdownFlags
		{
			/// <summary>
			/// All sessions are forcefully logged off. If this flag is not set and users other than the current user are logged on to the computer specified by
			/// the lpMachineName parameter, this function fails with a return value of ERROR_SHUTDOWN_USERS_LOGGED_ON.
			/// </summary>
			SHUTDOWN_FORCE_OTHERS = 0x00000001,
			/// <summary>
			/// Specifies that the originating session is logged off forcefully. If this flag is not set, the originating session is shut down interactively, so
			/// a shutdown is not guaranteed even if the function returns successfully.
			/// </summary>
			SHUTDOWN_FORCE_SELF = 0x00000002,
			/// <summary>The computer is shut down and rebooted.</summary>
			SHUTDOWN_RESTART = 0x00000004,
			/// <summary>The computer is shut down and powered down.</summary>
			SHUTDOWN_POWEROFF = 0x00000008,
			/// <summary>The computer is shut down but is not powered down or rebooted.</summary>
			SHUTDOWN_NOREBOOT = 0x00000010,
			/// <summary>Overrides the grace period so that the computer is shut down immediately.</summary>
			SHUTDOWN_GRACE_OVERRIDE       = 0x00000020,
			/// <summary>The computer installs any updates before starting the shutdown.</summary>
			SHUTDOWN_INSTALL_UPDATES = 0x00000040,
			/// <summary>
			/// The system is rebooted using the ExitWindowsEx function with the EWX_RESTARTAPPS flag. This restarts any applications that have been registered
			/// for restart using the RegisterApplicationRestart function.
			/// </summary>
			SHUTDOWN_RESTARTAPPS = 0x00000080,
			/// <summary></summary>
			SHUTDOWN_SKIP_SVC_PRESHUTDOWN = 0x00000100,
			/// <summary>
			/// Beginning with InitiateShutdown running on Windows 8, you must include the SHUTDOWN_HYBRID flag with one or more of the flags in this table to
			/// specify options for the shutdown.
			/// <para>Beginning with Windows 8, InitiateShutdown always initiate a full system shutdown if the SHUTDOWN_HYBRID flag is absent.</para>
			/// </summary>
			SHUTDOWN_HYBRID = 0x00000200,
			/// <summary></summary>
			SHUTDOWN_RESTART_BOOTOPTIONS  = 0x00000400,
			/// <summary></summary>
			SHUTDOWN_SOFT_REBOOT          = 0x00000800,
			/// <summary></summary>
			SHUTDOWN_MOBILE_UI            = 0x00001000,
		}

		/// <summary>Flags used in the ExitWindowsEx, <see cref="AdvApi32.InitiateShutdown"/> and <see cref="AdvApi32.InitiateSystemShutdownEx"/> functions.</summary>
		[Flags]
		public enum SystemShutDownReason : uint
		{
			/// <summary>The SHTDN reason flag comment required</summary>
			SHTDN_REASON_FLAG_COMMENT_REQUIRED = 0x01000000,
			/// <summary>The SHTDN reason flag dirty problem identifier required</summary>
			SHTDN_REASON_FLAG_DIRTY_PROBLEM_ID_REQUIRED = 0x02000000,
			/// <summary>The SHTDN reason flag clean UI</summary>
			SHTDN_REASON_FLAG_CLEAN_UI = 0x04000000,
			/// <summary>The SHTDN reason flag dirty UI</summary>
			SHTDN_REASON_FLAG_DIRTY_UI = 0x08000000,
			/// <summary>The SHTDN reason flag mobile UI reserved</summary>
			SHTDN_REASON_FLAG_MOBILE_UI_RESERVED = 0x10000000,
			/// <summary>
			/// The reason code is defined by the user. For more information, see Defining a Custom Reason Code. If this flag is not present, the reason code is
			/// defined by the system.
			/// </summary>
			SHTDN_REASON_FLAG_USER_DEFINED = 0x40000000,
			/// <summary>
			/// The shutdown was planned. The system generates a System State Data (SSD) file. This file contains system state information such as the processes,
			/// threads, memory usage, and configuration.
			/// <para>
			/// If this flag is not present, the shutdown was unplanned. Notification and reporting options are controlled by a set of policies. For example,
			/// after logging in, the system displays a dialog box reporting the unplanned shutdown if the policy has been enabled. An SSD file is created only
			/// if the SSD policy is enabled on the system. The administrator can use Windows Error Reporting to send the SSD data to a central location, or to Microsoft.
			/// </para>
			/// </summary>
			SHTDN_REASON_FLAG_PLANNED = 0x80000000,
			/// <summary>Other issue.</summary>
			SHTDN_REASON_MAJOR_OTHER = 0x00000000,
			/// <summary>No issue.</summary>
			SHTDN_REASON_MAJOR_NONE = 0x00000000,
			/// <summary>Hardware issue.</summary>
			SHTDN_REASON_MAJOR_HARDWARE = 0x00010000,
			/// <summary>Operating system issue.</summary>
			SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000,
			/// <summary>Software issue.</summary>
			SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000,
			/// <summary>Application issue.</summary>
			SHTDN_REASON_MAJOR_APPLICATION = 0x00040000,
			/// <summary>System failure.</summary>
			SHTDN_REASON_MAJOR_SYSTEM = 0x00050000,
			/// <summary>Power failure.</summary>
			SHTDN_REASON_MAJOR_POWER = 0x00060000,
			/// <summary>The InitiateSystemShutdown function was used instead of InitiateSystemShutdownEx.</summary>
			SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000,
			/// <summary>Other issue.</summary>
			SHTDN_REASON_MINOR_OTHER = 0x00000000,
			/// <summary>The SHTDN reason minor none</summary>
			SHTDN_REASON_MINOR_NONE = 0x000000ff,
			/// <summary>Maintenance.</summary>
			SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001,
			/// <summary>Installation.</summary>
			SHTDN_REASON_MINOR_INSTALLATION = 0x00000002,
			/// <summary>Upgrade.</summary>
			SHTDN_REASON_MINOR_UPGRADE = 0x00000003,
			/// <summary>Reconfigure.</summary>
			SHTDN_REASON_MINOR_RECONFIG = 0x00000004,
			/// <summary>Unresponsive.</summary>
			SHTDN_REASON_MINOR_HUNG = 0x00000005,
			/// <summary>Unstable.</summary>
			SHTDN_REASON_MINOR_UNSTABLE = 0x00000006,
			/// <summary>Disk.</summary>
			SHTDN_REASON_MINOR_DISK = 0x00000007,
			/// <summary>Processor.</summary>
			SHTDN_REASON_MINOR_PROCESSOR = 0x00000008,
			/// <summary>Network card.</summary>
			SHTDN_REASON_MINOR_NETWORKCARD = 0x00000009,
			/// <summary>Power supply.</summary>
			SHTDN_REASON_MINOR_POWER_SUPPLY = 0x0000000a,
			/// <summary>Unplugged.</summary>
			SHTDN_REASON_MINOR_CORDUNPLUGGED = 0x0000000b,
			/// <summary>Environment.</summary>
			SHTDN_REASON_MINOR_ENVIRONMENT = 0x0000000c,
			/// <summary>Driver.</summary>
			SHTDN_REASON_MINOR_HARDWARE_DRIVER = 0x0000000d,
			/// <summary>Other driver event.</summary>
			SHTDN_REASON_MINOR_OTHERDRIVER = 0x0000000e,
			/// <summary>Blue screen crash event.</summary>
			SHTDN_REASON_MINOR_BLUESCREEN = 0x0000000F,
			/// <summary>Service pack.</summary>
			SHTDN_REASON_MINOR_SERVICEPACK = 0x00000010,
			/// <summary>Hot fix.</summary>
			SHTDN_REASON_MINOR_HOTFIX = 0x00000011,
			/// <summary>Security patch.</summary>
			SHTDN_REASON_MINOR_SECURITYFIX = 0x00000012,
			/// <summary>Security issue.</summary>
			SHTDN_REASON_MINOR_SECURITY = 0x00000013,
			/// <summary>Network connectivity.</summary>
			SHTDN_REASON_MINOR_NETWORK_CONNECTIVITY = 0x00000014,
			/// <summary>WMI issue.</summary>
			SHTDN_REASON_MINOR_WMI = 0x00000015,
			/// <summary>Service pack uninstallation.</summary>
			SHTDN_REASON_MINOR_SERVICEPACK_UNINSTALL = 0x00000016,
			/// <summary>Hot fix uninstallation.</summary>
			SHTDN_REASON_MINOR_HOTFIX_UNINSTALL = 0x00000017,
			/// <summary>Security patch uninstallation.</summary>
			SHTDN_REASON_MINOR_SECURITYFIX_UNINSTALL = 0x00000018,
			/// <summary>MMC issue.</summary>
			SHTDN_REASON_MINOR_MMC = 0x00000019,
			/// <summary>System restore.</summary>
			SHTDN_REASON_MINOR_SYSTEMRESTORE = 0x0000001a,
			/// <summary>Terminal Services.</summary>
			SHTDN_REASON_MINOR_TERMSRV = 0x00000020,
			/// <summary>DC promotion.</summary>
			SHTDN_REASON_MINOR_DC_PROMOTION = 0x00000021,
			/// <summary>DC demotion.</summary>
			SHTDN_REASON_MINOR_DC_DEMOTION = 0x00000022,
			/// <summary>Unknown.</summary>
			SHTDN_REASON_UNKNOWN = SHTDN_REASON_MINOR_NONE,
			/// <summary>The InitiateSystemShutdown function was used instead of InitiateSystemShutdownEx.</summary>
			SHTDN_REASON_LEGACY_API = SHTDN_REASON_MAJOR_LEGACY_API | SHTDN_REASON_FLAG_PLANNED
		}

		/// <summary>
		/// The TRUSTEE_FORM enumeration contains values that indicate the type of data pointed to by the ptstrName member of the <see cref="TRUSTEE"/> structure.
		/// </summary>
		public enum TRUSTEE_FORM
		{
			/// <summary>The ptstrName member is a pointer to a security identifier (SID) that identifies the trustee.</summary>
			TRUSTEE_IS_SID,
			/// <summary>The ptstrName member is a pointer to a null-terminated string that identifies the trustee.</summary>
			TRUSTEE_IS_NAME,
			/// <summary>Indicates a trustee form that is not valid.</summary>
			TRUSTEE_BAD_FORM,
			/// <summary>
			/// The ptstrName member is a pointer to an OBJECTS_AND_SID structure that contains the SID of the trustee and the GUIDs of the object types in an
			/// object-specific access control entry (ACE).
			/// </summary>
			TRUSTEE_IS_OBJECTS_AND_SID,
			/// <summary>
			/// The ptstrName member is a pointer to an OBJECTS_AND_NAME structure that contains the name of the trustee and the names of the object types in an
			/// object-specific ACE.
			/// </summary>
			TRUSTEE_IS_OBJECTS_AND_NAME
		}

		/// <summary>The TRUSTEE_TYPE enumeration contains values that indicate the type of trustee identified by a <see cref="TRUSTEE"/> structure.</summary>
		public enum TRUSTEE_TYPE
		{
			/// <summary>The trustee type is unknown, but it may be valid.</summary>
			TRUSTEE_IS_UNKNOWN,
			/// <summary>Indicates a user.</summary>
			TRUSTEE_IS_USER,
			/// <summary>Indicates a group.</summary>
			TRUSTEE_IS_GROUP,
			/// <summary>Indicates a domain.</summary>
			TRUSTEE_IS_DOMAIN,
			/// <summary>Indicates an alias.</summary>
			TRUSTEE_IS_ALIAS,
			/// <summary>Indicates a well-known group.</summary>
			TRUSTEE_IS_WELL_KNOWN_GROUP,
			/// <summary>Indicates a deleted account.</summary>
			TRUSTEE_IS_DELETED,
			/// <summary>Indicates a trustee type that is not valid.</summary>
			TRUSTEE_IS_INVALID,
			/// <summary>Indicates a computer.</summary>
			TRUSTEE_IS_COMPUTER
		}

		/// <summary>Stops a system shutdown started by using the InitiateSystemShutdown function.</summary>
		/// <param name="lpMachineName">
		/// String that specifies the network name of the computer where the shutdown is to be stopped. If NULL or an empty string, the function stops the
		/// shutdown on the local computer.
		/// </param>
		/// <returns>0 on failure, non-zero for success</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AbortSystemShutdown(string lpMachineName);

		/// <summary>The AllocateAndInitializeSid function allocates and initializes a security identifier (SID) with up to eight subauthorities.</summary>
		/// <param name="sia">
		/// A pointer to a SID_IDENTIFIER_AUTHORITY structure. This structure provides the top-level identifier authority value to set in the SID.
		/// </param>
		/// <param name="subAuthorityCount">
		/// Specifies the number of subauthorities to place in the SID. This parameter also identifies how many of the subauthority parameters have meaningful
		/// values. This parameter must contain a value from 1 to 8.
		/// <para>
		/// For example, a value of 3 indicates that the subauthority values specified by the dwSubAuthority0, dwSubAuthority1, and dwSubAuthority2 parameters
		/// have meaningful values and to ignore the remainder.
		/// </para>
		/// </param>
		/// <param name="dwSubAuthority0">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority1">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority2">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority3">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority4">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority5">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority6">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority7">Subauthority value to place in the SID.</param>
		/// <param name="pSid">A pointer to a variable that receives the pointer to the allocated and initialized SID structure.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocateAndInitializeSid(ref SID_IDENTIFIER_AUTHORITY sia,
			byte subAuthorityCount, int dwSubAuthority0, int dwSubAuthority1,
			int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4,
			int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, out IntPtr pSid);

		/// <summary>
		/// Changes the configuration parameters of a service.
		/// </summary>
		/// <param name="hService">A handle to the service. This handle is returned by the OpenService or CreateService function and must have the SERVICE_CHANGE_CONFIG access right.</param>
		/// <param name="nServiceType">The type of service. Specify SERVICE_NO_CHANGE if you are not changing the existing service type. If you specify either SERVICE_WIN32_OWN_PROCESS or SERVICE_WIN32_SHARE_PROCESS, and the service is running in the context of the LocalSystem account, you can also specify SERVICE_INTERACTIVE_PROCESS.</param>
		/// <param name="nStartType">The service start options. Specify SERVICE_NO_CHANGE if you are not changing the existing start type.</param>
		/// <param name="nErrorControl">The severity of the error, and action taken, if this service fails to start. Specify SERVICE_NO_CHANGE if you are not changing the existing error control.</param>
		/// <param name="lpBinaryPathName">The fully qualified path to the service binary file. Specify NULL if you are not changing the existing path. If the path contains a space, it must be quoted so that it is correctly interpreted. For example, "d:\\my share\\myservice.exe" should be specified as "\"d:\\my share\\myservice.exe\"".
		/// <para>The path can also include arguments for an auto-start service. For example, "d:\\myshare\\myservice.exe arg1 arg2". These arguments are passed to the service entry point (typically the main function).</para>
		/// <para>If you specify a path on another computer, the share must be accessible by the computer account of the local computer because this is the security context used in the remote call. However, this requirement allows any potential vulnerabilities in the remote computer to affect the local computer. Therefore, it is best to use a local file.</para></param>
		/// <param name="lpLoadOrderGroup">The name of the load ordering group of which this service is a member. Specify NULL if you are not changing the existing group. Specify an empty string if the service does not belong to a group.
		/// <para>The startup program uses load ordering groups to load groups of services in a specified order with respect to the other groups. The list of load ordering groups is contained in the ServiceGroupOrder value of the following registry key:</para>
		/// <para>HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control</para></param>
		/// <param name="lpdwTagId">A pointer to a variable that receives a tag value that is unique in the group specified in the lpLoadOrderGroup parameter. Specify NULL if you are not changing the existing tag.
		/// <para>You can use a tag for ordering service startup within a load ordering group by specifying a tag order vector in the GroupOrderList value of the following registry key:</para>
		/// <para>HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control</para>
		/// <para>Tags are only evaluated for driver services that have SERVICE_BOOT_START or SERVICE_SYSTEM_START start types.</para></param>
		/// <param name="lpDependencies">A pointer to a double null-terminated array of null-separated names of services or load ordering groups that the system must start before this service can be started. (Dependency on a group means that this service can run if at least one member of the group is running after an attempt to start all members of the group.) Specify NULL if you are not changing the existing dependencies. Specify an empty string if the service has no dependencies.
		/// <para>You must prefix group names with SC_GROUP_IDENTIFIER so that they can be distinguished from a service name, because services and service groups share the same name space.</para></param>
		/// <param name="lpServiceStartName">The name of the account under which the service should run. Specify NULL if you are not changing the existing account name. If the service type is SERVICE_WIN32_OWN_PROCESS, use an account name in the form DomainName\UserName. The service process will be logged on as this user. If the account belongs to the built-in domain, you can specify .\UserName (note that the corresponding C/C++ string is ".\\UserName"). For more information, see Service User Accounts and the warning in the Remarks section.
		/// <para>A shared process can run as any user.</para>
		/// <para>If the service type is SERVICE_KERNEL_DRIVER or SERVICE_FILE_SYSTEM_DRIVER, the name is the driver object name that the system uses to load the device driver. Specify NULL if the driver is to use a default object name created by the I/O system.</para>
		/// <para>A service can be configured to use a managed account or a virtual account. If the service is configured to use a managed service account, the name is the managed service account name. If the service is configured to use a virtual account, specify the name as NT SERVICE\ServiceName. For more information about managed service accounts and virtual accounts, see the Service Accounts Step-by-Step Guide.</para>
		/// <para>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  Managed service accounts and virtual accounts are not supported until Windows 7 and Windows Server 2008 R2.</para></param>
		/// <param name="lpPassword">The password to the account name specified by the lpServiceStartName parameter. Specify NULL if you are not changing the existing password. Specify an empty string if the account has no password or if the service runs in the LocalService, NetworkService, or LocalSystem account. For more information, see Service Record List.
		/// <para>If the account name specified by the lpServiceStartName parameter is the name of a managed service account or virtual account name, the lpPassword parameter must be NULL.</para>
		/// <para>Passwords are ignored for driver services.</para></param>
		/// <param name="lpDisplayName">The display name to be used by applications to identify the service for its users. Specify NULL if you are not changing the existing display name; otherwise, this string has a maximum length of 256 characters. The name is case-preserved in the service control manager. Display name comparisons are always case-insensitive.
		/// <para>This parameter can specify a localized string using the following format:</para>
		/// <para>@[path\]dllname,-strID</para>
		/// <para>The string with identifier strID is loaded from dllname; the path is optional. For more information, see RegLoadMUIString.</para>
		/// <para>Windows Server 2003 and Windows XP:  Localized strings are not supported until Windows Vista.</para></param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ChangeServiceConfig(IntPtr hService, ServiceTypes nServiceType, ServiceStartType nStartType, ServiceErrorControlType nErrorControl,
			[MarshalAs(UnmanagedType.LPTStr), Optional] string lpBinaryPathName, [MarshalAs(UnmanagedType.LPTStr), Optional] string lpLoadOrderGroup, out uint lpdwTagId,
			[In, Optional] char[] lpDependencies, [MarshalAs(UnmanagedType.LPTStr), Optional] string lpServiceStartName, [MarshalAs(UnmanagedType.LPTStr), Optional] string lpPassword,
			[MarshalAs(UnmanagedType.LPTStr), Optional] string lpDisplayName);

		/// <summary>
		/// The ConvertSecurityDescriptorToStringSecurityDescriptor function converts a security descriptor to a string format. You can use the string format to
		/// store or transmit the security descriptor.
		/// <para>
		/// To convert the string-format security descriptor back to a valid, functional security descriptor, call the
		/// ConvertStringSecurityDescriptorToSecurityDescriptor function.
		/// </para>
		/// </summary>
		/// <param name="SecurityDescriptor">A pointer to the security descriptor to convert. The security descriptor can be in absolute or self-relative format.</param>
		/// <param name="RequestedStringSDRevision">
		/// Specifies the revision level of the output StringSecurityDescriptor string. Currently this value must be SDDL_REVISION_1.
		/// </param>
		/// <param name="SecurityInformation">
		/// Specifies a combination of the SECURITY_INFORMATION bit flags to indicate the components of the security descriptor to include in the output string.
		/// The BACKUP_SECURITY_INFORMATION flag is not applicable to this function. If the BACKUP_SECURITY_INFORMATION flag is passed in, the
		/// SecurityInformation parameter returns TRUE with null string output.
		/// </param>
		/// <param name="StringSecurityDescriptor">
		/// A pointer to a variable that receives a pointer to a null-terminated security descriptor string. For a description of the string format, see Security
		/// Descriptor String Format. To free the returned buffer, call the LocalFree function.
		/// </param>
		/// <param name="StringSecurityDescriptorLen">
		/// A pointer to a variable that receives the size, in TCHARs, of the security descriptor string returned in the StringSecurityDescriptor buffer. This
		/// parameter can be NULL if you do not need to retrieve the size.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool ConvertSecurityDescriptorToStringSecurityDescriptor(IntPtr SecurityDescriptor, uint RequestedStringSDRevision, 
			SECURITY_INFORMATION SecurityInformation, out IntPtr StringSecurityDescriptor, out IntPtr StringSecurityDescriptorLen);

		/// <summary>
		/// The ConvertSidToStringSid function converts a security identifier (SID) to a string format suitable for display, storage, or transmission.
		/// </summary>
		/// <param name="Sid">A pointer to the SID structure to be converted.</param>
		/// <param name="StringSid">A pointer to a variable that receives a pointer to a null-terminated SID string. To free the returned buffer, call the LocalFree function.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ConvertSidToStringSid(PSID Sid, out SafeHGlobalHandle StringSid);

		/// <summary>
		/// Converts a security identifier (SID) to a string format suitable for display, storage, or transmission.
		/// </summary>
		/// <param name="Sid">The SID structure to be converted.</param>
		/// <returns>A null-terminated SID string.</returns>
		public static string ConvertSidToStringSid(PSID Sid)
		{
			SafeHGlobalHandle str;
			return ConvertSidToStringSid(Sid, out str) ? str.ToString(-1) : null;
		}

		/// <summary>
		/// The ConvertStringSidToSid function converts a string-format security identifier (SID) into a valid, functional SID. You can use this function to
		/// retrieve a SID that the ConvertSidToStringSid function converted to string format.
		/// </summary>
		/// <param name="pStringSid">
		/// A pointer to a null-terminated string containing the string-format SID to convert. The SID string can use either the standard S-R-I-S-S… format for
		/// SID strings, or the SID string constant format, such as "BA" for built-in administrators. For more information about SID string notation, see SID Components.
		/// </param>
		/// <param name="sid">A pointer to a variable that receives a pointer to the converted SID. To free the returned buffer, call the LocalFree function.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ConvertStringSidToSid([In] string pStringSid, out IntPtr sid);

		/// <summary>
		/// The ConvertStringSidToSid function converts a string-format security identifier (SID) into a valid, functional SID. You can use this function to
		/// retrieve a SID that the ConvertSidToStringSid function converted to string format.
		/// </summary>
		/// <param name="pStringSid">
		/// A pointer to a null-terminated string containing the string-format SID to convert. The SID string can use either the standard S-R-I-S-S… format for
		/// SID strings, or the SID string constant format, such as "BA" for built-in administrators. For more information about SID string notation, see SID Components.
		/// </param>
		/// <param name="sid">A pointer to a variable that receives a pointer to the converted SID. To free the returned buffer, call the LocalFree function.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ConvertStringSidToSid([In] string pStringSid, out PSID sid);

		/// <summary>The CopySid function copies a security identifier (SID) to a buffer.</summary>
		/// <param name="cbDestSid">Specifies the length, in bytes, of the buffer receiving the copy of the SID.</param>
		/// <param name="destSid">A pointer to a buffer that receives a copy of the source SID structure.</param>
		/// <param name="sourceSid">A pointer to a SID structure that the function copies to the buffer pointed to by the pDestinationSid parameter.</param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CopySid(int cbDestSid, IntPtr destSid, IntPtr sourceSid);

		/// <summary>The EqualSid function tests two security identifier (SID) values for equality. Two SIDs must match exactly to be considered equal.</summary>
		/// <param name="sid1">A pointer to the first SID structure to compare. This structure is assumed to be valid.</param>
		/// <param name="sid2">A pointer to the second SID structure to compare. This structure is assumed to be valid.</param>
		/// <returns>
		/// If the SID structures are equal, the return value is nonzero.
		/// <para>If the SID structures are not equal, the return value is zero. To get extended error information, call GetLastError.</para>
		/// <para>If either SID structure is not valid, the return value is undefined.</para>
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EqualSid(PSID sid1, PSID sid2);

		/// <summary>The EqualSid function tests two security identifier (SID) values for equality. Two SIDs must match exactly to be considered equal.</summary>
		/// <param name="sid1">A pointer to the first SID structure to compare. This structure is assumed to be valid.</param>
		/// <param name="sid2">A pointer to the second SID structure to compare. This structure is assumed to be valid.</param>
		/// <returns>
		/// If the SID structures are equal, the return value is nonzero.
		/// <para>If the SID structures are not equal, the return value is zero. To get extended error information, call GetLastError.</para>
		/// <para>If either SID structure is not valid, the return value is undefined.</para>
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool EqualSid(IntPtr sid1, IntPtr sid2);

        /// <summary>
        /// The FreeInheritedFromArray function frees memory allocated by the GetInheritanceSource function.
        /// </summary>
        /// <param name="pInheritArray">A pointer to the array of INHERITED_FROM structures returned by GetInheritanceSource.</param>
        /// <param name="AceCnt">Number of entries in pInheritArray.</param>
        /// <param name="pfnArray">Unused. Set to NULL.</param>
        /// <returns>If the function succeeds, the return value is ERROR_SUCCESS. If the function fails, it returns a nonzero error code.</returns>
        [DllImport(nameof(AdvApi32))]
		public static extern Win32Error FreeInheritedFromArray(IntPtr pInheritArray, ushort AceCnt, IntPtr pfnArray);

		/// <summary>The FreeSid function frees a security identifier (SID) previously allocated by using the AllocateAndInitializeSid function.</summary>
		/// <param name="pSid">A pointer to the SID structure to free.</param>
		/// <returns>If the function succeeds, the function returns NULL. If the function fails, it returns a pointer to the SID structure represented by the pSid parameter.</returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern IntPtr FreeSid(IntPtr pSid);

		/// <summary>
		/// The GetAce function obtains a pointer to an access control entry (ACE) in an access control list (ACL).
		/// </summary>
		/// <param name="pAcl">A pointer to an ACL that contains the ACE to be retrieved.</param>
		/// <param name="dwAceIndex">The index of the ACE to be retrieved. A value of zero corresponds to the first ACE in the ACL, a value of one to the second ACE, and so on.</param>
		/// <param name="pAce">A pointer to a pointer that the function sets to the address of the ACE.</param>
		/// <returns>If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetAce(IntPtr pAcl, int dwAceIndex, out IntPtr pAce);

		/// <summary>The GetAclInformation function retrieves information about an access control list (ACL).</summary>
		/// <param name="pAcl">
		/// A pointer to an ACL. The function retrieves information about this ACL. If a null value is passed, the function causes an access violation.
		/// </param>
		/// <param name="pAclInformation">
		/// A pointer to a buffer to receive the requested information. The structure that is placed into the buffer depends on the information class requested
		/// in the dwAclInformationClass parameter.
		/// </param>
		/// <param name="nAclInformationLength">The size, in bytes, of the buffer pointed to by the pAclInformation parameter.</param>
		/// <param name="dwAclInformationClass">
		/// A value of the ACL_INFORMATION_CLASS enumeration that indicates the class of information requested. This parameter can be one of two values from this enumeration:
		/// <list type="bullet">
		/// <listItem><para>If the value is AclRevisionInformation, the function fills the buffer pointed to by the pAclInformation parameter with an ACL_REVISION_INFORMATION structure.</para></listItem>
		/// <listItem><para>If the value is AclSizeInformation, the function fills the buffer pointed to by the pAclInformation parameter with an ACL_SIZE_INFORMATION structure.</para></listItem>
		/// </list>
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern bool GetAclInformation(IntPtr pAcl, ref ACL_SIZE_INFORMATION pAclInformation, uint nAclInformationLength, uint dwAclInformationClass);

		/// <summary>
		/// The GetEffectiveRightsFromAcl function retrieves the effective access rights that an ACL structure grants to a specified trustee. The trustee's
		/// effective access rights are the access rights that the ACL grants to the trustee or to any groups of which the trustee is a member.
		/// </summary>
		/// <param name="pacl">A pointer to an ACL structure from which to get the trustee's effective access rights.</param>
		/// <param name="pTrustee">
		/// A pointer to a TRUSTEE structure that identifies the trustee. A trustee can be a user, group, or program (such as a Windows service). You can use a
		/// name or a security identifier (SID) to identify a trustee.
		/// </param>
		/// <param name="pAccessRights">A pointer to an ACCESS_MASK variable that receives the effective access rights of the trustee.</param>
		[DllImport(nameof(AdvApi32))]
		public static extern uint GetEffectiveRightsFromAcl(IntPtr pacl, [In] TRUSTEE pTrustee, ref uint pAccessRights);

		/// <summary>
		/// The GetInheritanceSource function returns information about the source of inherited access control entries (ACEs) in an access control list (ACL).
		/// </summary>
		/// <param name="pObjectName">A pointer to the name of the object that uses the ACL to be checked.</param>
		/// <param name="ObjectType">
		/// The type of object indicated by pObjectName. The possible values are SE_FILE_OBJECT, SE_REGISTRY_KEY, SE_DS_OBJECT, and SE_DS_OBJECT_ALL.
		/// </param>
		/// <param name="SecurityInfo">The type of ACL used with the object. The possible values are DACL_SECURITY_INFORMATION or SACL_SECURITY_INFORMATION.</param>
		/// <param name="Container">TRUE if the object is a container object or FALSE if the object is a leaf object. Note that the only leaf object is SE_FILE_OBJECT.</param>
		/// <param name="pObjectClassGuids">
		/// Optional list of GUIDs that identify the object types or names associated with pObjectName. This may be NULL if the object manager only supports one
		/// object class or has no GUID associated with the object class.
		/// </param>
		/// <param name="GuidCount">Number of GUIDs pointed to by pObjectClassGuids.</param>
		/// <param name="pAcl">The ACL for the object.</param>
		/// <param name="pfnArray">Reserved. Set this parameter to NULL.</param>
		/// <param name="pGenericMapping">The mapping of generic rights to specific rights for the object.</param>
		/// <param name="pInheritArray">
		/// A pointer to an array of INHERITED_FROM structures that the GetInheritanceSource function fills with the inheritance information. The caller must
		/// allocate enough memory for an entry for each ACE in the ACL.
		/// </param>
		/// <returns>If the function succeeds, the function returns ERROR_SUCCESS. If the function fails, it returns a nonzero error code defined in WinError.h.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto)]
		public static extern Win32Error GetInheritanceSource([MarshalAs(UnmanagedType.LPWStr)] string pObjectName, System.Security.AccessControl.ResourceType ObjectType, 
			SECURITY_INFORMATION SecurityInfo, [MarshalAs(UnmanagedType.Bool)] bool Container,
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5, ArraySubType = UnmanagedType.LPStruct)] Guid[] pObjectClassGuids,
			uint GuidCount, IntPtr pAcl, IntPtr pfnArray, ref GENERIC_MAPPING pGenericMapping, IntPtr pInheritArray);

		/// <summary>The GetLengthSid function returns the length, in bytes, of a valid security identifier (SID).</summary>
		/// <param name="pSid">A pointer to the SID structure whose length is returned. The structure is assumed to be valid.</param>
		/// <returns>
		/// If the SID structure is valid, the return value is the length, in bytes, of the SID structure.
		/// <para>
		/// If the SID structure is not valid, the return value is undefined. Before calling GetLengthSid, pass the SID to the IsValidSid function to verify that
		/// the SID is valid.
		/// </para>
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern int GetLengthSid(IntPtr pSid);

		/// <summary>The GetNamedSecurityInfo function retrieves a copy of the security descriptor for an object specified by name.</summary>
		/// <param name="pObjectName">
		/// A pointer to a null-terminated string that specifies the name of the object from which to retrieve security information. For descriptions of the
		/// string formats for the different object types, see SE_OBJECT_TYPE.
		/// </param>
		/// <param name="ObjectType">Specifies a value from the SE_OBJECT_TYPE enumeration that indicates the type of object named by the pObjectName parameter.</param>
		/// <param name="SecurityInfo">
		/// A set of bit flags that indicate the type of security information to retrieve. This parameter can be a combination of the SECURITY_INFORMATION bit flags.
		/// </param>
		/// <param name="ppsidOwner">
		/// A pointer to a variable that receives a pointer to the owner SID in the security descriptor returned in ppSecurityDescriptor or NULL if the security
		/// descriptor has no owner SID. The returned pointer is valid only if you set the OWNER_SECURITY_INFORMATION flag. Also, this parameter can be NULL if
		/// you do not need the owner SID.
		/// </param>
		/// <param name="ppsidGroup">
		/// A pointer to a variable that receives a pointer to the primary group SID in the returned security descriptor or NULL if the security descriptor has
		/// no group SID. The returned pointer is valid only if you set the GROUP_SECURITY_INFORMATION flag. Also, this parameter can be NULL if you do not need
		/// the group SID.
		/// </param>
		/// <param name="ppDacl">
		/// A pointer to a variable that receives a pointer to the DACL in the returned security descriptor or NULL if the security descriptor has no DACL. The
		/// returned pointer is valid only if you set the DACL_SECURITY_INFORMATION flag. Also, this parameter can be NULL if you do not need the DACL.
		/// </param>
		/// <param name="ppSacl">
		/// A pointer to a variable that receives a pointer to the SACL in the returned security descriptor or NULL if the security descriptor has no SACL. The
		/// returned pointer is valid only if you set the SACL_SECURITY_INFORMATION flag. Also, this parameter can be NULL if you do not need the SACL.
		/// </param>
		/// <param name="ppSecurityDescriptor">
		/// A pointer to a variable that receives a pointer to the security descriptor of the object. When you have finished using the pointer, free the returned
		/// buffer by calling the LocalFree function.
		/// <para>This parameter is required if any one of the ppsidOwner, ppsidGroup, ppDacl, or ppSacl parameters is not NULL.</para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is ERROR_SUCCESS. If the function fails, the return value is a nonzero error code defined in WinError.h.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetNamedSecurityInfo(
			string pObjectName,
			SE_OBJECT_TYPE ObjectType,
			SECURITY_INFORMATION SecurityInfo,
			ref PSID ppsidOwner,
			ref PSID ppsidGroup,
			ref IntPtr ppDacl,
			ref IntPtr ppSacl,
			out SECURITY_DESCRIPTOR ppSecurityDescriptor);

		/// <summary>The GetPrivateObjectSecurity function retrieves information from a private object's security descriptor.</summary>
		/// <param name="ObjectDescriptor">A pointer to a SECURITY_DESCRIPTOR structure. This is the security descriptor to be queried.</param>
		/// <param name="SecurityInformation">
		/// A set of bit flags that indicate the parts of the security descriptor to retrieve. This parameter can be a combination of the SECURITY_INFORMATION
		/// bit flags.
		/// </param>
		/// <param name="ResultantDescriptor">
		/// A pointer to a buffer that receives a copy of the requested information from the specified security descriptor. The SECURITY_DESCRIPTOR structure is
		/// returned in self-relative format.
		/// </param>
		/// <param name="DescriptorLength">Specifies the size, in bytes, of the buffer pointed to by the ResultantDescriptor parameter.</param>
		/// <param name="ReturnLength">
		/// A pointer to a variable the function sets to zero if the descriptor is copied successfully. If the buffer is too small for the security descriptor,
		/// this variable receives the number of bytes required. If this variable's value is greater than the value of the DescriptorLength parameter when the
		/// function returns, the function returns FALSE and none of the security descriptor is copied to the buffer.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetPrivateObjectSecurity(IntPtr ObjectDescriptor, SECURITY_INFORMATION SecurityInformation, IntPtr ResultantDescriptor,
			uint DescriptorLength, ref uint ReturnLength);

		/// <summary>The GetSecurityDescriptorDacl function retrieves a pointer to the discretionary access control list (DACL) in a specified security descriptor.</summary>
		/// <param name="pSecurityDescriptor">A pointer to the SECURITY_DESCRIPTOR structure that contains the DACL. The function retrieves a pointer to it.</param>
		/// <param name="lpbDaclPresent">
		/// A pointer to a value that indicates the presence of a DACL in the specified security descriptor. If lpbDaclPresent is TRUE, the security descriptor
		/// contains a DACL, and the remaining output parameters in this function receive valid values. If lpbDaclPresent is FALSE, the security descriptor does
		/// not contain a DACL, and the remaining output parameters do not receive valid values.
		/// <para>
		/// A value of TRUE for lpbDaclPresent does not mean that pDacl is not NULL. That is, lpbDaclPresent can be TRUE while pDacl is NULL, meaning that a NULL
		/// DACL is in effect. A NULL DACL implicitly allows all access to an object and is not the same as an empty DACL. An empty DACL permits no access to an
		/// object. For information about creating a proper DACL, see Creating a DACL.
		/// </para>
		/// </param>
		/// <param name="pDacl">
		/// A pointer to a pointer to an access control list (ACL). If a DACL exists, the function sets the pointer pointed to by pDacl to the address of the
		/// security descriptor's DACL. If a DACL does not exist, no value is stored.
		/// <para>
		/// If the function stores a NULL value in the pointer pointed to by pDacl, the security descriptor has a NULL DACL. A NULL DACL implicitly allows all
		/// access to an object.
		/// </para>
		/// <para>If an application expects a non-NULL DACL but encounters a NULL DACL, the application should fail securely and not allow access.</para>
		/// </param>
		/// <param name="lpbDaclDefaulted">
		/// A pointer to a flag set to the value of the SE_DACL_DEFAULTED flag in the SECURITY_DESCRIPTOR_CONTROL structure if a DACL exists for the security
		/// descriptor. If this flag is TRUE, the DACL was retrieved by a default mechanism; if FALSE, the DACL was explicitly specified by a user.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetSecurityDescriptorDacl(IntPtr pSecurityDescriptor, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclPresent,
			ref IntPtr pDacl, [MarshalAs(UnmanagedType.Bool)] out bool lpbDaclDefaulted);

		/// <summary>
		/// The GetSidSubAuthority function returns a pointer to a specified subauthority in a security identifier (SID). The subauthority value is a relative
		/// identifier (RID).
		/// </summary>
		/// <param name="pSid">A pointer to the SID structure from which a pointer to a subauthority is to be returned.</param>
		/// <param name="nSubAuthority">
		/// Specifies an index value identifying the subauthority array element whose address the function will return. The function performs no validation tests
		/// on this value. An application can call the GetSidSubAuthorityCount function to discover the range of acceptable values.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a pointer to the specified SID subauthority. To get extended error information, call GetLastError.
		/// <para>
		/// If the function fails, the return value is undefined. The function fails if the specified SID structure is not valid or if the index value specified
		/// by the nSubAuthority parameter is out of bounds.
		/// </para>
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern IntPtr GetSidSubAuthority(PSID pSid, uint nSubAuthority);

		/// <summary>
		/// Initiates a shutdown and restart of the specified computer, and restarts any applications that have been registered for restart.
		/// </summary>
		/// <param name="lpMachineName">The name of the computer to be shut down. If the value of this parameter is NULL, the local computer is shut down.</param>
		/// <param name="lpMessage">The message to be displayed in the interactive shutdown dialog box.</param>
		/// <param name="dwGracePeriod">The number of seconds to wait before shutting down the computer. If the value of this parameter is zero, the computer is shut down immediately. This value is limited to MAX_SHUTDOWN_TIMEOUT.
		/// <para>If the value of this parameter is greater than zero, and the dwShutdownFlags parameter specifies the flag SHUTDOWN_GRACE_OVERRIDE, the function fails and returns the error code ERROR_BAD_ARGUMENTS.</para></param>
		/// <param name="dwShutdownFlags">One or more bit flags that specify options for the shutdown.</param>
		/// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes. If this parameter is zero, the default is an undefined shutdown that is logged as "No title for this reason could be found". By default, it is also an unplanned shutdown. Depending on how the system is configured, an unplanned shutdown triggers the creation of a file that contains the system state information, which can delay shutdown. Therefore, do not use zero for this parameter.</param>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto)]
		public static extern Win32Error InitiateShutdown(string lpMachineName, string lpMessage, uint dwGracePeriod, ShutdownFlags dwShutdownFlags, SystemShutDownReason dwReason);

		/// <summary>Initiates a shutdown and optional restart of the specified computer.</summary>
		/// <param name="lpMachineName">
		/// String that specifies the network name of the computer to shut down. If NULL or an empty string, the function shuts down the local computer.
		/// </param>
		/// <param name="lpMessage">String that specifies a message to display in the shutdown dialog box. This parameter can be NULL if no message is required.</param>
		/// <param name="dwTimeout">
		/// Time that the shutdown dialog box should be displayed, in seconds. While this dialog box is displayed, shutdown can be stopped by the AbortSystemShutdown function.
		/// <para>If dwTimeout is not zero, InitiateSystemShutdownEx displays a dialog box on the specified computer. The dialog box displays the name of the user who called the function, displays the message specified by the lpMessage parameter, and prompts the user to log off. The dialog box beeps when it is created and remains on top of other windows in the system. The dialog box can be moved but not closed. A timer counts down the remaining time before shutdown.</para>
		/// <para>If dwTimeout is zero, the computer shuts down without displaying the dialog box, and the shutdown cannot be stopped by AbortSystemShutdown.</para>
		/// <para><c>Windows Server 2003 and Windows XP with SP1:</c> The time-out value is limited to MAX_SHUTDOWN_TIMEOUT seconds.</para>
		/// <para><c>Windows Server 2003 and Windows XP with SP1:</c> If the computer to be shut down is a Terminal Services server, the system displays a dialog box to all local and remote users warning them that shutdown has been initiated. The dialog box includes who requested the shutdown, the display message (see lpMessage), and how much time there is until the server is shut down.</para>
		/// </param>
		/// <param name="bForceAppsClosed">
		/// If this parameter is TRUE, applications with unsaved changes are to be forcibly closed. If this parameter is FALSE, the system displays a dialog box
		/// instructing the user to close the applications.
		/// </param>
		/// <param name="bRebootAfterShutdown">
		/// If this parameter is TRUE, the computer is to restart immediately after shutting down. If this parameter is FALSE, the system flushes all caches to
		/// disk and clears the screen.
		/// </param>
		/// <param name="dwReason">Reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes.</param>
		/// <returns>0 on failure, non-zero for success</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InitiateSystemShutdownEx(string lpMachineName, string lpMessage, uint dwTimeout,
			[MarshalAs(UnmanagedType.Bool)] bool bForceAppsClosed, [MarshalAs(UnmanagedType.Bool)] bool bRebootAfterShutdown,
			SystemShutDownReason dwReason);

		/// <summary>
		/// The IsValidSid function validates a security identifier (SID) by verifying that the revision number is within a known range, and that the number of
		/// subauthorities is less than the maximum.
		/// </summary>
		/// <param name="pSid">A pointer to the SID structure to validate. This parameter cannot be NULL.</param>
		/// <returns>
		/// If the SID structure is valid, the return value is nonzero. If the SID structure is not valid, the return value is zero. There is no extended error
		/// information for this function; do not call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsValidSid(PSID pSid);

		/// <summary>
		/// The LogonUser function attempts to log a user on to the local computer. The local computer is the computer from which LogonUser was called. You
		/// cannot use LogonUser to log on to a remote computer. You specify the user with a user name and domain and authenticate the user with a plain-text
		/// password. If the function succeeds, you receive a handle to a token that represents the logged-on user. You can then use this token handle to
		/// impersonate the specified user or, in most cases, to create a process that runs in the context of the specified user.
		/// </summary>
		/// <param name="lpszUserName">
		/// A pointer to a null-terminated string that specifies the name of the user. This is the name of the user account to log on to. If you use the user
		/// principal name (UPN) format, User@DNSDomainName, the lpszDomain parameter must be NULL.
		/// </param>
		/// <param name="lpszDomain">
		/// A pointer to a null-terminated string that specifies the name of the domain or server whose account database contains the lpszUsername account. If
		/// this parameter is NULL, the user name must be specified in UPN format. If this parameter is ".", the function validates the account by using only the
		/// local account database.
		/// </param>
		/// <param name="lpszPassword">
		/// A pointer to a null-terminated string that specifies the plain-text password for the user account specified by lpszUsername. When you have finished
		/// using the password, clear the password from memory by calling the SecureZeroMemory function. For more information about protecting passwords, see
		/// Handling Passwords.
		/// </param>
		/// <param name="dwLogonType">The type of logon operation to perform.</param>
		/// <param name="dwLogonProvider">Specifies the logon provider.</param>
		/// <param name="phObject">
		/// A pointer to a handle variable that receives a handle to a token that represents the specified user.
		/// <para>You can use the returned handle in calls to the ImpersonateLoggedOnUser function.</para>
		/// <para>
		/// In most cases, the returned handle is a primary token that you can use in calls to the CreateProcessAsUser function. However, if you specify the
		/// LOGON32_LOGON_NETWORK flag, LogonUser returns an impersonation token that you cannot use in CreateProcessAsUser unless you call DuplicateTokenEx to
		/// convert it to a primary token.
		/// </para>
		/// <para>When you no longer need this handle, close it by calling the CloseHandle function.</para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LogonUser(string lpszUserName, string lpszDomain, string lpszPassword, LogonUserType dwLogonType, LogonUserProvider dwLogonProvider,
			out SafeTokenHandle phObject);

		/// <summary>
		/// The LogonUserEx function attempts to log a user on to the local computer. The local computer is the computer from which LogonUserEx was called. You
		/// cannot use LogonUserEx to log on to a remote computer. You specify the user with a user name and domain and authenticate the user with a plaintext
		/// password. If the function succeeds, you receive a handle to a token that represents the logged-on user. You can then use this token handle to
		/// impersonate the specified user or, in most cases, to create a process that runs in the context of the specified user.
		/// </summary>
		/// <param name="lpszUserName">
		/// A pointer to a null-terminated string that specifies the name of the user. This is the name of the user account to log on to. If you use the user
		/// principal name (UPN) format, user@DNS_domain_name, the lpszDomain parameter must be NULL.
		/// </param>
		/// <param name="lpszDomain">
		/// A pointer to a null-terminated string that specifies the name of the domain or server whose account database contains the lpszUsername account. If
		/// this parameter is NULL, the user name must be specified in UPN format. If this parameter is ".", the function validates the account by using only the
		/// local account database.
		/// </param>
		/// <param name="lpszPassword">
		/// A pointer to a null-terminated string that specifies the plaintext password for the user account specified by lpszUsername. When you have finished
		/// using the password, clear the password from memory by calling the SecureZeroMemory function. For more information about protecting passwords, see
		/// Handling Passwords.
		/// </param>
		/// <param name="dwLogonType">The type of logon operation to perform.</param>
		/// <param name="dwLogonProvider">The logon provider.</param>
		/// <param name="phObject">
		/// A pointer to a handle variable that receives a handle to a token that represents the specified user.
		/// <para>You can use the returned handle in calls to the ImpersonateLoggedOnUser function.</para>
		/// <para>
		/// In most cases, the returned handle is a primary token that you can use in calls to the CreateProcessAsUser function. However, if you specify the
		/// LOGON32_LOGON_NETWORK flag, LogonUser returns an impersonation token that you cannot use in CreateProcessAsUser unless you call DuplicateTokenEx to
		/// convert it to a primary token.
		/// </para>
		/// <para>When you no longer need this handle, close it by calling the CloseHandle function.</para>
		/// </param>
		/// <param name="ppLogonSid">
		/// A pointer to a pointer to a security identifier (SID) that receives the SID of the user logged on.
		/// <para>When you have finished using the SID, free it by calling the LocalFree function.</para>
		/// </param>
		/// <param name="ppProfileBuffer">A pointer to a pointer that receives the address of a buffer that contains the logged on user's profile.</param>
		/// <param name="pdwProfileLength">A pointer to a DWORD that receives the length of the profile buffer.</param>
		/// <param name="pQuotaLimits">A pointer to a QUOTA_LIMITS structure that receives information about the quotas for the logged on user.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LogonUserEx(string lpszUserName, string lpszDomain, string lpszPassword, LogonUserType dwLogonType, LogonUserProvider dwLogonProvider,
			out SafeTokenHandle phObject, out PSID ppLogonSid, out SafeHGlobalHandle ppProfileBuffer, out uint pdwProfileLength, out QUOTA_LIMITS pQuotaLimits);

		/// <summary>
		/// The LookupAccountName function accepts the name of a system and an account as input. It retrieves a security identifier (SID) for the account and the
		/// name of the domain on which the account was found.
		/// </summary>
		/// <param name="lpSystemName">
		/// A pointer to a null-terminated character string that specifies the name of the system. This string can be the name of a remote computer. If this
		/// string is NULL, the account name translation begins on the local system. If the name cannot be resolved on the local system, this function will try
		/// to resolve the name using domain controllers trusted by the local system. Generally, specify a value for lpSystemName only when the account is in an
		/// untrusted domain and the name of a computer in that domain is known.
		/// </param>
		/// <param name="lpAccountName">
		/// A pointer to a null-terminated string that specifies the account name.
		/// <para>Use a fully qualified string in the domain_name\user_name format to ensure that LookupAccountName finds the account in the desired domain.</para>
		/// </param>
		/// <param name="Sid">
		/// A pointer to a buffer that receives the SID structure that corresponds to the account name pointed to by the lpAccountName parameter. If this
		/// parameter is NULL, cbSid must be zero.
		/// </param>
		/// <param name="cbSid">
		/// A pointer to a variable. On input, this value specifies the size, in bytes, of the Sid buffer. If the function fails because the buffer is too small
		/// or if cbSid is zero, this variable receives the required buffer size.
		/// </param>
		/// <param name="ReferencedDomainName">
		/// A pointer to a buffer that receives the name of the domain where the account name is found. For computers that are not joined to a domain, this
		/// buffer receives the computer name. If this parameter is NULL, the function returns the required buffer size.
		/// </param>
		/// <param name="cchReferencedDomainName">
		/// A pointer to a variable. On input, this value specifies the size, in TCHARs, of the ReferencedDomainName buffer. If the function fails because the
		/// buffer is too small, this variable receives the required buffer size, including the terminating null character. If the ReferencedDomainName parameter
		/// is NULL, this parameter must be zero.
		/// </param>
		/// <param name="peUse">A pointer to a SID_NAME_USE enumerated type that indicates the type of the account when the function returns.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. For extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupAccountName(string lpSystemName, string lpAccountName, PSID Sid, ref int cbSid,
					StringBuilder ReferencedDomainName, ref int cchReferencedDomainName, ref SID_NAME_USE peUse);

		/// <summary>
		/// The LookupAccountName function accepts the name of a system and an account as input. It retrieves a security identifier (SID) for the account and the
		/// name of the domain on which the account was found.
		/// </summary>
		/// <param name="systemName">
		/// A string that specifies the name of the system. This string can be the name of a remote computer. If this string is NULL, the account name
		/// translation begins on the local system. If the name cannot be resolved on the local system, this function will try to resolve the name using domain
		/// controllers trusted by the local system. Generally, specify a value for lpSystemName only when the account is in an untrusted domain and the name of
		/// a computer in that domain is known.
		/// </param>
		/// <param name="accountName">
		/// A string that specifies the account name.
		/// <para>Use a fully qualified string in the domain_name\user_name format to ensure that LookupAccountName finds the account in the desired domain.</para>
		/// </param>
		/// <param name="sid">A PSID class that corresponds to the account name pointed to by the lpAccountName parameter.</param>
		/// <param name="domainName">
		/// A string that receives the name of the domain where the account name is found. For computers that are not joined to a domain, this buffer receives
		/// the computer name.
		/// </param>
		/// <param name="snu">A SID_NAME_USE enumerated type that indicates the type of the account when the function returns.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. For extended error information, call GetLastError.
		/// </returns>
		public static bool LookupAccountName(string systemName, string accountName, out PSID sid, out string domainName, out SID_NAME_USE snu)
		{
			var sb = new StringBuilder(1024);
			var psid = new PSID(256);
			snu = (SID_NAME_USE)0;
			var sidSz = psid.Size;
			var sbSz = sb.Capacity;
			var ret = LookupAccountName(systemName, accountName, psid, ref sidSz, sb, ref sbSz, ref snu);
			sid = new PSID(psid);
			domainName = sb.ToString();
			return ret;
		}

		/// <summary>
		/// The LookupAccountSid function accepts a security identifier (SID) as input. It retrieves the name of the account for this SID and the name of the
		/// first domain on which this SID is found.
		/// </summary>
		/// <param name="lpSystemName">
		/// A pointer to a null-terminated character string that specifies the target computer. This string can be the name of a remote computer. If this
		/// parameter is NULL, the account name translation begins on the local system. If the name cannot be resolved on the local system, this function will
		/// try to resolve the name using domain controllers trusted by the local system. Generally, specify a value for lpSystemName only when the account is in
		/// an untrusted domain and the name of a computer in that domain is known.
		/// </param>
		/// <param name="lpSid">A pointer to the SID to look up.</param>
		/// <param name="lpName">
		/// A pointer to a buffer that receives a null-terminated string that contains the account name that corresponds to the lpSid parameter.
		/// </param>
		/// <param name="cchName">
		/// On input, specifies the size, in TCHARs, of the lpName buffer. If the function fails because the buffer is too small or if cchName is zero, cchName
		/// receives the required buffer size, including the terminating null character.
		/// </param>
		/// <param name="lpReferencedDomainName">
		/// A pointer to a buffer that receives a null-terminated string that contains the name of the domain where the account name was found.
		/// <para>
		/// On a server, the domain name returned for most accounts in the security database of the local computer is the name of the domain for which the server
		/// is a domain controller.
		/// </para>
		/// <para>
		/// On a workstation, the domain name returned for most accounts in the security database of the local computer is the name of the computer as of the
		/// last start of the system (backslashes are excluded). If the name of the computer changes, the old name continues to be returned as the domain name
		/// until the system is restarted.
		/// </para>
		/// <para>Some accounts are predefined by the system. The domain name returned for these accounts is BUILTIN.</para>
		/// </param>
		/// <param name="cchReferencedDomainName">
		/// On input, specifies the size, in TCHARs, of the lpReferencedDomainName buffer. If the function fails because the buffer is too small or if
		/// cchReferencedDomainName is zero, cchReferencedDomainName receives the required buffer size, including the terminating null character.
		/// </param>
		/// <param name="peUse">A pointer to a variable that receives a SID_NAME_USE value that indicates the type of the account.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupAccountSid(string lpSystemName, byte[] lpSid, StringBuilder lpName, ref int cchName,
			StringBuilder lpReferencedDomainName, ref int cchReferencedDomainName, out SID_NAME_USE peUse);

		/// <summary>
		/// The LookupAccountSid function accepts a security identifier (SID) as input. It retrieves the name of the account for this SID and the name of the
		/// first domain on which this SID is found.
		/// </summary>
		/// <param name="lpSystemName">
		/// A pointer to a null-terminated character string that specifies the target computer. This string can be the name of a remote computer. If this
		/// parameter is NULL, the account name translation begins on the local system. If the name cannot be resolved on the local system, this function will
		/// try to resolve the name using domain controllers trusted by the local system. Generally, specify a value for lpSystemName only when the account is in
		/// an untrusted domain and the name of a computer in that domain is known.
		/// </param>
		/// <param name="lpSid">A pointer to the SID to look up.</param>
		/// <param name="lpName">
		/// A pointer to a buffer that receives a null-terminated string that contains the account name that corresponds to the lpSid parameter.
		/// </param>
		/// <param name="cchName">
		/// On input, specifies the size, in TCHARs, of the lpName buffer. If the function fails because the buffer is too small or if cchName is zero, cchName
		/// receives the required buffer size, including the terminating null character.
		/// </param>
		/// <param name="lpReferencedDomainName">
		/// A pointer to a buffer that receives a null-terminated string that contains the name of the domain where the account name was found.
		/// <para>
		/// On a server, the domain name returned for most accounts in the security database of the local computer is the name of the domain for which the server
		/// is a domain controller.
		/// </para>
		/// <para>
		/// On a workstation, the domain name returned for most accounts in the security database of the local computer is the name of the computer as of the
		/// last start of the system (backslashes are excluded). If the name of the computer changes, the old name continues to be returned as the domain name
		/// until the system is restarted.
		/// </para>
		/// <para>Some accounts are predefined by the system. The domain name returned for these accounts is BUILTIN.</para>
		/// </param>
		/// <param name="cchReferencedDomainName">
		/// On input, specifies the size, in TCHARs, of the lpReferencedDomainName buffer. If the function fails because the buffer is too small or if
		/// cchReferencedDomainName is zero, cchReferencedDomainName receives the required buffer size, including the terminating null character.
		/// </param>
		/// <param name="peUse">A pointer to a variable that receives a SID_NAME_USE value that indicates the type of the account.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupAccountSid(string lpSystemName, PSID lpSid, StringBuilder lpName, ref int cchName,
			StringBuilder lpReferencedDomainName, ref int cchReferencedDomainName, out SID_NAME_USE peUse);

		/// <summary>
		/// The MapGenericMask function maps the generic access rights in an access mask to specific and standard access rights. The function applies a mapping supplied in a <see cref="GENERIC_MAPPING" /> structure.
		/// </summary>
		/// <param name="AccessMask">A pointer to an access mask.</param>
		/// <param name="GenericMapping">A pointer to a <see cref="GENERIC_MAPPING" /> structure specifying a mapping of generic access types to specific and standard access types.</param>
		[DllImport(nameof(AdvApi32))]
		public static extern void MapGenericMask(ref uint AccessMask, ref GENERIC_MAPPING GenericMapping);

		/// <summary>The RevertToSelf function terminates the impersonation of a client application.</summary>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RevertToSelf();

		/// <summary>
		/// The SetNamedSecurityInfo function sets specified security information in the security descriptor of a specified object. The caller identifies the
		/// object by name.
		/// </summary>
		/// <param name="pObjectName">
		/// A pointer to a null-terminated string that specifies the name of the object for which to set security information. This can be the name of a local or
		/// remote file or directory on an NTFS file system, network share, registry key, semaphore, event, mutex, file mapping, or waitable timer. For
		/// descriptions of the string formats for the different object types, see SE_OBJECT_TYPE.
		/// </param>
		/// <param name="ObjectType">A value of the SE_OBJECT_TYPE enumeration that indicates the type of object named by the pObjectName parameter.</param>
		/// <param name="SecurityInfo">
		/// A set of bit flags that indicate the type of security information to set. This parameter can be a combination of the SECURITY_INFORMATION bit flags.
		/// </param>
		/// <param name="ppsidOwner">
		/// A pointer to a SID structure that identifies the owner of the object. If the caller does not have the SeRestorePrivilege constant (see Privilege
		/// Constants), this SID must be contained in the caller's token, and must have the SE_GROUP_OWNER permission enabled. The SecurityInfo parameter must
		/// include the OWNER_SECURITY_INFORMATION flag. To set the owner, the caller must have WRITE_OWNER access to the object or have the
		/// SE_TAKE_OWNERSHIP_NAME privilege enabled. If you are not setting the owner SID, this parameter can be NULL.
		/// </param>
		/// <param name="ppsidGroup">
		/// A pointer to a SID that identifies the primary group of the object. The SecurityInfo parameter must include the GROUP_SECURITY_INFORMATION flag. If
		/// you are not setting the primary group SID, this parameter can be NULL.
		/// </param>
		/// <param name="ppDacl">
		/// A pointer to the new DACL for the object. The SecurityInfo parameter must include the DACL_SECURITY_INFORMATION flag. The caller must have WRITE_DAC
		/// access to the object or be the owner of the object. If you are not setting the DACL, this parameter can be NULL.
		/// </param>
		/// <param name="ppSacl">
		/// A pointer to the new SACL for the object. The SecurityInfo parameter must include any of the following flags: SACL_SECURITY_INFORMATION,
		/// LABEL_SECURITY_INFORMATION, ATTRIBUTE_SECURITY_INFORMATION, SCOPE_SECURITY_INFORMATION, or BACKUP_SECURITY_INFORMATION.
		/// <para>
		/// If setting SACL_SECURITY_INFORMATION or SCOPE_SECURITY_INFORMATION, the caller must have the SE_SECURITY_NAME privilege enabled. If you are not
		/// setting the SACL, this parameter can be NULL.
		/// </para>
		/// </param>
		/// <returns>If the function succeeds, the function returns ERROR_SUCCESS. If the function fails, it returns a nonzero error code defined in WinError.h.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SetNamedSecurityInfo(string pObjectName, SE_OBJECT_TYPE ObjectType, SECURITY_INFORMATION SecurityInfo, PSID ppsidOwner,
			PSID ppsidGroup, IntPtr ppDacl, IntPtr ppSacl);

		/// <summary>
		/// The ACCESS_ALLOWED_ACE structure defines an access control entry (ACE) for the discretionary access control list (DACL) that controls access to an
		/// object. An access-allowed ACE allows access to an object for a specific trustee identified by a security identifier (SID).
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct ACCESS_ALLOWED_ACE
		{
			/// <summary>
			/// ACE_HEADER structure that specifies the size and type of ACE. It also contains flags that control inheritance of the ACE by child objects. The
			/// AceType member of the ACE_HEADER structure should be set to ACCESS_ALLOWED_ACE_TYPE, and the AceSize member should be set to the total number of
			/// bytes allocated for the ACCESS_ALLOWED_ACE structure.
			/// </summary>
			public ACE_HEADER Header;
			/// <summary>Specifies an ACCESS_MASK structure that specifies the access rights granted by this ACE.</summary>
			public int Mask;
			/// <summary>
			/// The first DWORD of a trustee's SID. The remaining bytes of the SID are stored in contiguous memory after the SidStart member. This SID can be
			/// appended with application data.
			/// </summary>
			public int SidStart;

			/// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
			/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
			/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
			public override bool Equals(object obj)
			{
				if (obj is ACCESS_ALLOWED_ACE)
				{
					var that = (ACCESS_ALLOWED_ACE)obj;
					return Header.AceType == that.Header.AceType && Header.AceFlags == that.Header.AceFlags && Mask == that.Mask;
				}
				return base.Equals(obj);
			}

			/// <summary>Returns a hash code for this instance.</summary>
			/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
			public override int GetHashCode() => new { A = Header.AceFlags, B = Header.AceType, C = Mask }.GetHashCode();
		}

		/// <summary>The ACE_HEADER structure defines the type and size of an access control entry (ACE).</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct ACE_HEADER
		{
			/// <summary>Specifies the ACE type.</summary>
			public AceType AceType;
			/// <summary>Specifies a set of ACE type-specific control flags.</summary>
			public AceFlags AceFlags;
			/// <summary>Specifies the size, in bytes, of the ACE.</summary>
			public ushort AceSize;
		}

		/// <summary>The ACL_SIZE_INFORMATION structure contains information about the size of an ACL structure.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct ACL_SIZE_INFORMATION
		{
			/// <summary>The number of access control entries (ACEs) in the access control list (ACL).</summary>
			public uint AceCount;
			/// <summary>
			/// The number of bytes in the ACL actually used to store the ACEs and ACL structure. This may be less than the total number of bytes allocated to
			/// the ACL.
			/// </summary>
			public uint AclBytesInUse;
			/// <summary>The number of unused bytes in the ACL.</summary>
			public uint AclBytesFree;
		}

		/// <summary>
		/// Defines the mapping of generic access rights to specific and standard access rights for an object. When a client application requests generic access
		/// to an object, that request is mapped to the access rights defined in this structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct GENERIC_MAPPING
		{
			/// <summary>Specifies an access mask defining read access to an object.</summary>
			public uint GenericRead;

			/// <summary>Specifies an access mask defining write access to an object.</summary>
			public uint GenericWrite;

			/// <summary>Specifies an access mask defining execute access to an object.</summary>
			public uint GenericExecute;

			/// <summary>Specifies an access mask defining all possible types of access to an object.</summary>
			public uint GenericAll;

			/// <summary>Initializes a new instance of the <see cref="GENERIC_MAPPING"/> structure.</summary>
			/// <param name="read">The read mapping.</param>
			/// <param name="write">The write mapping.</param>
			/// <param name="execute">The execute mapping.</param>
			/// <param name="all">The 'all' mapping.</param>
			public GENERIC_MAPPING(uint read, uint write, uint execute, uint all)
			{
				GenericRead = read;
				GenericWrite = write;
				GenericExecute = execute;
				GenericAll = all;
			}
		}

		/// <summary>Provides information about an object's inherited access control entry (ACE).</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct INHERITED_FROM
		{
			/// <summary>
			/// Number of levels, or generations, between the object and the ancestor. Set this to zero for an explicit ACE. If the ancestor cannot be determined
			/// for the inherited ACE, set this member to –1.
			/// </summary>
			public int GenerationGap;

			/// <summary>Name of the ancestor from which the ACE was inherited. For an explicit ACE, set this to <c>null</c>.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string AncestorName;

			/// <summary>Initializes a new instance of the <see cref="INHERITED_FROM"/> structure.</summary>
			/// <param name="generationGap">The generation gap.</param>
			/// <param name="ancestorName">Name of the ancestor.</param>
			public INHERITED_FROM(int generationGap, string ancestorName)
			{
				GenerationGap = generationGap;
				AncestorName = ancestorName;
			}

			public override string ToString() => $"{AncestorName} : 0x{GenerationGap:X}";

			/// <summary>ACE is explicit.</summary>
			public static readonly INHERITED_FROM Explicit = new INHERITED_FROM(0, null);

			/// <summary>ACE inheritance cannot be determined.</summary>
			public static readonly INHERITED_FROM Indeterminate = new INHERITED_FROM(-1, null);
		}

		/// <summary>The QUOTA_LIMITS structure describes the amount of system resources available to a user.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct QUOTA_LIMITS
		{
			/// <summary>
			/// Specifies the amount of paged pool memory assigned to the user. The paged pool is an area of system memory (physical memory used by the operating
			/// system) for objects that can be written to disk when they are not being used.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to be used.</para>
			/// </summary>
			public uint PagedPoolLimit;
			/// <summary>
			/// Specifies the amount of nonpaged pool memory assigned to the user. The nonpaged pool is an area of system memory for objects that cannot be
			/// written to disk but must remain in physical memory as long as they are allocated.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to be used.</para>
			/// </summary>
			public uint NonPagedPoolLimit;
			/// <summary>
			/// Specifies the minimum set size assigned to the user. The "working set" of a process is the set of memory pages currently visible to the process
			/// in physical RAM memory. These pages are present in memory when the application is running and available for an application to use without
			/// triggering a page fault.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to NULL, which causes the default value to be used.</para>
			/// </summary>
			public uint MinimumWorkingSetSize;
			/// <summary>
			/// Specifies the maximum set size assigned to the user.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to be used.</para>
			/// </summary>
			public uint MaximumWorkingSetSize;
			/// <summary>
			/// Specifies the maximum size, in bytes, of the paging file, which is a reserved space on disk that backs up committed physical memory on the computer.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to 0, which causes the default value to be used.</para>
			/// </summary>
			public uint PagefileLimit;
			/// <summary>
			/// Indicates the maximum amount of time the process can run.
			/// <para>The value set in this member is not enforced by the LSA. You should set this member to NULL, which causes the default value to be used.</para>
			/// </summary>
			public long TimeLimit;
		}

		/// <summary>
		/// The SECURITY_DESCRIPTOR structure contains the security information associated with an object. Applications use this structure to set and query an object's security status.
		/// <para>Because the internal format of a security descriptor can vary, we recommend that applications not modify the SECURITY_DESCRIPTOR structure directly.</para></summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SECURITY_DESCRIPTOR
		{
			public byte Revision;
			public byte Sbz1;
			public SecurityDescriptorControl Control;
			public IntPtr Owner;
			public IntPtr Group;
			public IntPtr Sacl;
			public IntPtr Dacl;
		}

		/// <summary>The SID_IDENTIFIER_AUTHORITY structure represents the top-level authority of a security identifier (SID).</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct SID_IDENTIFIER_AUTHORITY
		{
			/// <summary>An array of 6 bytes specifying a SID's top-level authority.</summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] Value;

			/// <summary>Initializes a new instance of the <see cref="SID_IDENTIFIER_AUTHORITY"/> struct.</summary>
			/// <param name="value">The value.</param>
			/// <exception cref="System.ArgumentOutOfRangeException">value</exception>
			public SID_IDENTIFIER_AUTHORITY(byte[] value)
			{
				if (value == null || value.Length != 6)
					throw new ArgumentOutOfRangeException(nameof(value));
				Value = new byte[6];
				Array.Copy(value, Value, 6);
			}

			/// <summary>Initializes a new instance of the <see cref="SID_IDENTIFIER_AUTHORITY"/> struct.</summary>
			/// <param name="value">The value.</param>
			public SID_IDENTIFIER_AUTHORITY(long value)
			{
				Value = new byte[6];
				LongValue = value;
			}

			/// <summary>Gets or sets the long value.</summary>
			/// <value>The long value.</value>
			public long LongValue
			{
				get
				{
					long nAuthority = 0;
					for (var i = 0; i <= 5; i++)
					{
						nAuthority <<= 8;
						nAuthority |= Value[i];
					}
					return nAuthority;
				}
				set
				{
					var bsia = BitConverter.GetBytes(value);
					for (var i = 0; i <= 5; i++)
						Value[i] = bsia[5 - i];
				}
			}
		}

		/// <summary>
		/// The TRUSTEE structure identifies the user account, group account, or logon session to which an access control entry (ACE) applies. The structure can
		/// use a name or a security identifier (SID) to identify the trustee.
		/// <para>
		/// Access control functions, such as SetEntriesInAcl and GetExplicitEntriesFromAcl, use this structure to identify the logon account associated with the
		/// access control or audit control information in an EXPLICIT_ACCESS structure.
		/// </para>
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public sealed class TRUSTEE : IDisposable
		{
			/// <summary>
			/// A pointer to a TRUSTEE structure that identifies a server account that can impersonate the user identified by the ptstrName member. This member
			/// is not currently supported and must be NULL.
			/// </summary>
			public IntPtr pMultipleTrustee;
			/// <summary>A value of the MULTIPLE_TRUSTEE_OPERATION enumeration type. Currently, this member must be NO_MULTIPLE_TRUSTEE.</summary>
			public MULTIPLE_TRUSTEE_OPERATION MultipleTrusteeOperation;
			/// <summary>A value from the TRUSTEE_FORM enumeration type that indicates the type of data pointed to by the ptstrName member.</summary>
			public TRUSTEE_FORM TrusteeForm;
			/// <summary>
			/// A value from the TRUSTEE_TYPE enumeration type that indicates whether the trustee is a user account, a group account, or an unknown account type.
			/// </summary>
			public TRUSTEE_TYPE TrusteeType;
			/// <summary>
			/// A pointer to a buffer that identifies the trustee and, optionally, contains information about object-specific ACEs. The type of data depends on the value of the TrusteeForm member. This member can be one of the following values.
			/// <list type="table">
			/// <listheader><term>Value</term><description>Meaning</description></listheader>
			/// <item><term>TRUSTEE_IS_NAME</term><description>A pointer to a null-terminated string that contains the name of the trustee.</description></item>
			/// <item><term>TRUSTEE_IS_OBJECTS_AND_NAME</term><description>A pointer to an OBJECTS_AND_NAME structure that contains the name of the trustee and the names of the object types in an object-specific ACE.</description></item>
			/// <item><term>TRUSTEE_IS_OBJECTS_AND_SID</term><description>A pointer to an OBJECTS_AND_SID structure that contains the SID of the trustee and the GUIDs of the object types in an object-specific ACE.</description></item>
			/// <item><term>TRUSTEE_IS_SID</term><description>Pointer to the SID of the trustee.</description></item>
			/// </list>
			/// </summary>
			public IntPtr ptstrName;

			/// <summary>
			/// Initializes a new instance of the <see cref="TRUSTEE"/> class.
			/// </summary>
			/// <param name="sid">The sid.</param>
			public TRUSTEE(PSID sid = null)
			{
				if (sid != null) Sid = sid;
			}

			void IDisposable.Dispose()
			{
				if (ptstrName != IntPtr.Zero) Marshal.FreeHGlobal(ptstrName);
			}

			/// <summary>Gets or sets the name of the trustee.</summary>
			/// <value>A trustee name can have any of the following formats:
			/// <list type="bullet">
			/// <listItem>A fully qualified name, such as "g:\remotedir\abc".</listItem>
			/// <listItem>A domain account, such as "domain1\xyz".</listItem>
			/// <listItem>One of the predefined group names, such as "EVERYONE" or "GUEST".</listItem>
			/// <listItem>One of the following special names: "CREATOR GROUP", "CREATOR OWNER", "CURRENT_USER".</listItem>
			/// </list>
			///</value>
			public string Name
			{
				get { return TrusteeForm == TRUSTEE_FORM.TRUSTEE_IS_NAME ? Marshal.PtrToStringAuto(ptstrName) : null; }
				set { ((IDisposable)this).Dispose(); TrusteeForm = TRUSTEE_FORM.TRUSTEE_IS_NAME; ptstrName = Marshal.StringToHGlobalAuto(value); }
			}

			/// <summary>Gets or sets the sid for the trustee</summary>
			/// <value>The Sid.</value>
			public PSID Sid
			{
				get { return TrusteeForm == TRUSTEE_FORM.TRUSTEE_IS_SID ? PSID.Copy(ptstrName) : null; }
				set { ((IDisposable)this).Dispose(); TrusteeForm = TRUSTEE_FORM.TRUSTEE_IS_SID; ptstrName = value.Clone(); }
			}
		}
	}
}