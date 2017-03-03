using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Vanara.Extensions;
using Vanara.PInvoke;

namespace Vanara.Security.AccessControl
{
	/// <summary>Privilege determining the type of system operations that can be performed.</summary>
	[TypeConverter(typeof(SystemPrivilegeTypeConverter))]
	public enum SystemPrivilege
	{
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

		/// <summary>Privilege to replace a process-level token.</summary>
		AssignPrimaryToken = 0x00001000,

		/// <summary>Privilege to generate security audits.</summary>
		Audit,

		/// <summary>Privilege to backup files and directories.</summary>
		Backup,

		/// <summary>Privilege to bypass traverse checking.</summary>
		ChangeNotify,

		/// <summary>Privilege to create global objects.</summary>
		CreateGlobal,

		/// <summary>Privilege to create a pagefile.</summary>
		CreatePageFile,

		/// <summary>Privilege to create permanent shared objects.</summary>
		CreatePermanent,

		/// <summary>Privilege to create symbolic links.</summary>
		CreateSymbolicLink,

		/// <summary>Privilege to create a token object.</summary>
		CreateToken,

		/// <summary>Privilege to debug programs.</summary>
		Debug,

		/// <summary>Privilege to enable computer and user accounts to be trusted for delegation.</summary>
		EnableDelegation,

		/// <summary>Privilege to impersonate a client after authentication.</summary>
		Impersonate,

		/// <summary>Privilege to increase scheduling priority.</summary>
		IncreaseBasePriority,

		/// <summary>Privilege to adjust memory quotas for a process.</summary>
		IncreaseQuota,

		/// <summary>Privilege to increase a process working set.</summary>
		IncreaseWorkingSet,

		/// <summary>Privilege to load and unload device drivers.</summary>
		LoadDriver,

		/// <summary>Privilege to lock pages in memory.</summary>
		LockMemory,

		/// <summary>Privilege to add workstations to domain.</summary>
		MachineAccount,

		/// <summary>Privilege to manage the files on a volume.</summary>
		ManageVolume,

		/// <summary>Privilege to profile single process.</summary>
		ProfileSingleProcess,

		/// <summary>Privilege to modify an object label.</summary>
		Relabel,

		/// <summary>Privilege to force shutdown from a remote system.</summary>
		RemoteShutdown,

		/// <summary>Privilege to restore files and directories.</summary>
		Restore,

		/// <summary>Privilege to manage auditing and security log.</summary>
		Security,

		/// <summary>Privilege to shut down the system.</summary>
		Shutdown,

		/// <summary>Privilege to synchronize directory service data.</summary>
		SyncAgent,

		/// <summary>Privilege to modify firmware environment values.</summary>
		SystemEnvironment,

		/// <summary>Privilege to profile system performance.</summary>
		SystemProfile,

		/// <summary>Privilege to change the system time.</summary>
		SystemTime,

		/// <summary>Privilege to take ownership of files or other objects.</summary>
		TakeOwnership,

		/// <summary>Privilege to act as part of the operating system.</summary>
		TrustedComputerBase,

		/// <summary>Privilege to change the time zone.</summary>
		TimeZone,

		/// <summary>Privilege to access Credential Manager as a trusted caller.</summary>
		TrustedCredentialManagerAccess,

		/// <summary>Privilege to remove computer from docking station.</summary>
		Undock,

		/// <summary>Privilege to read unsolicited input from a terminal device.</summary>
		UnsolicitedInput
	}

	public static class PrivilegeExtension
	{
		public static SafeCoTaskMemHandle AdjustPrivilege(this AdvApi32.SafeTokenHandle hObj, SystemPrivilege priv, AdvApi32.PrivilegeAttributes attr)
		{
			var newState = new AdvApi32.PTOKEN_PRIVILEGES(priv.GetLUID(), attr);
			var prevState = AdvApi32.PTOKEN_PRIVILEGES.GetAllocatedAndEmptyInstance();
			var retLen = (uint)prevState.Size;
			if (!AdvApi32.AdjustTokenPrivileges(hObj, false, newState, newState.SizeInBytes, prevState, ref retLen))
				throw new Win32Exception();
			prevState.Resize((int)retLen);
			return prevState;
		}

		public static SafeCoTaskMemHandle AdjustPrivileges(this AdvApi32.SafeTokenHandle hObj, params ExtensionMethods.PrivilegeAndAttributes[] privileges)
		{
			if (privileges == null || privileges.Length == 0) return SafeCoTaskMemHandle.Null;
			var newState = new AdvApi32.PTOKEN_PRIVILEGES(privileges.Select(pa => new AdvApi32.LUID_AND_ATTRIBUTES(pa.Privilege.GetLUID(), pa.Attributes)).ToArray());
			var prevState = AdvApi32.PTOKEN_PRIVILEGES.GetAllocatedAndEmptyInstance();
			var retLen = (uint)prevState.Size;
			if (!AdvApi32.AdjustTokenPrivileges(hObj, false, newState, newState.SizeInBytes, prevState, ref retLen))
				throw new Win32Exception();
			prevState.Resize((int)retLen);
			return prevState;
		}

		public static void AdjustPrivileges(this AdvApi32.SafeTokenHandle hObj, AdvApi32.PTOKEN_PRIVILEGES privileges)
		{
			if (privileges == null) return;
			uint retLen = 0;
			if (!AdvApi32.AdjustTokenPrivileges(hObj, false, privileges, (uint)privileges.SizeInBytes, null, ref retLen))
				throw new Win32Exception();
		}

		public static void AdjustPrivileges(this AdvApi32.SafeTokenHandle hObj, SafeCoTaskMemHandle privileges)
		{
			if (privileges == null || privileges.IsInvalid) return;
			uint retLen = 0;
			if (!AdvApi32.AdjustTokenPrivileges(hObj, false, privileges, (uint)privileges.Size, SafeCoTaskMemHandle.Null, ref retLen))
				throw new Win32Exception();
		}

		public static AdvApi32.LUID GetLUID(this SystemPrivilege systemPrivilege, string systemName = null) => SystemPrivilegeTypeConverter.GetLUID(systemPrivilege, systemName);

		public static SystemPrivilege GetPrivilege(this AdvApi32.LUID luid, string systemName = null) => SystemPrivilegeTypeConverter.GetPrivilege(luid, systemName);

		public static bool HasPrivilege(this AdvApi32.SafeTokenHandle hObj, SystemPrivilege priv) => HasPrivileges(hObj, true, priv);

		public static bool HasPrivileges(this AdvApi32.SafeTokenHandle hObj, bool requireAll, params SystemPrivilege[] privs)
		{
			bool ret;
			if (!AdvApi32.PrivilegeCheck(hObj, new AdvApi32.PRIVILEGE_SET((AdvApi32.PrivilegeSetControl)(requireAll ? 1 : 0), privs.Select(p => new AdvApi32.LUID_AND_ATTRIBUTES(p.GetLUID(), AdvApi32.PrivilegeAttributes.SE_PRIVILEGE_ENABLED)).ToArray()), out ret))
				throw new Win32Exception();
			return ret;
		}

		public static IEnumerable<AdvApi32.LUID_AND_ATTRIBUTES> GetPrivileges(this AdvApi32.SafeTokenHandle hObj)
		{
			var privs = hObj.GetInfo(AdvApi32.TOKEN_INFORMATION_CLASS.TokenPrivileges);
			return ExtractPrivileges(privs);
		}

		private static IEnumerable<AdvApi32.LUID_AND_ATTRIBUTES> ExtractPrivileges(SafeHGlobalHandle hMem)
		{
			lock (hMem)
				return AdvApi32.PTOKEN_PRIVILEGES.FromPtr(hMem.DangerousGetHandle()).Privileges;
		}
	}

	internal class SystemPrivilegeTypeConverter : TypeConverter
	{
		internal static readonly Dictionary<SystemPrivilege, string> PrivLookup =
			new Dictionary<SystemPrivilege, string>(35)
		{
			{ SystemPrivilege.AssignPrimaryToken, "SeAssignPrimaryTokenPrivilege" },
			{ SystemPrivilege.Audit, "SeAuditPrivilege" },
			{ SystemPrivilege.Backup, "SeBackupPrivilege" },
			{ SystemPrivilege.ChangeNotify, "SeChangeNotifyPrivilege" },
			{ SystemPrivilege.CreateGlobal, "SeCreateGlobalPrivilege" },
			{ SystemPrivilege.CreatePageFile, "SeCreatePagefilePrivilege" },
			{ SystemPrivilege.CreatePermanent, "SeCreatePermanentPrivilege" },
			{ SystemPrivilege.CreateSymbolicLink, "SeCreateSymbolicLinkPrivilege" },
			{ SystemPrivilege.CreateToken, "SeCreateTokenPrivilege" },
			{ SystemPrivilege.Debug, "SeDebugPrivilege" },
			{ SystemPrivilege.EnableDelegation, "SeEnableDelegationPrivilege" },
			{ SystemPrivilege.Impersonate, "SeImpersonatePrivilege" },
			{ SystemPrivilege.IncreaseBasePriority, "SeIncreaseBasePriorityPrivilege" },
			{ SystemPrivilege.IncreaseQuota, "SeIncreaseQuotaPrivilege" },
			{ SystemPrivilege.IncreaseWorkingSet, "SeIncreaseWorkingSetPrivilege" },
			{ SystemPrivilege.LoadDriver, "SeLoadDriverPrivilege" },
			{ SystemPrivilege.LockMemory, "SeLockMemoryPrivilege" },
			{ SystemPrivilege.MachineAccount, "SeMachineAccountPrivilege" },
			{ SystemPrivilege.ManageVolume, "SeManageVolumePrivilege" },
			{ SystemPrivilege.ProfileSingleProcess, "SeProfileSingleProcessPrivilege" },
			{ SystemPrivilege.Relabel, "SeRelabelPrivilege" },
			{ SystemPrivilege.RemoteShutdown, "SeRemoteShutdownPrivilege" },
			{ SystemPrivilege.Restore, "SeRestorePrivilege" },
			{ SystemPrivilege.Security, "SeSecurityPrivilege" },
			{ SystemPrivilege.Shutdown, "SeShutdownPrivilege" },
			{ SystemPrivilege.SyncAgent, "SeSyncAgentPrivilege" },
			{ SystemPrivilege.SystemEnvironment, "SeSystemEnvironmentPrivilege" },
			{ SystemPrivilege.SystemProfile, "SeSystemProfilePrivilege" },
			{ SystemPrivilege.SystemTime, "SeSystemTimePrivilege" },
			{ SystemPrivilege.TakeOwnership, "SeTakeOwnershipPrivilege" },
			{ SystemPrivilege.TrustedComputerBase, "SeTcbPrivilege" },
			{ SystemPrivilege.TimeZone, "SeTimeZonePrivilege" },
			{ SystemPrivilege.TrustedCredentialManagerAccess, "SeTrustedCredManAccessPrivilege" },
			{ SystemPrivilege.Undock, "SeUndockPrivilege" },
			{ SystemPrivilege.UnsolicitedInput, "SeUnsolicitedInputPrivilege" },
			{ SystemPrivilege.InteractiveLogon, "SeInteractiveLogonRight" },
			{ SystemPrivilege.NetworkLogon, "SeNetworkLogonRight" },
			{ SystemPrivilege.BatchLogon, "SeBatchLogonRight" },
			{ SystemPrivilege.ServiceLogon, "SeServiceLogonRight" },
			{ SystemPrivilege.DenyInteractiveLogon, "SeDenyInteractiveLogonRight" },
			{ SystemPrivilege.DenyNetworkLogon, "SeDenyNetworkLogonRight" },
			{ SystemPrivilege.DenyBatchLogon, "SeDenyBatchLogonRight" },
			{ SystemPrivilege.DenyServiceLogon, "SeDenyServiceLogonRight" },
			{ SystemPrivilege.RemoteInteractiveLogon, "SeRemoteInteractiveLogonRight" },
			{ SystemPrivilege.DenyRemoteInteractiveLogon, "SeDenyRemoteInteractiveLogonRight" },
		};

		private static readonly Dictionary<SystemPrivilege, AdvApi32.LUID> luidLookup = new Dictionary<SystemPrivilege, AdvApi32.LUID>();

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => 
			sourceType == typeof(string) || sourceType == typeof(AdvApi32.LUID) || base.CanConvertFrom(context, sourceType);

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => 
			destinationType == typeof(string) || destinationType == typeof(AdvApi32.LUID) || base.CanConvertTo(context, destinationType);

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var s = value as string;
			if (s != null)
			{
				SystemPrivilege val;
				if (Enum.TryParse(s, true, out val))
					return val;
				try { return ConvertKnownString(s); } catch { }
			}
			if (value is AdvApi32.LUID)
				return GetPrivilege((AdvApi32.LUID)value);
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
				return PrivLookup[(SystemPrivilege)value];
			if (destinationType == typeof(AdvApi32.LUID))
				return GetLUID((SystemPrivilege)value);
			return base.ConvertTo(context, culture, value, destinationType);
		}

		internal static SystemPrivilege ConvertKnownString(string s)
		{
			try { return PrivLookup.First(v => string.Equals(s, v.Value, StringComparison.OrdinalIgnoreCase)).Key; }
			catch { System.Diagnostics.Debug.WriteLine($"Failed to convert privilege value '{s}'.");}
			return SystemPrivilege.Debug;
		}

		internal static AdvApi32.LUID GetLUID(SystemPrivilege systemPrivilege, string systemName = null)
		{
			AdvApi32.LUID val;
			lock (luidLookup)
			{
				if (!luidLookup.TryGetValue(systemPrivilege, out val))
				{
					val = AdvApi32.LUID.FromName(SystemPrivilegeTypeConverter.PrivLookup[systemPrivilege], systemName);
					luidLookup.Add(systemPrivilege, val);
				}
			}
			return val;
		}

		internal static SystemPrivilege GetPrivilege(AdvApi32.LUID luid, string systemName = null) => ConvertKnownString(luid.GetName(systemName));
	}
}