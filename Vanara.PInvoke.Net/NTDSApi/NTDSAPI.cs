using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using Vanara.Extensions;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static class NTDSApi
	{
		public enum DS_NAME_ERROR
		{
			DS_NAME_NO_ERROR = 0,

			// Generic processing error.
			DS_NAME_ERROR_RESOLVING = 1,

			// Couldn't find the name at all - or perhaps caller doesn't have
			// rights to see it.
			DS_NAME_ERROR_NOT_FOUND = 2,

			// Input name mapped to more than one output name.
			DS_NAME_ERROR_NOT_UNIQUE = 3,

			// Input name found, but not the associated output format.
			// Can happen if object doesn't have all the required attributes.
			DS_NAME_ERROR_NO_MAPPING = 4,

			// Unable to resolve entire name, but was able to determine which
			// domain object resides in.  Thus DS_NAME_RESULT_ITEM?.pDomain
			// is valid on return.
			DS_NAME_ERROR_DOMAIN_ONLY = 5,

			// Unable to perform a purely syntactical mapping at the client
			// without going out on the wire.
			DS_NAME_ERROR_NO_SYNTACTICAL_MAPPING = 6,

			// The name is from an external trusted forest.
			DS_NAME_ERROR_TRUST_REFERRAL = 7
		}

		[Flags]
		public enum DS_NAME_FLAGS
		{
			DS_NAME_NO_FLAGS = 0x0,

			// Perform a syntactical mapping at the client (if possible) without
			// going out on the wire.  Returns DS_NAME_ERROR_NO_SYNTACTICAL_MAPPING
			// if a purely syntactical mapping is not possible.
			DS_NAME_FLAG_SYNTACTICAL_ONLY = 0x1,

			// Force a trip to the DC for evaluation, even if this could be
			// locally cracked syntactically.
			DS_NAME_FLAG_EVAL_AT_DC = 0x2,

			// The call fails if the DC is not a GC
			DS_NAME_FLAG_GCVERIFY = 0x4,

			// Enable cross forest trust referral
			DS_NAME_FLAG_TRUST_REFERRAL = 0x8
		}

		public enum DS_NAME_FORMAT
		{
			/// <summary>
			/// Indicates the name is using an unknown name type. This format can impact performance because it forces the server to
			/// attempt to match all
			/// possible formats. Only use this value if the input format is unknown.
			/// </summary>
			DS_UNKNOWN_NAME = 0,

			/// <summary>
			/// Indicates that the fully qualified distinguished name is used. For example: CN = someone, OU = Users, DC = Engineering,
			/// DC = Fabrikam, DC = Com
			/// </summary>
			DS_FQDN_1779_NAME = 1,

			/// <summary>
			/// Indicates a Windows NT 4.0 account name. For example: Engineering\someone The domain-only version includes two trailing
			/// backslashes (\\).
			/// </summary>
			DS_NT4_ACCOUNT_NAME = 2,

			/// <summary>
			/// Indicates a user-friendly display name, for example, Jeff Smith. The display name is not necessarily the same as
			/// relative distinguished name (RDN).
			/// </summary>
			DS_DISPLAY_NAME = 3,

			/// <summary>
			/// Indicates a GUID string that the IIDFromString function returns. For example:
			/// {4fa050f0-f561-11cf-bdd9-00aa003a77b6}
			/// </summary>
			DS_UNIQUE_ID_NAME = 6,

			/// <summary>
			/// Indicates a complete canonical name. For example: engineering.fabrikam.com/software/someone The domain-only version
			/// includes a trailing forward
			/// slash (/).
			/// </summary>
			DS_CANONICAL_NAME = 7,

			/// <summary>Indicates that it is using the user principal name (UPN). For example: someone@engineering.fabrikam.com</summary>
			DS_USER_PRINCIPAL_NAME = 8,

			/// <summary>
			/// This element is the same as DS_CANONICAL_NAME except that the rightmost forward slash (/) is replaced with a newline
			/// character (\n), even in a
			/// domain-only case. For example: engineering.fabrikam.com/software\nsomeone
			/// </summary>
			DS_CANONICAL_NAME_EX = 9,

			/// <summary>Indicates it is using a generalized service principal name. For example: www/www.fabrikam.com@fabrikam.com</summary>
			DS_SERVICE_PRINCIPAL_NAME = 10,

			/// <summary>
			/// Indicates a Security Identifier (SID) for the object. This can be either the current SID or a SID from the object SID
			/// history. The SID string can
			/// use either the standard string representation of a SID, or one of the string constants defined in Sddl.h. For more
			/// information about converting a
			/// binary SID into a SID string, see SID Strings. The following is an example of a SID string:
			/// S-1-5-21-397955417-626881126-188441444-501
			/// </summary>
			DS_SID_OR_SID_HISTORY_NAME = 11,

			/// <summary>Not supported by the Directory Service (DS) APIs.</summary>
			DS_DNS_DOMAIN_NAME = 12,

			/// <summary>
			/// This causes DsCrackNames to return the distinguished names of all naming contexts in the current forest. The
			/// formatDesired parameter is ignored.
			/// cNames must be at least one and all strings in rpNames must have a length greater than zero characters. The contents of
			/// the rpNames strings is ignored.
			/// </summary>
			DS_LIST_NCS = unchecked((int)0xfffffff6)
		}

		private const uint NO_ERROR = 0;

		/// <summary>
		/// The DsBind function binds to a domain controller.DsBind uses the default process credentials to bind to the domain controller. To specify alternate
		/// credentials, use the DsBindWithCred function.
		/// </summary>
		/// <param name="DomainControllerName">
		/// Pointer to a null-terminated string that contains the name of the domain controller to bind to. This name can be the name of the domain controller or
		/// the fully qualified DNS name of the domain controller. Either name type can, optionally, be preceded by two backslash characters. All of the
		/// following examples represent correctly formatted domain controller names:
		/// <list type="bullet">
		/// <item><definition>"FAB-DC-01"</definition></item>
		/// <item><definition>"\\FAB-DC-01"</definition></item>
		/// <item><definition>"FAB-DC-01.fabrikam.com"</definition></item>
		/// <item><definition>"\\FAB-DC-01.fabrikam.com"</definition></item>
		/// </list>
		/// <para>This parameter can be NULL. For more information, see Remarks.</para>
		/// </param>
		/// <param name="DnsDomainName">
		/// Pointer to a null-terminated string that contains the fully qualified DNS name of the domain to bind to. This parameter can be NULL. For more
		/// information, see Remarks.
		/// </param>
		/// <param name="phDS">Address of a HANDLE value that receives the binding handle. To close this handle, pass it to the DsUnBind function.</param>
		/// <returns>Returns ERROR_SUCCESS if successful or a Windows or RPC error code otherwise.</returns>
		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint DsBind(
			string DomainControllerName, // in, optional
			string DnsDomainName, // in, optional
			out IntPtr phDS);

		/// <summary>The DsBindWithCred function binds to a domain controller using the specified credentials.</summary>
		/// <param name="DomainControllerName">
		/// Pointer to a null-terminated string that contains the name of the domain controller to bind to. This name can be the name of the domain controller or
		/// the fully qualified DNS name of the domain controller. Either name type can, optionally, be preceded by two backslash characters. All of the
		/// following examples represent correctly formatted domain controller names:
		/// <list type="bullet">
		/// <item><definition>"FAB-DC-01"</definition></item>
		/// <item><definition>"\\FAB-DC-01"</definition></item>
		/// <item><definition>"FAB-DC-01.fabrikam.com"</definition></item>
		/// <item><definition>"\\FAB-DC-01.fabrikam.com"</definition></item>
		/// </list>
		/// <para>This parameter can be NULL. For more information, see Remarks.</para>
		/// </param>
		/// <param name="DnsDomainName">
		/// Pointer to a null-terminated string that contains the fully qualified DNS name of the domain to bind to. This parameter can be NULL. For more
		/// information, see Remarks.
		/// </param>
		/// <param name="AuthIdentity">
		/// Contains an RPC_AUTH_IDENTITY_HANDLE value that represents the credentials to be used for the bind. The DsMakePasswordCredentials function is used to
		/// obtain this value. If this parameter is NULL, the credentials of the calling thread are used.
		/// <para>DsUnBind must be called before freeing this handle with the DsFreePasswordCredentials function.</para>
		/// </param>
		/// <param name="phDS">Address of a HANDLE value that receives the binding handle. To close this handle, pass it to the DsUnBind function.</param>
		/// <returns>Returns ERROR_SUCCESS if successful or a Windows or RPC error code otherwise.</returns>
		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint DsBindWithCred(
			string DomainControllerName, // in, optional
			string DnsDomainName, // in, optional
			SafeDsPasswordCredentialsHandle AuthIdentity, // in, optional
			out IntPtr phDS);

		/// <summary>
		/// The DsCrackNames function converts an array of directory service object names from one format to another. Name conversion enables client applications
		/// to map between the multiple names used to identify various directory service objects. For example, user objects can be identified by SAM account
		/// names (Domain\UserName), user principal name (UserName@Domain.com), or distinguished name.
		/// </summary>
		/// <param name="hSafeDs">
		/// Contains a directory service handle obtained from either the DSBind or DSBindWithCred function. If flags contains DS_NAME_FLAG_SYNTACTICAL_ONLY, hDS
		/// can be NULL.
		/// </param>
		/// <param name="flags">Contains one or more of the DS_NAME_FLAGS values used to determine how the name syntax will be cracked.</param>
		/// <param name="formatOffered">Contains one of the DS_NAME_FORMAT values that identifies the format of the input names.</param>
		/// <param name="formatDesired">
		/// Contains one of the DS_NAME_FORMAT values that identifies the format of the output names. The DS_SID_OR_SID_HISTORY_NAME value is not supported.
		/// </param>
		/// <param name="cNames">Contains the number of elements in the rpNames array.</param>
		/// <param name="rpNames">Pointer to an array of pointers to null-terminated strings that contain names to be converted.</param>
		/// <param name="ppResult">
		/// Pointer to a PDS_NAME_RESULT value that receives a DS_NAME_RESULT structure that contains the converted names. The caller must free this memory, when
		/// it is no longer required, by calling DsFreeNameResult.
		/// </param>
		/// <returns></returns>
		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint DsCrackNames(
			SafeDsHandle hSafeDs,
			DS_NAME_FLAGS flags,
			DS_NAME_FORMAT formatOffered,
			DS_NAME_FORMAT formatDesired,
			uint cNames,
			[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPTStr, SizeParamIndex = 4)] string[] rpNames,
			out IntPtr ppResult);

		/// <summary>A wrapper function for the DsCrackNames OS call</summary>
		/// <param name="hSafeDs">
		/// Contains a directory service handle obtained from either the DSBind or DSBindWithCred function. If flags contains DS_NAME_FLAG_SYNTACTICAL_ONLY, hDS
		/// can be NULL.
		/// </param>
		/// <param name="names">The names to be converted.</param>
		/// <param name="flags">Contains one or m
		/// ore of the DS_NAME_FLAGS values used to determine how the name syntax will be cracked.</param>
		/// <param name="formatOffered">Contains one of the DS_NAME_FORMAT values that identifies the format of the input names.</param>
		/// <param name="formatDesired">
		/// Contains one of the DS_NAME_FORMAT values that identifies the format of the output names. The DS_SID_OR_SID_HISTORY_NAME value is not supported.
		/// </param>
		/// <returns>The crack results.</returns>
		public static IEnumerable<DS_NAME_RESULT_ITEM> DsCrackNames(SafeDsHandle hSafeDs, string[] names = null,
			DS_NAME_FLAGS flags = DS_NAME_FLAGS.DS_NAME_NO_FLAGS,
			DS_NAME_FORMAT formatOffered = DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME,
			DS_NAME_FORMAT formatDesired = DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME)
		{
			IntPtr pResult;
			var err = DsCrackNames(hSafeDs, flags, formatOffered, formatDesired, (uint)(names?.Length ?? 0), names, out pResult);
			if (err != NO_ERROR)
				throw new Win32Exception((int)err);
			try
			{
				return pResult.ToStructure<DS_NAME_RESULT>().Items;
			}
			finally
			{
				DsFreeNameResult(pResult);
			}
		}

		/// <summary>
		/// The DsFreeNameResult function frees the memory held by a DS_NAME_RESULT structure. Use this function to free the memory allocated by the DsCrackNames function.
		/// </summary>
		/// <param name="pResult">Pointer to the DS_NAME_RESULT structure to be freed.</param>
		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto)]
		public static extern void DsFreeNameResult(IntPtr pResult /* DS_NAME_RESULT* */);

		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto)]
		public static extern void DsFreePasswordCredentials(IntPtr AuthIdentity);

		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto)]
		public static extern Win32Error DsMakePasswordCredentials(string User, string Domain, string Password, out IntPtr pAuthIdentity);

		/// <summary>The DsUnBind function finds an RPC session with a domain controller and unbinds a handle to the directory service (DS).</summary>
		/// <param name="phDS">Pointer to a bind handle to the directory service. This handle is provided by a call to DsBind, DsBindWithCred, or DsBindWithSpn.</param>
		/// <returns>0</returns>
		[DllImport(nameof(NTDSApi), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern uint DsUnBind(ref IntPtr phDS);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DS_NAME_RESULT
		{
			public uint cItems;
			public IntPtr rItems; // PDS_NAME_RESULT_ITEM

			public IEnumerable<DS_NAME_RESULT_ITEM> Items => rItems.ToIEnum<DS_NAME_RESULT_ITEM>((int)cItems);
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct DS_NAME_RESULT_ITEM
		{
			public DS_NAME_ERROR status;
			public string pDomain;
			public string pName;

			public override string ToString() => (status == DS_NAME_ERROR.DS_NAME_NO_ERROR ? pName : null) ?? "";
		}

		[SuppressUnmanagedCodeSecurity, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public class SafeDsHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			public SafeDsHandle(IntPtr hDs) : base(false) { SetHandle(hDs); }

			public SafeDsHandle(string dnsDomainName = null, string domainControllerName = null) : base(true)
			{
				var res = DsBind(domainControllerName, dnsDomainName, out handle);
				if (res != NO_ERROR)
					throw new Win32Exception((int)res);
			}

			public SafeDsHandle(SafeDsPasswordCredentialsHandle authIdentity, string dnsDomainName = null, string domainControllerName = null) : base(true)
			{
				var res = DsBindWithCred(domainControllerName, dnsDomainName, authIdentity, out handle);
				if (res != NO_ERROR)
					throw new Win32Exception((int)res);
			}

			public static SafeDsHandle Null { get; } = new SafeDsHandle(IntPtr.Zero);

			protected override bool ReleaseHandle() => DsUnBind(ref handle) == 0;
		}

		public class SafeDsPasswordCredentialsHandle : SafeHandleMinusOneIsInvalid
		{
			public SafeDsPasswordCredentialsHandle(string User, string Domain, string Password) : base(true)
			{
				DsMakePasswordCredentials(User, Domain, Password, out handle);
			}

			protected override bool ReleaseHandle() { DsFreePasswordCredentials(handle); return true; }
		}
	}
}