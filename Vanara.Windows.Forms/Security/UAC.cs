using System;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;
using Vanara.Extensions;

namespace Vanara.Security
{
	/// <summary>
	/// Provides information about the state of User Access Control for the system.
	/// </summary>
	public static partial class UAC
	{
		private static bool? enabled;

		/// <summary>
		/// Determines whether the provided process can be elevated. Effectively, this checks that UAC is available and that the process is running under an
		/// account that belongs to the Administrators group.
		/// </summary>
		/// <param name="process">The process. If this value is <c>null</c>, then the current process is used.</param>
		/// <returns><c>true</c> if this process can be elevated; otherwise, <c>false</c>.</returns>
		public static bool CanElevate(Process process = null) => IsEnabled() && (process ?? Process.GetCurrentProcess()).IsRunningAsAdmin();

		/// <summary>Determines whether the specified process is elevated.</summary>
		/// <param name="process">The process. If this value is <c>null</c>, then the current process is used.</param>
		/// <returns><c>true</c> if the specified process is elevated; otherwise, <c>false</c>.</returns>
		public static bool IsElevated(Process process = null) => (process ?? Process.GetCurrentProcess()).IsElevated();

		/// <summary>Determines whether UAC is enabled on this system.</summary>
		/// <returns><c>true</c> if UAC is enabled; otherwise, <c>false</c>.</returns>
		public static bool IsEnabled()
		{
			if (!enabled.HasValue)
			{
				if (Environment.OSVersion.Version.Major < 6)
					enabled = true;
				else
					enabled = Registry.GetValue(
						@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", 0).Equals(1);
			}
			return enabled.Value;
		}

		/// <summary>Runs the current application elevated if it isn't already. <note>This will close the current running instance.</note></summary>
		public static void RunCurrentApplicationElevated()
		{
			if (!WindowsIdentity.GetCurrent().IsAdmin())
			{
				// Launch itself as administrator
				var proc = new ProcessStartInfo(System.Windows.Forms.Application.ExecutablePath)
				{
					UseShellExecute = true,
					WorkingDirectory = Environment.CurrentDirectory,
					Verb = "runas"
				};
				try
				{
					Process.Start(proc);
					System.Windows.Forms.Application.Exit();
				}
				catch { }
			}
		}
	}
}