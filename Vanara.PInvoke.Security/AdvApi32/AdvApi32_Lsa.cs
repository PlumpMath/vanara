using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using Vanara.Extensions;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class AdvApi32
	{
		public const uint STATUS_SUCCESS = 0;
		public const uint STATUS_NO_MORE_ENTRIES = 0x8000001A;
		public const uint STATUS_NO_SUCH_PRIVILEGE = 0xC0000060;
		public const uint STATUS_SOME_NOT_MAPPED = 0x00000107;
		public const uint RPC_NT_INVALID_BOUND = 0xC0020023;

		[Flags]
		public enum LsaAccountAccessMask : uint
		{
			ACCOUNT_VIEW = 0x00000001,
			ACCOUNT_ADJUST_PRIVILEGES = 0x00000002,
			ACCOUNT_ADJUST_QUOTAS = 0x00000004,
			ACCOUNT_ADJUST_SYSTEM_ACCESS = 0x00000008,

			ACCOUNT_ALL_ACCESS = AccessTypes.STANDARD_RIGHTS_REQUIRED |
			                     ACCOUNT_VIEW |
			                     ACCOUNT_ADJUST_PRIVILEGES |
			                     ACCOUNT_ADJUST_QUOTAS |
			                     ACCOUNT_ADJUST_SYSTEM_ACCESS,

			ACCOUNT_READ = AccessTypes.STANDARD_RIGHTS_READ |
			               ACCOUNT_VIEW,

			ACCOUNT_WRITE = AccessTypes.STANDARD_RIGHTS_WRITE |
			                ACCOUNT_ADJUST_PRIVILEGES |
			                ACCOUNT_ADJUST_QUOTAS |
			                ACCOUNT_ADJUST_SYSTEM_ACCESS,

			ACCOUNT_EXECUTE = AccessTypes.STANDARD_RIGHTS_EXECUTE,
		}

		[Flags]
		public enum LsaLookupNamesFlags : uint
		{
			LSA_LOOKUP_ISOLATED_AS_LOCAL = 0x80000000
		}

		[Flags]
		public enum LsaPolicyRights : uint
		{
			POLICY_VIEW_LOCAL_INFORMATION = 1,
			POLICY_VIEW_AUDIT_INFORMATION = 2,
			POLICY_GET_PRIVATE_INFORMATION = 4,
			POLICY_TRUST_ADMIN = 8,
			POLICY_CREATE_ACCOUNT = 0x10,
			POLICY_CREATE_SECRET = 0x20,
			POLICY_CREATE_PRIVILEGE = 0x40,
			POLICY_SET_DEFAULT_QUOTA_LIMITS = 0x80,
			POLICY_SET_AUDIT_REQUIREMENTS = 0x100,
			POLICY_AUDIT_LOG_ADMIN = 0x200,
			POLICY_SERVER_ADMIN = 0x400,
			POLICY_LOOKUP_NAMES = 0x800,
			POLICY_NOTIFICATION = 0x1000,
			POLICY_ALL_ACCESS = AccessTypes.STANDARD_RIGHTS_REQUIRED |
			                    POLICY_VIEW_LOCAL_INFORMATION |
			                    POLICY_VIEW_AUDIT_INFORMATION |
			                    POLICY_GET_PRIVATE_INFORMATION |
			                    POLICY_TRUST_ADMIN |
			                    POLICY_CREATE_ACCOUNT |
			                    POLICY_CREATE_SECRET |
			                    POLICY_CREATE_PRIVILEGE |
			                    POLICY_SET_DEFAULT_QUOTA_LIMITS |
			                    POLICY_SET_AUDIT_REQUIREMENTS |
			                    POLICY_AUDIT_LOG_ADMIN |
			                    POLICY_SERVER_ADMIN |
			                    POLICY_LOOKUP_NAMES,
			POLICY_READ = AccessTypes.STANDARD_RIGHTS_READ |
			              POLICY_VIEW_AUDIT_INFORMATION |
			              POLICY_GET_PRIVATE_INFORMATION,
			POLICY_WRITE = AccessTypes.STANDARD_RIGHTS_WRITE |
			               POLICY_TRUST_ADMIN |
			               POLICY_CREATE_ACCOUNT |
			               POLICY_CREATE_SECRET |
			               POLICY_CREATE_PRIVILEGE |
			               POLICY_SET_DEFAULT_QUOTA_LIMITS |
			               POLICY_SET_AUDIT_REQUIREMENTS |
			               POLICY_AUDIT_LOG_ADMIN |
			               POLICY_SERVER_ADMIN,
			POLICY_EXECUTE = AccessTypes.STANDARD_RIGHTS_EXECUTE |
			                 POLICY_VIEW_LOCAL_INFORMATION |
			                 POLICY_LOOKUP_NAMES,
		}

		/// <summary>
		/// The LsaAddAccountRights function assigns one or more privileges to an account. If the account does not exist, LsaAddAccountRights creates it.
		/// </summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have the POLICY_LOOKUP_NAMES access right. If the account identified by the AccountSid parameter does
		/// not exist, the handle must have the POLICY_CREATE_ACCOUNT access right. For more information, see Opening a Policy Object Handle.
		/// </param>
		/// <param name="pSID">Pointer to the SID of the account to which the function assigns privileges.</param>
		/// <param name="UserRights">
		/// Pointer to an array of strings. Each string contains the name of a privilege to add to the account. For a list of privilege names, see Privilege Constants.
		/// </param>
		/// <param name="CountOfRights">Specifies the number of elements in the UserRights array.</param>
		/// <returns>
		/// If the function succeeds, the return value is STATUS_SUCCESS. If the function fails, the return value is an NTSTATUS code, which can be the following
		/// value or one of the LSA Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true), SuppressUnmanagedCodeSecurity]
		public static extern uint LsaAddAccountRights(
			SafeLsaPolicyHandle PolicyHandle,
			PSID pSID,
			[In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringArrayMarshaler))] string[] UserRights,
			int CountOfRights);

		/// <summary>The LsaClose function closes a handle to a Policy or TrustedDomain object.</summary>
		/// <param name="ObjectHandle">
		/// A handle to a Policy object returned by the LsaOpenPolicy function or to a TrustedDomain object returned by the LsaOpenTrustedDomainByName function.
		/// Following the completion of this call, the handle is no longer valid.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is STATUS_SUCCESS. If the function fails, the return value is an NTSTATUS code.For more information, see
		/// LSA Policy Function Return Values. You can use the LsaNtStatusToWinError function to convert the NTSTATUS code to a Windows error code.
		/// </returns>
	   [DllImport(nameof(AdvApi32), ExactSpelling = true)]
		private static extern uint LsaClose(IntPtr ObjectHandle);

		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaCreateAccount(SafeLsaPolicyHandle PolicyHandle, PSID AccountSid, LsaAccountAccessMask DesiredAccess, out SafeLsaPolicyHandle AccountHandle);

		/// <summary>The LsaEnumerateAccountRights function enumerates the privileges assigned to an account.</summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have the POLICY_LOOKUP_NAMES access right. For more information, see Opening a Policy Object Handle.
		/// </param>
		/// <param name="AccountSid">Pointer to the SID of the account for which to enumerate privileges.</param>
		/// <param name="UserRights">
		/// Receives a pointer to an array of LSA_UNICODE_STRING structures. Each structure contains the name of a privilege held by the account. For a list of
		/// privilege names, see Privilege Constants.
		/// </param>
		/// <param name="CountOfRights">Pointer to a variable that receives the number of privileges in the UserRights array.</param>
		/// <returns>
		/// If at least one account right is found, the function succeeds and returns STATUS_SUCCESS.
		/// <para>
		/// If no account rights are found or if the function fails for any other reason, the function returns an NTSTATUS code such as FILE_NOT_FOUND. For more
		/// information, see LSA Policy Function Return Values. Use the LsaNtStatusToWinError function to convert the NTSTATUS code to a Windows error code.
		/// </para>
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaEnumerateAccountRights(
			SafeLsaPolicyHandle PolicyHandle,
			PSID AccountSid,
			out SafeLsaMemoryHandle UserRights,
			out uint CountOfRights);

		/// <summary>The LsaEnumerateAccountRights function enumerates the privileges assigned to an account.</summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have the POLICY_LOOKUP_NAMES access right. For more information, see Opening a Policy Object Handle.
		/// </param>
		/// <param name="AccountSid">Pointer to the SID of the account for which to enumerate privileges.</param>
		/// <returns>An enumeration of strings containing the names of privileges held by the account. For a list of privilege names, see Privilege Constants.</returns>
		public static IEnumerable<string> LsaEnumerateAccountRights(SafeLsaPolicyHandle PolicyHandle, PSID AccountSid)
		{
			SafeLsaMemoryHandle mem;
			uint cnt;
			var ret = LsaEnumerateAccountRights(PolicyHandle, AccountSid, out mem, out cnt);
			var winErr = LsaNtStatusToWinError(ret);
			if (winErr == Win32Error.ERROR_FILE_NOT_FOUND) return new string[0];
			if (winErr != 0) throw new Win32Exception(winErr);
			return mem.DangerousGetHandle().ToIEnum<string, LSA_UNICODE_STRING>((int)cnt, u => (string)u.ToString().Clone());
		}

		/// <summary>
		/// The LsaEnumerateAccountsWithUserRight function returns the accounts in the database of a Local Security Authority (LSA) Policy object that hold a
		/// specified privilege. The accounts returned by this function hold the specified privilege directly through the user account, not as part of membership
		/// to a group.
		/// </summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have POLICY_LOOKUP_NAMES and POLICY_VIEW_LOCAL_INFORMATION user rights. For more information, see
		/// Opening a Policy Object Handle.
		/// </param>
		/// <param name="UserRights">
		/// A string that specifies the name of a privilege. For a list of privileges, see Privilege Constants and Account Rights Constants.
		/// <para>If this parameter is NULL, the function enumerates all accounts in the LSA database of the system associated with the Policy object.</para>
		/// </param>
		/// <param name="EnumerationBuffer">
		/// Pointer to a variable that receives a pointer to an array of LSA_ENUMERATION_INFORMATION structures. The Sid member of each structure is a pointer to
		/// the security identifier (SID) of an account that holds the specified privilege.
		/// </param>
		/// <param name="CountReturned">Pointer to a variable that receives the number of entries returned in the EnumerationBuffer parameter.</param>
		/// <returns>
		/// If the function succeeds, the function returns STATUS_SUCCESS. If the function fails, it returns an NTSTATUS code, which can be one of the following
		/// values or one of the LSA Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaEnumerateAccountsWithUserRight(
			SafeLsaPolicyHandle PolicyHandle,
			[In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringMarshaler))] string UserRights,
			out SafeLsaMemoryHandle EnumerationBuffer,
			out int CountReturned);

		/// <summary>
		/// The LsaEnumerateAccountsWithUserRight function returns the accounts in the database of a Local Security Authority (LSA) Policy object that hold a
		/// specified privilege. The accounts returned by this function hold the specified privilege directly through the user account, not as part of membership
		/// to a group.
		/// </summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have POLICY_LOOKUP_NAMES and POLICY_VIEW_LOCAL_INFORMATION user rights. For more information, see
		/// Opening a Policy Object Handle.
		/// </param>
		/// <param name="UserRights">
		/// A string that specifies the name of a privilege. For a list of privileges, see Privilege Constants and Account Rights Constants.
		/// <para>If this parameter is NULL, the function enumerates all accounts in the LSA database of the system associated with the Policy object.</para>
		/// </param>
		/// <returns>An enumeration of security identifiers (SID) of accounts that holds the specified privilege.</returns>
		public static IEnumerable<PSID> LsaEnumerateAccountsWithUserRight(SafeLsaPolicyHandle PolicyHandle, string UserRights)
		{
			SafeLsaMemoryHandle mem;
			int cnt;
			var ret = LsaEnumerateAccountsWithUserRight(PolicyHandle, UserRights, out mem, out cnt);
			if (ret == STATUS_NO_MORE_ENTRIES) return new PSID[0];
			var wret = LsaNtStatusToWinError(ret);
			if (wret != 0) throw new Win32Exception(wret);
			return mem.DangerousGetHandle().ToIEnum<PSID, LSA_ENUMERATION_INFORMATION>(cnt, u => PSID.Copy(u.Sid));
		}

		/// <summary>
		/// The LsaGetAppliedCAPIDs function returns an array of central access policies (CAPs) identifiers (CAPIDs) of all the CAPs applied on a specific computer.
		/// </summary>
		/// <param name="systemName">The name of the specific computer. The name can have the form of "ComputerName" or "\\ComputerName". If this parameter is NULL, then the function returns the CAPIDs of the local computer.</param>
		/// <param name="CAPIDs">A pointer to a variable that receives an array of pointers to CAPIDs that identify the CAPs available on the specified computer. When you have finished using the CAPIDs, call the LsaFreeMemory function on each element in the array and the entire array.</param>
		/// <param name="CAPIDCount">A pointer to a variable that receives the number of CAPs that are available on the specified computer. The array returned in the CAPIDs parameter contains the same number of elements as the CAPIDCount parameter.</param>
		/// <returns>
		/// If the function succeeds, the return value is STATUS_SUCCESS.
		/// <para>If the function fails, the return value is one of the LSA Policy Function Return Values. You can use the LsaNtStatusToWinError function to convert the NTSTATUS code to a Windows error code.</para></returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern uint LsaGetAppliedCAPIDs([In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringMarshaler))] string systemName,
			out SafeLsaMemoryHandle CAPIDs, out uint CAPIDCount);

		/// <summary>
		/// The LsaFreeMemory function frees memory allocated for an output buffer by an LSA function call. LSA functions that return variable-length output
		/// buffers always allocate the buffer on behalf of the caller. The caller must free this memory by passing the returned buffer pointer to LsaFreeMemory
		/// when the memory is no longer required.
		/// </summary>
		/// <param name="Buffer">Pointer to memory buffer that was allocated by an LSA function call. If LsaFreeMemory is successful, this buffer is freed.</param>
		/// <returns>
		/// If the function succeeds, the return value is STATUS_SUCCESS. If the function fails, the return value is an NTSTATUS code, which can be the following
		/// value or one of the LSA Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		private static extern uint LsaFreeMemory(IntPtr Buffer);

		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaGetSystemAccessAccount(SafeLsaPolicyHandle AccountHandle, out int SystemAccess);

		/// <summary>
		/// The LsaLookupNames2 function retrieves the security identifiers (SIDs) for specified account names. LsaLookupNames2 can look up the SID for any
		/// account in any domain in a Windows forest.
		/// </summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have the POLICY_LOOKUP_NAMES access right. For more information, see Opening a Policy Object Handle.
		/// </param>
		/// <param name="Flags">Values that control the behavior of this function.</param>
		/// <param name="Count">Specifies the number of names in the Names array. This is also the number of entries returned in the Sids array.</param>
		/// <param name="Names">
		/// An array of strings that contain the names to look up. These strings can be the names of user, group, or local group accounts, or the names of
		/// domains. Domain names can be DNS domain names or NetBIOS domain names.
		/// </param>
		/// <param name="ReferencedDomains">
		/// Receives a pointer to an LSA_REFERENCED_DOMAIN_LIST structure. The Domains member of this structure is an array that contains an entry for each
		/// domain in which a name was found. The DomainIndex member of each entry in the Sids array is the index of the Domains array entry for the domain in
		/// which the name was found.
		/// </param>
		/// <param name="Sids">
		/// Receives a pointer to an array of LSA_TRANSLATED_SID2 structures. Each entry in the Sids array contains the SID information for the corresponding
		/// entry in the Names array.
		/// </param>
		/// <returns>
		/// If the function succeeds, the function returns one of the following NTSTATUS values. If the function fails, the return value is the following
		/// NTSTATUS value or one of the LSA Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern uint LsaLookupNames2(
			SafeLsaPolicyHandle PolicyHandle,
			LsaLookupNamesFlags Flags,
			uint Count,
			[In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringArrayMarshaler))] string[] Names,
			out SafeLsaMemoryHandle ReferencedDomains,
			out SafeLsaMemoryHandle Sids);

		/// <summary>The LsaNtStatusToWinError function converts an NTSTATUS code returned by an LSA function to a Windows error code.</summary>
		/// <param name="NTSTATUS">An NTSTATUS code returned by an LSA function call. This value will be converted to a System error code.</param>
		/// <returns>
		/// The return value is the Windows error code that corresponds to the Status parameter. If there is no corresponding Windows error code, the return
		/// value is ERROR_MR_MID_NOT_FOUND.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern int LsaNtStatusToWinError(uint NTSTATUS);

		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaOpenAccount(SafeLsaPolicyHandle PolicyHandle, PSID AccountSid, LsaAccountAccessMask DesiredAccess, out SafeLsaPolicyHandle AccountHandle);

		/// <summary>
		/// The LsaOpenPolicy function opens a handle to the Policy object on a local or remote system. You must run the process "As Administrator" so that the
		/// call doesn't fail with ERROR_ACCESS_DENIED.
		/// </summary>
		/// <param name="SystemName">
		/// Name of the target system. The name can have the form "ComputerName" or "\\ComputerName". If this parameter is NULL, the function opens the Policy
		/// object on the local system.
		/// </param>
		/// <param name="ObjectAttributes">
		/// A pointer to an LSA_OBJECT_ATTRIBUTES structure that specifies the connection attributes. The structure members are not used; initialize them to NULL
		/// or zero.
		/// </param>
		/// <param name="DesiredAccess">
		/// An ACCESS_MASK that specifies the requested access rights. The function fails if the DACL of the target system does not allow the caller the
		/// requested access. To determine the access rights that you need, see the documentation for the LSA functions with which you want to use the policy handle.
		/// </param>
		/// <param name="PolicyHandle">A pointer to an LSA_HANDLE variable that receives a handle to the Policy object.</param>
		/// <returns>
		/// If the function succeeds, the function returns STATUS_SUCCESS. If the function fails, it returns an NTSTATUS code.For more information, see LSA
		/// Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true), SuppressUnmanagedCodeSecurity]
		public static extern uint LsaOpenPolicy(
			[In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringMarshaler))] string SystemName,
			ref LSA_OBJECT_ATTRIBUTES ObjectAttributes,
			LsaPolicyRights DesiredAccess,
			out SafeLsaPolicyHandle PolicyHandle);

		/// <summary>
		/// The LsaOpenPolicy function opens a handle to the Policy object on a local or remote system. You must run the process "As Administrator" so that the
		/// call doesn't fail with ERROR_ACCESS_DENIED.
		/// </summary>
		/// <param name="SystemName">
		/// Name of the target system. The name can have the form "ComputerName" or "\\ComputerName". If this parameter is NULL, the function opens the Policy
		/// object on the local system.
		/// </param>
		/// <param name="DesiredAccess">
		/// An ACCESS_MASK that specifies the requested access rights. The function fails if the DACL of the target system does not allow the caller the
		/// requested access. To determine the access rights that you need, see the documentation for the LSA functions with which you want to use the policy handle.
		/// </param>
		/// <returns>A pointer to an LSA_HANDLE variable that receives a handle to the Policy object.</returns>
		public static SafeLsaPolicyHandle LsaOpenPolicy(string SystemName, LsaPolicyRights DesiredAccess)
		{
			var oa = LSA_OBJECT_ATTRIBUTES.Empty;
			SafeLsaPolicyHandle handle;
			var ret = LsaNtStatusToWinError(LsaOpenPolicy(SystemName, ref oa, DesiredAccess, out handle));
			if (ret != 0) throw new Win32Exception(ret);
			return handle;
		}

		/// <summary>
		/// The LsaRemoveAccountRights function removes one or more privileges from an account. You can specify the privileges to be removed, or you can set a
		/// flag to remove all privileges. When you remove all privileges, the function deletes the account. If you specify privileges not held by the account,
		/// the function ignores them.
		/// </summary>
		/// <param name="PolicyHandle">
		/// A handle to a Policy object. The handle must have the POLICY_LOOKUP_NAMES access right. For more information, see Opening a Policy Object Handle.
		/// </param>
		/// <param name="AccountSid">Pointer to the security identifier (SID) of the account from which the privileges are removed.</param>
		/// <param name="AllRights">
		/// If TRUE, the function removes all privileges and deletes the account. In this case, the function ignores the UserRights parameter. If FALSE, the
		/// function removes the privileges specified by the UserRights parameter.
		/// </param>
		/// <param name="UserRights">
		/// An array of strings. Each string contains the name of a privilege to be removed from the account. For a list of privilege names, see Privilege Constants.
		/// </param>
		/// <param name="CountOfRights">Specifies the number of elements in the UserRights array.</param>
		/// <returns>
		/// If the function succeeds, the return value is STATUS_SUCCESS. If the function fails, the return value is an NTSTATUS code, which can be one of the
		/// following values or one of the LSA Policy Function Return Values.
		/// </returns>
		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaRemoveAccountRights(
			SafeLsaPolicyHandle PolicyHandle,
			PSID AccountSid,
			[MarshalAs(UnmanagedType.Bool)] bool AllRights,
			[In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(LsaUnicodeStringArrayMarshaler))] string[] UserRights,
			int CountOfRights);

		[DllImport(nameof(AdvApi32), ExactSpelling = true)]
		public static extern uint LsaSetSystemAccessAccount(SafeLsaPolicyHandle AccountHandle, int SystemAccess);

		[StructLayout(LayoutKind.Sequential)]
		public struct LSA_ENUMERATION_INFORMATION
		{
			public IntPtr Sid;
		}

		/// <summary>
		/// The LSA_OBJECT_ATTRIBUTES structure is used with the LsaOpenPolicy function to specify the attributes of the connection to the Policy object. When
		/// you call LsaOpenPolicy, initialize the members of this structure to NULL or zero because the function does not use the information.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct LSA_OBJECT_ATTRIBUTES
		{
			public int Length;
			public IntPtr RootDirectory;
			public IntPtr ObjectName;
			public int Attributes;
			public IntPtr SecurityDescriptor;
			public IntPtr SecurityQualityOfService;

			public static LSA_OBJECT_ATTRIBUTES Empty { get; } = new LSA_OBJECT_ATTRIBUTES();
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LSA_REFERENCED_DOMAIN_LIST
		{
			public uint Entries;
			public IntPtr Domains;

			public IEnumerable<LSA_TRUST_INFORMATION> DomainList => Domains == IntPtr.Zero ? new LSA_TRUST_INFORMATION[0] : Domains.ToIEnum<LSA_TRUST_INFORMATION>((int)Entries);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LSA_TRANSLATED_SID2
		{
			public SID_NAME_USE Use;
			public IntPtr Sid;
			public int DomainIndex;
			/// <summary>Not used.</summary>
			public uint Flags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LSA_TRUST_INFORMATION
		{
			public LSA_UNICODE_STRING Name;
			public IntPtr Sid;
		}

		internal class LsaUnicodeStringMarshaler : ICustomMarshaler
		{
			public static ICustomMarshaler GetInstance(string cookie) => new LsaUnicodeStringMarshaler();

			public object MarshalNativeToManaged(IntPtr pNativeData)
			{
				if (pNativeData == IntPtr.Zero) return null;
				var ret = pNativeData.ToStructure<LSA_UNICODE_STRING>();
				var s = (string)ret.ToString().Clone();
				LsaFreeMemory(pNativeData);
				return s;
			}

			public IntPtr MarshalManagedToNative(object ManagedObj)
			{
				var s = ManagedObj as string;
				if (s == null) return IntPtr.Zero;
				var str = new LSA_UNICODE_STRING(s);
				return str.StructureToPtr();
			}

			public void CleanUpNativeData(IntPtr pNativeData)
			{
				if (pNativeData == IntPtr.Zero) return;
				Marshal.FreeCoTaskMem(pNativeData);
				pNativeData = IntPtr.Zero;
			}

			public void CleanUpManagedData(object ManagedObj) { }

			public int GetNativeDataSize() => Marshal.SizeOf(typeof(LSA_UNICODE_STRING));
		}

		internal class LsaUnicodeStringArrayMarshaler : ICustomMarshaler
		{
			private static readonly Dictionary<IntPtr, int> pastCallArraySizes = new Dictionary<IntPtr, int>();

			public static ICustomMarshaler GetInstance(string cookie) => new LsaUnicodeStringArrayMarshaler();

			public object MarshalNativeToManaged(IntPtr pNativeData)
			{
				if (pNativeData == IntPtr.Zero) return null;
				throw new InvalidOperationException(@"Unable to marshal LSA_UNICODE_STRING arrays from unmanaged to managed code.");
			}

			public IntPtr MarshalManagedToNative(object ManagedObj)
			{
				var a = ManagedObj as string[];
				if (a != null)
				{
					var uma = Array.ConvertAll(a, p => new LSA_UNICODE_STRING(p));
					var sz = Marshal.SizeOf(typeof(LSA_UNICODE_STRING));
					var result = Marshal.AllocCoTaskMem(sz * a.Length);
					pastCallArraySizes.Add(result, a.Length);
					var ptr = result;
					foreach (var value in uma)
					{
						Marshal.StructureToPtr(value, ptr, false);
						ptr = new IntPtr(ptr.ToInt64() + sz);
					}
					return result;
				}
				return IntPtr.Zero;
			}

			public void CleanUpNativeData(IntPtr pNativeData)
			{
				if (pNativeData == IntPtr.Zero) return;
				int length;
				if (pastCallArraySizes.TryGetValue(pNativeData, out length))
				{
					var sz = Marshal.SizeOf(typeof(LSA_UNICODE_STRING));
					for (var i = 0; i < length; i++)
						Marshal.DestroyStructure(new IntPtr(pNativeData.ToInt64() + sz * i), typeof(LSA_UNICODE_STRING));
					pastCallArraySizes.Remove(pNativeData);
				}
				Marshal.FreeCoTaskMem(pNativeData);
				pNativeData = IntPtr.Zero;
			}

			public void CleanUpManagedData(object ManagedObj) { }

			public int GetNativeDataSize() => -1;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Size = 8)]
		public struct LSA_UNICODE_STRING
		{
			public ushort length;
			public ushort MaximumLength;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Buffer;

			public LSA_UNICODE_STRING(string s)
			{
				if (s == null)
				{
					length = MaximumLength = 0;
					Buffer = null;
				}
				else
				{
					var l = s.Length * System.Text.UnicodeEncoding.CharSize;
					// Unicode strings max. 32KB
					if (l >= ushort.MaxValue)
						throw new ArgumentException("String too long");
					Buffer = s;
					length = (ushort)l;
					MaximumLength = (ushort)(l + System.Text.UnicodeEncoding.CharSize);
				}
			}

			public int Length => length / System.Text.UnicodeEncoding.CharSize;

			public override string ToString() => Buffer;

			public static implicit operator string(LSA_UNICODE_STRING value) => value.ToString();
		}

		public sealed class SafeLsaMemoryHandle : SafeBuffer
		{
			public SafeLsaMemoryHandle() : base(true) { }

			public SafeLsaMemoryHandle(IntPtr ptr, bool own = true) : base(own) { SetHandle(ptr); }

			public static SafeLsaMemoryHandle Invalid { get; } = new SafeLsaMemoryHandle(IntPtr.Zero);

			protected override bool ReleaseHandle() => LsaFreeMemory(handle) == 0;
		}

		public sealed class SafeLsaPolicyHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			public SafeLsaPolicyHandle() : base(true) { }

			public SafeLsaPolicyHandle(IntPtr ptr, bool own = true) : base(own) { SetHandle(ptr); }

			protected override bool ReleaseHandle() => LsaClose(handle) == 0;
		}
	}
}