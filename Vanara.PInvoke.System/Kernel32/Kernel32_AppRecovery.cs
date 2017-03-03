using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Kernel32
	{
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate uint ApplicationRecoveryCallback(IntPtr pvParameter);

		[Flags]
		public enum ApplicationRestartFlags
		{
			/// <summary>Do not restart the process if it terminates due to an unhandled exception.</summary>
			RESTART_NO_CRASH = 1,

			/// <summary>Do not restart the process if it terminates due to the application not responding.</summary>
			RESTART_NO_HANG = 2,

			/// <summary>Do not restart the process if it terminates due to the installation of an update.</summary>
			RESTART_NO_PATCH = 4,

			/// <summary>Do not restart the process if the computer is restarted as the result of an update.</summary>
			RESTART_NO_REBOOT = 8,
		}

		[DllImport(nameof(Kernel32))]
		public static extern void ApplicationRecoveryFinished([MarshalAs(UnmanagedType.Bool)] bool bSuccess);

		[DllImport(nameof(Kernel32))]
		public static extern int ApplicationRecoveryInProgress([Out, MarshalAs(UnmanagedType.Bool)] out bool pbCanceled);

		[DllImport(nameof(Kernel32))]
		public static extern int GetApplicationRecoveryCallback(IntPtr hProcess, out ApplicationRecoveryCallback pRecoveryCallback, out IntPtr ppvParameter, out uint pdwPingInterval, out int pdwFlags);

		[DllImport(nameof(Kernel32), CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int GetApplicationRestartSettings(IntPtr hProcess, StringBuilder pwzCommandline, ref uint pcchSize, out ApplicationRestartFlags pdwFlags);

		[DllImport(nameof(Kernel32), CharSet = CharSet.Unicode)]
		public static extern int RegisterApplicationRecoveryCallback(ApplicationRecoveryCallback pRecoveryCallback, IntPtr pvParameter, uint dwPingInterval, uint dwFlags);

		[DllImport(nameof(Kernel32))]
		public static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.BStr)] string commandLineArgs, ApplicationRestartFlags flags);

		[DllImport(nameof(Kernel32))]
		public static extern int UnregisterApplicationRecoveryCallback();

		[DllImport(nameof(Kernel32))]
		public static extern int UnregisterApplicationRestart();
	}
}