using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public enum ProgressMessage
		{
			PBM_SETRANGE    = User32.WindowMessage.WM_USER+1,
			PBM_SETPOS      = User32.WindowMessage.WM_USER+2,
			PBM_DELTAPOS    = User32.WindowMessage.WM_USER+3,
			PBM_SETSTEP     = User32.WindowMessage.WM_USER+4,
			PBM_STEPIT      = User32.WindowMessage.WM_USER+5,
			PBM_SETRANGE32  = User32.WindowMessage.WM_USER+6,  // lParam = high, wParam = low
			PBM_GETRANGE    = User32.WindowMessage.WM_USER+7,  // wParam = return (TRUE ? low : high). lParam = PPBRANGE or NULL
			PBM_GETPOS      = User32.WindowMessage.WM_USER+8,
			PBM_SETBARCOLOR = User32.WindowMessage.WM_USER+9,  // lParam = bar color
			PBM_SETBKCOLOR  = CommonControlMessage.CCM_SETBKCOLOR,  // lParam = bkColor
			PBM_SETMARQUEE  = User32.WindowMessage.WM_USER+10,
			PBM_GETSTEP     = User32.WindowMessage.WM_USER+13,
			PBM_GETBKCOLOR  = User32.WindowMessage.WM_USER+14,
			PBM_GETBARCOLOR = User32.WindowMessage.WM_USER+15,
			PBM_SETSTATE    = User32.WindowMessage.WM_USER+16, // wParam = PBST_[State] (NORMAL, ERROR, PAUSED)
			PBM_GETSTATE    = User32.WindowMessage.WM_USER+17,
		}

		public enum ProgressState
		{
			PBST_NORMAL = 0x0001,
			PBST_ERROR  = 0x0002,
			PBST_PAUSED = 0x0003,
		}

		public enum ProgressStyle
		{
			PBS_SMOOTH        = 0x01,
			PBS_VERTICAL      = 0x04,
			PBS_MARQUEE       = 0x08,
			PBS_SMOOTHREVERSE = 0x10,
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PBRANGE
		{
			public int iLow;
			public int iHigh;
		}
	}

	public static partial class User32
	{
		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="progressRange">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ProgressMessage Msg, [MarshalAs(UnmanagedType.Bool)] bool wParam, ref ComCtl32.PBRANGE progressRange);
	}
}