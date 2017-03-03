using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.User32;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		/// <summary>Used in the <see cref="BUTTON_IMAGELIST"/> structure himl member to indicate that no glyph should be displayed.</summary>
		public static IntPtr BCCL_NOGLYPH = new IntPtr(-1);

		private const int BCM_FIRST = 0x1600;
		private const int BCN_FIRST = -1250;

		public enum ButtonImageListAlign
		{
			BUTTON_IMAGELIST_ALIGN_LEFT = 0,
			BUTTON_IMAGELIST_ALIGN_RIGHT = 1,
			BUTTON_IMAGELIST_ALIGN_TOP = 2,
			BUTTON_IMAGELIST_ALIGN_BOTTOM = 3,
			BUTTON_IMAGELIST_ALIGN_CENTER = 4  // Doesn't draw text
		}

		public enum ButtonMessage
		{
			BM_GETCHECK      = 0x00F0,
			BM_SETCHECK      = 0x00F1,
			BM_GETSTATE      = 0x00F2,
			BM_SETSTATE      = 0x00F3,
			BM_SETSTYLE      = 0x00F4,
			BM_CLICK         = 0x00F5,
			BM_GETIMAGE      = 0x00F6,
			BM_SETIMAGE      = 0x00F7,
			BM_SETDONTCLICK  = 0x00F8,
			BCM_GETIDEALSIZE = BCM_FIRST + 0x0001,
			BCM_SETIMAGELIST = BCM_FIRST + 0x0002,
			BCM_GETIMAGELIST = BCM_FIRST + 0x0003,
			BCM_SETTEXTMARGIN = BCM_FIRST + 0x0004,
			BCM_GETTEXTMARGIN = BCM_FIRST + 0x0005,
			BCM_SETDROPDOWNSTATE = BCM_FIRST + 0x0006,
			BCM_SETSPLITINFO = BCM_FIRST + 0x0007,
			BCM_GETSPLITINFO = BCM_FIRST + 0x0008,
			BCM_SETNOTE = BCM_FIRST + 0x0009,
			BCM_GETNOTE = BCM_FIRST + 0x000A,
			BCM_GETNOTELENGTH = BCM_FIRST + 0x000B,
			BCM_SETSHIELD = BCM_FIRST + 0x000C,
		}

		public enum ButtonNotification
		{
			BN_CLICKED        = 0,
			BN_PAINT          = 1,
			BN_HILITE         = 2,
			BN_UNHILITE       = 3,
			BN_DISABLE        = 4,
			BN_DOUBLECLICKED  = 5,
			BN_PUSHED         = BN_HILITE,
			BN_UNPUSHED       = BN_UNHILITE,
			BN_DBLCLK         = BN_DOUBLECLICKED,
			BN_SETFOCUS       = 6,
			BN_KILLFOCUS      = 7,
			BCN_HOTITEMCHANGE = BCN_FIRST + 0x0001,
			BCN_DROPDOWN = BCN_FIRST + 0x0002,
			NM_GETCUSTOMSPLITRECT = BCN_FIRST + 0x0003,
		}

		[Flags]
		public enum ButtonStateFlags
		{
			BST_UNCHECKED      = 0x0000,
			BST_CHECKED        = 0x0001,
			BST_INDETERMINATE  = 0x0002,
			BST_PUSHED         = 0x0004,
			BST_FOCUS          = 0x0008,
			BST_DROPDOWNPUSHED = 0x0400
		}

		public enum ButtonStyle
		{
			BS_SPLITBUTTON = 0x0000000C,
			BS_DEFSPLITBUTTON = 0x0000000D,
			BS_COMMANDLINK = 0x0000000E,
			BS_DEFCOMMANDLINK = 0x0000000F,
		}

		/// <summary>
		/// A set of flags that specify which members of <see cref="BUTTON_SPLITINFO"/> contain data to be set or which members are being requested.
		/// </summary>
		[Flags]
		public enum SplitButtonInfoMask
		{
			/// <summary>himlGlyph is valid.</summary>
			BCSIF_GLYPH = 0x1,
			/// <summary>himlGlyph is valid. Use when uSplitStyle is set to BCSS_IMAGE.</summary>
			BCSIF_IMAGE = 0x2,
			/// <summary>uSplitStyle is valid.</summary>
			BCSIF_STYLE = 0x4,
			/// <summary>size is valid.</summary>
			BCSIF_SIZE = 0x8
		}

		/// <summary>
		/// The split button style for the uSplitStyle member of <see cref="BUTTON_SPLITINFO"/>.
		/// </summary>
		[Flags]
		public enum SplitButtonInfoStyle
		{
			/// <summary>No split.</summary>
			BCSS_NOSPLIT = 0x1,
			/// <summary>Stretch glyph, but try to retain aspect ratio.</summary>
			BCSS_STRETCH = 0x2,
			/// <summary>Align the image or glyph horizontally with the left margin.</summary>
			BCSS_ALIGNLEFT = 0x4,
			/// <summary>Draw an icon image as the glyph.</summary>
			BCSS_IMAGE = 0x8
		}

		/// <summary>Contains information about an image list that is used with a button control.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct BUTTON_IMAGELIST
		{
			/// <summary>
			/// A handle to the image list. The provider retains ownership of the image list and is ultimately responsible for its disposal. Under Windows Vista,
			/// you can pass BCCL_NOGLYPH in this parameter to indicate that no glyph should be displayed.
			/// </summary>
			public IntPtr himl;
			/// <summary>A RECT that specifies the margin around the icon.</summary>
			public RECT margin;
			/// <summary>A UINT that specifies the alignment to use.</summary>
			public ButtonImageListAlign uAlign;
		}

		/// <summary>
		/// Contains information that defines a split button (BS_SPLITBUTTON and BS_DEFSPLITBUTTON styles). Used with the BCM_GETSPLITINFO and BCM_SETSPLITINFO messages.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct BUTTON_SPLITINFO
		{
			/// <summary>A set of flags that specify which members of this structure contain data to be set or which members are being requested.</summary>
			public SplitButtonInfoMask mask;
			/// <summary>A handle to the image list. The provider retains ownership of the image list and is ultimately responsible for its disposal.</summary>
			public IntPtr himlGlyph;
			/// <summary>The split button style.</summary>
			public SplitButtonInfoStyle uSplitButtonInfoStyle;
			/// <summary>A SIZE structure that specifies the size of the glyph in himlGlyph.</summary>
			public Size size;

			/// <summary>
			/// Initializes a new instance of the <see cref="BUTTON_SPLITINFO"/> struct and sets the uSplitStyle value.
			/// </summary>
			/// <param name="buttonInfoStyle">The style.</param>
			public BUTTON_SPLITINFO(SplitButtonInfoStyle buttonInfoStyle) : this() { uSplitButtonInfoStyle = buttonInfoStyle; mask = SplitButtonInfoMask.BCSIF_STYLE; }

			/// <summary>
			/// Initializes a new instance of the <see cref="BUTTON_SPLITINFO"/> struct and sets an ImageList
			/// </summary>
			/// <param name="hImageList">The h image list.</param>
			public BUTTON_SPLITINFO(HandleRef hImageList) : this() { himlGlyph = hImageList.Handle != IntPtr.Zero ? hImageList.Handle : IntPtr.Zero; mask = SplitButtonInfoMask.BCSIF_IMAGE; }
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMBCDROPDOWN
		{
			public User32.NMHDR hdr;
			public RECT rcButton;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMBCHOTITEM
		{
			public User32.NMHDR hdr;
			public int dwFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMCUSTOMSPLITRECTINFO
		{
			public User32.NMHDR hdr;
			public RECT rcClient;
			public RECT rcButton;
			public RECT rcSplit;
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
		/// <param name="splitInfo">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ButtonMessage Msg, int wParam, ref ComCtl32.BUTTON_SPLITINFO splitInfo);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="imageList">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ButtonMessage Msg, int wParam, ref ComCtl32.BUTTON_IMAGELIST imageList);
	}
}