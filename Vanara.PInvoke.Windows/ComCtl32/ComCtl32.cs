using System;
using System.Runtime.InteropServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int CCM_FIRST = 0x2000;
		public const int I_IMAGECALLBACK = -1;
		public const int I_IMAGENONE = -2;
		public const int NM_FIRST = 0;

		/// <summary>
		/// The set of bit flags that indicate which common control classes will be loaded from the DLL when calling
		/// <see cref="InitCommonControlsEx(ref INITCOMMONCONTROLSEX)" />.
		/// </summary>
		[Flags]
		public enum CommonControlClass
		{
			/// <summary>Load animate control class.</summary>
			ICC_ANIMATE_CLASS = 0X00000080,

			/// <summary>Load toolbar, status bar, trackbar, and tooltip control classes.</summary>
			ICC_BAR_CLASSES = 0X00000004,

			/// <summary>Load rebar control class.</summary>
			ICC_COOL_CLASSES = 0X00000400,

			/// <summary>Load date and time picker control class.</summary>
			ICC_DATE_CLASSES = 0X00000100,

			/// <summary>Load hot key control class.</summary>
			ICC_HOTKEY_CLASS = 0X00000040,

			/// <summary>Load IP address class.</summary>
			ICC_INTERNET_CLASSES = 0X00000800,

			/// <summary>Load a hyperlink control class.</summary>
			ICC_LINK_CLASS = 0X00008000,

			/// <summary>Load list-view and header control classes.</summary>
			ICC_LISTVIEW_CLASSES = 0X00000001,

			/// <summary>Load a native font control class.</summary>
			ICC_NATIVEFNTCTL_CLASS = 0x00002000,

			/// <summary>Load pager control class.</summary>
			ICC_PAGESCROLLER_CLASS = 0X00001000,

			/// <summary>Load progress bar control class.</summary>
			ICC_PROGRESS_CLASS = 0X00000020,

			/// <summary>
			/// Load one of the intrinsic User32 control classes. The user controls include button, edit, static, listbox,
			/// combobox, and scroll bar.
			/// </summary>
			ICC_STANDARD_CLASSES = 0X00004000,

			/// <summary>Load tab and tooltip control classes.</summary>
			ICC_TAB_CLASSES = 0X00000008,

			/// <summary>Load tree-view and tooltip control classes.</summary>
			ICC_TREEVIEW_CLASSES = 0X00000002,

			/// <summary>Load up-down control class.</summary>
			ICC_UPDOWN_CLASS = 0X00000010,

			/// <summary>Load ComboBoxEx class.</summary>
			ICC_USEREX_CLASSES = 0X00000200,

			/// <summary>
			/// Load animate control, header, hot key, list-view, progress bar, status bar, tab, tooltip, toolbar, trackbar,
			/// tree-view, and up-down control classes.
			/// </summary>
			ICC_WIN95_CLASSES = 0X000000FF
		}

		public enum CommonControlMessage
		{
			CCM_SETBKCOLOR        = CCM_FIRST + 1, // lParam is bkColor
			CCM_SETCOLORSCHEME    = CCM_FIRST + 2, // lParam is color scheme
			CCM_GETCOLORSCHEME    = CCM_FIRST + 3, // fills in COLORSCHEME pointed to by lParam
			CCM_GETDROPTARGET     = CCM_FIRST + 4,
			CCM_SETUNICODEFORMAT  = CCM_FIRST + 5,
			CCM_GETUNICODEFORMAT  = CCM_FIRST + 6,
			CCM_SETVERSION        = CCM_FIRST + 0x7,
			CCM_GETVERSION        = CCM_FIRST + 0x8,
			CCM_SETNOTIFYWINDOW   = CCM_FIRST + 0x9, // wParam == hwndParent.
			CCM_SETWINDOWTHEME    = CCM_FIRST + 0xb,
			CCM_DPISCALE          = CCM_FIRST + 0xc, // wParam == Awareness
		}

		public enum CommonControlNotification
		{
			NM_OUTOFMEMORY          = NM_FIRST-1,
			NM_CLICK                = NM_FIRST-2,    // uses NMCLICK struct
			NM_DBLCLK               = NM_FIRST-3,
			NM_RETURN               = NM_FIRST-4,
			NM_RCLICK               = NM_FIRST-5,    // uses NMCLICK struct
			NM_RDBLCLK              = NM_FIRST-6,
			NM_SETFOCUS             = NM_FIRST-7,
			NM_KILLFOCUS            = NM_FIRST-8,
			NM_CUSTOMDRAW           = NM_FIRST-12,
			NM_HOVER                = NM_FIRST-13,
			NM_NCHITTEST            = NM_FIRST-14,   // uses NMMOUSE struct
			NM_KEYDOWN              = NM_FIRST-15,   // uses NMKEY struct
			NM_RELEASEDCAPTURE      = NM_FIRST-16,
			NM_SETCURSOR            = NM_FIRST-17,   // uses NMMOUSE struct
			NM_CHAR                 = NM_FIRST-18,   // uses NMCHAR struct
			NM_TOOLTIPSCREATED      = NM_FIRST-19,   // notify of when the tooltips window is create
			NM_LDOWN                = NM_FIRST-20,
			NM_RDOWN                = NM_FIRST-21,
			NM_THEMECHANGED         = NM_FIRST-22,
			NM_FONTCHANGED          = NM_FIRST-23,
			NM_CUSTOMTEXT           = NM_FIRST-24,   // uses NMCUSTOMTEXT struct
			NM_TVSTATEIMAGECHANGING = NM_FIRST-24,   // uses NMTVSTATEIMAGECHANGING struct, defined after HTREEITEM
		}

		[Flags]
		public enum CustomDrawItemState
		{
			CDIS_SELECTED = 0x0001,
			CDIS_GRAYED = 0x0002,
			CDIS_DISABLED = 0x0004,
			CDIS_CHECKED = 0x0008,
			CDIS_FOCUS = 0x0010,
			CDIS_DEFAULT = 0x0020,
			CDIS_HOT = 0x0040,
			CDIS_MARKED = 0x0080,
			CDIS_INDETERMINATE = 0x0100,
			CDIS_SHOWKEYBOARDCUES = 0x0200,
			CDIS_NEARHOT = 0x0400,
			CDIS_OTHERSIDEHOT = 0x0800,
			CDIS_DROPHILITED = 0x1000,
		}

		/// <summary>The current drawing stage.</summary>
		[Flags]
		public enum CustomDrawStage
		{
			/// <summary>Before the painting cycle begins.</summary>
			CDDS_PREPAINT = 0x00000001,
			/// <summary>After the painting cycle is complete.</summary>
			CDDS_POSTPAINT = 0x00000002,
			/// <summary>Before the erasing cycle begins.</summary>
			CDDS_PREERASE = 0x00000003,
			/// <summary>After the erasing cycle is complete.</summary>
			CDDS_POSTERASE = 0x00000004,
			/// <summary>Indicates that the dwItemSpec, uItemState, and lItemlParam members are valid.</summary>
			CDDS_ITEM = 0x00010000,
			/// <summary>Before an item is drawn.</summary>
			CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT),
			/// <summary>After an item has been drawn.</summary>
			CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT),
			/// <summary>Before an item is erased.</summary>
			CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE),
			/// <summary>After an item has been erased.</summary>
			CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE),
			/// <summary>Flag combined with CDDS_ITEMPREPAINT or CDDS_ITEMPOSTPAINT if a subitem is being drawn. This will only be set if CDRF_NOTIFYITEMDRAW is returned from CDDS_PREPAINT.</summary>
			CDDS_SUBITEM = 0x00020000,
		}

		[Flags]
		public enum PropSheetHeaderFlags : uint
		{
			PSH_DEFAULT = 0x00000000,
			PSH_PROPTITLE = 0x00000001,
			PSH_USEHICON = 0x00000002,
			PSH_USEICONID = 0x00000004,
			PSH_PROPSHEETPAGE = 0x00000008,
			PSH_WIZARDHASFINISH = 0x00000010,
			PSH_WIZARD = 0x00000020,
			PSH_USEPSTARTPAGE = 0x00000040,
			PSH_NOAPPLYNOW = 0x00000080,
			PSH_USECALLBACK = 0x00000100,
			PSH_HASHELP = 0x00000200,
			PSH_MODELESS = 0x00000400,
			PSH_RTLREADING = 0x00000800,
			PSH_WIZARDCONTEXTHELP = 0x00001000,
			PSH_WIZARD97 = 0x01000000,
			PSH_WATERMARK = 0x00008000,
			PSH_USEHBMWATERMARK = 0x00010000, // user pass in a hbmWatermark instead of pszbmWatermark
			PSH_USEHPLWATERMARK = 0x00020000, //
			PSH_STRETCHWATERMARK = 0x00040000, // stretchwatermark also applies for the header
			PSH_HEADER = 0x00080000,
			PSH_USEHBMHEADER = 0x00100000,
			PSH_USEPAGELANG = 0x00200000, // use frame dialog template matched to page
			PSH_WIZARD_LITE = 0x00400000,
			PSH_NOCONTEXTHELP = 0x02000000,
			PSH_AEROWIZARD = 0x00004000,
			PSH_RESIZABLE = 0x04000000,
			PSH_HEADERBITMAP = 0x08000000,
			PSH_NOMARGIN = 0x10000000
		}

		/// <summary>
		/// Ensures that the common control DLL (Comctl32.dll) is loaded, and registers specific common control classes from the
		/// DLL. An application must call this function before creating a common control.
		/// </summary>
		/// <param name="icc">
		/// A pointer to an INITCOMMONCONTROLSEX structure that contains information specifying which control
		/// classes will be registered.
		/// </param>
		/// <returns>Returns TRUE if successful, or FALSE otherwise.</returns>
		[DllImport(nameof(ComCtl32))]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX icc);

		/// <summary>
		/// Ensures that the common control DLL (Comctl32.dll) is loaded, and registers specific common control classes from the
		/// DLL. An application must call this function before creating a common control.
		/// </summary>
		/// <param name="ccc">
		/// The <see cref="CommonControlClass" /> value to assign to the dwICC field in
		/// <see cref="INITCOMMONCONTROLSEX" />.
		/// </param>
		/// <returns>Returns TRUE if successful, or FALSE otherwise.</returns>
		public static bool InitCommonControlsEx(CommonControlClass ccc)
		{
			var icc = new INITCOMMONCONTROLSEX(ccc);
			return InitCommonControlsEx(ref icc);
		}

		/// <summary>
		/// Creates a property sheet and adds the pages defined in the specified property sheet header structure.
		/// </summary>
		/// <param name="psh">
		/// Pointer to a <see cref="PROPSHEETHEADER" /> structure that defines the frame and pages of a property
		/// sheet.
		/// </param>
		/// <returns></returns>
		[DllImport(nameof(ComCtl32), SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern int PropertySheet(ref PROPSHEETHEADER psh);

		public struct COLORSCHEME
		{
			public uint clrBtnHighlight;
			// highlight color
			public uint clrBtnShadow;

			public uint dwSize;
			// shadow color
		}
		/// <summary>
		/// Carries information used to load common control classes from the dynamic-link library (DLL). This structure is used
		/// with the <see cref="InitCommonControlsEx(ref INITCOMMONCONTROLSEX)" /> function.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct INITCOMMONCONTROLSEX
		{
			/// <summary>The size of the structure, in bytes.</summary>
			public int dwSize;

			/// <summary>
			/// The set of bit flags that indicate which common control classes will be loaded from the DLL when calling
			/// <see cref="InitCommonControlsEx(ref INITCOMMONCONTROLSEX)" />.
			/// </summary>
			public CommonControlClass dwICC;

			/// <summary>
			/// Initializes a new instance of the <see cref="INITCOMMONCONTROLSEX" /> class and sets the dwICC field.
			/// </summary>
			/// <param name="ccc">The <see cref="CommonControlClass" /> value to assign to the dwICC field.</param>
			public INITCOMMONCONTROLSEX(CommonControlClass ccc)
			{
				dwICC = ccc;
				dwSize = Marshal.SizeOf(typeof(INITCOMMONCONTROLSEX));
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMCUSTOMDRAW
		{
			public User32.NMHDR hdr;
			public CustomDrawStage dwDrawStage;
			public IntPtr hdc;
			public RECT rc;
			public IntPtr dwItemSpec;
			public CustomDrawItemState uItemState;
			public IntPtr lItemlParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PROPSHEETHEADER
		{
			public uint dwSize;
			public PropSheetHeaderFlags dwFlags;
			public IntPtr hwndParent;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public string pszCaption;
			public uint nPages;
			public uint nStartPage;
			public IntPtr phpage;
			public IntPtr pfnCallback;
			public IntPtr hbmWatermark;
			public IntPtr hplWatermark;
			public IntPtr hbmHeader;
		}
	}
}