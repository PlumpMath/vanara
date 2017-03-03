using System;
using System.Drawing;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int TTN_FIRST = -520;

		public enum ToolTipIcon
		{
			TTI_NONE = 0,
			TTI_INFO = 1,
			TTI_WARNING = 2,
			TTI_ERROR = 3,
			TTI_INFO_LARGE = 4,
			TTI_WARNING_LARGE = 5,
			TTI_ERROR_LARGE = 6,
		}

		[Flags]
		public enum ToolTipInfoFlags
		{
			TTF_IDISHWND = 0x0001,
			TTF_CENTERTIP = 0x0002,
			TTF_RTLREADING = 0x0004,
			TTF_SUBCLASS = 0x0010,
			TTF_TRACK = 0x0020,
			TTF_ABSOLUTE = 0x0080,
			TTF_TRANSPARENT = 0x0100,
			TTF_PARSELINKS = 0x1000,
			TTF_DI_SETITEM = 0x8000, // valid only on the TTN_NEEDTEXT callback
		}

		public enum ToolTipMessage
		{
			TTM_ACTIVATE = User32.WindowMessage.WM_USER + 1,
			TTM_SETDELAYTIME = User32.WindowMessage.WM_USER + 3,
			TTM_ADDTOOL = User32.WindowMessage.WM_USER + 50,
			TTM_DELTOOL = User32.WindowMessage.WM_USER + 51,
			TTM_NEWTOOLRECT = User32.WindowMessage.WM_USER + 52,
			TTM_RELAYEVENT = User32.WindowMessage.WM_USER + 7, // Win7: wParam = GetMessageExtraInfo() when relaying WM_MOUSEMOVE
			TTM_GETTOOLINFO = User32.WindowMessage.WM_USER + 53,
			TTM_SETTOOLINFO = User32.WindowMessage.WM_USER + 54,
			TTM_HITTEST = User32.WindowMessage.WM_USER + 55,
			TTM_GETTEXT = User32.WindowMessage.WM_USER + 56,
			TTM_UPDATETIPTEXT = User32.WindowMessage.WM_USER + 57,
			TTM_GETTOOLCOUNT = User32.WindowMessage.WM_USER + 13,
			TTM_ENUMTOOLS = User32.WindowMessage.WM_USER + 58,
			TTM_GETCURRENTTOOL = User32.WindowMessage.WM_USER + 59,
			TTM_WINDOWFROMPOINT = User32.WindowMessage.WM_USER + 16,
			TTM_TRACKACTIVATE = User32.WindowMessage.WM_USER + 17, // wParam = TRUE/FALSE start end  lparam = LPTOOLINFO
			TTM_TRACKPOSITION = User32.WindowMessage.WM_USER + 18, // lParam = dwPos
			TTM_SETTIPBKCOLOR = User32.WindowMessage.WM_USER + 19,
			TTM_SETTIPTEXTCOLOR = User32.WindowMessage.WM_USER + 20,
			TTM_GETDELAYTIME = User32.WindowMessage.WM_USER + 21,
			TTM_GETTIPBKCOLOR = User32.WindowMessage.WM_USER + 22,
			TTM_GETTIPTEXTCOLOR = User32.WindowMessage.WM_USER + 23,
			TTM_SETMAXTIPWIDTH = User32.WindowMessage.WM_USER + 24,
			TTM_GETMAXTIPWIDTH = User32.WindowMessage.WM_USER + 25,
			TTM_SETMARGIN = User32.WindowMessage.WM_USER + 26, // lParam = lprc
			TTM_GETMARGIN = User32.WindowMessage.WM_USER + 27, // lParam = lprc
			TTM_POP = User32.WindowMessage.WM_USER + 28,
			TTM_UPDATE = User32.WindowMessage.WM_USER + 29,
			TTM_GETBUBBLESIZE = User32.WindowMessage.WM_USER + 30,
			TTM_ADJUSTRECT = User32.WindowMessage.WM_USER + 31,
			TTM_SETTITLE = User32.WindowMessage.WM_USER + 33, // wParam = TTI_*, lParam = wchar* szTitle
			TTM_POPUP = User32.WindowMessage.WM_USER + 34,
			TTM_GETTITLE = User32.WindowMessage.WM_USER + 35, // wParam = 0, lParam = TTGETTITLE*
			TTM_SETWINDOWTHEME = CommonControlMessage.CCM_SETWINDOWTHEME
		}

		public enum ToolTipNotification
		{
			TTN_GETDISPINFO = TTN_FIRST - 10,
			TTN_SHOW = TTN_FIRST - 1,
			TTN_POP = TTN_FIRST - 2,
			TTN_LINKCLICK = TTN_FIRST - 3,
			TTN_NEEDTEXT = TTN_GETDISPINFO,
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTTDISPINFO
		{
			public User32.NMHDR hdr;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszText;
			[MarshalAs(UnmanagedType.LPWStr, SizeConst = 80)]
			public string szText;
			public IntPtr hinst;
			public uint uFlags;
			public IntPtr lParam;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TT_HITTESTINFO
		{
			public IntPtr hwnd;
			public Point pt;
			public TTTOOLINFO ti;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TTGETTITLE
		{
			public uint dwSize;
			public ToolTipIcon uTitleBitmap;
			public uint cch;
			[MarshalAs(UnmanagedType.LPWStr)] public string pszTitle;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TTTOOLINFO
		{
			public uint cbSize;
			public ToolTipInfoFlags uFlags;
			public IntPtr hwnd;
			public IntPtr uId;
			public RECT rect;
			public IntPtr hinst;
			[MarshalAs(UnmanagedType.LPWStr)] public string lpszText;
			public IntPtr lParam;
		    public IntPtr lpReserved;
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
		/// <param name="title">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.EditMessage Msg, int wParam, ref ComCtl32.TTGETTITLE title);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="toolInfo">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.EditMessage Msg, int wParam, ref ComCtl32.TTTOOLINFO toolInfo);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="hitTestInfo">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.EditMessage Msg, int wParam, ref ComCtl32.TT_HITTESTINFO hitTestInfo);
	}
}