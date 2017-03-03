using System;
using System.Drawing;
using System.Runtime.InteropServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int TCM_FIRST = 0x1300;
		public const int TCN_FIRST = -550;

		[Flags]
		public enum TabControlHitTestFlags
		{
			TCHT_NOWHERE = 0x0001,
			TCHT_ONITEMICON = 0x0002,
			TCHT_ONITEMLABEL = 0x0004,
			TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL,
		}

		[Flags]
		public enum TabControlItemMask
		{
			TCIF_TEXT       = 0x0001,
			TCIF_IMAGE      = 0x0002,
			TCIF_RTLREADING = 0x0004,
			TCIF_PARAM      = 0x0008,
			TCIF_STATE      = 0x0010,
		}

		[Flags]
		public enum TabControlItemStates
		{
			TCIS_BUTTONPRESSED = 0x0001,
			TCIS_HIGHLIGHTED   = 0x0002
		}

		public enum TabControlMessage
		{
			TCM_GETIMAGELIST = TCM_FIRST + 2,
			TCM_SETIMAGELIST = TCM_FIRST + 3,
			TCM_GETITEMCOUNT = TCM_FIRST + 4,
			TCM_GETITEM = TCM_FIRST + 60,
			TCM_SETITEM = TCM_FIRST + 61,
			TCM_INSERTITEM = TCM_FIRST + 62,
			TCM_DELETEITEM = TCM_FIRST + 8,
			TCM_DELETEALLITEMS = TCM_FIRST + 9,
			TCM_GETITEMRECT = TCM_FIRST + 10,
			TCM_GETCURSEL = TCM_FIRST + 11,
			TCM_SETCURSEL = TCM_FIRST + 12,
			TCM_HITTEST = TCM_FIRST + 13,
			TCM_SETITEMEXTRA = TCM_FIRST + 14,
			TCM_ADJUSTRECT = TCM_FIRST + 40,
			TCM_SETITEMSIZE = TCM_FIRST + 41,
			TCM_REMOVEIMAGE = TCM_FIRST + 42,
			TCM_SETPADDING = TCM_FIRST + 43,
			TCM_GETROWCOUNT = TCM_FIRST + 44,
			TCM_GETTOOLTIPS = TCM_FIRST + 45,
			TCM_SETTOOLTIPS = TCM_FIRST + 46,
			TCM_GETCURFOCUS = TCM_FIRST + 47,
			TCM_SETCURFOCUS = TCM_FIRST + 48,
			TCM_SETMINTABWIDTH = TCM_FIRST + 49,
			TCM_DESELECTALL = TCM_FIRST + 50,
			TCM_HIGHLIGHTITEM = TCM_FIRST + 51,
			TCM_SETEXTENDEDSTYLE = TCM_FIRST + 52,  // optional wParam == mask
			TCM_GETEXTENDEDSTYLE = TCM_FIRST + 53,
			TCM_SETUNICODEFORMAT = CommonControlMessage.CCM_SETUNICODEFORMAT,
			TCM_GETUNICODEFORMAT = CommonControlMessage.CCM_GETUNICODEFORMAT
		}

		public enum TabControlNotification
		{
			TCN_KEYDOWN = TCN_FIRST - 0,
			TCN_SELCHANGE = TCN_FIRST - 1,
			TCN_SELCHANGING = TCN_FIRST - 2,
			TCN_GETOBJECT = TCN_FIRST - 3,
			TCN_FOCUSCHANGE = TCN_FIRST - 4,
		}

		[Flags]
		public enum TabControlStyles
		{
			TCS_SCROLLOPPOSITE = 0x0001,   // assumes multiline tab
			TCS_BOTTOM = 0x0002,
			TCS_RIGHT = 0x0002,
			TCS_MULTISELECT = 0x0004,  // allow multi-select in button mode
			TCS_FLATBUTTONS = 0x0008,
			TCS_FORCEICONLEFT = 0x0010,
			TCS_FORCELABELLEFT = 0x0020,
			TCS_HOTTRACK = 0x0040,
			TCS_VERTICAL = 0x0080,
			TCS_TABS = 0x0000,
			TCS_BUTTONS = 0x0100,
			TCS_SINGLELINE = 0x0000,
			TCS_MULTILINE = 0x0200,
			TCS_RIGHTJUSTIFY = 0x0000,
			TCS_FIXEDWIDTH = 0x0400,
			TCS_RAGGEDRIGHT = 0x0800,
			TCS_FOCUSONBUTTONDOWN = 0x1000,
			TCS_OWNERDRAWFIXED = 0x2000,
			TCS_TOOLTIPS = 0x4000,
			TCS_FOCUSNEVER = 0x8000,
		}

		[Flags]
		public enum TabControlStylesEx
		{
			TCS_EX_FLATSEPARATORS = 0x00000001,
			TCS_EX_REGISTERDROP = 0x00000002
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TCHITTESTINFO
		{
			public Point pt;
			public TabControlHitTestFlags flags;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public sealed class TCITEM : IDisposable
		{
			public TabControlItemMask mask;
			public TabControlItemStates dwState;
			public TabControlItemStates dwStateMask;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;

			public TCITEM() { }

			public TCITEM(string text) { Text = text; }

			public string Text
			{
				get { return Marshal.PtrToStringUni(pszText); }
				set
				{
					((IDisposable)this).Dispose();
					pszText = Marshal.StringToCoTaskMemUni(value);
					cchTextMax = value.Length;
					mask |= TabControlItemMask.TCIF_TEXT;
				}
			}

			void IDisposable.Dispose()
			{
				if (pszText != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(pszText);
					cchTextMax = 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public sealed class TCITEMHEADER : IDisposable
		{
			public TabControlItemMask mask;
			public TabControlItemStates dwState;
			public TabControlItemStates dwStateMask;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;

			public TCITEMHEADER() { }

			public TCITEMHEADER(string text) { Text = text; }

			public string Text
			{
				get { return Marshal.PtrToStringUni(pszText); }
				set
				{
					((IDisposable)this).Dispose();
					pszText = Marshal.StringToCoTaskMemUni(value);
					cchTextMax = value.Length;
					mask |= TabControlItemMask.TCIF_TEXT;
				}
			}

			void IDisposable.Dispose()
			{
				if (pszText != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(pszText);
					cchTextMax = 0;
				}
			}
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
		/// <param name="item">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TabControlMessage Msg, int wParam, ref ComCtl32.TCHITTESTINFO item);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="item">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TabControlMessage Msg, int wParam, ComCtl32.TCITEM item);
	}
}