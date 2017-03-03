using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.Extensions;
using static Vanara.PInvoke.Windows;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming ReSharper disable FieldCanBeMadeReadOnly.Global ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class AdvApi32
	{
		[Flags]
		public enum PrivilegeAttributes : uint
		{
			/// <summary>The privilege is disabled.</summary>
			SE_PRIVILEGE_DISABLED = 0x00000000,
			/// <summary>The privilege is enabled by default.</summary>
			SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001,
			/// <summary>The privilege is enabled.</summary>
			SE_PRIVILEGE_ENABLED = 0x00000002,
			/// <summary>Used to remove a privilege. The other privileges in the list are reordered to remain contiguous.</summary>
			SE_PRIVILEGE_REMOVED = 0x00000004,
			/// <summary>
			/// The privilege was used to gain access to an object or service. This flag is used to identify the relevant privileges in a set passed by a client
			/// application that may contain unnecessary privileges.
			/// </summary>
			SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000
		}

		public enum PrivilegeSetControl
		{
			/// <summary>The presence of any privileges in the user's access token grants the access.</summary>
			None = 0,
			/// <summary>Indicates that all of the specified privileges must be held by the process requesting access.</summary>
			PRIVILEGE_SET_ALL_NECESSARY = 1
		}

		/// <summary>
		/// The SECURITY_IMPERSONATION_LEVEL enumeration contains values that specify security impersonation levels. Security impersonation levels govern the
		/// degree to which a server process can act on behalf of a client process.
		/// </summary>
		public enum SECURITY_IMPERSONATION_LEVEL
		{
			/// <summary>
			/// The server process cannot obtain identification information about the client, and it cannot impersonate the client. It is defined with no value
			/// given, and thus, by ANSI C rules, defaults to a value of zero.
			/// </summary>
			SecurityAnonymous,

			/// <summary>
			/// The server process can obtain information about the client, such as security identifiers and privileges, but it cannot impersonate the client.
			/// This is useful for servers that export their own objects, for example, database products that export tables and views. Using the retrieved
			/// client-security information, the server can make access-validation decisions without being able to use other services that are using the client's
			/// security context.
			/// </summary>
			SecurityIdentification,

			/// <summary>
			/// The server process can impersonate the client's security context on its local system. The server cannot impersonate the client on remote systems.
			/// </summary>
			SecurityImpersonation,

			/// <summary>The server process can impersonate the client's security context on remote systems.</summary>
			SecurityDelegation
		}

		/// <summary>The TOKEN_ELEVATION_TYPE enumeration indicates the elevation type of token being queried by the GetTokenInformation function.</summary>
		public enum TOKEN_ELEVATION_TYPE
		{
			/// <summary>The token does not have a linked token.</summary>
			TokenElevationTypeDefault = 1,

			/// <summary>The token is an elevated token.</summary>
			TokenElevationTypeFull,

			/// <summary>The token is a limited token.</summary>
			TokenElevationTypeLimited
		}

		/// <summary>
		/// The TOKEN_INFORMATION_CLASS enumeration contains values that specify the type of information being assigned to or retrieved from an access token.
		/// <para>The GetTokenInformation function uses these values to indicate the type of token information to retrieve.</para>
		/// <para>The SetTokenInformation function uses these values to set the token information.</para>
		/// </summary>
		public enum TOKEN_INFORMATION_CLASS
		{
			/// <summary>The buffer receives a TOKEN_USER structure that contains the user account of the token.</summary>
			TokenUser = 1,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that contains the group accounts associated with the token.</summary>
			TokenGroups,

			/// <summary>The buffer receives a TOKEN_PRIVILEGES structure that contains the privileges of the token.</summary>
			TokenPrivileges,

			/// <summary>The buffer receives a TOKEN_OWNER structure that contains the default owner security identifier (SID) for newly created objects.</summary>
			TokenOwner,

			/// <summary>The buffer receives a TOKEN_PRIMARY_GROUP structure that contains the default primary group SID for newly created objects.</summary>
			TokenPrimaryGroup,

			/// <summary>The buffer receives a TOKEN_DEFAULT_DACL structure that contains the default DACL for newly created objects.</summary>
			TokenDefaultDacl,

			/// <summary>
			/// The buffer receives a TOKEN_SOURCE structure that contains the source of the token. TOKEN_QUERY_SOURCE access is needed to retrieve this information.
			/// </summary>
			TokenSource,

			/// <summary>The buffer receives a TOKEN_TYPE value that indicates whether the token is a primary or impersonation token.</summary>
			TokenType,

			/// <summary>
			/// The buffer receives a SECURITY_IMPERSONATION_LEVEL value that indicates the impersonation level of the token. If the access token is not an
			/// impersonation token, the function fails.
			/// </summary>
			TokenImpersonationLevel,

			/// <summary>The buffer receives a TOKEN_STATISTICS structure that contains various token statistics.</summary>
			TokenStatistics,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that contains the list of restricting SIDs in a restricted token.</summary>
			TokenRestrictedSids,

			/// <summary>
			/// The buffer receives a DWORD value that indicates the Terminal Services session identifier that is associated with the token.
			/// <para>If the token is associated with the terminal server client session, the session identifier is nonzero.</para>
			/// <para>Windows Server 2003 and Windows XP: If the token is associated with the terminal server console session, the session identifier is zero.</para>
			/// <para>In a non-Terminal Services environment, the session identifier is zero.</para>
			/// <para>
			/// If TokenSessionId is set with SetTokenInformation, the application must have the Act As Part Of the Operating System privilege, and the
			/// application must be enabled to set the session ID in a token.
			/// </para>
			/// </summary>
			TokenSessionId,

			/// <summary>
			/// The buffer receives a TOKEN_GROUPS_AND_PRIVILEGES structure that contains the user SID, the group accounts, the restricted SIDs, and the
			/// authentication ID associated with the token.
			/// </summary>
			TokenGroupsAndPrivileges,

			/// <summary>Reserved.</summary>
			TokenSessionReference,

			/// <summary>The buffer receives a DWORD value that is nonzero if the token includes the SANDBOX_INERT flag.</summary>
			TokenSandBoxInert,

			/// <summary>Reserved.</summary>
			TokenAuditPolicy,

			/// <summary>
			/// The buffer receives a TOKEN_ORIGIN value.
			/// <para>
			/// If the token resulted from a logon that used explicit credentials, such as passing a name, domain, and password to the LogonUser function, then
			/// the TOKEN_ORIGIN structure will contain the ID of the logon session that created it.
			/// </para>
			/// <para>
			/// If the token resulted from network authentication, such as a call to AcceptSecurityContext or a call to LogonUser with dwLogonType set to
			/// LOGON32_LOGON_NETWORK or LOGON32_LOGON_NETWORK_CLEARTEXT, then this value will be zero.
			/// </para>
			/// </summary>
			TokenOrigin,

			/// <summary>The buffer receives a TOKEN_ELEVATION_TYPE value that specifies the elevation level of the token.</summary>
			TokenElevationType,

			/// <summary>The buffer receives a TOKEN_LINKED_TOKEN structure that contains a handle to another token that is linked to this token.</summary>
			TokenLinkedToken,

			/// <summary>The buffer receives a TOKEN_ELEVATION structure that specifies whether the token is elevated.</summary>
			TokenElevation,

			/// <summary>The buffer receives a DWORD value that is nonzero if the token has ever been filtered.</summary>
			TokenHasRestrictions,

			/// <summary>The buffer receives a TOKEN_ACCESS_INFORMATION structure that specifies security information contained in the token.</summary>
			TokenAccessInformation,

			/// <summary>The buffer receives a DWORD value that is nonzero if virtualization is allowed for the token.</summary>
			TokenVirtualizationAllowed,

			/// <summary>The buffer receives a DWORD value that is nonzero if virtualization is enabled for the token.</summary>
			TokenVirtualizationEnabled,

			/// <summary>The buffer receives a TOKEN_MANDATORY_LABEL structure that specifies the token's integrity level.</summary>
			TokenIntegrityLevel,

			/// <summary>The buffer receives a DWORD value that is nonzero if the token has the UIAccess flag set.</summary>
			TokenUIAccess,

			/// <summary>The buffer receives a TOKEN_MANDATORY_POLICY structure that specifies the token's mandatory integrity policy.</summary>
			TokenMandatoryPolicy,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that specifies the token's logon SID.</summary>
			TokenLogonSid,

			/// <summary>
			/// The buffer receives a DWORD value that is nonzero if the token is an application container token. Any callers who check the TokenIsAppContainer
			/// and have it return 0 should also verify that the caller token is not an identify level impersonation token. If the current token is not an
			/// application container but is an identity level token, you should return AccessDenied.
			/// </summary>
			TokenIsAppContainer,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that contains the capabilities associated with the token.</summary>
			TokenCapabilities,

			/// <summary>
			/// The buffer receives a TOKEN_APPCONTAINER_INFORMATION structure that contains the AppContainerSid associated with the token. If the token is not
			/// associated with an application container, the TokenAppContainer member of the TOKEN_APPCONTAINER_INFORMATION structure points to NULL.
			/// </summary>
			TokenAppContainerSid,

			/// <summary>
			/// The buffer receives a DWORD value that includes the application container number for the token. For tokens that are not application container
			/// tokens, this value is zero.
			/// </summary>
			TokenAppContainerNumber,

			/// <summary>The buffer receives a CLAIM_SECURITY_ATTRIBUTES_INFORMATION structure that contains the user claims associated with the token.</summary>
			TokenUserClaimAttributes,

			/// <summary>The buffer receives a CLAIM_SECURITY_ATTRIBUTES_INFORMATION structure that contains the device claims associated with the token.</summary>
			TokenDeviceClaimAttributes,

			/// <summary>This value is reserved.</summary>
			TokenRestrictedUserClaimAttributes,

			/// <summary>This value is reserved.</summary>
			TokenRestrictedDeviceClaimAttributes,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that contains the device groups that are associated with the token.</summary>
			TokenDeviceGroups,

			/// <summary>The buffer receives a TOKEN_GROUPS structure that contains the restricted device groups that are associated with the token.</summary>
			TokenRestrictedDeviceGroups,

			/// <summary>This value is reserved.</summary>
			TokenSecurityAttributes,

			/// <summary>This value is reserved.</summary>
			TokenIsRestricted
		}

		/// <summary>The TOKEN_TYPE enumeration contains values that differentiate between a primary token and an impersonation token.</summary>
		[Serializable]
		public enum TOKEN_TYPE
		{
			/// <summary>Indicates a primary token.</summary>
			TokenPrimary = 1,

			/// <summary>Indicates an impersonation token.</summary>
			TokenImpersonation = 2
		}

		/// <summary>
		/// The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token
		/// requires TOKEN_ADJUST_PRIVILEGES access.
		/// </summary>
		/// <param name="objectHandle">
		/// A handle to the access token that contains the privileges to be modified. The handle must have TOKEN_ADJUST_PRIVILEGES access to the token. If the
		/// PreviousState parameter is not NULL, the handle must also have TOKEN_QUERY access.
		/// </param>
		/// <param name="DisableAllPrivileges">
		/// Specifies whether the function disables all of the token's privileges. If this value is TRUE, the function disables all privileges and ignores the
		/// NewState parameter. If it is FALSE, the function modifies privileges based on the information pointed to by the NewState parameter.
		/// </param>
		/// <param name="NewState">
		/// A pointer to a TOKEN_PRIVILEGES structure that specifies an array of privileges and their attributes. If DisableAllPrivileges is TRUE, the function
		/// ignores this parameter. If the DisableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function enables, disables, or removes these
		/// privileges for the token. The following table describes the action taken by the AdjustTokenPrivileges function, based on the privilege attribute.
		/// </param>
		/// <param name="BufferLength">
		/// Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be zero if the PreviousState parameter is NULL.
		/// </param>
		/// <param name="PreviousState">
		/// A pointer to a buffer that the function fills with a TOKEN_PRIVILEGES structure that contains the previous state of any privileges that the function
		/// modifies. That is, if a privilege has been modified by this function, the privilege and its previous state are contained in the TOKEN_PRIVILEGES
		/// structure referenced by PreviousState. If the PrivilegeCount member of TOKEN_PRIVILEGES is zero, then no privileges have been changed by this
		/// function. This parameter can be NULL.
		/// <para>
		/// If you specify a buffer that is too small to receive the complete list of modified privileges, the function fails and does not adjust any privileges.
		/// In this case, the function sets the variable pointed to by the ReturnLength parameter to the number of bytes required to hold the complete list of
		/// modified privileges.
		/// </para>
		/// </param>
		/// <param name="ReturnLength">
		/// A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be
		/// NULL if PreviousState is NULL.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. To determine whether the function adjusted all of the specified privileges, call GetLastError,
		/// which returns either ERROR_SUCCESS, indicating that the function adjusted all specified privileges, or ERROR_NOT_ALL_ASSIGNED, indicating that the
		/// token does not have one or more of the privileges specified in the NewState parameter. The function may succeed with this error value even if no
		/// privileges were adjusted. The PreviousState parameter indicates the privileges that were adjusted.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustTokenPrivileges([In] SafeTokenHandle objectHandle,
			[In, MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(PTOKEN_PRIVILEGES.Marshaler))] PTOKEN_PRIVILEGES NewState,
			[In] uint BufferLength,
			[In, Out] SafeCoTaskMemHandle PreviousState,
			//[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(PTOKEN_PRIVILEGES.Marshaler), MarshalCookie = "Out")] PTOKEN_PRIVILEGES PreviousState,
			[In, Out] ref uint ReturnLength);

		/// <summary>
		/// The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token
		/// requires TOKEN_ADJUST_PRIVILEGES access.
		/// </summary>
		/// <param name="objectHandle">
		/// A handle to the access token that contains the privileges to be modified. The handle must have TOKEN_ADJUST_PRIVILEGES access to the token. If the
		/// PreviousState parameter is not NULL, the handle must also have TOKEN_QUERY access.
		/// </param>
		/// <param name="DisableAllPrivileges">
		/// Specifies whether the function disables all of the token's privileges. If this value is TRUE, the function disables all privileges and ignores the
		/// NewState parameter. If it is FALSE, the function modifies privileges based on the information pointed to by the NewState parameter.
		/// </param>
		/// <param name="NewState">
		/// A pointer to a TOKEN_PRIVILEGES structure that specifies an array of privileges and their attributes. If DisableAllPrivileges is TRUE, the function
		/// ignores this parameter. If the DisableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function enables, disables, or removes these
		/// privileges for the token. The following table describes the action taken by the AdjustTokenPrivileges function, based on the privilege attribute.
		/// </param>
		/// <param name="BufferLength">
		/// Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be zero if the PreviousState parameter is NULL.
		/// </param>
		/// <param name="PreviousState">
		/// A pointer to a buffer that the function fills with a TOKEN_PRIVILEGES structure that contains the previous state of any privileges that the function
		/// modifies. That is, if a privilege has been modified by this function, the privilege and its previous state are contained in the TOKEN_PRIVILEGES
		/// structure referenced by PreviousState. If the PrivilegeCount member of TOKEN_PRIVILEGES is zero, then no privileges have been changed by this
		/// function. This parameter can be NULL.
		/// <para>
		/// If you specify a buffer that is too small to receive the complete list of modified privileges, the function fails and does not adjust any privileges.
		/// In this case, the function sets the variable pointed to by the ReturnLength parameter to the number of bytes required to hold the complete list of
		/// modified privileges.
		/// </para>
		/// </param>
		/// <param name="ReturnLength">
		/// A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be
		/// NULL if PreviousState is NULL.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. To determine whether the function adjusted all of the specified privileges, call GetLastError,
		/// which returns either ERROR_SUCCESS, indicating that the function adjusted all specified privileges, or ERROR_NOT_ALL_ASSIGNED, indicating that the
		/// token does not have one or more of the privileges specified in the NewState parameter. The function may succeed with this error value even if no
		/// privileges were adjusted. The PreviousState parameter indicates the privileges that were adjusted.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para>
		/// </returns>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustTokenPrivileges([In] SafeTokenHandle objectHandle,
			[In, MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, [In] SafeCoTaskMemHandle NewState,
			[In] uint BufferLength, [In, Out] SafeCoTaskMemHandle PreviousState, [In, Out] ref uint ReturnLength);

		/// <summary>The DuplicateToken function creates a new access token that duplicates one already in existence.</summary>
		/// <param name="existingObjectHandle">A handle to an access token opened with TOKEN_DUPLICATE access.</param>
		/// <param name="ImpersonationLevel">Specifies a SECURITY_IMPERSONATION_LEVEL enumerated type that supplies the impersonation level of the new token.</param>
		/// <param name="duplicateObjectHandle">
		/// A pointer to a variable that receives a handle to the duplicate token. This handle has TOKEN_IMPERSONATE and TOKEN_QUERY access to the new token.
		/// When you have finished using the new token, call the CloseHandle function to close the token handle.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DuplicateToken(SafeTokenHandle existingObjectHandle,
			SECURITY_IMPERSONATION_LEVEL ImpersonationLevel, out SafeTokenHandle duplicateObjectHandle);

		/// <summary>
		/// The DuplicateTokenEx function creates a new access token that duplicates an existing token. This function can create either a primary token or an
		/// impersonation token.
		/// </summary>
		/// <param name="hExistingToken">A handle to an access token opened with TOKEN_DUPLICATE access.</param>
		/// <param name="dwDesiredAccess">
		/// Specifies the requested access rights for the new token. The DuplicateTokenEx function compares the requested access rights with the existing token's
		/// discretionary access control list (DACL) to determine which rights are granted or denied. To request the same access rights as the existing token,
		/// specify zero. To request all access rights that are valid for the caller, specify MAXIMUM_ALLOWED.
		/// </param>
		/// <param name="lpTokenAttributes">
		/// A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new token and determines whether child processes can
		/// inherit the token. If lpTokenAttributes is NULL, the token gets a default security descriptor and the handle cannot be inherited. If the security
		/// descriptor contains a system access control list (SACL), the token gets ACCESS_SYSTEM_SECURITY access right, even if it was not requested in dwDesiredAccess.
		/// <para>To set the owner in the security descriptor for the new token, the caller's process token must have the SE_RESTORE_NAME privilege set.</para>
		/// </param>
		/// <param name="ImpersonationLevel">
		/// Specifies a value from the SECURITY_IMPERSONATION_LEVEL enumeration that indicates the impersonation level of the new token.
		/// </param>
		/// <param name="TokenType">Specifies one of the following values from the TOKEN_TYPE enumeration.</param>
		/// <param name="phNewToken">
		/// A pointer to a HANDLE variable that receives the new token. When you have finished using the new token, call the CloseHandle function to close the
		/// token handle.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DuplicateTokenEx(SafeTokenHandle hExistingToken, AccessTypes dwDesiredAccess,
			SECURITY_ATTRIBUTES lpTokenAttributes,
			SECURITY_IMPERSONATION_LEVEL ImpersonationLevel, TOKEN_TYPE TokenType, out SafeTokenHandle phNewToken);

		/// <summary>
		/// The GetTokenInformation function retrieves a specified type of information about an access token. The calling process must have appropriate access
		/// rights to obtain the information.
		/// </summary>
		/// <param name="hObject">
		/// A handle to an access token from which information is retrieved. If TokenInformationClass specifies TokenSource, the handle must have
		/// TOKEN_QUERY_SOURCE access. For all other TokenInformationClass values, the handle must have TOKEN_QUERY access.
		/// </param>
		/// <param name="tokenInfoClass">
		/// Specifies a value from the TOKEN_INFORMATION_CLASS enumerated type to identify the type of information the function retrieves. Any callers who check
		/// the TokenIsAppContainer and have it return 0 should also verify that the caller token is not an identify level impersonation token. If the current
		/// token is not an application container but is an identity level token, you should return AccessDenied.
		/// </param>
		/// <param name="pTokenInfo">
		/// A pointer to a buffer the function fills with the requested information. The structure put into this buffer depends upon the type of information
		/// specified by the TokenInformationClass parameter.
		/// </param>
		/// <param name="tokenInfoLength">
		/// Specifies the size, in bytes, of the buffer pointed to by the TokenInformation parameter. If TokenInformation is NULL, this parameter must be zero.
		/// </param>
		/// <param name="returnLength">
		/// A pointer to a variable that receives the number of bytes needed for the buffer pointed to by the TokenInformation parameter. If this value is larger
		/// than the value specified in the TokenInformationLength parameter, the function fails and stores no data in the buffer.
		/// <para>
		/// If the value of the TokenInformationClass parameter is TokenDefaultDacl and the token has no default DACL, the function sets the variable pointed to
		/// by ReturnLength to sizeof(TOKEN_DEFAULT_DACL) and sets the DefaultDacl member of the TOKEN_DEFAULT_DACL structure to NULL.
		/// </para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetTokenInformation(SafeTokenHandle hObject, TOKEN_INFORMATION_CLASS tokenInfoClass, SafeHGlobalHandle pTokenInfo, int tokenInfoLength, out int returnLength);

		/// <summary>
		/// The LookupPrivilegeName function retrieves the name that corresponds to the privilege represented on a specific system by a specified locally unique identifier (LUID).
		/// </summary>
		/// <param name="lpSystemName">A pointer to a null-terminated string that specifies the name of the system on which the privilege name is retrieved. If a null string is specified, the function attempts to find the privilege name on the local system.</param>
		/// <param name="lpLuid">A pointer to the LUID by which the privilege is known on the target system.</param>
		/// <param name="lpName">A pointer to a buffer that receives a null-terminated string that represents the privilege name. For example, this string could be "SeSecurityPrivilege".</param>
		/// <param name="cchName">A pointer to a variable that specifies the size, in a TCHAR value, of the lpName buffer. When the function returns, this parameter contains the length of the privilege name, not including the terminating null character. If the buffer pointed to by the lpName parameter is too small, this variable contains the required size.</param>
		/// <returns>If the function succeeds, the function returns nonzero. If the function fails, it returns zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeName(string lpSystemName, ref LUID lpLuid, System.Text.StringBuilder lpName, ref int cchName);

		/// <summary>
		/// The LookupPrivilegeValue function retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified
		/// privilege name.
		/// </summary>
		/// <param name="lpSystemName">
		/// A pointer to a null-terminated string that specifies the name of the system on which the privilege name is retrieved. If a null string is specified,
		/// the function attempts to find the privilege name on the local system.
		/// </param>
		/// <param name="lpName">
		/// A pointer to a null-terminated string that specifies the name of the privilege, as defined in the Winnt.h header file. For example, this parameter
		/// could specify the constant, SE_SECURITY_NAME, or its corresponding string, "SeSecurityPrivilege".
		/// </param>
		/// <param name="lpLuid">
		/// A pointer to a variable that receives the LUID by which the privilege is known on the system specified by the lpSystemName parameter.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

		/// <summary>The OpenProcessToken function opens the access token associated with a process.</summary>
		/// <param name="ProcessHandle">A handle to the process whose access token is opened. The process must have the PROCESS_QUERY_INFORMATION access permission.</param>
		/// <param name="DesiredAccess">
		/// Specifies an access mask that specifies the requested types of access to the access token. These requested access types are compared with the
		/// discretionary access control list (DACL) of the token to determine which accesses are granted or denied.
		/// </param>
		/// <param name="TokenHandle">A pointer to a handle that identifies the newly opened access token when the function returns.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken([In] IntPtr ProcessHandle, AccessTypes DesiredAccess, out SafeTokenHandle TokenHandle);

		/// <summary>The OpenThreadToken function opens the access token associated with a thread.</summary>
		/// <param name="ThreadHandle">A handle to the thread whose access token is opened.</param>
		/// <param name="DesiredAccess">
		/// Specifies an access mask that specifies the requested types of access to the access token. These requested access types are reconciled against the
		/// token's discretionary access control list (DACL) to determine which accesses are granted or denied.
		/// <para>For a list of access rights for access tokens, see Access Rights for Access-Token Objects.</para>
		/// </param>
		/// <param name="OpenAsSelf">
		/// TRUE if the access check is to be made against the process-level security context.
		/// <para>FALSE if the access check is to be made against the current security context of the thread calling the OpenThreadToken function.</para>
		/// <para>
		/// The OpenAsSelf parameter allows the caller of this function to open the access token of a specified thread when the caller is impersonating a token
		/// at SecurityIdentification level. Without this parameter, the calling thread cannot open the access token on the specified thread because it is
		/// impossible to open executive-level objects by using the SecurityIdentification impersonation level.
		/// </para>
		/// </param>
		/// <param name="TokenHandle">A pointer to a variable that receives the handle to the newly opened access token.</param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenThreadToken([In] IntPtr ThreadHandle, AccessTypes DesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool OpenAsSelf, out SafeTokenHandle TokenHandle);

		/// <summary>
		/// The PrivilegeCheck function determines whether a specified set of privileges are enabled in an access token. The PrivilegeCheck function is typically
		/// called by a server application to check the privileges of a client's access token.
		/// </summary>
		/// <param name="ClientToken">
		/// A handle to an access token representing a client process. This handle must have been obtained by opening the token of a thread impersonating the
		/// client. The token must be open for TOKEN_QUERY access.
		/// </param>
		/// <param name="RequiredPrivileges">
		/// A pointer to a PRIVILEGE_SET structure. The Privilege member of this structure is an array of LUID_AND_ATTRIBUTES structures. Before calling
		/// PrivilegeCheck, use the Privilege array to indicate the set of privileges to check. Set the Control member to PRIVILEGE_SET_ALL_NECESSARY if all of
		/// the privileges must be enabled; or set it to zero if it is sufficient that any one of the privileges be enabled.
		/// <para>
		/// When PrivilegeCheck returns, the Attributes member of each LUID_AND_ATTRIBUTES structure is set to SE_PRIVILEGE_USED_FOR_ACCESS if the corresponding
		/// privilege is enabled.
		/// </para>
		/// </param>
		/// <param name="pfResult">
		/// A pointer to a value the function sets to indicate whether any or all of the specified privileges are enabled in the access token. If the Control
		/// member of the PRIVILEGE_SET structure specifies PRIVILEGE_SET_ALL_NECESSARY, this value is TRUE only if all the privileges are enabled; otherwise,
		/// this value is TRUE if any of the privileges are enabled.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PrivilegeCheck(SafeTokenHandle ClientToken,
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(PRIVILEGE_SET.Marshaler))] PRIVILEGE_SET RequiredPrivileges,
			[MarshalAs(UnmanagedType.Bool)] out bool pfResult);

		/// <summary>
		/// The PrivilegeCheck function determines whether a specified set of privileges are enabled in an access token. The PrivilegeCheck function is typically
		/// called by a server application to check the privileges of a client's access token.
		/// </summary>
		/// <param name="ClientToken">
		/// A handle to an access token representing a client process. This handle must have been obtained by opening the token of a thread impersonating the
		/// client. The token must be open for TOKEN_QUERY access.
		/// </param>
		/// <param name="RequiredPrivileges">
		/// A pointer to a PRIVILEGE_SET structure. The Privilege member of this structure is an array of LUID_AND_ATTRIBUTES structures. Before calling
		/// PrivilegeCheck, use the Privilege array to indicate the set of privileges to check. Set the Control member to PRIVILEGE_SET_ALL_NECESSARY if all of
		/// the privileges must be enabled; or set it to zero if it is sufficient that any one of the privileges be enabled.
		/// <para>
		/// When PrivilegeCheck returns, the Attributes member of each LUID_AND_ATTRIBUTES structure is set to SE_PRIVILEGE_USED_FOR_ACCESS if the corresponding
		/// privilege is enabled.
		/// </para>
		/// </param>
		/// <param name="pfResult">
		/// A pointer to a value the function sets to indicate whether any or all of the specified privileges are enabled in the access token. If the Control
		/// member of the PRIVILEGE_SET structure specifies PRIVILEGE_SET_ALL_NECESSARY, this value is TRUE only if all the privileges are enabled; otherwise,
		/// this value is TRUE if any of the privileges are enabled.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool PrivilegeCheck(SafeTokenHandle ClientToken, SafeHGlobalHandle RequiredPrivileges, [MarshalAs(UnmanagedType.Bool)] out bool pfResult);

		/// <summary>
		/// The SetThreadToken function assigns an impersonation token to a thread. The function can also cause a thread to stop using an impersonation token.
		/// </summary>
		/// <param name="Thread">
		/// A pointer to a handle to the thread to which the function assigns the impersonation token. If Thread is NULL, the function assigns the impersonation
		/// token to the calling thread.
		/// </param>
		/// <param name="Token">
		/// A handle to the impersonation token to assign to the thread. This handle must have been opened with TOKEN_IMPERSONATE access rights. For more
		/// information, see Access Rights for Access-Token Objects. If Token is NULL, the function causes the thread to stop using an impersonation token.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns nonzero. If the function fails, it returns zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetThreadToken(IntPtr Thread, SafeTokenHandle Token);

		/// <summary>
		/// An LUID is a 64-bit value guaranteed to be unique only on the system on which it was generated. The uniqueness of a locally unique identifier (LUID)
		/// is guaranteed only until the system is restarted.
		/// <para>Applications must use functions and structures to manipulate LUID values.</para>
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LUID
		{
			/// <summary>Low order bits.</summary>
			public uint LowPart;

			/// <summary>High order bits.</summary>
			public int HighPart;

			public string GetName(string systemName = null)
			{
				var sb = new StringBuilder(1024);
				var sz = sb.Capacity;
				if (!LookupPrivilegeName(systemName, ref this, sb, ref sz))
					throw new Win32Exception();
				return sb.ToString();
			}

			public static LUID FromName(string name, string systemName = null)
			{
				LUID val;
				if (!LookupPrivilegeValue(systemName, name, out val))
					throw new Win32Exception();
				return val;
			}

			public override string ToString() => $"0x{MAKELONG64(LowPart, (uint)HighPart):X}";
		}

		/// <summary>The LUID_AND_ATTRIBUTES structure represents a locally unique identifier (LUID) and its attributes.</summary>
		/// <remarks>
		/// An LUID_AND_ATTRIBUTES structure can represent an LUID whose attributes change frequently, such as when the LUID is used to represent privileges in
		/// the PRIVILEGE_SET structure. Privileges are represented by LUIDs and have attributes indicating whether they are currently enabled or disabled.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LUID_AND_ATTRIBUTES
		{
			/// <summary>Specifies a LUID value.</summary>
			public LUID Luid;

			/// <summary>
			/// Specifies attributes of the LUID. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the LUID.
			/// </summary>
			public PrivilegeAttributes Attributes;

			public LUID_AND_ATTRIBUTES(LUID luid, PrivilegeAttributes attr)
			{
				Luid = luid;
				Attributes = attr;
			}

			public override string ToString() => $"{Luid}:{Attributes}";
		}

		/// <summary>
		/// The PRIVILEGE_SET structure specifies a set of privileges. It is also used to indicate which, if any, privileges are held by a user or group
		/// requesting access to an object.
		/// </summary>
		public class PRIVILEGE_SET
		{
			/// <summary>Specifies the number of privileges in the privilege set.</summary>
			public uint PrivilegeCount;

			/// <summary>
			/// Specifies a control flag related to the privileges. The PRIVILEGE_SET_ALL_NECESSARY control flag is currently defined. It indicates that all of
			/// the specified privileges must be held by the process requesting access. If this flag is not set, the presence of any privileges in the user's
			/// access token grants the access.
			/// </summary>
			public PrivilegeSetControl Control;

			/// <summary>Specifies an array of LUID_AND_ATTRIBUTES structures describing the set's privileges.</summary>
			public LUID_AND_ATTRIBUTES[] Privilege;

			internal PRIVILEGE_SET() : this(PrivilegeSetControl.PRIVILEGE_SET_ALL_NECESSARY, null) { }

			public PRIVILEGE_SET(PrivilegeSetControl control, LUID luid, PrivilegeAttributes attribute)
			{
				PrivilegeCount = 1;
				Control = control;
				Privilege = new[] { new LUID_AND_ATTRIBUTES(luid, attribute) };
			}

			public PRIVILEGE_SET(PrivilegeSetControl control, LUID_AND_ATTRIBUTES[] privileges)
			{
				PrivilegeCount = (uint)(privileges?.Length ?? 0);
				Control = control;
				Privilege = privileges ?? new LUID_AND_ATTRIBUTES[0];
			}

			public uint SizeInBytes => Marshaler.GetSize(PrivilegeCount);

			public override string ToString() => $"Count:{PrivilegeCount}";

			internal class Marshaler : ICustomMarshaler
			{
				public static ICustomMarshaler GetInstance(string cookie) => new Marshaler();

				public object MarshalNativeToManaged(IntPtr pNativeData)
				{
					if (pNativeData == IntPtr.Zero) return new PRIVILEGE_SET();
					var sz = Marshal.SizeOf(typeof(uint));
					var cnt = Marshal.ReadInt32(pNativeData);
					var ctrl = (PrivilegeSetControl)Marshal.ReadInt32(pNativeData, sz);
					var privPtr = Marshal.ReadIntPtr(pNativeData, sz*2);
					return new PRIVILEGE_SET { PrivilegeCount = (uint)cnt, Control = ctrl, Privilege = cnt > 0 ? privPtr.ToIEnum<LUID_AND_ATTRIBUTES>(cnt).ToArray() : new LUID_AND_ATTRIBUTES[0] };
				}

				public IntPtr MarshalManagedToNative(object ManagedObj)
				{
					if (!(ManagedObj is PRIVILEGE_SET)) return IntPtr.Zero;
					var ps = (PRIVILEGE_SET)ManagedObj;
					var ptr = Marshal.AllocCoTaskMem((int)GetSize(ps.PrivilegeCount));
					Marshal.WriteInt32(ptr, (int)ps.PrivilegeCount);
					Marshal.WriteInt32(ptr, Marshal.SizeOf(typeof(int)), (int)ps.Control);
					InteropExtensions.MarshalToPtr(ps.Privilege, ptr, Marshal.SizeOf(typeof(int)) * 2);
					return ptr;
				}

				internal static uint GetSize(uint privCount) => (uint)Marshal.SizeOf(typeof(uint)) * 2 + (uint)(Marshal.SizeOf(typeof(LUID_AND_ATTRIBUTES)) * (privCount == 0 ? 1 : privCount));

				public void CleanUpNativeData(IntPtr pNativeData) { Marshal.FreeCoTaskMem(pNativeData); }

				public void CleanUpManagedData(object ManagedObj) { }

				public int GetNativeDataSize() => -1;
			}
		}

		/// <summary>
		/// The SID_AND_ATTRIBUTES structure represents a security identifier (SID) and its attributes. SIDs are used to uniquely identify users or groups.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct SID_AND_ATTRIBUTES
		{
			/// <summary>A pointer to a SID structure.</summary>
			public IntPtr Sid;

			/// <summary>
			/// Specifies attributes of the SID. This value contains up to 32 one-bit flags. Its meaning depends on the definition and use of the SID.
			/// </summary>
			public uint Attributes;
		}

		/// <summary>The TOKEN_ELEVATION structure indicates whether a token has elevated privileges.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_ELEVATION
		{
			/// <summary>A nonzero value if the token has elevated privileges; otherwise, a zero value.</summary>
			[MarshalAs(UnmanagedType.Bool)]
			public bool TokenIsElevated;
		}

		/// <summary>The TOKEN_GROUPS structure contains information about the group security identifiers (SIDs) in an access token.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_GROUPS
		{
			/// <summary>Specifies the number of groups in the access token.</summary>
			public uint GroupCount;

			/// <summary>Specifies an array of SID_AND_ATTRIBUTES structures that contain a set of SIDs and corresponding attributes.</summary>
			[MarshalAs(UnmanagedType.ByValArray)]
			public SID_AND_ATTRIBUTES[] Groups;

			public TOKEN_GROUPS(uint count = 0)
			{
				GroupCount = count;
				Groups = new SID_AND_ATTRIBUTES[count];
			}
		}

		/// <summary>The TOKEN_MANDATORY_LABEL structure specifies the mandatory integrity level for a token.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_MANDATORY_LABEL
		{
			/// <summary>A SID_AND_ATTRIBUTES structure that specifies the mandatory integrity level of the token.</summary>
			public SID_AND_ATTRIBUTES Label;
		}

		public class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			private const int ERROR_INSUFFICIENT_BUFFER = 122;
			private const int ERROR_NO_TOKEN = 0x000003F0;

			public static SafeTokenHandle Null { get; } = new SafeTokenHandle(IntPtr.Zero, false);

			public SafeTokenHandle(IntPtr hToken, bool own = true) : base(own)
			{
				SetHandle(hToken);
			}

			internal SafeTokenHandle() : base(true) { }

			public static SafeTokenHandle FromProcess(IntPtr hProcess, AccessTypes desiredAccess = AccessTypes.TOKEN_DUPLICATE)
			{
				SafeTokenHandle val;
				if (!OpenProcessToken(hProcess, desiredAccess, out val))
					throw new Win32Exception();
				return val;
			}

			public static SafeTokenHandle FromThread(IntPtr hThread, AccessTypes desiredAccess = AccessTypes.TOKEN_DUPLICATE, bool openAsSelf = true)
			{
				SafeTokenHandle val;
				if (!OpenThreadToken(hThread, desiredAccess, openAsSelf, out val))
				{
					if (Marshal.GetLastWin32Error() == ERROR_NO_TOKEN)
					{
						var pval = FromProcess(System.Diagnostics.Process.GetCurrentProcess().Handle);
						if (!DuplicateTokenEx(pval, AccessTypes.TOKEN_IMPERSONATE | desiredAccess, null, SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, TOKEN_TYPE.TokenImpersonation, out val))
							throw new Win32Exception();
						if (!SetThreadToken(IntPtr.Zero, val))
							throw new Win32Exception();
					}
					else
						throw new Win32Exception();
				}
				return val;
			}

			public T GetConvertedInfo<T>(TOKEN_INFORMATION_CLASS type)
			{
				using (var pType = GetInfo(type))
				{
					// Marshal from native to .NET.
					switch (type)
					{
						case TOKEN_INFORMATION_CLASS.TokenType:
						case TOKEN_INFORMATION_CLASS.TokenImpersonationLevel:
						case TOKEN_INFORMATION_CLASS.TokenSessionId:
						case TOKEN_INFORMATION_CLASS.TokenSandBoxInert:
						case TOKEN_INFORMATION_CLASS.TokenOrigin:
						case TOKEN_INFORMATION_CLASS.TokenElevationType:
						case TOKEN_INFORMATION_CLASS.TokenHasRestrictions:
						case TOKEN_INFORMATION_CLASS.TokenUIAccess:
						case TOKEN_INFORMATION_CLASS.TokenVirtualizationAllowed:
						case TOKEN_INFORMATION_CLASS.TokenVirtualizationEnabled:
							return (T)Convert.ChangeType(Marshal.ReadInt32((IntPtr)pType), typeof(T));

						case TOKEN_INFORMATION_CLASS.TokenLinkedToken:
							if (typeof(T) == typeof(IntPtr))
								return (T)Convert.ChangeType(Marshal.ReadIntPtr((IntPtr)pType), typeof(T));
							return default(T);

						case TOKEN_INFORMATION_CLASS.TokenUser:
						case TOKEN_INFORMATION_CLASS.TokenGroups:
						case TOKEN_INFORMATION_CLASS.TokenPrivileges:
						case TOKEN_INFORMATION_CLASS.TokenOwner:
						case TOKEN_INFORMATION_CLASS.TokenPrimaryGroup:
						case TOKEN_INFORMATION_CLASS.TokenDefaultDacl:
						case TOKEN_INFORMATION_CLASS.TokenSource:
						case TOKEN_INFORMATION_CLASS.TokenStatistics:
						case TOKEN_INFORMATION_CLASS.TokenRestrictedSids:
						case TOKEN_INFORMATION_CLASS.TokenGroupsAndPrivileges:
						case TOKEN_INFORMATION_CLASS.TokenElevation:
						case TOKEN_INFORMATION_CLASS.TokenAccessInformation:
						case TOKEN_INFORMATION_CLASS.TokenIntegrityLevel:
						case TOKEN_INFORMATION_CLASS.TokenMandatoryPolicy:
						case TOKEN_INFORMATION_CLASS.TokenLogonSid:
							return (T)Marshal.PtrToStructure((IntPtr)pType, typeof(T));

						default:
							return default(T);
					}
				}
			}

			public SafeHGlobalHandle GetInfo(TOKEN_INFORMATION_CLASS type)
			{
				// Get information size
				int cbSize;
				if (!GetTokenInformation(this, type, SafeHGlobalHandle.Null, 0, out cbSize))
					if (Marshal.GetLastWin32Error() != Win32Error.ERROR_INSUFFICIENT_BUFFER)
						throw new Win32Exception();

				// Retrieve token information.
				var pType = new SafeHGlobalHandle(cbSize);
				if (!GetTokenInformation(this, type, pType, cbSize, out cbSize))
					throw new Win32Exception();

				return pType;
			}

			protected override bool ReleaseHandle() => CloseHandle(handle);

			/// <summary>Closes an open object handle.</summary>
			/// <param name="hObject">A valid handle to an open object.</param>
			/// <returns>
			/// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.
			/// </returns>
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool CloseHandle(IntPtr hObject);
		}

	    /// <summary>The TOKEN_PRIVILEGES structure contains information about a set of privileges for an access token.</summary>
		public class PTOKEN_PRIVILEGES
		{
			/// <summary>This must be set to the number of entries in the Privileges array.</summary>
			public int PrivilegeCount;

			/// <summary>
			/// Specifies an array of LUID_AND_ATTRIBUTES structures. Each structure contains the LUID and attributes of a privilege. To get the name of the
			/// privilege associated with a LUID, call the LookupPrivilegeName function, passing the address of the LUID as the value of the lpLuid parameter.
			/// </summary>
			public LUID_AND_ATTRIBUTES[] Privileges;

			public PTOKEN_PRIVILEGES() : this(null) { }

			public PTOKEN_PRIVILEGES(LUID luid, PrivilegeAttributes attribute)
			{
				PrivilegeCount = 1;
				Privileges = new[] {new LUID_AND_ATTRIBUTES(luid, attribute)};
			}

			public PTOKEN_PRIVILEGES(LUID_AND_ATTRIBUTES[] values)
			{
				PrivilegeCount = values?.Length ?? 0;
				Privileges = values ?? new LUID_AND_ATTRIBUTES[0];
			}

			public uint SizeInBytes => Marshaler.GetSize(PrivilegeCount);

			public static PTOKEN_PRIVILEGES FromPtr(IntPtr hMem) => Marshaler.GetInstance(null).MarshalNativeToManaged(hMem) as PTOKEN_PRIVILEGES;

			public static SafeCoTaskMemHandle GetAllocatedAndEmptyInstance(int privilegeCount = 100) => new SafeCoTaskMemHandle((int)Marshaler.GetSize(privilegeCount));

			public override string ToString() => $"Count:{PrivilegeCount}";

			internal class Marshaler : ICustomMarshaler
			{
				private bool allocOut;
				private Marshaler(string cookie) { allocOut = cookie == "Out"; }
				public static ICustomMarshaler GetInstance(string cookie) => new Marshaler(cookie);

				public object MarshalNativeToManaged(IntPtr pNativeData)
				{
					if (pNativeData == IntPtr.Zero) return new PTOKEN_PRIVILEGES();
					var sz = Marshal.SizeOf(typeof(uint));
					var cnt = Marshal.ReadInt32(pNativeData);
					var privPtr = new IntPtr(pNativeData.ToInt64() + sz); //Marshal.ReadIntPtr(pNativeData, sz));
					return new PTOKEN_PRIVILEGES { PrivilegeCount = cnt, Privileges = cnt > 0 ? privPtr.ToIEnum<LUID_AND_ATTRIBUTES>(cnt).ToArray() : new LUID_AND_ATTRIBUTES[0] };
				}

				public IntPtr MarshalManagedToNative(object ManagedObj)
				{
					var ps = ManagedObj as PTOKEN_PRIVILEGES;
					if (ps == null) return IntPtr.Zero;
					if (allocOut)
					{
						var sz = Math.Abs(ps.PrivilegeCount);
						ps.PrivilegeCount = 0;
						return Marshal.AllocCoTaskMem(sz);
					}
					var ptr = Marshal.AllocCoTaskMem((int)GetSize(ps.PrivilegeCount));
					Marshal.WriteInt32(ptr, ps.PrivilegeCount);
					if (ps.Privileges != null)
						InteropExtensions.MarshalToPtr(ps.Privileges, ptr, Marshal.SizeOf(typeof(int)));
					return ptr;
				}

				internal static uint GetSize(int privCount) => (uint)Marshal.SizeOf(typeof(uint)) + (uint)(Marshal.SizeOf(typeof(LUID_AND_ATTRIBUTES)) * (privCount == 0 ? 1 : Math.Abs(privCount)));

				public void CleanUpNativeData(IntPtr pNativeData) { Marshal.FreeCoTaskMem(pNativeData); }

				public void CleanUpManagedData(object ManagedObj) { }

				public int GetNativeDataSize() => -1;
			}
		}
	}
}