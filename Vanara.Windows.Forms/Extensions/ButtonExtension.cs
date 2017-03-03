using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vanara.PInvoke;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		public static void SetElevationRequiredState(this ButtonBase btn, bool required = true)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				if (!btn.IsHandleCreated) return;
				if (required) btn.FlatStyle = FlatStyle.System;
				User32.SendMessage(new HandleRef(btn, btn.Handle), (int)ComCtl32.ButtonMessage.BCM_SETSHIELD, IntPtr.Zero, required ? new IntPtr(1) : IntPtr.Zero);
				btn.Invalidate();
			}
			else
				throw new PlatformNotSupportedException();
		}
	}
}