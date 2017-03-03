using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		public static void SetCueBanner(this ComboBox cb, string cueBannerText)
		{
			if (System.Environment.OSVersion.Version.Major >= 6)
			{
				if (!cb.IsHandleCreated) return;
				SendMessage(new HandleRef(cb, cb.Handle), (int)ComCtl32.ComboBoxMessage.CB_SETCUEBANNER, IntPtr.Zero, cueBannerText);
				cb.Invalidate();
			}
			else
				throw new PlatformNotSupportedException();
		}
	}
}