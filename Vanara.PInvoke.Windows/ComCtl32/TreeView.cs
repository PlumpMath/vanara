using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Vanara.Extensions;
using HTREEITEM = System.IntPtr;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const int I_CHILDRENAUTO = -2;
		public const int I_CHILDRENCALLBACK = -1;
		public const int TV_FIRST = 0x1100;
		/// <summary>TreeView's custom draw return meaning don't draw images.  valid on CDRF_NOTIFYITEMPREPAINT</summary>
		public const int TVCDRF_NOIMAGES = 0x00010000;

		public const int TVN_FIRST = -400;
		public delegate int PFNTVCOMPARE(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

		/// <remarks>Values are unconfirmed</remarks>
		public enum AsyncDrawRetFlags
		{
			ADRF_DRAWSYNC = 0, // 'draw synchronously (act same as if LVS_EX_DRAWIMAGEASYNC weren't set)
			ADRF_DRAWNOTHING = 1, // 'draw nothing (LVADPART_ITEM, LVADPART_GROUP)
			ADRF_DRAWFALLBACK = 2, // 'draw fallback text (LVADPART_IMAGETITLE)
			ADRF_DRAWIMAGE = 3, //'draw image returned in iRetImageIndex instead
		}

		public enum TreeViewActionFlag
		{
			TVGN_ROOT = 0x0000,
			TVGN_NEXT = 0x0001,
			TVGN_PREVIOUS = 0x0002,
			TVGN_PARENT = 0x0003,
			TVGN_CHILD = 0x0004,
			TVGN_FIRSTVISIBLE = 0x0005,
			TVGN_NEXTVISIBLE = 0x0006,
			TVGN_PREVIOUSVISIBLE = 0x0007,
			TVGN_DROPHILITE = 0x0008,
			TVGN_CARET = 0x0009,
			TVGN_LASTVISIBLE = 0x000A,
			TVGN_NEXTSELECTED = 0x000B,
			TVSI_NOSINGLEEXPAND = 0x8000
		}

		[Flags]
		public enum TreeViewExpandFlags
		{
			TVE_COLLAPSE = 0x0001,
			TVE_EXPAND = 0x0002,
			TVE_TOGGLE = 0x0003,
			TVE_EXPANDPARTIAL = 0x4000,
			TVE_COLLAPSERESET = 0x8000,
		}

		[Flags]
		public enum TreeViewHitTestFlags
		{
			TVHT_NOWHERE = 0x0001,
			TVHT_ONITEMICON = 0x0002,
			TVHT_ONITEMLABEL = 0x0004,
			TVHT_ONITEM = TVHT_ONITEMICON | TVHT_ONITEMLABEL | TVHT_ONITEMSTATEICON,
			TVHT_ONITEMINDENT = 0x0008,
			TVHT_ONITEMBUTTON = 0x0010,
			TVHT_ONITEMRIGHT = 0x0020,
			TVHT_ONITEMSTATEICON = 0x0040,
			TVHT_ABOVE = 0x0100,
			TVHT_BELOW = 0x0200,
			TVHT_TORIGHT = 0x0400,
			TVHT_TOLEFT = 0x0800,
		}

		[Flags]
		public enum TreeViewItemMask
		{
			TVIF_TEXT          = 0x0001,
			TVIF_IMAGE         = 0x0002,
			TVIF_PARAM         = 0x0004,
			TVIF_STATE         = 0x0008,
			TVIF_HANDLE        = 0x0010,
			TVIF_SELECTEDIMAGE = 0x0020,
			TVIF_CHILDREN      = 0x0040,
			TVIF_INTEGRAL      = 0x0080,
			TVIF_STATEEX       = 0x0100,
			TVIF_EXPANDEDIMAGE = 0x0200,
			TVIF_DI_SETITEM	   = 0x1000,
		}

		[Flags]
		public enum TreeViewItemStates
		{
			TVIS_SELECTED       = 0x0002,
			TVIS_CUT            = 0x0004,
			TVIS_DROPHILITED    = 0x0008,
			TVIS_BOLD           = 0x0010,
			TVIS_EXPANDED       = 0x0020,
			TVIS_EXPANDEDONCE   = 0x0040,
			TVIS_EXPANDPARTIAL  = 0x0080,
			TVIS_OVERLAYMASK    = 0x0F00,
			TVIS_STATEIMAGEMASK = 0xF000,
			TVIS_USERMASK       = 0xF000,
		}

		[Flags]
		public enum TreeViewItemStatesEx
		{
			TVIS_EX_FLAT     = 0x0001,
			TVIS_EX_DISABLED = 0x0002,
			//TVIS_EX_HWND     = 0x0004,
		}

		public enum TreeViewMessage
		{
			TVM_DELETEITEM = TV_FIRST + 1,
			TVM_EXPAND = TV_FIRST + 2,
			TVM_GETITEMRECT = TV_FIRST + 4,
			TVM_GETCOUNT = TV_FIRST + 5,
			TVM_GETINDENT = TV_FIRST + 6,
			TVM_SETINDENT = TV_FIRST + 7,
			TVM_GETIMAGELIST = TV_FIRST + 8,
			TVM_SETIMAGELIST = TV_FIRST + 9,
			TVM_GETNEXTITEM = TV_FIRST + 10,
			TVM_SELECTITEM = TV_FIRST + 11,
			TVM_GETEDITCONTROL = TV_FIRST + 15,
			TVM_GETVISIBLECOUNT = TV_FIRST + 16,
			TVM_HITTEST = TV_FIRST + 17,
			TVM_CREATEDRAGIMAGE = TV_FIRST + 18,
			TVM_SORTCHILDREN = TV_FIRST + 19,
			TVM_ENSUREVISIBLE = TV_FIRST + 20,
			TVM_SORTCHILDRENCB = TV_FIRST + 21,
			TVM_ENDEDITLABELNOW = TV_FIRST + 22,
			TVM_SETTOOLTIPS = TV_FIRST + 24,
			TVM_GETTOOLTIPS = TV_FIRST + 25,
			TVM_SETINSERTMARK = TV_FIRST + 26,
			TVM_SETUNICODEFORMAT = CommonControlMessage.CCM_SETUNICODEFORMAT,
			TVM_GETUNICODEFORMAT = CommonControlMessage.CCM_GETUNICODEFORMAT,
			TVM_SETITEMHEIGHT = TV_FIRST + 27,
			TVM_GETITEMHEIGHT = TV_FIRST + 28,
			TVM_SETBKCOLOR = TV_FIRST + 29,
			TVM_SETTEXTCOLOR = TV_FIRST + 30,
			TVM_GETBKCOLOR = TV_FIRST + 31,
			TVM_GETTEXTCOLOR = TV_FIRST + 32,
			TVM_SETSCROLLTIME = TV_FIRST + 33,
			TVM_GETSCROLLTIME = TV_FIRST + 34,
			TVM_SETINSERTMARKCOLOR = TV_FIRST + 37,
			TVM_GETINSERTMARKCOLOR = TV_FIRST + 38,
			TVM_SETBORDER = TV_FIRST + 35,
			TVM_GETITEMSTATE = TV_FIRST + 39,
			TVM_SETLINECOLOR = TV_FIRST + 40,
			TVM_GETLINECOLOR = TV_FIRST + 41,
			TVM_MAPACCIDTOHTREEITEM = TV_FIRST + 42,
			TVM_MAPHTREEITEMTOACCID = TV_FIRST + 43,
			TVM_SETEXTENDEDSTYLE = TV_FIRST + 44,
			TVM_GETEXTENDEDSTYLE = TV_FIRST + 45,
			TVM_INSERTITEM = TV_FIRST + 50,
			TVM_SETAUTOSCROLLINFO = TV_FIRST + 59,
			TVM_SETHOT = TV_FIRST + 58,
			TVM_GETITEM = TV_FIRST + 62,
			TVM_SETITEM = TV_FIRST + 63,
			TVM_GETISEARCHSTRING = TV_FIRST + 64,
			TVM_EDITLABEL = TV_FIRST + 65,
			TVM_GETSELECTEDCOUNT = TV_FIRST + 70,
			TVM_SHOWINFOTIP = TV_FIRST + 71,
			TVM_GETITEMPARTRECT = TV_FIRST + 72,
		}

		public enum TreeViewNotification
		{
			TVN_ASYNCDRAW = TVN_FIRST - 20,
			TVN_BEGINDRAG = TVN_FIRST - 56,
			TVN_BEGINLABELEDIT = TVN_FIRST - 59,
			TVN_BEGINRDRAG = TVN_FIRST - 57,
			TVN_DELETEITEM = TVN_FIRST - 58,
			TVN_ENDLABELEDIT = TVN_FIRST - 60,
			TVN_GETDISPINFO = TVN_FIRST - 52,
			TVN_GETINFOTIP = TVN_FIRST - 14,
			TVN_ITEMCHANGED = TVN_FIRST - 19,
			TVN_ITEMCHANGING = TVN_FIRST - 17,
			TVN_ITEMEXPANDED = TVN_FIRST - 55,
			TVN_ITEMEXPANDING = TVN_FIRST - 54,
			TVN_KEYDOWN = TVN_FIRST - 12,
			TVN_SELCHANGED = TVN_FIRST - 51,
			TVN_SELCHANGING = TVN_FIRST - 50,
			TVN_SETDISPINFO = TVN_FIRST - 53,
			TVN_SINGLEEXPAND = TVN_FIRST - 15,
		}

		public enum TreeViewNotificationReturnBehavior
		{
			TVNRET_DEFAULT = 0,
			TVNRET_SKIPOLD = 1,
			TVNRET_SKIPNEW = 2,
		}

		public enum TreeViewSelChangedCause
		{
			TVC_UNKNOWN    = 0x0000,
			TVC_BYMOUSE    = 0x0001,
			TVC_BYKEYBOARD = 0x0002,
		}

		[Flags]
		public enum TreeViewSetBorderFlags
		{
			TVSBF_XBORDER = 0x00000001,
			TVSBF_YBORDER = 0x00000002,
		}

		public enum TreeViewSetImageListType
		{
			TVSIL_NORMAL = 0,
			TVSIL_STATE = 2
		}

		public enum TreeViewState
		{
			PBST_NORMAL = 0x0001,
			PBST_ERROR = 0x0002,
			PBST_PAUSED = 0x0003,
		}

		[Flags]
		public enum TreeViewStyle
		{
			TVS_HASBUTTONS = 0x0001,
			TVS_HASLINES = 0x0002,
			TVS_LINESATROOT = 0x0004,
			TVS_EDITLABELS = 0x0008,
			TVS_DISABLEDRAGDROP = 0x0010,
			TVS_SHOWSELALWAYS = 0x0020,
			TVS_RTLREADING = 0x0040,
			TVS_NOTOOLTIPS = 0x0080,
			TVS_CHECKBOXES = 0x0100,
			TVS_TRACKSELECT = 0x0200,
			TVS_SINGLEEXPAND = 0x0400,
			TVS_INFOTIP = 0x0800,
			TVS_FULLROWSELECT = 0x1000,
			TVS_NOSCROLL = 0x2000,
			TVS_NONEVENHEIGHT = 0x4000,
			TVS_NOHSCROLL = 0x8000,  // TVS_NOSCROLL overrides this
		}

		[Flags]
		public enum TreeViewStyleEx
		{
			TVS_EX_NOSINGLECOLLAPSE = 0x0001,
			TVS_EX_MULTISELECT = 0x0002,
			TVS_EX_DOUBLEBUFFER = 0x0004,
			TVS_EX_NOINDENTSTATE = 0x0008,
			TVS_EX_RICHTOOLTIP = 0x0010,
			TVS_EX_AUTOHSCROLL = 0x0020,
			TVS_EX_FADEINOUTEXPANDOS = 0x0040,
			TVS_EX_PARTIALCHECKBOXES = 0x0080,
			TVS_EX_EXCLUSIONCHECKBOXES = 0x0100,
			TVS_EX_DIMMEDCHECKBOXES = 0x0200,
			TVS_EX_DRAWIMAGEASYNC = 0x0400,
		}

		public enum TVITEMPART
		{
			TVGIPR_BUTTON = 0x0001,
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTREEVIEW
		{
			public User32.NMHDR hdr;
			public int action;
			public TVITEM itemOld;
			public TVITEM itemNew;
			public Point ptDrag;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVASYNCDRAW
		{
			public User32.NMHDR hdr;
			public IMAGELISTDRAWPARAMS pimldp;    // the draw that failed
			public HRESULT hr;                   // why it failed
			public HTREEITEM hItem;                // item that failed to draw icon
			public IntPtr lParam;               // its data
			public AsyncDrawRetFlags dwRetFlags;           // What listview should do on return
			public int iRetImageIndex;       // used if ADRF_DRAWIMAGE is returned
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVCUSTOMDRAW
		{
			public NMCUSTOMDRAW nmcd;
			public int clrText;
			public int clrTextBk;
			public int iLevel;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVDISPINFO
		{
			public User32.NMHDR hdr;
			public TVITEM item;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVDISPINFOEX
		{
			public User32.NMHDR hdr;
			public TVITEMEX item;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVGETINFOTIP
		{
			public User32.NMHDR hdr;
			public IntPtr pszText;
			public int cchTextMax;
			public HTREEITEM hItem;
			public IntPtr lParam;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct NMTVKEYDOWN
		{
			public User32.NMHDR hdr;
			public ushort wVKey;
			public uint flags;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public sealed class TVGETITEMPARTRECTINFO : IDisposable
		{
			public HTREEITEM hti;
			public IntPtr prc;
			public TVITEMPART partID;

			public TVGETITEMPARTRECTINFO(HTREEITEM hTreeNode)
			{
				hti = hTreeNode;
				prc = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(RECT)));
				partID = TVITEMPART.TVGIPR_BUTTON;
			}

			public Rectangle Bounds => prc.ToStructure<RECT>();

			void IDisposable.Dispose() { Marshal.FreeCoTaskMem(prc); }
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TVHITTESTINFO
		{
			public Point pt;
			public TreeViewHitTestFlags flags;
			public HTREEITEM hItem;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TVINSERTSTRUCT
		{
			public HTREEITEM hParent;
			public HTREEITEM hInsertAfter;
			public TVITEMEX itemex;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TVITEMCHANGE
		{
			public User32.NMHDR hdr;
			public TreeViewItemMask uChanged;
			public HTREEITEM hItem;
			public TreeViewItemStates uStateNew;
			public TreeViewItemStates uStateOld;
			public IntPtr lParam;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TVITEM
		{
			public TreeViewItemMask mask;
			public HTREEITEM hItem;
			public uint state;
			public TreeViewItemStates stateMask;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;
			public int iSelectedImage;
			public int cChildren;
			public IntPtr lParam;

			public bool Bold
			{
				get { return GetState(TreeViewItemStates.TVIS_BOLD); }
				set { SetState(TreeViewItemStates.TVIS_BOLD, value); }
			}

			public bool Expanded
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDED); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDED, value); }
			}

			public bool ExpandedOnce
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDEDONCE); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDEDONCE, value); }
			}

			public bool ExpandedPartial
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDPARTIAL); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDPARTIAL, value); }
			}

			public uint OverlayImageIndex
			{
				get { return (state & 0x00000F00) >> 8; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(OverlayImageIndex), "Overlay image index must be between 0 and 15");
					mask |= TreeViewItemMask.TVIF_STATE;
					stateMask |= TreeViewItemStates.TVIS_OVERLAYMASK;
					state = (value << 8) | (state & 0xFFFFF0FF);
				}
			}

			public bool Selected
			{
				get { return GetState(TreeViewItemStates.TVIS_SELECTED); }
				set { SetState(TreeViewItemStates.TVIS_SELECTED, value); }
			}

			public bool SelectedForCut
			{
				get { return GetState(TreeViewItemStates.TVIS_CUT); }
				set { SetState(TreeViewItemStates.TVIS_CUT, value); }
			}

			public bool SelectedForDragDrop
			{
				get { return GetState(TreeViewItemStates.TVIS_DROPHILITED); }
				set { SetState(TreeViewItemStates.TVIS_DROPHILITED, value); }
			}

			public TreeViewItemStates State => (TreeViewItemStates)(state & 0x000000FF);

			public uint StateImageIndex
			{
				get { return (state & 0x0000F000) >> 12; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(StateImageIndex), "State image index must be between 0 and 15");
					mask |= TreeViewItemMask.TVIF_STATE;
					stateMask |= TreeViewItemStates.TVIS_STATEIMAGEMASK;
					state = (value << 12) | (state & 0xFFFF0FFF);
				}
			}

			public string Text
			{
				get { return pszText == LPSTR_TEXTCALLBACK ? null : Marshal.PtrToStringUni(pszText); }
			}

			public bool UseTextCallback
			{
				get { return pszText == LPSTR_TEXTCALLBACK; }
				set
				{
					if (value)
						pszText = LPSTR_TEXTCALLBACK;
					mask |= TreeViewItemMask.TVIF_TEXT;
				}
			}

			public bool GetState(TreeViewItemStates itemState) => State.IsFlagSet(itemState);

			public void SetState(TreeViewItemStates itemState, bool on = true)
			{
				mask |= TreeViewItemMask.TVIF_STATE;
				stateMask |= itemState;
				var tempState = State;
				EnumExtensions.SetFlags(ref tempState, itemState, on);
				state = (uint)tempState | (state & 0xFFFFFF00);
			}

			public void SetText(IntPtr managedStringPtr, int stringLen)
			{
				pszText = managedStringPtr;
				cchTextMax = stringLen;
				mask |= TreeViewItemMask.TVIF_TEXT;
			}

			public override string ToString() => $"TVITEM: pszText={Text}; iImage={iImage}; iSelectedImage={iSelectedImage}; state={state}; cChildren={cChildren}";
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TVITEMEX
		{
			public TreeViewItemMask mask;
			public HTREEITEM hItem;
			public uint state;
			public TreeViewItemStates stateMask;
			public IntPtr pszText;
			public int cchTextMax;
			public int iImage;
			public int iSelectedImage;
			public int cChildren;
			public IntPtr lParam;
			public int iIntegral;
			public TreeViewItemStatesEx uStateEx;
			public IntPtr hwnd;
			public int iExpandedImage;
			public int iReserved;

			public bool Bold
			{
				get { return GetState(TreeViewItemStates.TVIS_BOLD); }
				set { SetState(TreeViewItemStates.TVIS_BOLD, value); }
			}

			public bool Expanded
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDED); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDED, value); }
			}

			public bool ExpandedOnce
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDEDONCE); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDEDONCE, value); }
			}

			public bool ExpandedPartial
			{
				get { return GetState(TreeViewItemStates.TVIS_EXPANDPARTIAL); }
				set { SetState(TreeViewItemStates.TVIS_EXPANDPARTIAL, value); }
			}

			public uint OverlayImageIndex
			{
				get { return (state & 0x00000F00) >> 8; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(OverlayImageIndex), "Overlay image index must be between 0 and 15");
					mask |= TreeViewItemMask.TVIF_STATE;
					stateMask |= TreeViewItemStates.TVIS_OVERLAYMASK;
					state = (value << 8) | (state & 0xFFFFF0FF);
				}
			}

			public bool Selected
			{
				get { return GetState(TreeViewItemStates.TVIS_SELECTED); }
				set { SetState(TreeViewItemStates.TVIS_SELECTED, value); }
			}

			public bool SelectedForCut
			{
				get { return GetState(TreeViewItemStates.TVIS_CUT); }
				set { SetState(TreeViewItemStates.TVIS_CUT, value); }
			}

			public bool SelectedForDragDrop
			{
				get { return GetState(TreeViewItemStates.TVIS_DROPHILITED); }
				set { SetState(TreeViewItemStates.TVIS_DROPHILITED, value); }
			}

			public TreeViewItemStates State => (TreeViewItemStates)(state & 0x000000FF);

			public uint StateImageIndex
			{
				get { return (state & 0x0000F000) >> 12; }
				set
				{
					if (value > 15)
						throw new ArgumentOutOfRangeException(nameof(StateImageIndex), "State image index must be between 0 and 15");
					mask |= TreeViewItemMask.TVIF_STATE;
					stateMask |= TreeViewItemStates.TVIS_STATEIMAGEMASK;
					state = (value << 12) | (state & 0xFFFF0FFF);
				}
			}

			public string Text
			{
				get { return pszText == LPSTR_TEXTCALLBACK ? null : Marshal.PtrToStringUni(pszText); }
			}

			public bool UseTextCallback
			{
				get { return pszText == LPSTR_TEXTCALLBACK; }
				set
				{
					if (value)
						pszText = LPSTR_TEXTCALLBACK;
					mask |= TreeViewItemMask.TVIF_TEXT;
				}
			}

			public bool GetState(TreeViewItemStates itemState) => State.IsFlagSet(itemState);

			public void SetState(TreeViewItemStates itemState, bool on = true)
			{
				mask |= TreeViewItemMask.TVIF_STATE;
				stateMask |= itemState;
				var tempState = State;
				EnumExtensions.SetFlags(ref tempState, itemState, on);
				state = (uint)tempState | (state & 0xFFFFFF00);
			}

			public void SetText(IntPtr managedStringPtr, int stringLen)
			{
				pszText = managedStringPtr;
				cchTextMax = stringLen;
				mask |= TreeViewItemMask.TVIF_TEXT;
			}

			public override string ToString() => $"TVITEM: pszText={Text}; iImage={iImage}; iSelectedImage={iSelectedImage}; state={state}; iExpandedImage={iExpandedImage}; iIntegral={iIntegral}; cChildren={cChildren}";
		}

		/// <summary>
		/// Contains information used to sort child items in a tree-view control. This structure is used with the TVM_SORTCHILDRENCB message. This structure is
		/// identical to the TV_SORTCB structure, but it has been renamed to follow current naming conventions.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TVSORTCB
		{
			/// <summary>Handle to the parent item.</summary>
			public HTREEITEM hParent;
			/// <summary>
			/// Address of an application-defined callback function, which is called during a sort operation each time the relative order of two list items needs
			/// to be compared.
			/// </summary>
			public PFNTVCOMPARE lpfnCompare;
			/// <summary>Application-defined value that gets passed as the lParamSort argument in the callback function specified in lpfnCompare.</summary>
			public IntPtr lParam;
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
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TreeViewMessage Msg, int wParam, ComCtl32.TVGETITEMPARTRECTINFO item);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="hitTestInfo">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TreeViewMessage Msg, [MarshalAs(UnmanagedType.Bool)] bool wParam, ref ComCtl32.TVHITTESTINFO hitTestInfo);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="item">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TreeViewMessage Msg, int wParam, ref ComCtl32.TVINSERTSTRUCT item);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="sortInfo">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TreeViewMessage Msg, int wParam, ref ComCtl32.TVSORTCB sortInfo);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="item">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		public static extern IntPtr SendMessage(HandleRef hWnd, ComCtl32.TreeViewMessage Msg, int wParam, ref ComCtl32.TVITEMEX item);
	}
}