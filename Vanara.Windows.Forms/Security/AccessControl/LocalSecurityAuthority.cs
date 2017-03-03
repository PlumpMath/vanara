using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Vanara.Extensions;
using Vanara.PInvoke;
using static Vanara.PInvoke.AdvApi32;
// ReSharper disable InconsistentNaming

namespace Vanara.Security.AccessControl
{
	/// <summary>Provides access to the local security authority on a given server.</summary>
	public class LocalSecurityAuthority : IDisposable
	{
		private static LocalSecurityAuthority local;
		private readonly string svr;
		private SafeLsaPolicyHandle handle;

		/// <summary>Initializes a new instance of the <see cref="LocalSecurityAuthority"/> class.</summary>
		/// <param name="server">The server. Use <c>null</c> for the local server.</param>
		/// <param name="access">The access rights mask for the actions to be taken.</param>
		public LocalSecurityAuthority(string server = null, DesiredAccess access = DesiredAccess.AllAccess)
		{
			handle = LsaOpenPolicy(server, (LsaPolicyRights)((uint)AccessTypes.STANDARD_RIGHTS_REQUIRED | (uint)access));
			svr = server;
			CurrentUserLogonRights = new LogonRights(this);
			CurrentUserPrivileges = new AccountPrivileges(this);
		}

		/// <summary>
		/// Account rights determine the type of logon that a user account can perform. An administrator assigns account rights to user and group accounts. Each
		/// user's account rights include those granted to the user and to the groups to which the user belongs.
		/// </summary>
		[Flags]
		public enum AccountLogonRights
		{
			/// <summary>Required for an account to log on using the interactive logon type.</summary>
			InteractiveLogon = 0x00000001,

			/// <summary>Required for an account to log on using the network logon type.</summary>
			NetworkLogon = 0x00000002,

			/// <summary>Required for an account to log on using the batch logon type.</summary>
			BatchLogon = 0x00000004,

			/// <summary>Required for an account to log on using the service logon type.</summary>
			ServiceLogon = 0x00000010,

			/// <summary>Explicitly denies an account the right to log on using the interactive logon type.</summary>
			DenyInteractiveLogon = 0x00000040,

			/// <summary>Explicitly denies an account the right to log on using the network logon type.</summary>
			DenyNetworkLogon = 0x00000080,

			/// <summary>Explicitly denies an account the right to log on using the batch logon type.</summary>
			DenyBatchLogon = 0x00000100,

			/// <summary>Explicitly denies an account the right to log on using the service logon type.</summary>
			DenyServiceLogon = 0x00000200,

			/// <summary>Remote interactive logon</summary>
			RemoteInteractiveLogon = 0x00000400,

			/// <summary>Explicitly denies an account the right to log on remotely using the interactive logon type.</summary>
			DenyRemoteInteractiveLogon = 0x00000800,
		}

		/// <summary>Access rights for a local security policy.</summary>
		[Flags]
		public enum DesiredAccess
		{
			/// <summary>
			/// This access type is needed to read the target system's miscellaneous security policy information. This
			/// includes the default quota, auditing, server state and role information, and trust information. This
			/// access type is also needed to enumerate trusted domains, accounts, and privileges.
			/// </summary>
			ViewLocalInformation = 1,

			/// <summary>This access type is needed to view audit trail or audit requirements information.</summary>
			ViewAuditInformation = 2,

			/// <summary>
			/// This access type is needed to view sensitive information, such as the names of accounts established for
			/// trusted domain relationships.
			/// </summary>
			GetPrivateInformation = 4,

			/// <summary>This access type is needed to change the account domain or primary domain information.</summary>
			TrustAdmin = 8,

			/// <summary>This access type is needed to create a new Account object.</summary>
			CreateAccount = 0x10,

			/// <summary>This access type is needed to create a new Private Data object.</summary>
			CreateSecret = 0x20,

			/// <summary>Set the default system quotas that are applied to user accounts.</summary>
			SetDefaultQuotaLimits = 0x80,

			/// <summary>This access type is needed to update the auditing requirements of the system.</summary>
			SetAuditRequirements = 0x100,

			/// <summary>
			/// This access type is needed to change the characteristics of the audit trail such as its maximum size or
			/// the retention period for audit records, or to clear the log.
			/// </summary>
			AuditLogAdmin = 0x200,

			/// <summary>
			/// This access type is needed to modify the server state or role (master/replica) information. It is also
			/// needed to change the replica source and account name information.
			/// </summary>
			ServerAdmin = 0x400,

			/// <summary>This access type is needed to translate between names and SIDs.</summary>
			LookupNames = 0x800,

			/// <summary>All access.</summary>
			AllAccess = 0xFFF,
		}

		/// <summary>Gets the current user's system access.</summary>
		/// <value>The current user's system access.</value>
		public LogonRights CurrentUserLogonRights { get; }

		/// <summary>Gets the current user's account rights.</summary>
		/// <value>The current user's account rights.</value>
		public AccountPrivileges CurrentUserPrivileges { get; }

		/// <summary>Gets a <see cref="LocalSecurityAuthority"/> instance for the local server and rights to lookup names.</summary>
		/// <value>A <see cref="LocalSecurityAuthority"/> instance for the local server.</value>
		public static LocalSecurityAuthority Local => local ?? (local = new LocalSecurityAuthority());

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() { handle = null; }

		/// <summary>Enumerates the accounts with the specified privilege.</summary>
		/// <param name="privilege">The privilege name.</param>
		/// <returns>An array of <see cref="SecurityIdentifier"/> objects representing all accounts with the specified privilege.</returns>
		public IEnumerable<SecurityIdentifier> EnumerateAccountsWithRight(string privilege)
		{
			SafeLsaMemoryHandle buffer;
			int count;
			var ret = LsaEnumerateAccountsWithUserRight(handle, privilege, out buffer, out count);
			if (ret == STATUS_NO_MORE_ENTRIES)
				return new SecurityIdentifier[0];
			ThrowIfLsaError(ret);
			return buffer.DangerousGetHandle().ToIEnum<SecurityIdentifier, LSA_ENUMERATION_INFORMATION>(count, i => new SecurityIdentifier(i.Sid));
		}

		/// <summary>Enumerates the accounts with the specified privilege.</summary>
		/// <param name="privilege">The privilege.</param>
		/// <returns>An array of <see cref="SecurityIdentifier"/> objects representing all accounts with the specified privilege.</returns>
		public IEnumerable<SecurityIdentifier> EnumerateAccountsWithRight(SystemPrivilege privilege) => EnumerateAccountsWithRight(FromPriv(privilege));

		/// <summary>
		/// Looks up an account name and returns information about it in a <see cref="SidLookupResult"/> class.
		/// </summary>
		/// <param name="name">The name of the account.</param>
		/// <returns>A <see cref="SidLookupResult"/> for the <paramref name="name"/>.</returns>
		public SidLookupResult LookupName(string name) => LookupNames(false, name).FirstOrDefault();

		/// <summary>
		/// Looks up a list of account names and returns information about each in a <see cref="SidLookupResult"/> class.
		/// </summary>
		/// <param name="localOnly">if set to <c>true</c> restrict the search to only local accounts.</param>
		/// <param name="names">The account names to lookup.</param>
		/// <returns>Contains a corresponding result for each name provided in <paramref name="names"/>.</returns>
		/// <exception cref="ArgumentException">At least one user name must be supplied.</exception>
		public IList<SidLookupResult> LookupNames(bool localOnly, params string[] names)
		{
			if (names == null || names.Length == 0)
				throw new ArgumentException(@"At least one user name must be supplied.", nameof(names));
			SafeLsaMemoryHandle domains, sids;
			var ret = LsaLookupNames2(handle, localOnly ? LsaLookupNamesFlags.LSA_LOOKUP_ISOLATED_AS_LOCAL : 0, (uint)names.Length, names, out domains, out sids);
			if (ret != STATUS_SUCCESS && ret != STATUS_SOME_NOT_MAPPED)
				ThrowIfLsaError(ret);
			var d = domains.DangerousGetHandle().ToStructure<LSA_REFERENCED_DOMAIN_LIST>().DomainList.ToArray();
			var ts = sids.DangerousGetHandle().ToIEnum<LSA_TRANSLATED_SID2>(names.Length).ToArray();
			var retVal = new SidLookupResult[names.Length];
			for (var i = 0; i < names.Length; i++)
				retVal[i] = new SidLookupResult(names[i], ref ts[i], d);
			return retVal;
		}

		/// <summary>Gets the system access for the specified user.</summary>
		/// <param name="user">The user name of the account for which to manage privileges.</param>
		/// <returns>A <see cref="LogonRights"/> instance for the specified user.</returns>
		public LogonRights UserLogonRights(string user) => new LogonRights(this, user);

		/// <summary>Gets the account rights for the specified user.</summary>
		/// <param name="user">The user name of the account for which to manage privileges.</param>
		/// <returns>A <see cref="AccountPrivileges"/> instance for the specified user.</returns>
		public AccountPrivileges UserPrivileges(string user) => new AccountPrivileges(this, user);

		private static string FromPriv(SystemPrivilege priv) => SystemPrivilegeTypeConverter.PrivLookup[priv];

		private static void ThrowIfLsaError(uint lsaRetVal)
		{
			Marshal.ThrowExceptionForHR(LsaNtStatusToWinError(lsaRetVal));
		}

		private void AddRights(string accountName, params string[] privilegeNames)
		{
			ThrowIfLsaError(LsaAddAccountRights(handle, GetSid(accountName), privilegeNames, privilegeNames.Length));
		}

		private SafeLsaPolicyHandle GetAccount(string accountName, LsaAccountAccessMask mask = LsaAccountAccessMask.ACCOUNT_VIEW)
		{
			var sid = GetSid(accountName);
			SafeLsaPolicyHandle hAcct;
			var res = LsaNtStatusToWinError(LsaOpenAccount(handle, sid, mask, out hAcct));
			if (res == Win32Error.ERROR_FILE_NOT_FOUND)
				ThrowIfLsaError(LsaCreateAccount(handle, sid, mask, out hAcct));
			else if (res != 0)
				Marshal.ThrowExceptionForHR(res);
			return hAcct;
		}

		private IEnumerable<string> GetRights(string accountName)
		{
			try
			{
				return LsaEnumerateAccountRights(handle, GetSid(accountName));
			}
			catch (FileNotFoundException)
			{
				using (GetAccount(accountName))
					return LsaEnumerateAccountRights(handle, GetSid(accountName));
			}
		}

		private PSID GetSid(string accountName)
		{
			int sidSize = 0, nameSize = 0;
			SID_NAME_USE accountType = 0;
			LookupAccountName(svr, accountName, new PSID(), ref sidSize, null, ref nameSize, ref accountType);
			var domainName = new System.Text.StringBuilder(nameSize);
			var sid = new PSID(sidSize);
			if (!LookupAccountName(string.Empty, accountName, sid, ref sidSize, domainName, ref nameSize, ref accountType))
				throw new System.ComponentModel.Win32Exception();
			return sid;
		}

		private AccountLogonRights GetSystemAccess(SafeLsaPolicyHandle hAcct)
		{
			int rights;
			ThrowIfLsaError(LsaGetSystemAccessAccount(hAcct, out rights));
			return (AccountLogonRights)rights;
		}

		private void RemoveAllRights(string accountName)
		{
			ThrowIfLsaError(LsaRemoveAccountRights(handle, GetSid(accountName), true, null, 0));
		}

		private void RemoveRights(string accountName, params string[] privilegeNames)
		{
			ThrowIfLsaError(LsaRemoveAccountRights(handle, GetSid(accountName), false, privilegeNames, privilegeNames.Length));
		}

		private void SetSystemAccess(SafeLsaPolicyHandle hAcct, AccountLogonRights rights)
		{
			var cur = GetSystemAccess(hAcct);
			ThrowIfLsaError(LsaSetSystemAccessAccount(hAcct, (int)(cur | rights)));
		}

		/// <summary>Allows for the privileges of a user to be retrieved, enumerated and set.</summary>
		public class AccountPrivileges : IEnumerable<SystemPrivilege>
		{
			private readonly LocalSecurityAuthority ctrl;
			private readonly string user;

			/// <summary>Initializes a new instance of the <see cref="AccountPrivileges"/> class.</summary>
			/// <param name="parent">The parent.</param>
			/// <param name="userName">Name of the user.</param>
			public AccountPrivileges(LocalSecurityAuthority parent, string userName = null)
			{
				ctrl = parent; user = userName ?? CurrentUserName;
			}

			private static string CurrentUserName { get; } = WindowsIdentity.GetCurrent().Name;

			/// <summary>Gets or sets the enablement of the specified privilege.</summary>
			/// <value><c>true</c> if the specified privilege is enabled; otherwise <c>false</c>.</value>
			/// <param name="privilege">The name of the privilege.</param>
			/// <returns>A value that represents the enablement of the specified privilege.</returns>
			public bool this[string privilege]
			{
				get { return ctrl.GetRights(user).Contains(privilege); }
				set { if (value) ctrl.AddRights(user, privilege); else ctrl.RemoveRights(user, privilege); }
			}

			/// <summary>Gets or sets the enablement of the specified privilege.</summary>
			/// <value><c>true</c> if the specified privilege is enabled; otherwise <c>false</c>.</value>
			/// <param name="privilege">The name of the privilege.</param>
			/// <returns>A value that represents the enablement of the specified privilege.</returns>
			public bool this[SystemPrivilege privilege]
			{
				get { return ctrl.GetRights(user).Contains(FromPriv(privilege)); }
				set { if (value) ctrl.AddRights(user, FromPriv(privilege)); else ctrl.RemoveRights(user, FromPriv(privilege)); }
			}

			/// <summary>Returns an enumerator that iterates all of the user's current privileges.</summary>
			/// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
			public IEnumerator<SystemPrivilege> GetEnumerator() => ctrl.GetRights(user).Select(SystemPrivilegeTypeConverter.ConvertKnownString).GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			/// <summary>Removes all privileges.</summary>
			public void RemoveAll()
			{
				ctrl.RemoveAllRights(user);
			}
		}

		/// <summary>Allows for the privileges of a user to be retrieved, enumerated and set.</summary>
		public class LogonRights : IEnumerable<AccountLogonRights>
		{
			private readonly LocalSecurityAuthority ctrl;
			private readonly string user;

			/// <summary>Initializes a new instance of the <see cref="AccountPrivileges"/> class.</summary>
			/// <param name="parent">The parent.</param>
			/// <param name="userName">Name of the user.</param>
			public LogonRights(LocalSecurityAuthority parent, string userName = null)
			{
				ctrl = parent; user = userName ?? CurrentUserName;
			}

			private static string CurrentUserName { get; } = WindowsIdentity.GetCurrent().Name;

			/// <summary>Gets or sets the enablement of the specified privilege.</summary>
			/// <value><c>true</c> if the specified privilege is enabled; otherwise <c>false</c>.</value>
			/// <param name="right">The name of the privilege.</param>
			/// <returns>A value that represents the enablement of the specified privilege.</returns>
			public bool this[AccountLogonRights right]
			{
				get { return (ctrl.GetSystemAccess(ctrl.GetAccount(user)) & right) == right; }
				set
				{
					var hAcct = ctrl.GetAccount(user, LsaAccountAccessMask.ACCOUNT_VIEW | LsaAccountAccessMask.ACCOUNT_ADJUST_SYSTEM_ACCESS);
					var cur = ctrl.GetSystemAccess(hAcct);
					var hasFlag = cur.HasFlag(right);
					if ((hasFlag && value) || (!hasFlag && !value))
						return;
					if (value)
						cur |= right;
					else
						cur &= ~right;
					ctrl.SetSystemAccess(hAcct, cur);
				}
			}

			/// <summary>Returns an enumerator that iterates through the assigned rights.</summary>
			/// <returns>A <see cref="IEnumerator{T}"/> that can be used to iterate through the assigned rights.</returns>
			public IEnumerator<AccountLogonRights> GetEnumerator()
			{
				EnumFlagIndexer<AccountLogonRights> r = ctrl.GetSystemAccess(ctrl.GetAccount(user));
				foreach (AccountLogonRights t in Enum.GetValues(typeof(AccountLogonRights)))
					if (r[t]) yield return t;
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		/// <summary>Contains a corresponding result for each name provided to the <see cref="LocalSecurityAuthority.LookupNames"/> method.</summary>
		public class SidLookupResult
		{
			internal SidLookupResult(string name, ref LSA_TRANSLATED_SID2 tsid, LSA_TRUST_INFORMATION[] domains)
			{
				Name = name;
				SidType = tsid.Use;
				Sid = Array.IndexOf(new[] { 1, 2, 4, 5, 9 }, (int)SidType) != -1 ? new SecurityIdentifier(tsid.Sid) : null;
				Domain = tsid.DomainIndex >= 0 && tsid.DomainIndex < domains.Length ? domains[tsid.DomainIndex].Name.ToString() : null;
			}

			/// <summary>Gets the domain associated with the supplied <see cref="Name"/>.</summary>
			public string Domain { get; }

			/// <summary>Gets the corresponding lookup name.</summary>
			public string Name { get; }

			/// <summary>Gets the <see cref="SecurityIdentifier"/> associated with the supplied <see cref="Name"/>.</summary>
			public SecurityIdentifier Sid { get; }

			/// <summary>Gets the <see cref="SID_NAME_USE"/> associated with the supplied <see cref="Name"/>.</summary>
			public SID_NAME_USE SidType { get; }

			/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
			/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
			public override string ToString() => $"Sid: {Sid?.Value} ({SidType}); Domain: {Domain}";
		}
	}
}