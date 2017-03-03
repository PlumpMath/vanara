using System;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class NetApi32
	{
		public const int MAX_PREFERRED_LENGTH = -1;

		public enum DomainControllerAddressType
		{
			/// <summary>The address is a string IP address (for example, "\\157.55.94.74") of the domain controller.</summary>
			DS_INET_ADDRESS = 1,
			/// <summary>The address is a NetBIOS name, for example, "\\phoenix", of the domain controller.</summary>
			DS_NETBIOS_ADDRESS = 2
		}

		[Flags]
		public enum DsGetDcNameFlags : uint
		{
			/// <summary>When called from a domain controller, specifies that the returned domain controller name should not be the current computer. If the current computer is not a domain controller, this flag is ignored. This flag can be used to obtain the name of another domain controller in the domain.</summary>
			DS_AVOID_SELF = 0x4000,
			/// <summary>If the DS_FORCE_REDISCOVERY flag is not specified, this function uses cached domain controller data. If the cached data is more than 15 minutes old, the cache is refreshed by pinging the domain controller. If this flag is specified, this refresh is avoided even if the cached data is expired. This flag should be used if the DsGetDcName function is called periodically.</summary>
			DS_BACKGROUND_ONLY = 0x100,
			/// <summary>DsGetDcName attempts to find a domain controller that supports directory service functions. If a domain controller that supports directory services is not available, DsGetDcName returns the name of a non-directory service domain controller. However, DsGetDcName only returns a non-directory service domain controller after the attempt to find a directory service domain controller times out.</summary>
			DS_DIRECTORY_SERVICE_PREFERRED = 0x20,
			/// <summary>Requires that the returned domain controller support directory services.</summary>
			DS_DIRECTORY_SERVICE_REQUIRED = 0x10,
			/// <summary>Requires that the returned domain controller be running Windows Server 2008 or later.</summary>
			DS_DIRECTORY_SERVICE_6_REQUIRED = 0x80000,
			/// <summary>Requires that the returned domain controller be running Windows Server 2012 or later.</summary>
			DS_DIRECTORY_SERVICE_8_REQUIRED = 0x00200000,
			/// <summary>Requires that the returned domain controller be running Windows Server 2012 R2 or later.</summary>
			DS_DIRECTORY_SERVICE_9_REQUIRED = 0x00400000,
			/// <summary>Requires that the returned domain controller be running Windows Server 2016 or later.</summary>
			DS_DIRECTORY_SERVICE_10_REQUIRED = 0x00800000,
			/// <summary>Forces cached domain controller data to be ignored. When the DS_FORCE_REDISCOVERY flag is not specified, DsGetDcName may return cached domain controller data. If this flag is specified, DsGetDcName will not use cached information (if any exists) but will instead perform a fresh domain controller discovery.
			/// <para>This flag should not be used under normal conditions, as using the cached domain controller information has better performance characteristics and helps to ensure that the same domain controller is used consistently by all applications. This flag should be used only after the application determines that the domain controller returned by DsGetDcName (when called without this flag) is not accessible. In that case, the application should repeat the DsGetDcName call with this flag to ensure that the unuseful cached information (if any) is ignored and a reachable domain controller is discovered.</para></summary>
			DS_FORCE_REDISCOVERY = 0x01,
			/// <summary>Requires that the returned domain controller be a global catalog server for the forest of domains with this domain as the root. If this flag is set and the DomainName parameter is not NULL, DomainName must specify a forest name. This flag cannot be combined with the DS_PDC_REQUIRED or DS_KDC_REQUIRED flags.</summary>
			DS_GC_SERVER_REQUIRED = 0x40,
			/// <summary>DsGetDcName attempts to find a domain controller that is a reliable time server. The Windows Time Service can be configured to declare one or more domain controllers as a reliable time server. For more information, see the Windows Time Service documentation. This flag is intended to be used only by the Windows Time Service.</summary>
			DS_GOOD_TIMESERV_PREFERRED = 0x2000,
			/// <summary>This parameter indicates that the domain controller must have an IP address. In that case, DsGetDcName will place the Internet protocol address of the domain controller in the DomainControllerAddress member of DomainControllerInfo.</summary>
			DS_IP_REQUIRED = 0x200,
			/// <summary>Specifies that the DomainName parameter is a DNS name. This flag cannot be combined with the DS_IS_FLAT_NAME flag.
			/// <para>Specify either DS_IS_DNS_NAME or DS_IS_FLAT_NAME. If neither flag is specified, DsGetDcName may take longer to find a domain controller because it may have to search for both the DNS-style and flat name.</para></summary>
			DS_IS_DNS_NAME = 0x20000,
			/// <summary>Specifies that the DomainName parameter is a flat name. This flag cannot be combined with the DS_IS_DNS_NAME flag.</summary>
			DS_IS_FLAT_NAME = 0x10000,
			/// <summary>Requires that the returned domain controller be currently running the Kerberos Key Distribution Center service. This flag cannot be combined with the DS_PDC_REQUIRED or DS_GC_SERVER_REQUIRED flags.</summary>
			DS_KDC_REQUIRED = 0x400,
			/// <summary>Specifies that the server returned is an LDAP server. The server returned is not necessarily a domain controller. No other services are implied to be present at the server. The server returned does not necessarily have a writable config container nor a writable schema container. The server returned may not necessarily be used to create or modify security principles. This flag may be used with the DS_GC_SERVER_REQUIRED flag to return an LDAP server that also hosts a global catalog server. The returned global catalog server is not necessarily a domain controller. No other services are implied to be present at the server. If this flag is specified, the DS_PDC_REQUIRED, DS_TIMESERV_REQUIRED, DS_GOOD_TIMESERV_PREFERRED, DS_DIRECTORY_SERVICES_PREFERED, DS_DIRECTORY_SERVICES_REQUIRED, and DS_KDC_REQUIRED flags are ignored.</summary>
			DS_ONLY_LDAP_NEEDED = 0x8000,
			/// <summary>Requires that the returned domain controller be the primary domain controller for the domain. This flag cannot be combined with the DS_KDC_REQUIRED or DS_GC_SERVER_REQUIRED flags.</summary>
			DS_PDC_REQUIRED = 0x80,
			/// <summary>Specifies that the names returned in the DomainControllerName and DomainName members of DomainControllerInfo should be DNS names. If a DNS name is not available, an error is returned. This flag cannot be specified with the DS_RETURN_FLAT_NAME flag. This flag implies the DS_IP_REQUIRED flag.</summary>
			DS_RETURN_DNS_NAME = 0x40000000,
			/// <summary>Specifies that the names returned in the DomainControllerName and DomainName members of DomainControllerInfo should be flat names. If a flat name is not available, an error is returned. This flag cannot be specified with the DS_RETURN_DNS_NAME flag.</summary>
			DS_RETURN_FLAT_NAME = 0x80000000,
			/// <summary>Requires that the returned domain controller be currently running the Windows Time Service.</summary>
			DS_TIMESERV_REQUIRED = 0x800,
			/// <summary>When this flag is specified, DsGetDcName attempts to find a domain controller in the same site as the caller. If no such domain controller is found, it will find a domain controller that can provide topology information and call DsBindToISTG to obtain a bind handle, then call DsQuerySitesByCost over UDP to determine the "next closest site," and finally cache the name of the site found. If no domain controller is found in that site, then DsGetDcName falls back on the default method of locating a domain controller.
			/// <para>If this flag is used in conjunction with a non-NULL value in the input parameter SiteName, then ERROR_INVALID_FLAGS is thrown.</para>
			/// <para>Also, the kind of search employed with DS_TRY_NEXT_CLOSEST_SITE is site-specific, so this flag is ignored if it is used in conjunction with DS_PDC_REQUIRED. Finally, DS_TRY_NEXTCLOSEST_SITE is ignored when used in conjunction with DS_RETURN_FLAT_NAME because that uses NetBIOS to resolve the name, but the domain of the domain controller found won't necessarily match the domain to which the client is joined.</para>
			/// <note>Note  This flag is Group Policy enabled. If you enable the "Next Closest Site" policy setting, Next Closest Site DC Location will be turned on for the machine across all available but un-configured network adapters. If you disable the policy setting, Next Closest Site DC Location will not be used by default for the machine across all available but un-configured network adapters. However, if a DC Locator call is made using the DS_TRY_NEXTCLOSEST_SITE flag explicitly, DsGetDcName honors the Next Closest Site behavior. If you do not configure this policy setting, Next Closest Site DC Location will be not be used by default for the machine across all available but un-configured network adapters. If the DS_TRY_NEXTCLOSEST_SITE flag is used explicitly, the Next Closest Site behavior will be used.</note>
			/// </summary>
			DS_TRY_NEXTCLOSEST_SITE = 0x40000,
			/// <summary>Requires that the returned domain controller be writable; that is, host a writable copy of the directory service.</summary>
			DS_WRITABLE_REQUIRED = 0x1000,
			/// <summary>Requires that the returned domain controller be currently running the Active Directory web service.</summary>
			DS_WEB_SERVICE_REQUIRED = 0x00100000,
		}

		/// <summary>
		/// The information level to use for platform-specific information.
		/// </summary>
		public enum ServerPlatform
		{
			/// <summary>The MS-DOS platform.</summary>
			PLATFORM_ID_DOS = 300,
			/// <summary>The OS/2 platform.</summary>
			PLATFORM_ID_OS2 = 400,
			/// <summary>The Windows NT platform.</summary>
			PLATFORM_ID_NT = 500,
			/// <summary>The OSF platform.</summary>
			PLATFORM_ID_OSF = 600,
			/// <summary>The VMS platform.</summary>
			PLATFORM_ID_VMS = 700
		}

		[Flags]
		public enum NetServerEnumFilter : uint
		{
			SV_TYPE_WORKSTATION = 0x00000001,
			SV_TYPE_SERVER = 0x00000002,
			SV_TYPE_SQLSERVER = 0x00000004,
			SV_TYPE_DOMAINCTRL = 0x00000008,
			SV_TYPE_BACKUPDOMAINCTRL = 0x00000010,
			SV_TYPE_TIMESOURCE = 0x00000020,
			SV_TYPE_APPLEFILINGPROTOCOL = 0x00000040,
			SV_TYPE_NOVELL = 0x00000080,
			SV_TYPE_DOMAINMEMBER = 0x00000100,
			SV_TYPE_PRINTQUEUESERVER = 0x00000200,
			SV_TYPE_DIALINSERVER = 0x00000400,
			SV_TYPE_XENIXSERVER = 0x00000800,
			SV_TYPE_UNIXSERVER = 0x00000800,
			SV_TYPE_NT = 0x00001000,
			SV_TYPE_WINDOWSFORWORKGROUPS = 0x00002000,
			SV_TYPE_MICROSOFTFILEANDPRINTSERVER = 0x00004000,
			SV_TYPE_NTSERVER = 0x00008000,
			SV_TYPE_BROWSERSERVICE = 0x00010000,
			SV_TYPE_BACKUPBROWSERSERVICE = 0x00020000,
			SV_TYPE_MASTERBROWSERSERVICE = 0x00040000,
			SV_TYPE_DOMAINMASTER = 0x00080000,
			SV_TYPE_OSF1SERVER = 0x00100000,
			SV_TYPE_VMSSERVER = 0x00200000,
			SV_TYPE_WINDOWS = 0x00400000,
			SV_TYPE_DFS = 0x00800000,
			SV_TYPE_NTCLUSTER = 0x01000000,
			SV_TYPE_TERMINALSERVER = 0x02000000,
			SV_TYPE_VIRTUALNTCLUSTER = 0x04000000,
			SV_TYPE_DCE = 0x10000000,
			SV_TYPE_ALTERNATETRANSPORT = 0x20000000,
			SV_TYPE_LOCALLISTONLY = 0x40000000,
			SV_TYPE_PRIMARYDOMAIN = 0x80000000,
			SV_TYPE_ALL = 0xFFFFFFFF
		};

		/// <summary>
		/// The DsGetDcName function returns the name of a domain controller in a specified domain. This function accepts additional domain controller selection
		/// criteria to indicate preference for a domain controller with particular characteristics.
		/// </summary>
		/// <param name="ComputerName">
		/// Pointer to a null-terminated string that specifies the name of the server to process this function. Typically, this parameter is NULL, which
		/// indicates that the local computer is used.
		/// </param>
		/// <param name="DomainName">
		/// Pointer to a null-terminated string that specifies the name of the domain or application partition to query. This name can either be a DNS style
		/// name, for example, fabrikam.com, or a flat-style name, for example, Fabrikam. If a DNS style name is specified, the name may be specified with or
		/// without a trailing period.
		/// <para>
		/// If the Flags parameter contains the DS_GC_SERVER_REQUIRED flag, DomainName must be the name of the forest. In this case, DsGetDcName fails if
		/// DomainName specifies a name that is not the forest root.
		/// </para>
		/// <para>
		/// If the Flags parameter contains the DS_GC_SERVER_REQUIRED flag and DomainName is NULL, DsGetDcName attempts to find a global catalog in the forest of
		/// the computer identified by ComputerName, which is the local computer if ComputerName is NULL.
		/// </para>
		/// <para>
		/// If DomainName is NULL and the Flags parameter does not contain the DS_GC_SERVER_REQUIRED flag, ComputerName is set to the default domain name of the
		/// primary domain of the computer identified by ComputerName.
		/// </para>
		/// </param>
		/// <param name="DomainGuid">
		/// Pointer to a GUID structure that specifies the GUID of the domain queried. If DomainGuid is not NULL and the domain specified by DomainName or
		/// ComputerName cannot be found, DsGetDcName attempts to locate a domain controller in the domain having the GUID specified by DomainGuid.
		/// </param>
		/// <param name="SiteName">
		/// Pointer to a null-terminated string that specifies the name of the site where the returned domain controller should physically exist. If this
		/// parameter is NULL, DsGetDcName attempts to return a domain controller in the site closest to the site of the computer specified by ComputerName. This
		/// parameter should be NULL, by default.
		/// </param>
		/// <param name="Flags">
		/// Contains a set of flags that provide additional data used to process the request. This parameter can be a combination of the following values.
		/// </param>
		/// <param name="DomainControllerInfo">
		/// Pointer to a PDOMAIN_CONTROLLER_INFO value that receives a pointer to a DOMAIN_CONTROLLER_INFO structure that contains data about the domain
		/// controller selected. This structure is allocated by DsGetDcName. The caller must free the structure using the NetApiBufferFree function when it is no
		/// longer required.
		/// </param>
		/// <returns>
		/// If the function returns domain controller data, the return value is ERROR_SUCCESS.
		/// <para>If the function fails, the return value can be one of the following error codes.</para>
		/// </returns>
		[DllImport(nameof(NetApi32), ExactSpelling = true, CharSet = CharSet.Auto)]
		public static extern uint DsGetDcName(string ComputerName, string DomainName, IntPtr DomainGuid, string SiteName, DsGetDcNameFlags Flags, out SafeNetApiBuffer DomainControllerInfo);

		/// <summary>
		/// The NetApiBufferFree function frees the memory that the NetApiBufferAllocate function allocates. Applications should also call NetApiBufferFree to free the memory that other network management functions use internally to return information.
		/// </summary>
		/// <param name="pBuf">A pointer to a buffer returned previously by another network management function or memory allocated by calling the NetApiBufferAllocate function.</param>
		/// <returns>If the function succeeds, the return value is NERR_Success. If the function fails, the return value is a system error code.</returns>
		[DllImport(nameof(NetApi32), ExactSpelling = true), SuppressUnmanagedCodeSecurity]
		public static extern int NetApiBufferFree(IntPtr pBuf);

		/// <summary>
		/// The NetServerEnum function lists all servers of the specified type that are visible in a domain.
		/// </summary>
		/// <param name="servernane">Reserved; must be NULL.</param>
		/// <param name="level">The information level of the data requested.</param>
		/// <param name="bufptr">A pointer to the buffer that receives the data. The format of this data depends on the value of the level parameter. This buffer is allocated by the system and must be freed using the NetApiBufferFree function. Note that you must free the buffer even if the function fails with ERROR_MORE_DATA.</param>
		/// <param name="prefmaxlen">The preferred maximum length of returned data, in bytes. If you specify MAX_PREFERRED_LENGTH, the function allocates the amount of memory required for the data. If you specify another value in this parameter, it can restrict the number of bytes that the function returns. If the buffer size is insufficient to hold all entries, the function returns ERROR_MORE_DATA. For more information, see Network Management Function Buffers and Network Management Function Buffer Lengths.</param>
		/// <param name="entriesread">A pointer to a value that receives the count of elements actually enumerated.</param>
		/// <param name="totalentries">A pointer to a value that receives the total number of visible servers and workstations on the network. Note that applications should consider this value only as a hint.</param>
		/// <param name="servertype">A value that filters the server entries to return from the enumeration.</param>
		/// <param name="domain">A pointer to a constant string that specifies the name of the domain for which a list of servers is to be returned. The domain name must be a NetBIOS domain name (for example, microsoft). The NetServerEnum function does not support DNS-style names (for example, microsoft.com). If this parameter is NULL, the primary domain is implied.</param>
		/// <param name="resume_handle">Reserved; must be set to zero.</param>
		/// <returns>If the function succeeds, the return value is NERR_Success.</returns>
		[DllImport(nameof(NetApi32), ExactSpelling = true, CharSet = CharSet.Unicode), SuppressUnmanagedCodeSecurity]
		public static extern int NetServerEnum(
			[MarshalAs(UnmanagedType.LPWStr)] string servernane, // must be null
			int level,
			out SafeNetApiBuffer bufptr,
			int prefmaxlen,
			out int entriesread,
			out int totalentries,
			NetServerEnumFilter servertype,
			[MarshalAs(UnmanagedType.LPWStr)] string domain, // null for login domain
			IntPtr resume_handle // Must be IntPtr.Zero
			);

		/// <summary>
		/// The NetServerGetInfo function retrieves current configuration information for the specified server.
		/// </summary>
		/// <param name="servername">Pointer to a string that specifies the name of the remote server on which the function is to execute. If this parameter is NULL, the local computer is used.</param>
		/// <param name="level">Specifies the information level of the data.</param>
		/// <param name="bufptr">Pointer to the buffer that receives the data. The format of this data depends on the value of the level parameter. This buffer is allocated by the system and must be freed using the NetApiBufferFree function.</param>
		/// <returns>If the function succeeds, the return value is NERR_Success.</returns>
		[DllImport(nameof(NetApi32), ExactSpelling = true)]
		public static extern int NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string servername, int level, out SafeNetApiBuffer bufptr);

		/// <summary>
		/// The DOMAIN_CONTROLLER_INFO structure is used with the DsGetDcName function to receive data about a domain controller.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DOMAIN_CONTROLLER_INFO
		{
			/// <summary>
			/// Pointer to a null-terminated string that specifies the computer name of the discovered domain controller. The returned computer name is prefixed with "\\". The DNS-style name, for example, "\\phoenix.fabrikam.com", is returned, if available. If the DNS-style name is not available, the flat-style name (for example, "\\phoenix") is returned. This example would apply if the domain is a Windows NT 4.0 domain or if the domain does not support the IP family of protocols.
			/// </summary>
			public string DomainControllerName;
			/// <summary>
			/// Pointer to a null-terminated string that specifies the address of the discovered domain controller. The address is prefixed with "\\". This string is one of the types defined by the DomainControllerAddressType member.
			/// </summary>
			public string DomainControllerAddress;
			/// <summary>
			/// Indicates the type of string that is contained in the DomainControllerAddress member. 
			/// </summary>
			public DomainControllerAddressType DomainControllerAddressType;
			/// <summary>
			/// The GUID of the domain. This member is zero if the domain controller does not have a Domain GUID; for example, the domain controller is not a Windows 2000 domain controller.
			/// </summary>
			public Guid DomainGuid;
			/// <summary>
			/// Pointer to a null-terminated string that specifies the name of the domain. The DNS-style name, for example, "fabrikam.com", is returned if available. Otherwise, the flat-style name, for example, "fabrikam", is returned. This name may be different than the requested domain name if the domain has been renamed.
			/// </summary>
			public string DomainName;
			/// <summary>
			/// Pointer to a null-terminated string that specifies the name of the domain at the root of the DS tree. The DNS-style name, for example, "fabrikam.com", is returned if available. Otherwise, the flat-style name, for example, "fabrikam" is returned.
			/// </summary>
			public string DnsForestName;
			/// <summary>
			/// Contains a set of flags that describe the domain controller. This can be zero or a combination of one or more of the following values.
			/// </summary>
			public DsGetDcNameFlags Flags;
			/// <summary>
			/// Pointer to a null-terminated string that specifies the name of the site where the domain controller is located. This member may be NULL if the domain controller is not in a site; for example, the domain controller is a Windows NT 4.0 domain controller.
			/// </summary>
			public string DcSiteName;
			/// <summary>
			/// Pointer to a null-terminated string that specifies the name of the site that the computer belongs to. The computer is specified in the ComputerName parameter passed to DsGetDcName. This member may be NULL if the site that contains the computer cannot be found; for example, if the DS administrator has not associated the subnet that the computer is in with a valid site.
			/// </summary>
			public string ClientSiteName;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SERVER_INFO_100 : INetServerInfo
		{
			public ServerPlatform sv100_platform_id;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv100_name;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SERVER_INFO_101 : INetServerInfo
		{
			public ServerPlatform sv101_platform_id;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv101_name;
			public int sv101_version_major;
			public int sv101_version_minor;
			public NetServerEnumFilter sv101_type;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv101_comment;

			public Version Version => new Version(sv101_version_major, sv101_version_minor);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SERVER_INFO_102 : INetServerInfo
		{
			public ServerPlatform sv102_platform_id;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv102_name;
			public int sv102_version_major;
			public int sv102_version_minor;
			public NetServerEnumFilter sv102_type;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv102_comment;
			public int sv102_users;
			public int sv102_disc;
			[MarshalAs(UnmanagedType.Bool)]
			public bool sv102_hidden;
			public int sv102_announce;
			public int sv102_anndelta;
			public int sv102_licenses;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string sv102_userpath;

			public Version Version => new Version(sv102_version_major, sv102_version_minor);
		}

		public class SafeNetApiBuffer : GenericSafeHandle
		{
			public SafeNetApiBuffer() : base(NetApiBufferFreeWithChk) { }

			public SafeNetApiBuffer(IntPtr ptr, bool own = true) : base(ptr, NetApiBufferFreeWithChk, own) { }

			private static bool NetApiBufferFreeWithChk(IntPtr buffer) => NetApiBufferFree(buffer) == 0;
		}
	}
}