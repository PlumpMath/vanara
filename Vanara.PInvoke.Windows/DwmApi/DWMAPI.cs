using System;
using System.Drawing;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class DwmApi
	{
		/// <summary>Flags used by the DWM_BLURBEHIND structure to indicate which of its members contain valid information.</summary>
		[Flags]
		public enum DWM_BLURBEHIND_Mask
		{
			/// <summary>A value for the fEnable member has been specified.</summary>
			DWM_BB_ENABLE = 0X00000001,
			/// <summary>A value for the hRgnBlur member has been specified.</summary>
			DWM_BB_BLURREGION = 0X00000002,
			/// <summary>A value for the fTransitionOnMaximized member has been specified.</summary>
			DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
		}

		/// <summary>Flags used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions to specify window attributes for non-client rendering.</summary>
		public enum DWMWINDOWATTRIBUTE
		{
			/// <summary>Use with DwmGetWindowAttribute. Discovers whether non-client rendering is enabled. The retrieved value is of type BOOL. TRUE if non-client rendering is enabled; otherwise, FALSE.</summary>
			DWMWA_NCRENDERING_ENABLED = 1,
			/// <summary>Use with DwmSetWindowAttribute. Sets the non-client rendering policy. The pvAttribute parameter points to a value from the DWMNCRENDERINGPOLICY enumeration.</summary>
			DWMWA_NCRENDERING_POLICY,
			/// <summary>Use with DwmSetWindowAttribute. Enables or forcibly disables DWM transitions. The pvAttribute parameter points to a value of TRUE to disable transitions or FALSE to enable transitions.</summary>
			DWMWA_TRANSITIONS_FORCEDISABLED,
			/// <summary>Use with DwmSetWindowAttribute. Enables content rendered in the non-client area to be visible on the frame drawn by DWM. The pvAttribute parameter points to a value of TRUE to enable content rendered in the non-client area to be visible on the frame; otherwise, it points to FALSE.</summary>
			DWMWA_ALLOW_NCPAINT,
			/// <summary>Use with DwmGetWindowAttribute. Retrieves the bounds of the caption button area in the window-relative space. The retrieved value is of type RECT.</summary>
			DWMWA_CAPTION_BUTTON_BOUNDS,
			/// <summary>Use with DwmSetWindowAttribute. Specifies whether non-client content is right-to-left (RTL) mirrored. The pvAttribute parameter points to a value of TRUE if the non-client content is right-to-left (RTL) mirrored; otherwise, it points to FALSE.</summary>
			DWMWA_NONCLIENT_RTL_LAYOUT,
			/// <summary>Use with DwmSetWindowAttribute. Forces the window to display an iconic thumbnail or peek representation (a static bitmap), even if a live or snapshot representation of the window is available. This value normally is set during a window's creation and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of TRUE to require a iconic thumbnail or peek representation; otherwise, it points to FALSE.</summary>
			DWMWA_FORCE_ICONIC_REPRESENTATION,
			/// <summary>Use with DwmSetWindowAttribute. Sets how Flip3D treats the window. The pvAttribute parameter points to a value from the DWMFLIP3DWINDOWPOLICY enumeration.</summary>
			DWMWA_FLIP3D_POLICY,
			/// <summary>Use with DwmGetWindowAttribute. Retrieves the extended frame bounds rectangle in screen space. The retrieved value is of type RECT.</summary>
			DWMWA_EXTENDED_FRAME_BOUNDS,
			/// <summary>Use with DwmSetWindowAttribute. The window will provide a bitmap for use by DWM as an iconic thumbnail or peek representation (a static bitmap) for the window. DWMWA_HAS_ICONIC_BITMAP can be specified with DWMWA_FORCE_ICONIC_REPRESENTATION. DWMWA_HAS_ICONIC_BITMAP normally is set during a window's creation and not changed throughout the window's lifetime. Some scenarios, however, might require the value to change over time. The pvAttribute parameter points to a value of TRUE to inform DWM that the window will provide an iconic thumbnail or peek representation; otherwise, it points to FALSE.
			/// <para><c>Windows Vista and earlier:</c> This value is not supported.</para></summary>
			DWMWA_HAS_ICONIC_BITMAP,
			/// <summary>Use with DwmSetWindowAttribute. Do not show peek preview for the window. The peek view shows a full-sized preview of the window when the mouse hovers over the window's thumbnail in the taskbar. If this attribute is set, hovering the mouse pointer over the window's thumbnail dismisses peek (in case another window in the group has a peek preview showing). The pvAttribute parameter points to a value of TRUE to prevent peek functionality or FALSE to allow it.
			/// <para><c>Windows Vista and earlier:</c> This value is not supported.</para></summary>
			DWMWA_DISALLOW_PEEK,
			/// <summary>Use with DwmSetWindowAttribute. Prevents a window from fading to a glass sheet when peek is invoked. The pvAttribute parameter points to a value of TRUE to prevent the window from fading during another window's peek or FALSE for normal behavior.
			/// <para><c>Windows Vista and earlier:</c> This value is not supported.</para></summary>
			DWMWA_EXCLUDED_FROM_PEEK,
			/// <summary>Use with DwmGetWindowAttribute. Cloaks the window such that it is not visible to the user. The window is still composed by DWM.
			/// <para><c>Using with DirectComposition:</c> Use the DWMWA_CLOAK flag to cloak the layered child window when animating a representation of the window's content via a DirectComposition visual which has been associated with the layered child window. For more details on this usage case, see How to How to animate the bitmap of a layered child window.</para>
			/// <para><c>Windows 7 and earlier:</c> This value is not supported.</para></summary>
			DWMWA_CLOAK,
			/// <summary>Use with DwmGetWindowAttribute. If the window is cloaked, provides one of the following values explaining why:
			/// <list type="table">
			/// <listheader><term>Name (Value)</term><definition>Meaning</definition></listheader>
			/// <item><term>DWM_CLOAKED_APP 0x0000001</term><definition>The window was cloaked by its owner application.</definition></item>
			/// <item><term>DWM_CLOAKED_SHELL 0x0000002</term><definition>The window was cloaked by the Shell.</definition></item>
			/// <item><term>DWM_CLOAKED_INHERITED 0x0000004</term><definition>The cloak value was inherited from its owner window.</definition></item>
			/// </list>
			/// <para><c>Windows 7 and earlier:</c> This value is not supported.</para></summary>
			DWMWA_CLOAKED,
			/// <summary>Use with DwmSetWindowAttribute. Freeze the window's thumbnail image with its current visuals. Do no further live updates on the thumbnail image to match the window's contents.
			/// <para><c>Windows 7 and earlier:</c> This value is not supported.</para></summary>
			DWMWA_FREEZE_REPRESENTATION,
		}

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmEnableBlurBehindWindow(IntPtr hWnd, ref DWM_BLURBEHIND pDwmBlurbehind);

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmEnableComposition(int compositionAction);

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

		//[DllImport(nameof(DwmApi), ExactSpelling = true)]
		//public static extern HRESULT DwmGetColorizationColor(out uint ColorizationColor, [MarshalAs(UnmanagedType.Bool)]out bool ColorizationOpaqueBlend);

		[DllImport(nameof(DwmApi), EntryPoint = "#127")]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmGetColorizationParameters(ref DWM_COLORIZATION_PARAMS parameters);

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, IntPtr pvAttribute, int cbAttribute);

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmIsCompositionEnabled(ref int pfEnabled);

		[DllImport(nameof(DwmApi), EntryPoint = "#131")]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmSetColorizationParameters(ref DWM_COLORIZATION_PARAMS parameters, uint unk);

		[DllImport(nameof(DwmApi), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern HRESULT DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, [In] IntPtr pvAttribute, int cbAttribute);

		[StructLayout(LayoutKind.Sequential)]
		public struct DWM_BLURBEHIND
		{
			public DWM_BLURBEHIND_Mask dwFlags;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fEnable;
			public IntPtr hRgnBlur;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fTransitionOnMaximized;

			public DWM_BLURBEHIND(bool enabled)
			{
				fEnable = enabled;
				hRgnBlur = IntPtr.Zero;
				fTransitionOnMaximized = false;
				dwFlags = DWM_BLURBEHIND_Mask.DWM_BB_ENABLE;
			}

			public System.Drawing.Region Region => System.Drawing.Region.FromHrgn(hRgnBlur);

			public bool TransitionOnMaximized
			{
				get { return fTransitionOnMaximized; }
				set
				{
					fTransitionOnMaximized = value;
					dwFlags |= DWM_BLURBEHIND_Mask.DWM_BB_TRANSITIONONMAXIMIZED;
				}
			}

			public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
			{
				hRgnBlur = region.GetHrgn(graphics);
				dwFlags |= DWM_BLURBEHIND_Mask.DWM_BB_BLURREGION;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DWM_COLORIZATION_PARAMS
		{
			public uint clrColor;
			public uint clrAfterGlow;
			public uint nIntensity;
			public uint clrAfterGlowBalance;
			public uint clrBlurBalance;
			public uint clrGlassReflectionIntensity;
			[MarshalAs(UnmanagedType.Bool)]
			public bool fOpaque;
		}

		/// <summary>Margins structure for theme related functions.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct MARGINS
		{
			public int cxLeftWidth;
			public int cxRightWidth;
			public int cyTopHeight;
			public int cyBottomHeight;

			public static readonly MARGINS Empty = new MARGINS(0);
			public static readonly MARGINS Infinite = new MARGINS(-1);

			public MARGINS(int left, int right, int top, int bottom)
			{
				cxLeftWidth = left;
				cxRightWidth = right;
				cyTopHeight = top;
				cyBottomHeight = bottom;
			}

			public MARGINS(int allMargins)
			{
				cxLeftWidth = cxRightWidth = cyTopHeight = cyBottomHeight = allMargins;
			}

			public int Left { get { return cxLeftWidth; } set { cxLeftWidth = value; } }

			public int Right { get { return cxRightWidth; } set { cxRightWidth = value; } }

			public int Top { get { return cyTopHeight; } set { cyTopHeight = value; } }

			public int Bottom { get { return cyBottomHeight; } set { cyBottomHeight = value; } }

			public static bool operator !=(MARGINS m1, MARGINS m2) => !m1.Equals(m2);

			public static bool operator ==(MARGINS m1, MARGINS m2) => m1.Equals(m2);

			public override bool Equals(object obj)
			{
				if (obj is MARGINS)
				{
					var m2 = (MARGINS)obj;
					return cxLeftWidth == m2.cxLeftWidth && cxRightWidth == m2.cxRightWidth && cyTopHeight == m2.cyTopHeight && cyBottomHeight == m2.cyBottomHeight;
				}
				return base.Equals(obj);
			}

			public override int GetHashCode() => cxLeftWidth ^ RotateLeft(cyTopHeight, 8) ^ RotateLeft(cxRightWidth, 0x10) ^ RotateLeft(cyBottomHeight, 0x18);

			public override string ToString() => $"{{Left={cxLeftWidth},Right={cxRightWidth},Top={cyTopHeight},Bottom={cyBottomHeight}}}";

			private static int RotateLeft(int value, int nBits)
			{
				nBits = nBits % 0x20;
				return (value << nBits) | (value >> (0x20 - nBits));
			}
		}
	}
}