using System;
using System.Runtime.InteropServices;
using Vanara.Extensions;
using static Vanara.PInvoke.ComCtl32;
using static Vanara.PInvoke.User32;
// ReSharper disable InconsistentNaming
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int I_COLUMNSCALLBACK = -1;
		public const int I_GROUPIDCALLBACK = -1;
		public const int I_GROUPIDNONE = -2;

		public static readonly IntPtr LPSTR_TEXTCALLBACK = (IntPtr)(-1);
		private const int LVM_FIRST = 0x1000;
		private const int LVN_FIRST = -0x100;

		public enum ListViewArrange
		{
			LVA_DEFAULT = 0x0000,
			LVA_ALIGNLEFT = 0x0001,
			LVA_ALIGNTOP = 0x0002,
			LVA_SNAPTOGRID = 0x0005,
		}

		[Flags]
		public enum ListViewBkImageFlag : uint
		{
			LVBKIF_SOURCE_NONE = 0X00000000,
			LVBKIF_SOURCE_HBITMAP = 0X00000001,
			LVBKIF_SOURCE_URL = 0X00000002,
			LVBKIF_SOURCE_MASK = 0X00000003,
			LVBKIF_STYLE_NORMAL = 0X00000000,
			LVBKIF_STYLE_TILE = 0X00000010,
			LVBKIF_STYLE_MASK = 0X00000010,
			LVBKIF_FLAG_TILEOFFSET = 0X00000100,
			LVBKIF_TYPE_WATERMARK = 0X10000000,
			LVBKIF_FLAG_ALPHABLEND = 0X20000000,
		}

		[Flags]
		public enum ListViewColumMask
		{
			LVCF_FMT = 0X0001,
			LVCF_WIDTH = 0X0002,
			LVCF_TEXT = 0X0004,
			LVCF_SUBITEM = 0X0008,
			LVCF_IMAGE = 0X0010,
			LVCF_ORDER = 0X0020,
			LVCF_MINWIDTH = 0X0040,
			LVCF_DEFAULTWIDTH = 0X0080,
			LVCF_IDEALWIDTH = 0X0100,
		}

		[Flags]
		public enum ListViewColumnFormat
		{
			LVCFMT_LEFT = 0X0000,
			LVCFMT_RIGHT = 0X0001,
			LVCFMT_CENTER = 0X0002,
			LVCFMT_JUSTIFYMASK = 0X0003,
			LVCFMT_IMAGE = 0X0800,
			LVCFMT_BITMAPONRIGHT = 0X1000,
			LVCFMT_COLHASIMAGES = 0X8000,
			LVCFMT_FIXEDWIDTH = 0X00100,
			LVCFMT_NODPISCALE = 0X40000,
			LVCFMT_FIXEDRATIO = 0X80000,
			LVCFMT_LINEBREAK = 0X100000,
			LVCFMT_FILL = 0X200000,
			LVCFMT_WRAP = 0X400000,
			LVCFMT_NOTITLE = 0X800000,
			LVCFMT_TILEPLACEMENTMASK = LVCFMT_LINEBREAK | LVCFMT_FILL,
			LVCFMT_SPLITBUTTON = 0X1000000,
		}

		[Flags]
		public enum ListViewFindInfoFlag
		{
			LVFI_PARAM = 0X0001,
			LVFI_STRING = 0X0002,
			LVFI_SUBSTRING = 0X0004,
			LVFI_PARTIAL = 0X0008,
			LVFI_WRAP = 0X0020,
			LVFI_NEARESTXY = 0X0040,
		}

		[Flags]
		public enum ListViewGroupAlignment
		{
			LVGA_HEADER_LEFT = 0x00000001,
			LVGA_HEADER_CENTER = 0x00000002,
			LVGA_HEADER_RIGHT = 0x00000004,  // Don't forget to validate exclusivity
			LVGA_FOOTER_LEFT = 0x00000008,
			LVGA_FOOTER_CENTER = 0x00000010,
			LVGA_FOOTER_RIGHT = 0x00000020,  // Don't forget to validate exclusivity
		}

		[Flags]
		public enum ListViewGroupMask : uint
		{
			LVGF_NONE = 0x00000000,
			LVGF_HEADER = 0x00000001,
			LVGF_FOOTER = 0x00000002,
			LVGF_STATE = 0x00000004,
			LVGF_ALIGN = 0x00000008,
			LVGF_GROUPID = 0x00000010,
			LVGF_SUBTITLE = 0x00000100,  // pszSubtitle is valid
			LVGF_TASK = 0x00000200,  // pszTask is valid
			LVGF_DESCRIPTIONTOP = 0x00000400,  // pszDescriptionTop is valid
			LVGF_DESCRIPTIONBOTTOM = 0x00000800,  // pszDescriptionBottom is valid
			LVGF_TITLEIMAGE = 0x00001000,  // iTitleImage is valid
			LVGF_EXTENDEDIMAGE = 0x00002000,  // iExtendedImage is valid
			LVGF_ITEMS = 0x00004000,  // iFirstItem and cItems are valid
			LVGF_SUBSET = 0x00008000,  // pszSubsetTitle is valid
			LVGF_SUBSETITEMS = 0x00010000,  // readonly, cItems holds count of items in visible subset, iFirstItem is valid
		}

		[Flags]
		public enum ListViewGroupMetricsMask
		{
			LVGMF_NONE = 0x00000000,
			LVGMF_BORDERSIZE = 0x00000001,
			LVGMF_BORDERCOLOR = 0x00000002,
			LVGMF_TEXTCOLOR = 0x00000004,
		}

		public enum ListViewGroupRect
		{
			LVGGR_GROUP = 0, // Entire expanded group
			LVGGR_HEADER = 1, // Header only (collapsed group)
			LVGGR_LABEL = 2, // Label only
			LVGGR_SUBSETLINK = 3, // subset link only
		}

		/// <summary></summary>
		[Flags]
		public enum ListViewGroupState : uint
		{
			/// <summary>Groups are expanded, the group name is displayed, and all items in the group are displayed.</summary>
			LVGS_NORMAL = 0x00000000,

			/// <summary>The group is collapsed.</summary>
			LVGS_COLLAPSED = 0x00000001,

			/// <summary>The group is hidden.</summary>
			LVGS_HIDDEN = 0x00000002,

			/// <summary>Version 6.00 and later. The group does not display a header.</summary>
			LVGS_NOHEADER = 0x00000004,

			/// <summary>Version 6.00 and later. The group can be collapsed.</summary>
			LVGS_COLLAPSIBLE = 0x00000008,

			/// <summary>Version 6.00 and later. The group has keyboard focus.</summary>
			LVGS_FOCUSED = 0x00000010,

			/// <summary>Version 6.00 and later. The group is selected.</summary>
			LVGS_SELECTED = 0x00000020,

			/// <summary>Version 6.00 and later. The group displays only a portion of its items.</summary>
			LVGS_SUBSETED = 0x00000040,

			/// <summary>Version 6.00 and later. The subset link of the group has keyboard focus.</summary>
			LVGS_SUBSETLINKFOCUSED = 0x00000080,
		}

		[Flags]
		public enum ListViewHitTestFlag : uint
		{
			LVHT_NOWHERE = 0X00000001,
			LVHT_ONITEMICON = 0X00000002,
			LVHT_ONITEMLABEL = 0X00000004,
			LVHT_ONITEMSTATEICON = 0X00000008,
			LVHT_ONITEM = LVHT_ONITEMICON | LVHT_ONITEMLABEL | LVHT_ONITEMSTATEICON,
			LVHT_ABOVE = 0X00000008,
			LVHT_BELOW = 0X00000010,
			LVHT_TORIGHT = 0X00000020,
			LVHT_TOLEFT = 0X00000040,
			LVHT_EX_GROUP_HEADER = 0X10000000,
			LVHT_EX_GROUP_FOOTER = 0X20000000,
			LVHT_EX_GROUP_COLLAPSE = 0X40000000,
			LVHT_EX_GROUP_BACKGROUND = 0X80000000,
			LVHT_EX_GROUP_STATEICON = 0X01000000,
			LVHT_EX_GROUP_SUBSETLINK = 0X02000000,
			LVHT_EX_GROUP = LVHT_EX_GROUP_BACKGROUND | LVHT_EX_GROUP_COLLAPSE | LVHT_EX_GROUP_FOOTER | LVHT_EX_GROUP_HEADER | LVHT_EX_GROUP_STATEICON | LVHT_EX_GROUP_SUBSETLINK,
			LVHT_EX_ONCONTENTS = 0X04000000,
			LVHT_EX_FOOTER = 0X08000000,
		}

		public enum ListViewImageList
		{
			LVSIL_NORMAL,
			LVSIL_SMALL,
			LVSIL_STATE,
			LVSIL_GROUPHEADER
		}

		public enum ListViewInsertMarkFlag
		{
			/// <summary>The insertion point appears before the item</summary>
			LVIM_BEFORE,

			/// <summary>The insertion point appears after the item</summary>
			LVIM_AFTER
		}

		/// <summary>
		/// Set of flags that specify which members of the <see cref="LVITEM"/> structure contain data to be set or which members are being requested. This
		/// member can have one or more of the following flags set:
		/// </summary>
		[Flags]
		public enum ListViewItemMask : uint
		{
			/// <summary>The pszText member is valid or must be set.</summary>
			LVIF_TEXT = 0x00000001,

			/// <summary>The iImage member is valid or must be set.</summary>
			LVIF_IMAGE = 0x00000002,

			/// <summary>The lParam member is valid or must be set.</summary>
			LVIF_PARAM = 0x00000004,

			/// <summary>The state member is valid or must be set.</summary>
			LVIF_STATE = 0x00000008,

			/// <summary>The iIndent member is valid or must be set.</summary>
			LVIF_INDENT = 0x00000010,

			/// <summary>
			/// The control will not generate LVN_GETDISPINFO to retrieve text information if it receives an LVM_GETITEM message. Instead, the pszText member
			/// will contain LPSTR_TEXTCALLBACK.
			/// </summary>
			LVIF_NORECOMPUTE = 0x00000800,

			/// <summary>
			/// The iGroupId member is valid or must be set. If this flag is not set when an LVM_INSERTITEM message is sent, the value of iGroupId is assumed to
			/// be I_GROUPIDCALLBACK.
			/// </summary>
			LVIF_GROUPID = 0x00000100,

			/// <summary>The cColumns member is valid or must be set.</summary>
			LVIF_COLUMNS = 0x00000200,

			/// <summary>
			/// Windows Vista and later. The piColFmt member is valid or must be set. If this flag is used, the cColumns member is valid or must be set.
			/// </summary>
			LVIF_COLFMT = 0x00010000,

			/// <summary>
			/// The operating system should store the requested list item information and not ask for it again. This flag is used only with the LVN_GETDISPINFO
			/// notification code.
			/// </summary>
			LVIF_DISETITEM = 0x1000,

			/// <summary>Complete mask.</summary>
			LVIF_ALL = 0x0001FFFF
		}

		public enum ListViewItemRect
		{
			LVIR_BOUNDS,
			LVIR_ICON,
			LVIR_LABEL,
			LVIR_SELECTBOUNDS
		}

		/// <summary>
		/// An item's state value consists of the item's state, an optional overlay mask index, and an optional state image mask index. An item's state
		/// determines its appearance and functionality. The state can be zero or one or more of the following values:
		/// </summary>
		[Flags]
		public enum ListViewItemState : uint
		{
			/// <summary>No flags set.</summary>
			LVIS_NONE = 0x0000,

			/// <summary>
			/// The item has the focus, so it is surrounded by a standard focus rectangle. Although more than one item may be selected, only one item can have
			/// the focus.
			/// </summary>
			LVIS_FOCUSED = 0x0001,

			/// <summary>
			/// The item is selected. The appearance of a selected item depends on whether it has the focus and also on the system colors used for selection.
			/// </summary>
			LVIS_SELECTED = 0x0002,

			/// <summary>The item is marked for a cut-and-paste operation.</summary>
			LVIS_CUT = 0x0004,

			/// <summary>The item is highlighted as a drag-and-drop target.</summary>
			LVIS_DROPHILITED = 0x0008,

			/// <summary>Undocumented.</summary>
			LVIS_GLOW = 0x0010,

			// ///
			// <summary>Not currently supported.</summary>
			// Activating = 0x0020,
			/// <summary>The item's overlay image index is retrieved by a mask.</summary>
			LVIS_OVERLAYMASK = 0x0F00,

			/// <summary>The item's state image index is retrieved by a mask.</summary>
			LVIS_STATEIMAGEMASK = 0xF000,

			/// <summary>All flags.</summary>
			LVIS_ALL = 0xFFFFFFFF
		}

		/// <summary>LVM_ Messages for SendMessage</summary>
		public enum ListViewMessage : uint
		{
			LVM_SETUNICODEFORMAT = 0X2005,        // CCM_SETUNICODEFORMAT,
			LVM_GETUNICODEFORMAT = 0X2006,        // CCM_GETUNICODEFORMAT,
			LVM_GETBKCOLOR = LVM_FIRST + 0,
			LVM_SETBKCOLOR = LVM_FIRST + 1,
			LVM_GETIMAGELIST = LVM_FIRST + 2,
			LVM_SETIMAGELIST = LVM_FIRST + 3,
			LVM_GETITEMCOUNT = LVM_FIRST + 4,
			LVM_GETITEM = LVM_FIRST + 75,
			LVM_SETITEM = LVM_FIRST + 76,
			LVM_INSERTITEM = LVM_FIRST + 77,
			LVM_DELETEITEM = LVM_FIRST + 8,
			LVM_DELETEALLITEMS = LVM_FIRST + 9,
			LVM_GETCALLBACKMASK = LVM_FIRST + 10,
			LVM_SETCALLBACKMASK = LVM_FIRST + 11,
			LVM_GETNEXTITEM = LVM_FIRST + 12,
			LVM_FINDITEM = LVM_FIRST + 83,
			LVM_GETITEMRECT = LVM_FIRST + 14,
			LVM_SETITEMPOSITION = LVM_FIRST + 15,
			LVM_GETITEMPOSITION = LVM_FIRST + 16,
			LVM_GETSTRINGWIDTH = LVM_FIRST + 87,
			LVM_HITTEST = LVM_FIRST + 18,
			LVM_ENSUREVISIBLE = LVM_FIRST + 19,
			LVM_SCROLL = LVM_FIRST + 20,
			LVM_REDRAWITEMS = LVM_FIRST + 21,
			LVM_ARRANGE = LVM_FIRST + 22,
			LVM_EDITLABEL = LVM_FIRST + 118,
			LVM_GETEDITCONTROL = LVM_FIRST + 24,
			LVM_GETCOLUMN = LVM_FIRST + 95,
			LVM_SETCOLUMN = LVM_FIRST + 96,
			LVM_INSERTCOLUMN = LVM_FIRST + 97,
			LVM_DELETECOLUMN = LVM_FIRST + 28,
			LVM_GETCOLUMNWIDTH = LVM_FIRST + 29,
			LVM_SETCOLUMNWIDTH = LVM_FIRST + 30,
			LVM_GETHEADER = LVM_FIRST + 31,
			LVM_CREATEDRAGIMAGE = LVM_FIRST + 33,
			LVM_GETVIEWRECT = LVM_FIRST + 34,
			LVM_GETTEXTCOLOR = LVM_FIRST + 35,
			LVM_SETTEXTCOLOR = LVM_FIRST + 36,
			LVM_GETTEXTBKCOLOR = LVM_FIRST + 37,
			LVM_SETTEXTBKCOLOR = LVM_FIRST + 38,
			LVM_GETTOPINDEX = LVM_FIRST + 39,
			LVM_GETCOUNTPERPAGE = LVM_FIRST + 40,
			LVM_GETORIGIN = LVM_FIRST + 41,
			LVM_UPDATE = LVM_FIRST + 42,
			LVM_SETITEMSTATE = LVM_FIRST + 43,
			LVM_GETITEMSTATE = LVM_FIRST + 44,
			LVM_GETITEMTEXT = LVM_FIRST + 115,
			LVM_SETITEMTEXT = LVM_FIRST + 116,
			LVM_SETITEMCOUNT = LVM_FIRST + 47,
			LVM_SORTITEMS = LVM_FIRST + 48,
			LVM_SETITEMPOSITION32 = LVM_FIRST + 49,
			LVM_GETSELECTEDCOUNT = LVM_FIRST + 50,
			LVM_GETITEMSPACING = LVM_FIRST + 51,
			LVM_GETISEARCHSTRING = LVM_FIRST + 117,
			LVM_SETICONSPACING = LVM_FIRST + 53,
			LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54,            // OPTIONAL WPARAM == MASK
			LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55,
			LVM_GETSUBITEMRECT = LVM_FIRST + 56,
			LVM_SUBITEMHITTEST = LVM_FIRST + 57,
			LVM_SETCOLUMNORDERARRAY = LVM_FIRST + 58,
			LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59,
			LVM_SETHOTITEM = LVM_FIRST + 60,
			LVM_GETHOTITEM = LVM_FIRST + 61,
			LVM_SETHOTCURSOR = LVM_FIRST + 62,
			LVM_GETHOTCURSOR = LVM_FIRST + 63,
			LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64,
			LVM_SETWORKAREAS = LVM_FIRST + 65,
			LVM_GETWORKAREAS = LVM_FIRST + 70,
			LVM_GETNUMBEROFWORKAREAS = LVM_FIRST + 73,
			LVM_GETSELECTIONMARK = LVM_FIRST + 66,
			LVM_SETSELECTIONMARK = LVM_FIRST + 67,
			LVM_SETHOVERTIME = LVM_FIRST + 71,
			LVM_GETHOVERTIME = LVM_FIRST + 72,
			LVM_SETTOOLTIPS = LVM_FIRST + 74,
			LVM_GETTOOLTIPS = LVM_FIRST + 78,
			LVM_SORTITEMSEX = LVM_FIRST + 81,
			LVM_SETBKIMAGE = LVM_FIRST + 138,
			LVM_GETBKIMAGE = LVM_FIRST + 139,
			LVM_SETSELECTEDCOLUMN = LVM_FIRST + 140,
			LVM_SETVIEW = LVM_FIRST + 142,
			LVM_GETVIEW = LVM_FIRST + 143,
			LVM_INSERTGROUP = LVM_FIRST + 145,
			LVM_SETGROUPINFO = LVM_FIRST + 147,
			LVM_GETGROUPINFO = LVM_FIRST + 149,
			LVM_REMOVEGROUP = LVM_FIRST + 150,
			LVM_MOVEGROUP = LVM_FIRST + 151,
			LVM_GETGROUPCOUNT = LVM_FIRST + 152,
			LVM_GETGROUPINFOBYINDEX = LVM_FIRST + 153,
			LVM_MOVEITEMTOGROUP = LVM_FIRST + 154,
			LVM_GETGROUPRECT = LVM_FIRST + 98,
			LVM_SETGROUPMETRICS = LVM_FIRST + 155,
			LVM_GETGROUPMETRICS = LVM_FIRST + 156,
			LVM_ENABLEGROUPVIEW = LVM_FIRST + 157,
			LVM_SORTGROUPS = LVM_FIRST + 158,
			LVM_INSERTGROUPSORTED = LVM_FIRST + 159,
			LVM_REMOVEALLGROUPS = LVM_FIRST + 160,
			LVM_HASGROUP = LVM_FIRST + 161,
			LVM_GETGROUPSTATE = LVM_FIRST + 92,
			LVM_GETFOCUSEDGROUP = LVM_FIRST + 93,
			LVM_SETTILEVIEWINFO = LVM_FIRST + 162,
			LVM_GETTILEVIEWINFO = LVM_FIRST + 163,
			LVM_SETTILEINFO = LVM_FIRST + 164,
			LVM_GETTILEINFO = LVM_FIRST + 165,
			LVM_SETINSERTMARK = LVM_FIRST + 166,
			LVM_GETINSERTMARK = LVM_FIRST + 167,
			LVM_INSERTMARKHITTEST = LVM_FIRST + 168,
			LVM_GETINSERTMARKRECT = LVM_FIRST + 169,
			LVM_SETINSERTMARKCOLOR = LVM_FIRST + 170,
			LVM_GETINSERTMARKCOLOR = LVM_FIRST + 171,
			LVM_GETSELECTEDCOLUMN = LVM_FIRST + 174,
			LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175,
			LVM_GETOUTLINECOLOR = LVM_FIRST + 176,
			LVM_SETOUTLINECOLOR = LVM_FIRST + 177,
			LVM_CANCELEDITLABEL = LVM_FIRST + 179,
			LVM_MAPINDEXTODD = LVM_FIRST + 180,
			LVM_MAPIDTOINDEX = LVM_FIRST + 181,
			LVM_ISITEMVISIBLE = LVM_FIRST + 182,
			LVM_GETACCVERSION = LVM_FIRST + 193,
			LVM_GETEMPTYTEXT = LVM_FIRST + 204,
			LVM_GETFOOTERRECT = LVM_FIRST + 205,
			LVM_GETFOOTERINFO = LVM_FIRST + 206,
			LVM_GETFOOTERITEMRECT = LVM_FIRST + 207,
			LVM_GETFOOTERITEM = LVM_FIRST + 208,
			LVM_GETITEMINDEXRECT = LVM_FIRST + 209,
			LVM_SETITEMINDEXSTATE = LVM_FIRST + 210,
			LVM_GETNEXTITEMINDEX = LVM_FIRST + 211,
			LVM_SETPRESERVEALPHA = LVM_FIRST + 212,
			/*LVM_SetBkImage               = SETBKIMAGEW,
			LVM_GetBkImage               = GETBKIMAGEW,*/
		}

		[Flags]
		public enum ListViewNextItemFlag
		{
			LVNI_ALL = 0X0000,
			LVNI_FOCUSED = 0X0001,
			LVNI_SELECTED = 0X0002,
			LVNI_CUT = 0X0004,
			LVNI_DROPHILITED = 0X0008,
			LVNI_STATEMASK = LVNI_FOCUSED | LVNI_SELECTED | LVNI_CUT | LVNI_DROPHILITED,
			LVNI_VISIBLEORDER = 0X0010,
			LVNI_PREVIOUS = 0X0020,
			LVNI_VISIBLEONLY = 0X0040,
			LVNI_SAMEGROUPONLY = 0X0080,
			LVNI_ABOVE = 0X0100,
			LVNI_BELOW = 0X0200,
			LVNI_TOLEFT = 0X0400,
			LVNI_TORIGHT = 0X0800,
			LVNI_DIRECTIONMASK = LVNI_ABOVE | LVNI_BELOW | LVNI_TOLEFT | LVNI_TORIGHT,
		}

		public enum ListViewNotification
		{
			LVN_BEGINDRAG = -109,
			LVN_BEGINLABELEDIT = -175,
			LVN_BEGINRDRAG = -111,
			LVN_COLUMNCLICK = -108,
			LVN_ENDLABELEDIT = -176,
			LVN_GETDISPINFO = -177,
			LVN_GETINFOTIP = -158,
			LVN_ITEMACTIVATE = -114,
			LVN_ITEMCHANGED = -101,
			LVN_ITEMCHANGING = -100,
			LVN_KEYDOWN = -155,
			LVN_ODCACHEHINT = -113,
			LVN_ODFINDITEM = -179,
			LVN_ODSTATECHANGED = -115,
			LVN_SETDISPINFO = -178,
			LVN_COLUMNDROPDOWN = -164,
		}

		public enum ListViewSearchDirection
		{
			VK_PRIOR = 0x21,
			VK_NEXT = 0x22,
			VK_END = 0x23,
			VK_HOME = 0x24,
			VK_LEFT = 0x25,
			VK_UP = 0x26,
			VK_RIGHT = 0x27,
			VK_DOWN = 0x28,
		}

		[Flags]
		public enum ListViewStyle
		{
			LVS_ICON = 0x0000,
			LVS_REPORT = 0x0001,
			LVS_SMALLICON = 0x0002,
			LVS_LIST = 0x0003,
			LVS_TYPEMASK = 0x0003,
			LVS_SINGLESEL = 0x0004,
			LVS_SHOWSELALWAYS = 0x0008,
			LVS_SORTASCENDING = 0x0010,
			LVS_SORTDESCENDING = 0x0020,
			LVS_SHAREIMAGELISTS = 0x0040,
			LVS_NOLABELWRAP = 0x0080,
			LVS_AUTOARRANGE = 0x0100,
			LVS_EDITLABELS = 0x0200,
			LVS_OWNERDATA = 0x1000,
			LVS_NOSCROLL = 0x2000,
			LVS_TYPESTYLEMASK = 0xfc00,
			LVS_ALIGNTOP = 0x0000,
			LVS_ALIGNLEFT = 0x0800,
			LVS_ALIGNMASK = 0x0c00,
			LVS_OWNERDRAWFIXED = 0x0400,
			LVS_NOCOLUMNHEADER = 0x4000,
			LVS_NOSORTHEADER = 0x8000,
		}

		[Flags]
		public enum ListViewStyleEx : uint
		{
			LVS_EX_GRIDLINES = 0X00000001,
			LVS_EX_SUBITEMIMAGES = 0X00000002,
			LVS_EX_CHECKBOXES = 0X00000004,
			LVS_EX_TRACKSELECT = 0X00000008,
			LVS_EX_HEADERDRAGDROP = 0X00000010,
			LVS_EX_FULLROWSELECT = 0X00000020,
			LVS_EX_ONECLICKACTIVATE = 0X00000040,
			LVS_EX_TWOCLICKACTIVATE = 0X00000080,
			LVS_EX_FLATSB = 0X00000100,
			LVS_EX_REGIONAL = 0X00000200,
			LVS_EX_INFOTIP = 0X00000400,
			LVS_EX_UNDERLINEHOT = 0X00000800,
			LVS_EX_UNDERLINECOLD = 0X00001000,
			LVS_EX_MULTIWORKAREAS = 0X00002000,
			LVS_EX_LABELTIP = 0X00004000,
			LVS_EX_BORDERSELECT = 0X00008000,
			LVS_EX_DOUBLEBUFFER = 0X00010000,
			LVS_EX_HIDELABELS = 0X00020000,
			LVS_EX_SINGLEROW = 0X00040000,
			LVS_EX_SNAPTOGRID = 0X00080000,
			LVS_EX_SIMPLESELECT = 0X00100000,
			LVS_EX_JUSTIFYCOLUMNS = 0X00200000,
			LVS_EX_TRANSPARENTBKGND = 0X00400000,
			LVS_EX_TRANSPARENTSHADOWTEXT = 0X00800000,
			LVS_EX_AUTOAUTOARRANGE = 0X01000000,
			LVS_EX_HEADERINALLVIEWS = 0X02000000,
			LVS_EX_AUTOCHECKSELECT = 0X08000000,
			LVS_EX_AUTOSIZECOLUMNS = 0X10000000,
			LVS_EX_COLUMNSNAPPOINTS = 0X40000000,
			LVS_EX_COLUMNOVERFLOW = 0X80000000,
		}

		public enum ListViewTileViewFlag : uint
		{
			LVTVIF_AUTOSIZE = 0x00000000,
			LVTVIF_FIXEDWIDTH = 0x00000001,
			LVTVIF_FIXEDHEIGHT = 0x00000002,
			LVTVIF_FIXEDSIZE = 0x00000003,
			LVTVIF_EXTENDED = 0x00000004,
		}

		[Flags]
		public enum ListViewTileViewMask : uint
		{
			LVTVIM_TILESIZE = 0x00000001,
			LVTVIM_COLUMNS = 0x00000002,
			LVTVIM_LABELMARGIN = 0x00000004,
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct LVFINDINFO
		{
			public ListViewFindInfoFlag flags;
			public string psz;
			public IntPtr lParam;
			public int ptX;
			public int ptY;
			public ListViewSearchDirection vkDirection;

			public LVFINDINFO(string searchString, bool allowPartial, bool wrap) : this()
			{
				psz = searchString;
				flags = ListViewFindInfoFlag.LVFI_STRING;
				if (allowPartial)
					flags |= ListViewFindInfoFlag.LVFI_PARTIAL;
				if (wrap)
					flags |= ListViewFindInfoFlag.LVFI_WRAP;
			}

			public LVFINDINFO(IntPtr lParam) : this()
			{
				flags = ListViewFindInfoFlag.LVFI_PARAM;
				this.lParam = lParam;
			}

			public LVFINDINFO(System.Drawing.Point pt, ListViewSearchDirection searchDirection) : this()
			{
				flags = ListViewFindInfoFlag.LVFI_NEARESTXY;
				ptX = pt.X;
				ptY = pt.Y;
				vkDirection = searchDirection;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LVITEMINDEX
		{
			public int iItem;
			public int iGroup;
		}

		public struct LVTILECOLUMNINFO
		{
			public uint columnIndex;
			public ListViewColumnFormat format;

			public LVTILECOLUMNINFO(uint colIdx, ListViewColumnFormat fmt = 0)
			{
				columnIndex = colIdx;
				format = fmt;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMLISTVIEW
		{
			public User32.NMHDR hdr;
			public int iItem;
			public int iSubItem;
			public int uNewState;
			public int uOldState;
			public int uChanged;
			public System.Drawing.Point ptAction;
			public IntPtr lParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVBKIMAGE : IDisposable
		{
			public ListViewBkImageFlag ulFlags;
			public IntPtr hBmp = IntPtr.Zero;
			public IntPtr pszImage = IntPtr.Zero;
			public uint cchImageMax;
			public int xOffset;
			public int yOffset;

			public LVBKIMAGE(System.Drawing.Bitmap bmp, bool isWatermark, bool isWatermarkAlphaBlended)
			{
				Bitmap = bmp;
				ulFlags = isWatermark ? ListViewBkImageFlag.LVBKIF_TYPE_WATERMARK : ListViewBkImageFlag.LVBKIF_SOURCE_HBITMAP;
				if (isWatermark && isWatermarkAlphaBlended)
					ulFlags |= ListViewBkImageFlag.LVBKIF_FLAG_ALPHABLEND;
			}

			public LVBKIMAGE(System.Drawing.Bitmap bmp, bool isTiled)
			{
				Bitmap = bmp;
				ulFlags = ListViewBkImageFlag.LVBKIF_SOURCE_HBITMAP;
				if (isTiled)
					ulFlags |= ListViewBkImageFlag.LVBKIF_STYLE_TILE;
			}

			public LVBKIMAGE(string url, bool isTiled)
			{
				Url = url;
				ulFlags = ListViewBkImageFlag.LVBKIF_SOURCE_URL;
				if (isTiled)
					ulFlags |= ListViewBkImageFlag.LVBKIF_STYLE_TILE;
			}

			public LVBKIMAGE() : this(ListViewBkImageFlag.LVBKIF_SOURCE_NONE)
			{
			}

			public LVBKIMAGE(ListViewBkImageFlag flags)
			{
				ulFlags = flags;
				if (ulFlags.IsFlagSet(ListViewBkImageFlag.LVBKIF_SOURCE_URL))
				{
					cchImageMax = 1024;
					StringExtensions.AllocString(ref pszImage, ref cchImageMax);
				}
			}

			public System.Drawing.Bitmap Bitmap
			{
				get { return hBmp != IntPtr.Zero ? System.Drawing.Image.FromHbitmap(hBmp) : null; }
				set { hBmp = value?.GetHbitmap() ?? IntPtr.Zero; }
			}

			public string Url
			{
				get { return StringExtensions.GetString(pszImage); }
				set { StringExtensions.SetString(ref pszImage, ref cchImageMax, value); }
			}

			void IDisposable.Dispose()
			{
				StringExtensions.FreeString(ref pszImage, ref cchImageMax);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVCOLUMN : IDisposable
		{
			public ListViewColumMask mask;
			public ListViewColumnFormat fmt;
			public int cx;
			public IntPtr pszText;
			public uint cchTextMax;
			public int iSubItem;
			public int iImage;
			public int iOrder;
			public int cxMin;
			public int cxDefault;
			public int cxIdeal;

			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
			public LVCOLUMN(ListViewColumMask mask)
			{
				this.mask = mask;
				if (mask.IsFlagSet(ListViewColumMask.LVCF_TEXT))
					StringExtensions.AllocString(ref pszText, ref cchTextMax);
			}

			public ListViewColumnFormat Format
			{
				get { return fmt; }
				set { fmt = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_FMT); }
			}

			public string Text
			{
				get { return StringExtensions.GetString(pszText); }
				set { StringExtensions.SetString(ref pszText, ref cchTextMax, value); EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_TEXT, value != null); }
			}

			public int Subitem
			{
				get { return iSubItem; }
				set { iSubItem = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_SUBITEM); }
			}

			public int ImageListIndex
			{
				get { return iImage; }
				set { iImage = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_IMAGE); }
			}

			public int ColumnPosition
			{
				get { return iOrder; }
				set { iOrder = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_ORDER); }
			}

			public int DefaultWidth
			{
				get { return cxDefault; }
				set { cxDefault = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_DEFAULTWIDTH); }
			}

			public int MinWidth
			{
				get { return cxMin; }
				set { cxMin = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_MINWIDTH); }
			}

			public int IdealWidth
			{
				get { return cxIdeal; }
				set { cxIdeal = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_IDEALWIDTH); }
			}

			public int Width
			{
				get { return cx; }
				set { cx = value; EnumExtensions.SetFlags(ref mask, ListViewColumMask.LVCF_WIDTH); }
			}

			void IDisposable.Dispose()
			{
				StringExtensions.FreeString(ref pszText, ref cchTextMax);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVGROUP : IDisposable
		{
			public int cbSize;
			public ListViewGroupMask mask;
			public IntPtr pszHeader;
			public uint cchHeader;
			public IntPtr pszFooter;
			public uint cchFooter;
			public int iGroupId;
			public ListViewGroupState stateMask;
			public ListViewGroupState state;
			public ListViewGroupAlignment uAlign;
			public IntPtr pszSubtitle;
			public uint cchSubtitle;
			public IntPtr pszTask;
			public uint cchTask;
			public IntPtr pszDescriptionTop;
			public uint cchDescriptionTop;
			public IntPtr pszDescriptionBottom;
			public uint cchDescriptionBottom;
			public int iTitleImage;
			public int iExtendedImage;
			public int iFirstItem;
			public uint cItems;
			public IntPtr pszSubsetTitle;
			public uint cchSubsetTitle;

			/*public LVGROUP(ListViewGroup grp) : this(ListViewGroupMask.LVGF_NONE, grp.Header)
			{
				HeaderAlignment = grp.HeaderAlignment;
				var pi = grp.GetType().GetProperty("ID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, typeof(int), Type.EmptyTypes, null);
				if (pi != null)
					ID = (int)pi.GetValue(grp, null);
			}*/

			[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
			public LVGROUP(ListViewGroupMask mask = ListViewGroupMask.LVGF_NONE, string header = null)
			{
				cbSize = Marshal.SizeOf(this);
				this.mask = mask;

				if (header != null)
					Header = header;
				else if ((mask & ListViewGroupMask.LVGF_HEADER) != 0)
					StringExtensions.AllocString(ref pszHeader, ref cchHeader);

				if ((mask & ListViewGroupMask.LVGF_FOOTER) != 0)
					StringExtensions.AllocString(ref pszFooter, ref cchFooter);

				if ((mask & ListViewGroupMask.LVGF_SUBTITLE) != 0)
					StringExtensions.AllocString(ref pszSubtitle, ref cchSubtitle);

				if ((mask & ListViewGroupMask.LVGF_TASK) != 0)
					StringExtensions.AllocString(ref pszTask, ref cchTask);

				if ((mask & ListViewGroupMask.LVGF_DESCRIPTIONBOTTOM) != 0)
					StringExtensions.AllocString(ref pszDescriptionBottom, ref cchDescriptionBottom);

				if ((mask & ListViewGroupMask.LVGF_DESCRIPTIONTOP) != 0)
					StringExtensions.AllocString(ref pszDescriptionTop, ref cchDescriptionTop);
			}

			public string DescriptionBottom
			{
				get { return StringExtensions.GetString(pszDescriptionBottom); }
				[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_DESCRIPTIONBOTTOM, StringExtensions.SetString(ref pszDescriptionBottom, ref cchDescriptionBottom, value)); }
			}

			public string DescriptionTop
			{
				get { return StringExtensions.GetString(pszDescriptionTop); }
				[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_DESCRIPTIONTOP, StringExtensions.SetString(ref pszDescriptionTop, ref cchDescriptionTop, value)); }
			}

			public string Footer
			{
				get { return StringExtensions.GetString(pszFooter); }
				[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_FOOTER, StringExtensions.SetString(ref pszFooter, ref cchFooter, value)); }
			}

			public int ID
			{
				get { return iGroupId; }
				set { iGroupId = value; mask |= ListViewGroupMask.LVGF_GROUPID; }
			}

			public int TitleImageIndex
			{
				get { return iTitleImage; }
				set { iTitleImage = value; mask |= ListViewGroupMask.LVGF_TITLEIMAGE; }
			}

			public int ExtendedImageIndex
			{
				get { return iExtendedImage; }
				set { iExtendedImage = value; mask |= ListViewGroupMask.LVGF_EXTENDEDIMAGE; }
			}

			public int FirstItem => iFirstItem;

			public uint ItemCount => cItems;

			public ListViewGroupAlignment Alignment
			{
				get { return uAlign; }
				set { uAlign = value; mask |= ListViewGroupMask.LVGF_ALIGN; }
			}

			public string Header
			{
				get { return StringExtensions.GetString(pszHeader); }
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_HEADER, StringExtensions.SetString(ref pszHeader, ref cchHeader, value)); }
			}

			public string Subtitle
			{
				get { return StringExtensions.GetString(pszSubtitle); }
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_SUBTITLE, StringExtensions.SetString(ref pszSubtitle, ref cchSubtitle, value)); }
			}

			public string Task
			{
				get { return StringExtensions.GetString(pszTask); }
				[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
				set { EnumExtensions.SetFlags(ref mask, ListViewGroupMask.LVGF_TASK, StringExtensions.SetString(ref pszTask, ref cchTask, value)); }
			}

			public void Dispose()
			{
				StringExtensions.FreeString(ref pszHeader, ref cchHeader);
				StringExtensions.FreeString(ref pszFooter, ref cchFooter);
				StringExtensions.FreeString(ref pszSubtitle, ref cchSubtitle);
				StringExtensions.FreeString(ref pszTask, ref cchTask);
				StringExtensions.FreeString(ref pszDescriptionBottom, ref cchDescriptionBottom);
				StringExtensions.FreeString(ref pszDescriptionTop, ref cchDescriptionTop);
			}

			public void SetState(ListViewGroupState gState, bool on = true)
			{
				mask |= ListViewGroupMask.LVGF_STATE;
				stateMask |= gState;
				EnumExtensions.SetFlags(ref state, gState, on);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVGROUPMETRICS
		{
			public uint cbSize = (uint)Marshal.SizeOf(typeof(LVGROUPMETRICS));
			public ListViewGroupMetricsMask mask;
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
			public uint crLeft;
			public uint crTop;
			public uint crRight;
			public uint crBottom;
			public uint crHeader;
			public uint crFooter;

			public LVGROUPMETRICS(ListViewGroupMetricsMask mask = ListViewGroupMetricsMask.LVGMF_NONE)
			{
				this.mask = mask;
			}

			public LVGROUPMETRICS(int left, int top, int right, int bottom)
			{
				SetBorderSize(left, top, right, bottom);
			}

			public void SetBorderSize(int left, int top, int right, int bottom)
			{
				mask = ListViewGroupMetricsMask.LVGMF_BORDERSIZE;
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVHITTESTINFO
		{
			public int pt_x;
			public int pt_y;
			public ListViewHitTestFlag flags;
			public int iItem;
			public int iSubItem;
			public int iGroup;

			public LVHITTESTINFO(System.Drawing.Point pt)
			{
				pt_x = pt.X; pt_y = pt.Y;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVINSERTMARK
		{
			public uint cbSize = (uint)Marshal.SizeOf(typeof(LVINSERTMARK));
			public ListViewInsertMarkFlag dwFlags;
			public int iItem;
			public int dwReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVITEM : IDisposable
		{
			public ListViewItemMask mask;
			public int iItem;
			public int iSubItem;
			public uint state;
			public ListViewItemState stateMask;
			public IntPtr pszText;
			public uint cchTextMax;
			public int iImage;
			public IntPtr lParam;
			public int iIndent;
			public int iGroupId;
			public uint cColumns;
			public IntPtr puColumns;
			public IntPtr piColFmt;
			public int iGroup;

			public LVITEM(int item, int subitem = 0, ListViewItemMask mask = ListViewItemMask.LVIF_ALL, ListViewItemState stateMask = ListViewItemState.LVIS_NONE)
			{
				if (mask.IsFlagSet(ListViewItemMask.LVIF_TEXT))
				{
					cchTextMax = 1024;
					StringExtensions.AllocString(ref pszText, ref cchTextMax);
				}
				iItem = item;
				iSubItem = subitem;
				this.stateMask = stateMask;
			}

			public LVITEM(int item, int subitem, string text)
			{
				iItem = item;
				iSubItem = subitem;
				Text = text;
			}

			public LVITEM(int item)
			{
				iItem = item;
			}

			public int GroupId
			{
				get { return iGroupId; }
				set { iGroupId = value; EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_GROUPID); }
			}

			public int ImageIndex
			{
				get { return iImage; }
				set { iImage = value; EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_IMAGE); }
			}

			public int Indent
			{
				get { return iIndent; }
				set { iIndent = value; EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_INDENT); }
			}

			public IntPtr LParam
			{
				get { return lParam; }
				set { lParam = value; EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_PARAM); }
			}

			public string Text
			{
				get { return StringExtensions.GetString(pszText); }
				set { StringExtensions.SetString(ref pszText, ref cchTextMax, value); EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_TEXT); }
			}

			public LVTILECOLUMNINFO[] TileColumns
			{
				get
				{
					var ret = new LVTILECOLUMNINFO[cColumns];
					var cols = new int[cColumns];
					var fmts = new int[cColumns];
					Marshal.Copy(puColumns, cols, 0, (int)cColumns);
					if (piColFmt != IntPtr.Zero)
						Marshal.Copy(piColFmt, fmts, 0, (int)cColumns);
					for (var i = 0; i < cColumns; i++)
						ret[i] = new LVTILECOLUMNINFO() { columnIndex = (uint)cols[i], format = (ListViewColumnFormat)fmts[i] };
					return ret;
				}
				set
				{
					if (value == null)
						throw new ArgumentNullException();
					cColumns = (uint)value.Length;
					if (value.Length > 0)
					{
						var cols = new int[cColumns];
						var fmts = new int[cColumns];
						var hasFmts = false;
						for (var i = 0; i < cColumns; i++)
						{
							cols[i] = (int)value[i].columnIndex;
							fmts[i] = (int)value[i].format;
							if (fmts[i] != 0) hasFmts = true;
						}
						puColumns = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * (int)cColumns);
						Marshal.Copy(cols, 0, puColumns, (int)cColumns);
						EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_COLUMNS);
						if (hasFmts)
						{
							piColFmt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * (int)cColumns);
							Marshal.Copy(fmts, 0, piColFmt, (int)cColumns);
							EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_COLFMT);
						}
					}
					else
					{
						puColumns = IntPtr.Zero;
						piColFmt = IntPtr.Zero;
						EnumExtensions.SetFlags(ref mask, ListViewItemMask.LVIF_COLFMT | ListViewItemMask.LVIF_COLUMNS, false);
					}
				}
			}

			public int[] VisibleTileColumns
			{
				get
				{
					var cols = new int[cColumns];
					Marshal.Copy(puColumns, cols, 0, (int)cColumns);
					return cols;
				}
				set
				{
					if (value == null)
						value = new int[0];
					cColumns = (uint)value.Length;
					if (value.Length > 0)
					{
						puColumns = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * (int)cColumns);
						Marshal.Copy(value, 0, puColumns, (int)cColumns);
						mask.SetFlags(ListViewItemMask.LVIF_COLUMNS);
					}
					else
					{
						puColumns = IntPtr.Zero;
						mask.SetFlags(ListViewItemMask.LVIF_COLUMNS, false);
					}
				}
			}

			public ListViewItemState State => (ListViewItemState)(state & 0x000000FF);

			public bool GetState(ListViewItemState itemState) => State.IsFlagSet(itemState);

			public void SetState(ListViewItemState itemState, bool on = true)
			{
				mask |= ListViewItemMask.LVIF_STATE;
				stateMask |= itemState;
				var tempState = State;
				EnumExtensions.SetFlags(ref tempState, itemState, on);
				state = (uint)tempState | (state & 0xFFFFFF00);
			}

			public uint OverlayImageIndex
			{
				get { return (state & 0x00000F00) >> 8; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(OverlayImageIndex), "Overlay image index must be between 0 and 15");
					mask |= ListViewItemMask.LVIF_STATE;
					stateMask |= ListViewItemState.LVIS_OVERLAYMASK;
					state = (value << 8) | (state & 0xFFFFF0FF);
				}
			}

			public uint StateImageIndex
			{
				get { return (state & 0x0000F000) >> 12; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(StateImageIndex), "State image index must be between 0 and 15");
					mask |= ListViewItemMask.LVIF_STATE;
					stateMask |= ListViewItemState.LVIS_STATEIMAGEMASK;
					state = (value << 12) | (state & 0xFFFF0FFF);
				}
			}

			public override string ToString() => $"LVITEM: pszText={Text}; iItem={iItem}; iSubItem={iSubItem}; state={state}; iGroupId={iGroupId}; cColumns={cColumns}";

			void IDisposable.Dispose()
			{
				StringExtensions.FreeString(ref pszText, ref cchTextMax);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public class LVTILEVIEWINFO
		{
			public uint cbSize = (uint)Marshal.SizeOf(typeof(LVTILEVIEWINFO));
			public ListViewTileViewMask dwMask;
			public ListViewTileViewFlag dwFlags;
			public SIZE sizeTile;
			public int cLines;
			public RECT rcLabelMargin;

			public LVTILEVIEWINFO(ListViewTileViewMask mask)
			{
				dwMask = mask;
			}

			public bool AutoSize
			{
				get { return dwFlags.IsFlagSet(ListViewTileViewFlag.LVTVIF_AUTOSIZE); }
				set { dwFlags = ListViewTileViewFlag.LVTVIF_AUTOSIZE; dwMask |= ListViewTileViewMask.LVTVIM_TILESIZE; sizeTile.cy = sizeTile.cx = 0; }
			}

			public System.Drawing.Size TileSize
			{
				get { return sizeTile; }
				set { sizeTile = value; dwMask |= ListViewTileViewMask.LVTVIM_TILESIZE; dwFlags |= ListViewTileViewFlag.LVTVIF_FIXEDSIZE; }
			}

			public int TileHeight
			{
				get { return sizeTile.cy; }
				set { sizeTile.cy = value; dwMask |= ListViewTileViewMask.LVTVIM_TILESIZE; dwFlags |= ListViewTileViewFlag.LVTVIF_FIXEDHEIGHT; }
			}

			public int TileWidth
			{
				get { return sizeTile.cx; }
				set { sizeTile.cx = value; dwMask |= ListViewTileViewMask.LVTVIM_TILESIZE; dwFlags |= ListViewTileViewFlag.LVTVIF_FIXEDWIDTH; }
			}

			public int MaxTextLines
			{
				get { return cLines; }
				set { cLines = value; dwMask |= ListViewTileViewMask.LVTVIM_COLUMNS; }
			}

			public RECT TilePadding
			{
				get { return rcLabelMargin; }
				set { rcLabelMargin = value; dwMask |= ListViewTileViewMask.LVTVIM_LABELMARGIN; }
			}
		}
	}

	public static partial class User32
	{
		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVBKIMAGE bkImage);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVCOLUMN column);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ref ComCtl32.LVFINDINFO findInfo);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVGROUP group);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVGROUPMETRICS metrics);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVHITTESTINFO hitTestInfo);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVINSERTMARK insertMark);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, ref System.Drawing.Point wParam, [In, Out] ComCtl32.LVINSERTMARK insertMark);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVITEM item);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [In, Out] ref ComCtl32.LVITEMINDEX wParam, [In, MarshalAs(UnmanagedType.SysInt)] int lParam);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] int wParam, [In, Out] ComCtl32.LVTILEVIEWINFO tileViewInfo);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.ListViewMessage message, [MarshalAs(UnmanagedType.SysInt)] ComCtl32.ListViewImageList wParam, [In, Out] IntPtr hImageList);
	}
}