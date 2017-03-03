using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using static Vanara.PInvoke.AdvApi32;
using AUTHZ_RESOURCE_MANAGER_HANDLE = System.IntPtr;
using AUTHZ_CLIENT_CONTEXT_HANDLE = System.IntPtr;
using AUTHZ_AUDIT_EVENT_HANDLE = System.IntPtr;
using AUTHZ_ACCESS_CHECK_RESULTS_HANDLE = System.IntPtr;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Authz
	{
		/// <summary>
		/// The AuthzAccessCheckCallback function is an application-defined function that handles callback access control entries (ACEs) during an access check.
		/// AuthzAccessCheckCallback is a placeholder for the application-defined function name. The application registers this callback by calling AuthzInitializeResourceManager.
		/// </summary>
		/// <param name="hAuthzClientContext">A handle to a client context.</param>
		/// <param name="pAce">A pointer to the ACE to evaluate for inclusion in the call to the AuthzAccessCheck function.</param>
		/// <param name="pArgs">Data passed in the DynamicGroupArgs parameter of the call to AuthzAccessCheck or AuthzCachedAccessCheck.</param>
		/// <param name="pbAceApplicable">A pointer to a Boolean variable that receives the results of the evaluation of the logic defined by the application.
		/// <para>The results are TRUE if the logic determines that the ACE is applicable and will be included in the call to AuthzAccessCheck; otherwise, the results are FALSE.</para></param>
		/// <returns>
		/// If the function succeeds, the function returns TRUE.
		/// <para>If the function is unable to perform the evaluation, it returns FALSE. Use SetLastError to return an error to the access check function.</para>
		/// </returns>
		[UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public delegate bool AuthzAccessCheckCallback(AUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext, ref AdvApi32.ACE_HEADER pAce, IntPtr pArgs, [MarshalAs(UnmanagedType.Bool)] ref bool pbAceApplicable);

		/// <summary>
		/// The AuthzComputeGroupsCallback function is an application-defined function that creates a list of security identifiers (SIDs) that apply to a client. AuthzComputeGroupsCallback is a placeholder for the application-defined function name.
		/// </summary>
		/// <param name="hAuthzClientContext">A handle to a client context.</param>
		/// <param name="Args">Data passed in the DynamicGroupArgs parameter of a call to the AuthzInitializeContextFromAuthzContext, AuthzInitializeContextFromSid, or AuthzInitializeContextFromToken function.</param>
		/// <param name="pSidAttrArray">A pointer to a pointer variable that receives the address of an array of SID_AND_ATTRIBUTES structures. These structures represent the groups to which the client belongs.</param>
		/// <param name="pSidCount">The number of structures in pSidAttrArray.</param>
		/// <param name="pRestrictedSidAttrArray">A pointer to a pointer variable that receives the address of an array of SID_AND_ATTRIBUTES structures. These structures represent the groups from which the client is restricted.</param>
		/// <param name="pRestrictedSidCount">The number of structures in pSidRestrictedAttrArray.</param>
		/// <returns>If the function successfully returns a list of SIDs, the return value is TRUE.
		/// <para>If the function fails, the return value is FALSE.</para></returns>
		[UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public delegate bool AuthzComputeGroupsCallback(AUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext, IntPtr Args, ref AdvApi32.SID_AND_ATTRIBUTES pSidAttrArray, out uint pSidCount, out AdvApi32.SID_AND_ATTRIBUTES pRestrictedSidAttrArray, out uint pRestrictedSidCount);

		/// <summary>
		/// The AuthzFreeGroupsCallback function is an application-defined function that frees memory allocated by the AuthzComputeGroupsCallback function. AuthzFreeGroupsCallback is a placeholder for the application-defined function name.
		/// </summary>
		/// <param name="pSidAttrArray">A pointer to memory allocated by AuthzComputeGroupsCallback.</param>
		[UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		public delegate void AuthzFreeGroupsCallback(ref AdvApi32.SID_AND_ATTRIBUTES pSidAttrArray);

		/// <summary>
		/// The AUTHZ_CONTEXT_INFORMATION_CLASS enumeration specifies the type of information to be retrieved from an existing AuthzClientContext. This enumeration is used by the AuthzGetInformationFromContext function.
		/// </summary>
		public enum AUTHZ_CONTEXT_INFORMATION_CLASS
		{
			/// <summary>
			/// Retrieves a TOKEN_USER structure that contains a user security identifier (SID) and its attribute.
			/// </summary>
			AuthzContextInfoUserSid = 1,
			/// <summary>
			/// Retrieves a TOKEN_GROUPS structure that contains the group SIDs to which the user belongs and their attributes.
			/// </summary>
			AuthzContextInfoGroupsSids,
			/// <summary>
			/// Retrieves a TOKEN_GROUPS structure that contains the restricted group SIDs in the context and their attributes.
			/// </summary>
			AuthzContextInfoRestrictedSids,
			/// <summary>
			/// Retrieves a TOKEN_PRIVILEGES structure that contains the privileges held by the user.
			/// </summary>
			AuthzContextInfoPrivileges,
			/// <summary>
			/// Retrieves the expiration time set on the context.
			/// </summary>
			AuthzContextInfoExpirationTime,
			/// <summary>
			/// This constant is reserved. Do not use it.
			/// </summary>
			AuthzContextInfoServerContext,
			/// <summary>
			/// Retrieves an LUID structures used by the resource manager to identify the context.
			/// </summary>
			AuthzContextInfoIdentifier,
			/// <summary>
			/// This constant is reserved. Do not use it.
			/// </summary>
			AuthzContextInfoSource,
			/// <summary>
			/// This constant is reserved. Do not use it.
			/// </summary>
			AuthzContextInfoAll,
			/// <summary>
			/// This constant is reserved. Do not use it.
			/// </summary>
			AuthzContextInfoAuthenticationId,
			/// <summary>
			/// Retrieves an AUTHZ_SECURITY_ATTRIBUTES_INFORMATION structure that contains security attributes.
			/// <para><c>Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoSecurityAttributes,
			/// <summary>
			/// Retrieves a TOKEN_GROUPS structure that contains device SIDs and their attributes.
			/// <para><c>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoDeviceSids,
			/// <summary>
			/// Retrieves a AUTHZ_SECURITY_ATTRIBUTES_INFORMATION structure that contains user claims.
			/// <para><c>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoUserClaims,
			/// <summary>
			/// Retrieves a AUTHZ_SECURITY_ATTRIBUTES_INFORMATION structure that contains device claims.
			/// <para><c>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoDeviceClaims,
			/// <summary>
			/// Retrieves a TOKEN_APPCONTAINER_INFORMATION structure that contains the app container SID.
			/// <para><c>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoAppContainerSid,
			/// <summary>
			/// Retrieves a TOKEN_GROUPS structure that contains capability SIDs.
			/// <para><c>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:</c>  This value is not supported.</para>
			/// </summary>
			AuthzContextInfoCapabilitySids
		}

		/// <summary>Used by the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> structure.</summary>
		public enum AUTHZ_SECURITY_ATTRIBUTE_DATATYPE : ushort
		{
			/// <summary>Invalid value.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_INVALID = 0x00,
			/// <summary>The Values member refers to a security attribute that is of INT64 type.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_INT64 = 0x01,
			/// <summary>The Values member refers to a security attribute that is of UINT64 type.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_UINT64 = 0x02,
			/// <summary>The Values member refers to a security attribute that is of STRING type.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_STRING = 0x03,
			/// <summary>The Values member refers to a security attribute that is of AUTHZ_SECURITY_ATTRIBUTE_TYPE_FQBN type.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_FQBN = 0x04,
			/// <summary>
			/// The Values member refers to a security attribute that is of AUTHZ_SECURITY_ATTRIBUTE_TYPE_SID type.
			/// <para><c>Windows Server 2008 R2 and Windows 7:</c> This value type is not available.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_SID = 0x05,
			/// <summary>
			/// The Values member refers to a security attribute that is of AUTHZ_SECURITY_ATTRIBUTE_TYPE_BOOLEAN type.
			/// <para><c>Windows Server 2008 R2 and Windows 7:</c> This value type is not available.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_BOOLEAN = 0x06,
			/// <summary>The Values member refers to a security attribute that is of AUTHZ_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING type.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING = 0x10
		}

		/// <summary>Used by the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> structure.</summary>
		public enum AUTHZ_SECURITY_ATTRIBUTE_FLAGS
		{
			/// <summary>This security attribute is not inherited across processes.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_NON_INHERITABLE = 1,
			/// <summary>The value of the attribute is case sensitive. This flag is valid for values that contain string types.</summary>
			AUTHZ_SECURITY_ATTRIBUTE_VALUE_CASE_SENSITIVE = 2
		}

		/// <summary>
		/// The AUTHZ_SECURITY_ATTRIBUTE_OPERATION enumeration indicates the type of modification to be made to security attributes by a call to the AuthzModifySecurityAttributes function.
		/// </summary>
		public enum AUTHZ_SECURITY_ATTRIBUTE_OPERATION
		{
			/// <summary>
			/// Do not perform any modification.
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION_NONE = 0,
			/// <summary>
			/// Delete all existing security attributes and their values in the token and replace them with the specified attributes and values.
			/// <para>If no new attributes are specified, all existing attributes and values are deleted.</para>
			/// <para>This operation must be the only operation specified and can be specified only once in a single call to AuthzModifySecurityAttributes. If the operation is not specified as the first in the list of operations, the call to AuthzModifySecurityAttributes fails. If the operation is specified as the first in the array of operations performed, the rest of the operations are ignored.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION_REPLACE_ALL,
			/// <summary>
			/// Add a new attribute or a new value to an existing attribute.
			/// <para>If the value specified for any attribute already exists for that attribute, the call to AuthzModifySecurityAttributes fails.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION_ADD,
			/// <summary>
			/// Delete the specified values of the specified attributes. If an attribute is specified without a value, that attribute is deleted.
			/// <para>If this operation results in an attribute that does not contain any values, that attribute is deleted.</para>
			/// <para>If a value is specified that does not match an existing attribute, no modifications are performed and the call to AuthzModifySecurityAttributes fails.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION_DELETE,
			/// <summary>
			/// The existing values of the specified security attributes are replaced by the specified new values.
			/// <para>If any of the specified attributes does not already exist, they are added.</para>
			/// <para>When no value is specified for an attribute, that attribute is deleted. Otherwise, the operation is simply ignored and no failure is reported.</para>
			/// </summary>
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION_REPLACE
		}

		/// <summary>
		/// The AUTHZ_SID_OPERATION enumeration indicates the type of SID operations that can be made by a call to the AuthzModifySids function.
		/// </summary>
		public enum AUTHZ_SID_OPERATION
		{
			/// <summary>
			/// Do not modify anything.
			/// </summary>
			AUTHZ_SID_OPERATION_NONE = 0,
			/// <summary>
			/// Deletes all existing SIDs and replaces them with the specified SIDs. If the replacement SIDs are not specified, all existing SIDs are deleted. This operation can be specified only once and must be the only operation specified.
			/// </summary>
			AUTHZ_SID_OPERATION_REPLACE_ALL = 1,
			/// <summary>
			/// Adds a new SID. If the SID already exists, the call fails.
			/// </summary>
			AUTHZ_SID_OPERATION_ADD = 2,
			/// <summary>
			/// Deletes the specified SID. If no matching SID is found, no modifications are done and the call fails.
			/// </summary>
			AUTHZ_SID_OPERATION_DELETE = 3,
			/// <summary>
			/// Replaces the existing SID with the specified SID. If the SID does not already exist, then adds the SID.
			/// </summary>
			AUTHZ_SID_OPERATION_REPLACE = 4
		}

		/// <summary>Flags used in the <see cref="Authz.AuthzAccessCheck"/> method.</summary>
		[Flags]
		public enum AuthzAccessCheckFlags
		{
			/// <summary>If phAccessCheckResults is not NULL, a deep copy of the security descriptor is copied to the handle referenced by phAccessCheckResults.</summary>
			NONE = 0,
			/// <summary>
			/// A deep copy of the security descriptor is not performed. The calling application must pass the address of an AUTHZ_ACCESS_CHECK_RESULTS_HANDLE
			/// handle in phAccessCheckResults. The AuthzAccessCheck function sets this handle to a security descriptor that must remain valid during subsequent
			/// calls to AuthzCachedAccessCheck.
			/// </summary>
			AUTHZ_ACCESS_CHECK_NO_DEEP_COPY_SD = 0x00000001,
		}

		/// <summary>
		/// Flags for the <see cref="Authz.AuthzInitializeObjectAccessAuditEvent"/> method.
		/// </summary>
		[Flags]
		public enum AuthzAuditEventFlags
		{
			/// <summary>
			/// No flags set.
			/// </summary>
			NONE = 0,
			/// <summary>
			/// Disable generation of success audits.
			/// </summary>
			AUTHZ_NO_SUCCESS_AUDIT = 0x00000001,
			/// <summary>
			/// Disable generation of failure audits.
			/// </summary>
			AUTHZ_NO_FAILURE_AUDIT = 0x00000002,
			/// <summary>
			/// Use pointers to the passed strings instead of allocating memory and copying the strings. The calling application must ensure that the passed memory stays valid during access checks.
			/// </summary>
			AUTHZ_NO_ALLOC_STRINGS = 0x00000004,
			/// <summary>
			/// Undocumented.
			/// </summary>
			AUTHZ_WPD_CATEGORY_FLAG = 0x00000010,
		}

		/// <summary>
		/// Flags for the <see cref="Authz.AuthzInitializeContextFromSid"/> method.
		/// </summary>
		[Flags]
		public enum AuthzContextFlags
		{
			/// <summary>
			/// Default value.
			/// <para>AuthzInitializeContextFromSid attempts to retrieve the user's token group information by performing an S4U logon.</para>
			/// <para>If S4U logon is not supported by the user's domain or the calling computer, AuthzInitializeContextFromSid queries the user's account object for group information. When an account is queried directly, some groups that represent logon characteristics, such as Network, Interactive, Anonymous, Network Service, or Local Service, are omitted. Applications can explicitly add such group SIDs by implementing the AuthzComputeGroupsCallback function or calling the AuthzAddSidsToContext function.</para>
			/// </summary>
			NONE = 0,
			/// <summary>
			/// Causes AuthzInitializeContextFromSid to skip all group evaluations. When this flag is used, the context returned contains only the SID specified by the UserSid parameter. The specified SID can be an arbitrary or application-specific SID. Other SIDs can be added to this context by implementing the AuthzComputeGroupsCallback function or by calling the AuthzAddSidsToContext function.
			/// </summary>
			AUTHZ_SKIP_TOKEN_GROUPS = 0x2,
			/// <summary>
			/// Causes AuthzInitializeContextFromSid to fail if Windows Services For User is not available to retrieve token group information.
			/// <para><c>Windows XP:</c> This flag is not supported.</para>
			/// </summary>
			AUTHZ_REQUIRE_S4U_LOGON = 0x4,
			/// <summary>
			/// Causes AuthzInitializeContextFromSid to retrieve privileges for the new context. If this function performs an S4U logon, it retrieves privileges from the token. Otherwise, the function retrieves privileges from all SIDs in the context.
			/// </summary>
			AUTHZ_COMPUTE_PRIVILEGES = 0x8
		}

		/// <summary>Flags for the <see cref="Authz.AuthzInitializeResourceManager"/> method.</summary>
		[Flags]
		public enum AuthzResourceManagerFlags
		{
			/// <summary>
			/// Default call to the function. The resource manager is initialized as the principal identified in the process token, and auditing is in effect.
			/// Note that unless the AUTHZ_RM_FLAG_NO_AUDIT flag is set, SeAuditPrivilege must be enabled for the function to succeed.
			/// </summary>
			NONE = 0,
			/// <summary>Auditing is not in effect. If this flag is set, the caller does not need to have SeAuditPrivilege enabled to call this function.</summary>
			AUTHZ_RM_FLAG_NO_AUDIT = 0x1,
			/// <summary>The resource manager is initialized as the identity of the thread token.</summary>
			AUTHZ_RM_FLAG_INITIALIZE_UNDER_IMPERSONATION = 0x2,
			/// <summary>The resource manager ignores CAP IDs and does not evaluate centralized access policies.</summary>
			AUTHZ_RM_FLAG_NO_CENTRALIZED_ACCESS_POLICIES = 0x4
		}

		/// <summary>
		/// The AuthzAccessCheck function determines which access bits can be granted to a client for a given set of security descriptors. The AUTHZ_ACCESS_REPLY
		/// structure returns an array of granted access masks and error status. Optionally, access masks that will always be granted can be cached, and a handle
		/// to cached values is returned.
		/// </summary>
		/// <param name="flags">
		/// A DWORD value that specifies how the security descriptor is copied. This parameter can be one of the following values.
		/// <para>Starting with Windows 8 and Windows Server 2012, when you call this function on a remote context handle, the upper 16 bits must be zero.</para>
		/// </param>
		/// <param name="hAuthzClientContext">
		/// A handle to a structure that represents the client.
		/// <para>Starting with Windows 8 and Windows Server 2012, the client context can be local or remote.</para>
		/// </param>
		/// <param name="pRequest">
		/// A pointer to an AUTHZ_ACCESS_REQUEST structure that specifies the desired access mask, principal self security identifier (SID), and the object type
		/// list structure, if it exists.
		/// </param>
		/// <param name="AuditEvent">
		/// A structure that contains object-specific audit information. When the value of this parameter is not null, an audit is automatically requested.
		/// Static audit information is read from the resource manager structure.
		/// <para>
		/// Starting with Windows 8 and Windows Server 2012, when you use this function with a remote context handle, the value of the parameter must be NULL.
		/// </para>
		/// </param>
		/// <param name="pSecurityDescriptor">
		/// A pointer to a SECURITY_DESCRIPTOR structure to be used for access checks. The owner SID for the object is picked from this security descriptor. A
		/// NULL discretionary access control list (DACL) in this security descriptor represents a NULL DACL for the entire object. Make sure the security
		/// descriptor contains OWNER and DACL information, or an error code 87 or "invalid parameter" message will be generated. <note
		/// type="important"><c>Important</c> NULL DACLs permit all types of access to all users; therefore, do not use NULL DACLs. For information about
		/// creating a DACL, see Creating a DACL.</note>
		/// <para>A NULL system access control list (SACL) in this security descriptor is treated the same way as an empty SACL.</para>
		/// </param>
		/// <param name="OptionalSecurityDescriptorArray">
		/// An array of SECURITY_DESCRIPTOR structures. NULL access control lists (ACLs) in these security descriptors are treated as empty ACLs. The ACL for the
		/// entire object is the logical concatenation of all of the ACLs.
		/// </param>
		/// <param name="OptionalSecurityDescriptorCount">The number of security descriptors not including the primary security descriptor.</param>
		/// <param name="pReply">
		/// A pointer to an AUTHZ_ACCESS_REPLY structure that contains the results of the access check. Before calling the AuthzAccessCheck function, an
		/// application must allocate memory for the GrantedAccessMask and SaclEvaluationResults members of the AUTHZ_ACCESS_REPLY structure referenced by pReply.
		/// </param>
		/// <param name="phAccessCheckResults">
		/// A pointer to return a handle to the cached results of the access check. When this parameter value is not null, the results of this access check call
		/// will be cached. This results in a MAXIMUM_ALLOWED check.
		/// <para>
		/// Starting with Windows 8 and Windows Server 2012, when you use this function with a remote context handle, the value of the parameter must be NULL.
		/// </para>
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzAccessCheck(AuthzAccessCheckFlags flags, SafeAUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext, ref AUTHZ_ACCESS_REQUEST pRequest,
			SafeAUTHZ_AUDIT_EVENT_HANDLE AuditEvent, ref AdvApi32.SECURITY_DESCRIPTOR pSecurityDescriptor, AdvApi32.SECURITY_DESCRIPTOR[] OptionalSecurityDescriptorArray,
			uint OptionalSecurityDescriptorCount, [In, Out] AUTHZ_ACCESS_REPLY pReply, out SafeAUTHZ_ACCESS_CHECK_RESULTS_HANDLE phAccessCheckResults);

		/// <summary>The AuthzFreeAuditEvent function frees the structure allocated by the AuthzInitializeObjectAccessAuditEvent function.</summary>
		/// <param name="pAuditEventInfo">A pointer to the AUTHZ_AUDIT_EVENT_HANDLE structure to be freed.</param>
		/// <returns>
		/// If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzFreeAuditEvent(AUTHZ_AUDIT_EVENT_HANDLE pAuditEventInfo);

		/// <summary>
		/// The AuthzFreeContext function frees all structures and memory associated with the client context. The list of handles for a client is freed in this call.
		/// <para>
		/// Starting with Windows Server 2012 and Windows 8, this function also frees the memory associated with device groups, user claims, and device claims.
		/// </para>
		/// </summary>
		/// <param name="AuthzClientContext">The AUTHZ_CLIENT_CONTEXT_HANDLE structure to be freed.</param>
		/// <returns>
		/// If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzFreeContext(AUTHZ_CLIENT_CONTEXT_HANDLE AuthzClientContext);

		/// <summary>The AuthzFreeHandle function finds and deletes a handle from the handle list.</summary>
		/// <param name="AuthzHandle">A handle to be freed.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzFreeHandle(AUTHZ_ACCESS_CHECK_RESULTS_HANDLE AuthzHandle);

		/// <summary>
		/// The AuthzFreeResourceManager function frees a resource manager object.
		/// </summary>
		/// <param name="AuthzResourceManager">The AUTHZ_RESOURCE_MANAGER_HANDLE to be freed.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzFreeResourceManager(AUTHZ_RESOURCE_MANAGER_HANDLE AuthzResourceManager);

		/// <summary>
		/// The AuthzGetInformationFromContext function returns information about an Authz context.
		/// <para>Starting with Windows Server 2012 and Windows 8, device groups are returned as a TOKEN_GROUPS structure. User and device claims are returned as an AUTHZ_SECURITY_ATTRIBUTES_INFORMATION structure.</para></summary>
		/// <param name="hAuthzClientContext">A handle to the context.</param>
		/// <param name="InfoClass">A value of the AUTHZ_CONTEXT_INFORMATION_CLASS enumeration that indicates the type of information to be returned.</param>
		/// <param name="BufferSize">Size of the buffer passed.</param>
		/// <param name="pSizeRequired">A pointer to a DWORD of the buffer size required for returning the structure.</param>
		/// <param name="Buffer">A pointer to memory that can receive the information. The structure returned depends on the information requested in the InfoClass parameter</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzGetInformationFromContext(
			SafeAUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext,
			AUTHZ_CONTEXT_INFORMATION_CLASS InfoClass,
			uint BufferSize,
			out uint pSizeRequired,
			IntPtr Buffer);

		/// <summary>
		/// The AuthzInitializeCompoundContext function creates a user-mode context from the given user and device security contexts.
		/// </summary>
		/// <param name="UserContext">User context to create the compound context from.</param>
		/// <param name="DeviceContext">Device context to create the compound context from. This must not be the same as the user context.</param>
		/// <param name="phCompoundContext">Used to return the resultant compound context.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzInitializeCompoundContext(SafeAUTHZ_CLIENT_CONTEXT_HANDLE UserContext, SafeAUTHZ_CLIENT_CONTEXT_HANDLE DeviceContext, out SafeAUTHZ_CLIENT_CONTEXT_HANDLE phCompoundContext);

		/// <summary>
		/// The AuthzInitializeContextFromSid function creates a user-mode client context from a user security identifier (SID). Domain SIDs retrieve token group attributes from the Active Directory.
		/// <note>Note  If possible, call the AuthzInitializeContextFromToken function instead of AuthzInitializeContextFromSid. For more information, see Remarks.</note>
		/// </summary>
		/// <param name="Flags">The following flags are defined.
		/// <para>Starting with Windows 8 and Windows Server 2012, when you call this function on a remote context handle, the upper 16 bits must be zero.</para></param>
		/// <param name="userSid">The SID of the user for whom a client context will be created. This must be a valid user or computer account unless the AUTHZ_SKIP_TOKEN_GROUPS flag is used.</param>
		/// <param name="AuthzResourceManager">A handle to the resource manager creating this client context. This handle is stored in the client context structure.
		/// <para>Starting with Windows 8 and Windows Server 2012, the resource manager can be local or remote and is obtained by calling the AuthzInitializeRemoteResourceManager function.</para></param>
		/// <param name="pExpirationTime">Expiration date and time of the token. If no value is passed, the token never expires. Expiration time is not currently enforced.</param>
		/// <param name="Identitifier">Specific identifier of the resource manager. This parameter is not currently used.</param>
		/// <param name="DynamicGroupArgs">A pointer to parameters to be passed to the callback function that computes dynamic groups. This parameter can be NULL if no dynamic parameters are passed to the callback function.
		/// <para>Starting with Windows 8 and Windows Server 2012, this parameter must be NULL if the resource manager is remote. Otherwise, ERROR_NOT_SUPPORTED will be set.</para></param>
		/// <param name="pAuthzClientContext">A pointer to the handle to the client context that the AuthzInitializeContextFromSid function creates. When you have finished using the handle, free it by calling the AuthzFreeContext function.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzInitializeContextFromSid(
			AuthzContextFlags Flags,
			AdvApi32.PSID userSid,
			SafeAUTHZ_RESOURCE_MANAGER_HANDLE AuthzResourceManager,
			IntPtr pExpirationTime,
			AdvApi32.LUID Identitifier,
			IntPtr DynamicGroupArgs,
			out SafeAUTHZ_CLIENT_CONTEXT_HANDLE pAuthzClientContext);

		/// <summary>
		/// The AuthzInitializeObjectAccessAuditEvent function initializes auditing for an object.
		/// </summary>
		/// <param name="Flags">Modifies the audit.</param>
		/// <param name="hAuditEventType">Reserved. This parameter should be set to NULL.</param>
		/// <param name="szOperationType">String that indicates the operation that is to be audited.</param>
		/// <param name="szObjectType">String that indicates the type of object being accessed.</param>
		/// <param name="szObjectName">String the indicates the name of the object being accessed.</param>
		/// <param name="szAdditionalInfo">String, defined by the Resource Manager, for additional audit information.</param>
		/// <param name="phAuditEvent">Pointer that receives an AUTHZ_AUDIT_EVENT_HANDLE structure.</param>
		/// <param name="dwAdditionalParamCount">Must be set to zero.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzInitializeObjectAccessAuditEvent(
			AuthzAuditEventFlags Flags,
			IntPtr hAuditEventType,
			[MarshalAs(UnmanagedType.LPWStr)] string szOperationType,
			[MarshalAs(UnmanagedType.LPWStr)] string szObjectType,
			[MarshalAs(UnmanagedType.LPWStr)] string szObjectName,
			[MarshalAs(UnmanagedType.LPWStr)] string szAdditionalInfo,
			out SafeAUTHZ_AUDIT_EVENT_HANDLE phAuditEvent,
			uint dwAdditionalParamCount);

		/// <summary>
		/// The AuthzInitializeResourceManager function uses Authz to verify that clients have access to various resources.
		/// </summary>
		/// <param name="flags">A DWORD value that defines how the resource manager is initialized.
		/// <para>AUTHZ_RM_FLAG_NO_AUDIT and AUTHZ_RM_FLAG_INITIALIZE_UNDER_IMPERSONATION can be bitwise-combined.</para></param>
		/// <param name="pfnAccessCheck">A pointer to the AuthzAccessCheckCallback callback function that the resource manager calls each time it encounters a callback access control entry (ACE) during access control list (ACL) evaluation in AuthzAccessCheck or AuthzCachedAccessCheck. This parameter can be NULL if no access check callback function is used.</param>
		/// <param name="pfnComputeDynamicGroups">A pointer to the AuthzComputeGroupsCallback callback function called by the resource manager during initialization of an AuthzClientContext handle. This parameter can be NULL if no callback function is used to compute dynamic groups.</param>
		/// <param name="pfnFreeDynamicGroups">A pointer to the AuthzFreeGroupsCallback callback function called by the resource manager to free security identifier (SID) attribute arrays allocated by the compute dynamic groups callback. This parameter can be NULL if no callback function is used to compute dynamic groups.</param>
		/// <param name="name">A string that identifies the resource manager. This parameter can be NULL if the resource manager does not need a name.</param>
		/// <param name="rm">A pointer to the returned resource manager handle. When you have finished using the handle, free it by calling the AuthzFreeResourceManager function.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzInitializeResourceManager(
			AuthzResourceManagerFlags flags,
			AuthzAccessCheckCallback pfnAccessCheck,
			AuthzComputeGroupsCallback pfnComputeDynamicGroups,
			AuthzFreeGroupsCallback pfnFreeDynamicGroups,
			[MarshalAs(UnmanagedType.LPWStr)] string name,
			out SafeAUTHZ_RESOURCE_MANAGER_HANDLE rm);

		/// <summary>
		/// The AuthzModifyClaims function adds, deletes, or modifies user and device claims in the Authz client context.
		/// </summary>
		/// <param name="hAuthzClientContext">A handle to the client context to be modified.</param>
		/// <param name="ClaimClass">Type of information to be modified. The caller can specify AuthzContextInfoUserClaims or AuthzContextInfoDeviceClaims.</param>
		/// <param name="pClaimOperations">A pointer to an array of AUTHZ_SECURITY_ATTRIBUTE_OPERATION enumeration values that specify the type of claim modification to make.</param>
		/// <param name="pClaims">A pointer to an AUTHZ_SECURITY_ATTRIBUTES_INFORMATION structure that specifies the claims to modify.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzModifyClaims(SafeAUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext, AUTHZ_CONTEXT_INFORMATION_CLASS ClaimClass, 
			AUTHZ_SECURITY_ATTRIBUTE_OPERATION[] pClaimOperations, ref AUTHZ_SECURITY_ATTRIBUTES_INFORMATION pClaims);

		/// <summary>
		/// The AuthzModifySids function adds, deletes, or modifies user and device groups in the Authz client context.
		/// </summary>
		/// <param name="hAuthzClientContext">A handle to the client context to be modified.</param>
		/// <param name="SidClass">Type of information to be modified. The caller can specify AuthzContextInfoGroupsSids, AuthzContextInfoRestrictedSids, or AuthzContextInfoDeviceSids.</param>
		/// <param name="pSidOperations">A pointer to an array of AUTHZ_SID_OPERATION enumeration values that specify the group modifications to make.</param>
		/// <param name="pSids">A pointer to a TOKEN_GROUPS structure that specifies the groups to modify.</param>
		/// <returns>If the function succeeds, the function returns TRUE. If the function fails, it returns FALSE. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(Authz), ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AuthzModifySids(SafeAUTHZ_CLIENT_CONTEXT_HANDLE hAuthzClientContext, AUTHZ_CONTEXT_INFORMATION_CLASS SidClass, 
			AUTHZ_SID_OPERATION[] pSidOperations, ref AdvApi32.TOKEN_GROUPS pSids);

		/// <summary>The AUTHZ_ACCESS_REPLY structure defines an access check reply.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public sealed class AUTHZ_ACCESS_REPLY : IDisposable
		{
			/// <summary>
			/// The number of elements in the GrantedAccessMask, SaclEvaluationResults, and Error arrays. This number matches the number of entries in the object
			/// type list structure used in the access check. If no object type is used to represent the object, then set ResultListLength to one.
			/// </summary>
			public int ResultListLength;
			/// <summary>An array of granted access masks. Memory for this array is allocated by the application before calling AccessCheck.</summary>
			public IntPtr GrantedAccessMask;
			/// <summary>
			/// An array of system access control list (SACL) evaluation results. Memory for this array is allocated by the application before calling
			/// AccessCheck. SACL evaluation will only be performed if auditing is requested.
			/// </summary>
			public IntPtr SaclEvaluationResults;
			/// <summary>An array of results for each element of the array. Memory for this array is allocated by the application before calling AccessCheck.</summary>
			public IntPtr Error;

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_ACCESS_REPLY"/> struct.
			/// </summary>
			/// <param name="count">The count of array members in each of the three arrays.</param>
			public AUTHZ_ACCESS_REPLY(uint count)
			{
				if (count == 0) return;
				ResultListLength = (int)count;
				int sz = Marshal.SizeOf(typeof(uint))*(int)count;
				AllocGrantedAccessMask = new SafeHGlobalHandle(sz);
				GrantedAccessMask = AllocGrantedAccessMask.DangerousGetHandle();
				AllocSaclEvaluationResults = new SafeHGlobalHandle(sz);
				SaclEvaluationResults = AllocSaclEvaluationResults.DangerousGetHandle();
				AllocError = new SafeHGlobalHandle(sz);
				Error = AllocError.DangerousGetHandle();
			}

#pragma warning disable 612
			/// <summary>Gets or sets the granted access mask values. The length of this array must match the value in <see cref="ResultListLength"/>.</summary>
			/// <value>The granted access mask values.</value>
			public uint[] GrantedAccessMaskValues
			{
				get { return AllocGrantedAccessMask?.ToIEnum<uint>(ResultListLength).ToArray(); }
				set
				{
					if (value == null && ResultListLength != 0)
						throw new ArgumentNullException(nameof(GrantedAccessMaskValues), $"Value cannot be null if {nameof(ResultListLength)} field does not equal 0.");
					if (value != null && value.Length != ResultListLength)
						throw new ArgumentOutOfRangeException(nameof(GrantedAccessMaskValues), $"Number of items must match value of {nameof(ResultListLength)} field.");
					AllocGrantedAccessMask = SafeHGlobalHandle.Alloc<uint>(value);
					GrantedAccessMask = AllocGrantedAccessMask.DangerousGetHandle();
				}
			}

			/// <summary>Gets or sets the system access control list (SACL) evaluation results values. The length of this array must match the value in <see cref="ResultListLength"/>.</summary>
			/// <value>The system access control list (SACL) evaluation results values.</value>
			public uint[] SaclEvaluationResultsValues
			{
				get { return AllocSaclEvaluationResults?.ToIEnum<uint>(ResultListLength).ToArray(); }
				set
				{
					if (value == null && ResultListLength != 0)
						throw new ArgumentNullException(nameof(SaclEvaluationResultsValues), $"Value cannot be null if {nameof(ResultListLength)} field does not equal 0.");
					if (value != null && value.Length != ResultListLength)
						throw new ArgumentOutOfRangeException(nameof(SaclEvaluationResultsValues), $"Number of items must match value of {nameof(ResultListLength)} field.");
					AllocSaclEvaluationResults = SafeHGlobalHandle.Alloc<uint>(value);
					SaclEvaluationResults = AllocSaclEvaluationResults.DangerousGetHandle();
				}
			}

			/// <summary>Gets or sets the results for each element of the array. The length of this array must match the value in <see cref="ResultListLength"/>.</summary>
			/// <value>The results values.</value>
			public uint[] ErrorValues
			{
				get { return AllocError?.ToIEnum<uint>(ResultListLength).ToArray(); }
				set
				{
					if (value == null && ResultListLength != 0)
						throw new ArgumentNullException(nameof(ErrorValues), $"Value cannot be null if {nameof(ResultListLength)} field does not equal 0.");
					if (value != null && value.Length != ResultListLength)
						throw new ArgumentOutOfRangeException(nameof(ErrorValues), $"Number of items must match value of {nameof(ResultListLength)} field.");
					AllocError = SafeHGlobalHandle.Alloc<uint>(value);
					Error = AllocError.DangerousGetHandle();
				}
			}
#pragma warning restore 612

			private SafeHGlobalHandle AllocGrantedAccessMask { get; set; }

			private SafeHGlobalHandle AllocSaclEvaluationResults { get; set; }

			private SafeHGlobalHandle AllocError { get; set; }

			void IDisposable.Dispose()
			{
				AllocGrantedAccessMask?.Dispose();
				GrantedAccessMask = IntPtr.Zero;
				AllocSaclEvaluationResults?.Dispose();
				SaclEvaluationResults = IntPtr.Zero;
				AllocError?.Dispose();
				Error = IntPtr.Zero;
				ResultListLength = 0;
			}
		}

		/// <summary>
		/// The AUTHZ_ACCESS_REQUEST structure defines an access check request.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct AUTHZ_ACCESS_REQUEST
		{
			/// <summary>
			/// The type of access to test for.
			/// </summary>
			public uint DesiredAccess;
			/// <summary>
			/// The security identifier (SID) to use for the principal self SID in the access control list (ACL).
			/// </summary>
			[MarshalAs(UnmanagedType.LPArray)]
			public IntPtr PrincipalSelfSid;
			/// <summary>
			/// An array of OBJECT_TYPE_LIST structures in the object tree for the object. Set to NULL unless the application checks access at the property level.
			/// </summary>
			[MarshalAs(UnmanagedType.LPArray)] public OBJECT_TYPE_LIST[] ObjectTypeList;
			/// <summary>
			/// The number of elements in the ObjectTypeList array. This member is necessary only if the application checks access at the property level.
			/// </summary>
			public uint ObjectTypeListLength;
			/// <summary>
			/// A pointer to memory to pass to AuthzAccessCheckCallback when checking callback access control entries (ACEs).
			/// </summary>
			public IntPtr OptionalArguments;

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_ACCESS_REQUEST"/> struct.
			/// </summary>
			/// <param name="access">The access.</param>
			public AUTHZ_ACCESS_REQUEST(AdvApi32.AccessTypes access) : this() { DesiredAccess = (uint)access; }

			/// <summary>
			/// Gets or sets the object types.
			/// </summary>
			/// <value>
			/// The object types.
			/// </value>
			public OBJECT_TYPE_LIST[] ObjectTypes { get { return ObjectTypeList; } set { ObjectTypeList = value; ObjectTypeListLength = (uint)(value?.Length ?? 0); } }
		}

		/// <summary>The AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE structure specifies a fully qualified binary name value associated with a security attribute.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE
		{
			/// <summary>The version number of the structure.</summary>
			public ulong Version;
			/// <summary>A pointer to strings that specify the names of the publisher, the product, and the original binary file of the value.</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string pName;
		}

		/// <summary>The AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE structure specifies an octet string value for a security attribute.</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE
		{
			/// <summary>A pointer to the value.</summary>
			public IntPtr pValue;
			/// <summary>The length, in bytes, of the pValue member.</summary>
			public uint ValueLength;
		}

		/// <summary>The AUTHZ_SECURITY_ATTRIBUTE_V1 structure defines a security attribute that can be associated with an authorization context.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
		public sealed class AUTHZ_SECURITY_ATTRIBUTE_V1 : IDisposable
		{
			/// <summary>A pointer to a name of a security attribute.</summary>
			[MarshalAs(UnmanagedType.LPWStr)] public string pName;
			/// <summary>The data type of the values pointed to by the Values member.</summary>
			public AUTHZ_SECURITY_ATTRIBUTE_DATATYPE ValueType;
			/// <summary>Reserved for future use.</summary>
			public ushort Reserved;
			/// <summary>A combination of one or more of the following values.</summary>
			public AUTHZ_SECURITY_ATTRIBUTE_FLAGS Flags;
			/// <summary>The number of values specified in the Values member.</summary>
			public uint ValueCount;
			/// <summary>A pointer to the value.</summary>
			public IntPtr pValue;

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<long> value)
			{
				Init(name, value, AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_INT64);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<ulong> value)
			{
				Init(name, value, AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_UINT64);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<bool> value)
			{
				Init(name, value.Select(Convert.ToInt64).ToList(), AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_BOOLEAN);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<string> value)
			{
				pName = name;
				SafeValue = SafeHGlobalHandle.Alloc(value, CharSet.Unicode);
				ValueType = AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_STRING;
				ValueCount = (uint)value.Count;
				pValue = SafeValue.DangerousGetHandle();
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE> value)
			{
				Init(name, value, AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_FQBN);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="AUTHZ_SECURITY_ATTRIBUTE_V1"/> struct.
			/// </summary>
			/// <param name="name">The name.</param>
			/// <param name="value">The value.</param>
			public AUTHZ_SECURITY_ATTRIBUTE_V1(string name, ICollection<AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE> value)
			{
				Init(name, value, AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING);
			}

			private void Init<T>(string name, ICollection<T> value, AUTHZ_SECURITY_ATTRIBUTE_DATATYPE type) where T : struct
			{
				pName = name;
				SafeValue = SafeHGlobalHandle.Alloc(value, value.Count);
				ValueType = type;
				ValueCount = (uint)value.Count;
				pValue = SafeValue.DangerousGetHandle();
			}

			private SafeHGlobalHandle SafeValue { get; set; }

			void IDisposable.Dispose()
			{
				SafeValue.Dispose();
				pValue = IntPtr.Zero;
			}

			/// <summary>Gets the list of <c>long</c> values.</summary>
			public IEnumerable<long> pInt64 => ValueType == AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_INT64 ? SafeValue.ToIEnum<long>((int)ValueCount) : null;

			/// <summary>Gets the list of <c>ulong</c> values.</summary>
			public IEnumerable<ulong> pUint64 => ValueType == AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_UINT64 ? SafeValue.ToIEnum<ulong>((int)ValueCount) : null;

			/// <summary>Gets the list of string values.</summary>
			public IEnumerable<string> ppString => ValueType == AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_STRING ? SafeValue.ToStringEnum((int)ValueCount, CharSet.Unicode) : null;

			/// <summary>Gets the list of <see cref="AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE"/> structures.</summary>
			public IEnumerable<AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE> pFqbn => ValueType == AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_FQBN ? SafeValue.ToIEnum<AUTHZ_SECURITY_ATTRIBUTE_FQBN_VALUE>((int)ValueCount) : null;

			/// <summary>Gets the list of <see cref="AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE"/> structures.</summary>
			public IEnumerable<AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE> pOctetString => ValueType == AUTHZ_SECURITY_ATTRIBUTE_DATATYPE.AUTHZ_SECURITY_ATTRIBUTE_TYPE_OCTET_STRING ? SafeValue.ToIEnum<AUTHZ_SECURITY_ATTRIBUTE_OCTET_STRING_VALUE>((int)ValueCount) : null;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
		public struct AUTHZ_SECURITY_ATTRIBUTES_INFORMATION
		{
			public ushort Version;
			public ushort Reserved;
			public uint AttributeCount;
			[MarshalAs(UnmanagedType.LPArray)]
			public AUTHZ_SECURITY_ATTRIBUTE_V1[] pAttributeV1;
		}

		public class SafeAUTHZ_ACCESS_CHECK_RESULTS_HANDLE : GenericSafeHandle
		{
			public SafeAUTHZ_ACCESS_CHECK_RESULTS_HANDLE() : base(AuthzFreeContext) { }

			public SafeAUTHZ_ACCESS_CHECK_RESULTS_HANDLE(AUTHZ_ACCESS_CHECK_RESULTS_HANDLE ptr, bool own = true) : base(ptr, AuthzFreeHandle, own) { }
		}

		public class SafeAUTHZ_AUDIT_EVENT_HANDLE : GenericSafeHandle
		{
			public SafeAUTHZ_AUDIT_EVENT_HANDLE() : base(AuthzFreeContext) { }

			public SafeAUTHZ_AUDIT_EVENT_HANDLE(AUTHZ_AUDIT_EVENT_HANDLE ptr, bool own = true) : base(ptr, AuthzFreeAuditEvent, own) { }
		}

		public class SafeAUTHZ_CLIENT_CONTEXT_HANDLE : GenericSafeHandle
		{
			public SafeAUTHZ_CLIENT_CONTEXT_HANDLE() : base(AuthzFreeContext) { }

			public SafeAUTHZ_CLIENT_CONTEXT_HANDLE(AUTHZ_CLIENT_CONTEXT_HANDLE ptr, bool own = true) : base(ptr, AuthzFreeContext, own) { }
		}

		public class SafeAUTHZ_RESOURCE_MANAGER_HANDLE : GenericSafeHandle
		{
			public SafeAUTHZ_RESOURCE_MANAGER_HANDLE() : base(AuthzFreeResourceManager) { }

			public SafeAUTHZ_RESOURCE_MANAGER_HANDLE(AUTHZ_RESOURCE_MANAGER_HANDLE ptr, bool own = true) : base(ptr, AuthzFreeResourceManager, own) { }
		}
	}
}