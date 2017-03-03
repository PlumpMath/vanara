using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Vanara.Security;
using Vanara.Security.AccessControl;
using Vanara.Windows.Forms.Annotations;
using static Vanara.PInvoke.AdvApi32;

namespace Vanara.Extensions
{
	public enum ProcessIntegrityLevel
	{
		Untrusted,
		Undefined,
		Low,
		Medium,
		High,
		System
	}

	public static partial class ExtensionMethods
	{
		public static void DisablePrivilege(this Process process, SystemPrivilege privilege)
		{
			using (var hObj = SafeTokenHandle.FromProcess(process.Handle, AccessTypes.TOKEN_ADJUST_PRIVILEGES | AccessTypes.TOKEN_QUERY))
			{
				hObj.AdjustPrivilege(privilege, PrivilegeAttributes.SE_PRIVILEGE_DISABLED);
			}
		}

		public static void EnablePrivilege(this Process process, SystemPrivilege privilege)
		{
			using (var hObj = SafeTokenHandle.FromProcess(process.Handle, AccessTypes.TOKEN_ADJUST_PRIVILEGES | AccessTypes.TOKEN_QUERY))
			{
				hObj.AdjustPrivilege(privilege, PrivilegeAttributes.SE_PRIVILEGE_ENABLED);
			}
		}

		/// <summary>Gets the child processes.</summary>
		/// <param name="p">The process.</param>
		/// <param name="includeDescendants">if set to <c>true</c> include descendants of child processes as well.</param>
		/// <returns>A <see cref="IEnumerable{Process}"/> reference for enumerating child processes.</returns>
		public static IEnumerable<Process> GetChildProcesses(this Process p, bool includeDescendants = false)
		{
			if (p == null) throw new ArgumentNullException(nameof(p));

			var l = new List<Tuple<int, int>>();
			var scope = p.MachineName == "." ? new ManagementScope() : new ManagementScope($"\\\\{p.MachineName}\\root\\cimv2");
			using (var mos = new ManagementObjectSearcher(scope, new ObjectQuery("select ProcessId, ParentProcessId from win32_process")))
			{
				foreach (var obj in mos.Get().OfType<ManagementObject>())
					l.Add(new Tuple<int, int>(Convert.ToInt32(obj["ProcessId"]), Convert.ToInt32(obj["ParentProcessId"])));
			}
			return GetChildProcesses(p.Id, l.GroupBy(i => i.Item2).ToDictionary(g => g.Key, g => g.ToList()), p.MachineName, includeDescendants);
		}

		/// <summary>
		/// The function gets the integrity level of the current process. Integrity level is only available on Windows Vista and newer operating systems, thus
		/// GetProcessIntegrityLevel throws an exception if it is called on systems prior to Windows Vista.
		/// </summary>
		/// <returns>Returns the integrity level of the current process.</returns>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// When any native Windows API call fails, the function throws a Win32Exception with the last error code.
		/// </exception>
		/// <exception cref="System.ArgumentNullException"><paramref name="p"/> must be a valid <see cref="Process"/>.</exception>
		public static ProcessIntegrityLevel GetIntegrityLevel(this Process p)
		{
			if (p == null)
				throw new ArgumentNullException(nameof(p));

			// Open the access token of the current process with TOKEN_QUERY. 
			var hObject = SafeTokenHandle.FromProcess(p.Handle, AccessTypes.TOKEN_QUERY | AccessTypes.TOKEN_DUPLICATE);

			// Marshal the TOKEN_MANDATORY_LABEL struct from native to .NET object. 
			var tokenIL = hObject.GetConvertedInfo<TOKEN_MANDATORY_LABEL>(TOKEN_INFORMATION_CLASS.TokenIntegrityLevel);

			// Integrity Level SIDs are in the form of S-1-16-0xXXXX. (e.g. S-1-16-0x1000 stands for low integrity level SID). There is one and only one subauthority.
			var pIL = GetSidSubAuthority(tokenIL.Label.Sid, 0);
			var il = Marshal.ReadInt32(pIL);
			if (il == 0)
				return ProcessIntegrityLevel.Untrusted;
			if (il == 0x1000)
				return ProcessIntegrityLevel.Low;
			if (il >= 0x2000 && il < 0x3000)
				return ProcessIntegrityLevel.Medium;
			if (il >= 0x4000)
				return ProcessIntegrityLevel.System;
			if (il >= 0x3000)
				return ProcessIntegrityLevel.High;
			return ProcessIntegrityLevel.Undefined;
		}

		/// <summary>
		/// Gets the parent process.
		/// </summary>
		/// <returns>A <see cref="Process"/> object for the process that called the specified process. <c>null</c> if no parent can be established.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="p"/> must be a valid <see cref="Process"/>.</exception>
		public static Process GetParentProcess(this Process p)
		{
			if (p == null)
				throw new ArgumentNullException(nameof(p));
			try
			{
				using (var mo = new ManagementObject($"win32_process.handle='{p.Id}'"))
				{
					mo.Get();
					return Process.GetProcessById(Convert.ToInt32(mo["ParentProcessId"]), p.MachineName);
				}
			}
			catch { }
			return null;
		}

		/// <summary>Gets the privileges for this process.</summary>
		/// <param name="process">The process.</param>
		/// <returns>
		/// An enumeration of <see cref="PrivilegeAndAttributes"/> instances that include the process privileges and their associated attributes (enabled,
		/// disabled, removed, etc.).
		/// </returns>
		public static IEnumerable<PrivilegeAndAttributes> GetPrivileges(this Process process)
		{
			using (var hObj = SafeTokenHandle.FromProcess(process.Handle, AccessTypes.TOKEN_QUERY | AccessTypes.TOKEN_DUPLICATE))
			{
				return hObj.GetPrivileges().Select(la => new PrivilegeAndAttributes(la.Luid.GetPrivilege(process.MachineName), la.Attributes));
			}
		}

		/// <summary>Determines whether the specified privilege is had by the process.</summary>
		/// <param name="process">The process.</param>
		/// <param name="priv">The privilege.</param>
		/// <returns><c>true</c> if the process has the specified privilege; otherwise, <c>false</c>.</returns>
		public static bool HasPrivilege(this Process process, SystemPrivilege priv) => HasPrivileges(process, true, priv);

		/// <summary>Determines whether the specified privileges are had by the process.</summary>
		/// <param name="process">The process.</param>
		/// <param name="requireAll">if set to <c>true</c> require all privileges to be enabled in order to return <c>true</c>.</param>
		/// <param name="privs">The privileges to check.</param>
		/// <returns><c>true</c> if the process has the specified privilege; otherwise, <c>false</c>.</returns>
		public static bool HasPrivileges(this Process process, bool requireAll, params SystemPrivilege[] privs)
		{
			using (var hObj = SafeTokenHandle.FromProcess(process.Handle, AccessTypes.TOKEN_QUERY | AccessTypes.TOKEN_DUPLICATE))
				return hObj.HasPrivileges(requireAll, privs);
		}

		/// <summary>
		/// The function gets the elevation information of the current process. It dictates whether the process is elevated or not. Token elevation is only
		/// available on Windows Vista and newer operating systems, thus IsProcessElevated throws a C++ exception if it is called on systems prior to Windows
		/// Vista. It is not appropriate to use this function to determine whether a process is run as administrator.
		/// </summary>
		/// <returns>Returns true if the process is elevated. Returns false if it is not.</returns>
		/// <exception cref="System.ComponentModel.Win32Exception">
		/// When any native Windows API call fails, the function throws a Win32Exception with the last error code.
		/// </exception>
		/// <exception cref="System.ArgumentNullException"><paramref name="p"/> must be a valid <see cref="Process"/>.</exception>
		/// <remarks>
		/// TOKEN_INFORMATION_CLASS provides TokenElevationType to check the elevation type (TokenElevationTypeDefault / TokenElevationTypeLimited /
		/// TokenElevationTypeFull) of the process. It is different from TokenElevation in that, when UAC is turned off, elevation type always returns
		/// TokenElevationTypeDefault even though the process is elevated (Integrity Level == High). In other words, it is not safe to say if the process is
		/// elevated based on elevation type. Instead, we should use TokenElevation.
		/// </remarks>
		public static bool IsElevated(this Process p)
		{
			if (p == null)
				throw new ArgumentNullException(nameof(p));

			try
			{
				// Open the access token of the current process with TOKEN_QUERY. 
				using (var hObject = SafeTokenHandle.FromProcess(p.Handle, AccessTypes.TOKEN_QUERY | AccessTypes.TOKEN_DUPLICATE))
				{
					// Marshal the TOKEN_ELEVATION struct from native to .NET object. 
					var elevation = hObject.GetConvertedInfo<TOKEN_ELEVATION>(TOKEN_INFORMATION_CLASS.TokenElevation);

					// TOKEN_ELEVATION.TokenIsElevated is a non-zero value if the token has elevated privileges; otherwise, a zero value.
					return elevation.TokenIsElevated;
				}
			}
			catch { }
			return false;
		}

		/// <summary>
		/// The function checks whether the primary access token of the process belongs to user account that is a member of the local Administrators group,
		/// even if it currently is not elevated.
		/// </summary>
		/// <param name="proc">The process to check.</param>
		/// <returns>
		/// Returns true if the primary access token of the process belongs to user account that is a member of the local Administrators group. Returns false
		/// if the token does not.
		/// </returns>
		public static bool IsRunningAsAdmin(this Process proc)
		{
			SafeTokenHandle hObjectToCheck = null;

			// Open the access token of the current process for query and duplicate.
			using (var hObject = SafeTokenHandle.FromProcess(proc.Handle, AccessTypes.TOKEN_QUERY | AccessTypes.TOKEN_DUPLICATE))
			{
				// Determine whether system is running Windows Vista or later operating systems (major version >= 6) because they support linked tokens, but
				// previous versions (major version < 6) do not.
				if (Environment.OSVersion.Version.Major >= 6)
				{
					// Marshal the TOKEN_ELEVATION_TYPE enum from native to .NET.
					var elevType = hObject.GetConvertedInfo<TOKEN_ELEVATION_TYPE>(TOKEN_INFORMATION_CLASS.TokenElevationType);

					// If limited, get the linked elevated token for further check.
					if (elevType == TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited)
					{
						// Marshal the linked token value from native to .NET.
						hObjectToCheck = new SafeTokenHandle(hObject.GetConvertedInfo<IntPtr>(TOKEN_INFORMATION_CLASS.TokenLinkedToken));
					}
				}

				// CheckTokenMembership requires an impersonation token. If we just got a linked token, it already is an impersonation token. If we did not get a
				// linked token, duplicate the original into an impersonation token for CheckTokenMembership.
				if (hObjectToCheck == null)
				{
					if (!DuplicateToken(hObject, SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, out hObjectToCheck))
						throw new Win32Exception();
				}
			}

			if (hObjectToCheck == null || hObjectToCheck.IsInvalid) return false;

			// Check if the token to be checked contains admin SID.
			var id = new WindowsIdentity(hObjectToCheck.DangerousGetHandle());
			return id.IsAdmin();
		}

		public static void RemovePrivilege(this Process process, SystemPrivilege privilege)
		{
			using (var hObj = SafeTokenHandle.FromProcess(process.Handle, AccessTypes.TOKEN_ADJUST_PRIVILEGES | AccessTypes.TOKEN_QUERY))
			{
				hObj.AdjustPrivilege(privilege, PrivilegeAttributes.SE_PRIVILEGE_REMOVED);
			}
		}

		private static IEnumerable<Process> GetChildProcesses(int pid, [NotNull] Dictionary<int, List<Tuple<int, int>>> allProcs, string machineName, bool allChildren = true)
		{
			if (allProcs == null) throw new ArgumentNullException(nameof(allProcs));
			foreach (var val in allProcs[pid])
			{
				var cpid = val.Item1;
				if (allChildren && allProcs.ContainsKey(cpid))
					foreach (var cval in GetChildProcesses(cpid, allProcs, machineName))
						yield return cval;
				Process retProc = null;
				try { retProc = Process.GetProcessById(cpid, machineName); } catch { }
				if (retProc != null) yield return retProc;
			}
		}

		public class PrivilegeAndAttributes
		{
			internal PrivilegeAndAttributes(SystemPrivilege p, PrivilegeAttributes a) { Privilege = p; Attributes = a; }
			public SystemPrivilege Privilege { get; }
			public PrivilegeAttributes Attributes { get; }
		}
	}
}
