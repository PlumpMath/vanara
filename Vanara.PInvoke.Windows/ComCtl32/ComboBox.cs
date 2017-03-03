using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int CBEN_FIRST = -800;
		public const int CBM_FIRST = 0x1700;
		public const string WC_COMBOBOXEX = "ComboBoxEx32";

		[Flags]
		public enum ComboBoxExItemMask
		{
			CBEIF_TEXT = 0x00000001,
			CBEIF_IMAGE = 0x00000002,
			CBEIF_SELECTEDIMAGE = 0x00000004,
			CBEIF_OVERLAY = 0x00000008,
			CBEIF_INDENT = 0x00000010,
			CBEIF_LPARAM = 0x00000020,
			CBEIF_DI_SETITEM = 0x10000000,
		}

		public enum ComboBoxExNotification
		{
			CBEN_INSERTITEM = CBEN_FIRST - 1,
			CBEN_DELETEITEM = CBEN_FIRST - 2,
			CBEN_BEGINEDIT = CBEN_FIRST - 4,
			CBEN_ENDEDITA = CBEN_FIRST - 5,
			CBEN_ENDEDITW = CBEN_FIRST - 6,
			CBEN_GETDISPINFO = CBEN_FIRST - 7,
			CBEN_DRAGBEGIN = CBEN_FIRST - 9,
		}

		public enum ComboBoxExStyle
		{
			CBES_EX_NOEDITIMAGE = 0x00000001,
			CBES_EX_NOEDITIMAGEINDENT = 0x00000002,
			CBES_EX_PATHWORDBREAKPROC = 0x00000004,
			CBES_EX_NOSIZELIMIT = 0x00000008,
			CBES_EX_CASESENSITIVE = 0x00000010,
			CBES_EX_TEXTENDELLIPSIS = 0x00000020,
		}

		[Flags]
		public enum ComboBoxInfoState
		{
			None = 0,
			STATE_SYSTEM_INVISIBLE = 0x00008000,
			STATE_SYSTEM_PRESSED = 0x00000008
		}

		public enum ComboBoxMessage
		{
			CB_GETEDITSEL = 0x0140,
			CB_LIMITTEXT = 0x0141,
			CB_SETEDITSEL = 0x0142,
			CB_ADDSTRING = 0x0143,
			CB_DELETESTRING = 0x0144,
			CB_DIR = 0x0145,
			CB_GETCOUNT = 0x0146,
			CB_GETCURSEL = 0x0147,
			CB_GETLBTEXT = 0x0148,
			CB_GETLBTEXTLEN = 0x0149,
			CB_INSERTSTRING = 0x014A,
			CB_RESETCONTENT = 0x014B,
			CB_FINDSTRING = 0x014C,
			CB_SELECTSTRING = 0x014D,
			CB_SETCURSEL = 0x014E,
			CB_SHOWDROPDOWN = 0x014F,
			CB_GETITEMDATA = 0x0150,
			CB_SETITEMDATA = 0x0151,
			CB_GETDROPPEDCONTROLRECT = 0x0152,
			CB_SETITEMHEIGHT = 0x0153,
			CB_GETITEMHEIGHT = 0x0154,
			CB_SETEXTENDEDUI = 0x0155,
			CB_GETEXTENDEDUI = 0x0156,
			CB_GETDROPPEDSTATE = 0x0157,
			CB_FINDSTRINGEXACT = 0x0158,
			CB_SETLOCALE = 0x0159,
			CB_GETLOCALE = 0x015A,
			CB_GETTOPINDEX = 0x015b,
			CB_SETTOPINDEX = 0x015c,
			CB_GETHORIZONTALEXTENT = 0x015d,
			CB_SETHORIZONTALEXTENT = 0x015e,
			CB_GETDROPPEDWIDTH = 0x015f,
			CB_SETDROPPEDWIDTH = 0x0160,
			CB_INITSTORAGE = 0x0161,
			CB_MULTIPLEADDSTRING = 0x0163,
			CB_GETCOMBOBOXINFO = 0x0164,
			CB_SETMINVISIBLE = CBM_FIRST + 1,
			CB_GETMINVISIBLE = CBM_FIRST + 2,
			CB_SETCUEBANNER = CBM_FIRST + 3,
			CB_GETCUEBANNER = CBM_FIRST + 4,
		}

		public enum ComboBoxExMessage
		{
			CBEM_SETIMAGELIST = User32.WindowMessage.WM_USER + 2,
			CBEM_GETIMAGELIST = User32.WindowMessage.WM_USER + 3,
			CBEM_DELETEITEM = ComboBoxMessage.CB_DELETESTRING,
			CBEM_GETCOMBOCONTROL = User32.WindowMessage.WM_USER + 6,
			CBEM_GETEDITCONTROL = User32.WindowMessage.WM_USER + 7,
			CBEM_SETEXSTYLE = User32.WindowMessage.WM_USER + 8, // use  SETEXTENDEDSTYLE instead
			CBEM_SETEXTENDEDSTYLE = User32.WindowMessage.WM_USER + 14, // lparam == new style, wParam (optional) == mask
			CBEM_GETEXSTYLE = User32.WindowMessage.WM_USER + 9, // use GETEXTENDEDSTYLE instead
			CBEM_GETEXTENDEDSTYLE = User32.WindowMessage.WM_USER + 9,
			CBEM_SETUNICODEFORMAT = CommonControlMessage.CCM_SETUNICODEFORMAT,
			CBEM_GETUNICODEFORMAT = CommonControlMessage.CCM_GETUNICODEFORMAT,
			CBEM_HASEDITCHANGED = User32.WindowMessage.WM_USER + 10,
			CBEM_INSERTITEM = User32.WindowMessage.WM_USER + 11,
			CBEM_SETITEM = User32.WindowMessage.WM_USER + 12,
			CBEM_GETITEM = User32.WindowMessage.WM_USER + 13,
			CBEM_SETWINDOWTHEME = CommonControlMessage.CCM_SETWINDOWTHEME,
		}

		/// <summary>Combo Box Notification Codes</summary>
		public enum ComboBoxNotification
		{
			CBN_ERRSPACE = (-1),
			CBN_SELCHANGE = 1,
			CBN_DBLCLK = 2,
			CBN_SETFOCUS = 3,
			CBN_KILLFOCUS = 4,
			CBN_EDITCHANGE = 5,
			CBN_EDITUPDATE = 6,
			CBN_DROPDOWN = 7,
			CBN_CLOSEUP = 8,
			CBN_SELENDOK = 9,
			CBN_SELENDCANCEL = 10,
		}

		public enum ComboBoxStyle
		{
			CBS_SIMPLE = 0x0001,
			CBS_DROPDOWN = 0x0002,
			CBS_DROPDOWNLIST = 0x0003,
			CBS_OWNERDRAWFIXED = 0x0010,
			CBS_OWNERDRAWVARIABLE = 0x0020,
			CBS_AUTOHSCROLL = 0x0040,
			CBS_OEMCONVERT = 0x0080,
			CBS_SORT = 0x0100,
			CBS_HASSTRINGS = 0x0200,
			CBS_NOINTEGRALHEIGHT = 0x0400,
			CBS_DISABLENOSCROLL = 0x0800,
			CBS_UPPERCASE = 0x2000,
			CBS_LOWERCASE = 0x4000,
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public sealed class COMBOBOXEXITEM : IDisposable
		{
			public ComboBoxExItemMask mask;
			[MarshalAs(UnmanagedType.SysInt)] public int iItem;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;
			public int iSelectedImage;
			public int iOverlay;
			public int iIndent;
			public IntPtr lParam;

			public COMBOBOXEXITEM() { }

			public COMBOBOXEXITEM(string text) { Text = text; }

			public string Text
			{
				get { return pszText == LPSTR_TEXTCALLBACK ? null : Marshal.PtrToStringUni(pszText); }
				set
				{
					((IDisposable)this).Dispose();
					pszText = Marshal.StringToCoTaskMemUni(value);
					cchTextMax = value.Length;
					mask |= ComboBoxExItemMask.CBEIF_TEXT;
				}
			}

			public bool UseTextCallback
			{
				get { return pszText == LPSTR_TEXTCALLBACK; }
				set
				{
					if (value)
					{
						((IDisposable)this).Dispose();
						pszText = LPSTR_TEXTCALLBACK;
					}
					mask |= ComboBoxExItemMask.CBEIF_TEXT;
				}
			}

			void IDisposable.Dispose()
			{
				if (pszText != IntPtr.Zero && pszText != LPSTR_TEXTCALLBACK)
				{
					Marshal.FreeCoTaskMem(pszText);
					cchTextMax = 0;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct COMBOBOXINFO
		{
			public int cbSize;
			public RECT rcItem;
			public RECT rcButton;
			public ComboBoxInfoState buttonState;
			public IntPtr hwndCombo;
			public IntPtr hwndEdit;
			public IntPtr hwndList;

			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand,
				Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
			public static COMBOBOXINFO FromHandle(HandleRef hComboBox)
			{
				if (hComboBox.Handle == IntPtr.Zero)
					throw new ArgumentException("ComboBox handle cannot be NULL.", nameof(hComboBox));

				var cbi = new COMBOBOXINFO() {cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO))};
				User32.SendMessage(hComboBox, ComboBoxMessage.CB_GETCOMBOBOXINFO, 0, ref cbi);
				return cbi;
			}

			public bool Invisible => (buttonState & ComboBoxInfoState.STATE_SYSTEM_INVISIBLE) == ComboBoxInfoState.STATE_SYSTEM_INVISIBLE;

			public bool Pressed => (buttonState & ComboBoxInfoState.STATE_SYSTEM_PRESSED) == ComboBoxInfoState.STATE_SYSTEM_PRESSED;

			public System.Drawing.Rectangle ItemRectangle => rcItem;

			public System.Drawing.Rectangle ButtonRectangle => rcButton;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMCOMBOBOXEX
		{
			public User32.NMHDR hdr;
			public COMBOBOXEXITEM ceItem;
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
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ComboBoxMessage Msg, int wParam, ref ComCtl32.COMBOBOXINFO item);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="item">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ComboBoxExMessage Msg, int wParam, ComCtl32.COMBOBOXEXITEM item);
	}
}
