using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
	// TODO: Ready this class for library
	internal static class WinInet
	{
		[DllImport("inetcpl.cpl", SetLastError = true)]
		private static extern int LaunchInternetControlPanel(IntPtr hWnd);

		[DllImport(nameof(WinInet), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetQueryOption(
			IntPtr hInternet,
			int dwOption,
			IntPtr optionsList,
			ref int bufferLength
			);

		[DllImport(nameof(WinInet), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InternetSetOption(
			IntPtr hInternet,
			int dwOption,
			IntPtr lpBuffer,
			int lpdwBufferLength
			);
	}
}