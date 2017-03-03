using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.Gdi32;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public enum IMAGELISTCOPYFLAG
		{
			/// <summary>The source image is copied to the destination image's index. This operation results in multiple instances of a given image.</summary>
			ILCF_MOVE = 0,
			/// <summary>The source and destination images exchange positions within the image list.</summary>
			ILCF_SWAP = 1
		}

		/// <summary>Passed to the IImageList::Draw method in the fStyle member of IMAGELISTDRAWPARAMS.</summary>
		[Flags]
		public enum IMAGELISTDRAWFLAGS : uint
		{
			/// <summary>
			/// Draws the image using the background color for the image list. If the background color is the CLR_NONE value, the image is drawn transparently
			/// using the mask.
			/// </summary>
			ILD_NORMAL = 0X00000000,
			/// <summary>
			/// Draws the image transparently using the mask, regardless of the background color. This value has no effect if the image list does not contain a mask.
			/// </summary>
			ILD_TRANSPARENT = 0x00000001,
			/// <summary>
			/// Draws the image, blending 25 percent with the blend color specified by rgbFg. This value has no effect if the image list does not contain a mask.
			/// </summary>
			ILD_BLEND25 = 0X00000002,
			/// <summary>Same as ILD_BLEND25</summary>
			ILD_FOCUS = ILD_BLEND25,
			/// <summary>
			/// Draws the image, blending 50 percent with the blend color specified by rgbFg. This value has no effect if the image list does not contain a mask.
			/// </summary>
			ILD_BLEND50 = 0X00000004,
			/// <summary>Same as ILD_BLEND50</summary>
			ILD_SELECTED = ILD_BLEND50,
			/// <summary>Same as ILD_BLEND50</summary>
			ILD_BLEND = ILD_BLEND50,
			/// <summary>Draws the mask.</summary>
			ILD_MASK = 0X00000010,
			/// <summary>If the overlay does not require a mask to be drawn, set this flag.</summary>
			ILD_IMAGE = 0X00000020,
			/// <summary>Draws the image using the raster operation code specified by the dwRop member.</summary>
			ILD_ROP = 0X00000040,
			/// <summary>To extract the overlay image from the fStyle member, use the logical AND to combine fStyle with the ILD_OVERLAYMASK value.</summary>
			ILD_OVERLAYMASK = 0x00000F00,
			/// <summary>Preserves the alpha channel in the destination.</summary>
			ILD_PRESERVEALPHA = 0x00001000,
			/// <summary>Causes the image to be scaled to cx, cy instead of being clipped.</summary>
			ILD_SCALE = 0X00002000,
			/// <summary>Scales the image to the current dpi of the display.</summary>
			ILD_DPISCALE = 0X00004000,
			/// <summary>
			/// <c>Windows Vista and later.</c> Draw the image if it is available in the cache. Do not extract it automatically. The called draw method returns
			/// E_PENDING to the calling component, which should then take an alternative action, such as, provide another image and queue a background task to
			/// force the image to be loaded via ForceImagePresent using the ILFIP_ALWAYS flag. The ILD_ASYNC flag then prevents the extraction operation from
			/// blocking the current thread and is especially important if a draw method is called from the user interface (UI) thread.
			/// </summary>
			ILD_ASYNC = 0X00008000
		}

		public enum IMAGELISTITEMFLAG
		{
			ILIF_ALPHA = 1,
			ILIF_LOWQUALITY = 2
		}

		/// <summary>The following flags are passed to the IImageList::Draw method in the fState member of IMAGELISTDRAWPARAMS.</summary>
		[Flags]
		public enum IMAGELISTSTATEFLAGS
		{
			/// <summary>The image state is not modified.</summary>
			ILS_NORMAL = 0x00000000,
			/// <summary>Not supported.</summary>
			ILS_GLOW = 0x00000001,
			/// <summary>Not supported.</summary>
			ILS_SHADOW = 0x00000002,
			/// <summary>Reduces the color saturation of the icon to grayscale. This only affects 32bpp images.</summary>
			ILS_SATURATE = 0x00000004,
			/// <summary>
			/// Alpha blends the icon. Alpha blending controls the transparency level of an icon, according to the value of its alpha channel. The value of the
			/// alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255, with 0 being completely
			/// transparent, and 255 being completely opaque.
			/// </summary>
			ILS_ALPHA = 0x00000008,
		}

		public const uint CLR_DEFAULT = 0xFF000000;
		public const uint CLR_NONE = 0xFFFFFFFF;

		/// <summary>
		/// Draws an image list item In the specified device context. The function uses the specified drawing style and blends the
		/// image with the specified color.
		/// </summary>
		/// <param name="himl">A handle to the image list</param>
		/// <param name="i">The index of the image to draw.</param>
		/// <param name="hdcDst">A handle to the destination device context.</param>
		/// <param name="x">The x-coordinate at which to draw within the specified device context.</param>
		/// <param name="y">The y-coordinate at which to draw within the specified device context.</param>
		/// <param name="dx">
		/// The width of the portion of the image to draw relative to the upper-left corner of the image. If dx and dy are zero,
		/// the function draws the entire
		/// image. The function does not ensure that the parameters are valid.
		/// </param>
		/// <param name="dy">
		/// The height of the portion of the image to draw, relative to the upper-left corner of the image. If dx and dy are zero,
		/// the function draws the entire
		/// image. The function does not ensure that the parameters are valid.
		/// </param>
		/// <param name="rgbBk">
		/// The background color of the image. This parameter can be an application-defined RGB value or one of the following
		/// values: <see cref="CLR_NONE" /> or
		/// <see cref="CLR_DEFAULT" />.
		/// </param>
		/// <param name="rgbFg">
		/// The foreground color of the image. This parameter can be an application-defined RGB value or one of the following
		/// values: <see cref="CLR_NONE" /> or
		/// <see cref="CLR_DEFAULT" />.
		/// </param>
		/// <param name="fStyle">
		/// The drawing style and, optionally, the overlay image. For information about specifying an overlay image index, see the
		/// comments section at the end of
		/// this topic. This parameter can be a combination of an overlay image index and one or more of the
		/// <see cref="IMAGELISTDRAWFLAGS" /> values.
		/// </param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		[DllImport(nameof(ComCtl32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImageList_DrawEx(HandleRef himl, int i, Gdi32.SafeDCHandle hdcDst, int x, int y, int dx, int dy,
			uint rgbBk, uint rgbFg, IMAGELISTDRAWFLAGS fStyle);

		/// <summary>
		/// Creates an icon from an image and mask in an image list.
		/// </summary>
		/// <param name="himl">A handle to the image list.</param>
		/// <param name="i">An index of the image.</param>
		/// <param name="flags">A combination of flags that specify the drawing style.</param>
		/// <returns>Returns the handle to the icon if successful, or NULL otherwise.</returns>
		[DllImport(nameof(ComCtl32), ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, IMAGELISTDRAWFLAGS flags);

		/// <summary>
		/// Adds a specified image to the list of images to be used as overlay masks. An image list can have up to four overlay
		/// masks In version 4.70 and earlier
		/// and up to 15 In version 4.71. The function assigns an overlay mask index to the specified image.
		/// </summary>
		/// <param name="himl">A handle to the image list</param>
		/// <param name="iImage">
		/// The zero-based index of an image In the himl image list. This index identifies the image to use as
		/// an overlay mask.
		/// </param>
		/// <param name="iOverlay">The one-based index of the overlay mask.</param>
		/// <returns>Returns nonzero if successful, or zero otherwise.</returns>
		[DllImport(nameof(ComCtl32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImageList_SetOverlayImage(HandleRef himl, int iImage, int iOverlay);

		[ComImport, Guid("46EB5926-582E-4017-9FDF-E8998DAA0950"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IImageList
		{
			int Add(IntPtr hbmImage, [Optional] IntPtr hbmMask);
			int ReplaceIcon(int i, IntPtr hicon);
			void SetOverlayImage(int iImage, int iOverlay);
			void Replace(int i, IntPtr hbmImage, [Optional] IntPtr hbmMask);
			int AddMasked(IntPtr hbmImage, int crMask);
			void Draw(IMAGELISTDRAWPARAMS pimldp);
			void Remove(int i);
			IntPtr GetIcon(int i, IMAGELISTDRAWFLAGS flags);
			IMAGEINFO GetImageInfo(int i);
			void Copy(int iDst, IImageList punkSrc, int iSrc, IMAGELISTCOPYFLAG uFlags);
			IImageList Merge(int i1, IImageList punk2, int i2, int dx, int dy, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
			IImageList Clone([MarshalAs(UnmanagedType.LPStruct)] Guid riid);
			RECT GetImageRect(int i);
			void GetIconSize(out int cx, out int cy);
			void SetIconSize(int cx, int cy);
			int GetImageCount();
			void SetImageCount(int uNewCount);
			void SetBkColor(int clrBk, ref int pclr);
			int GetBkColor();
			void BeginDrag(int iTrack, int dxHotspot, int dyHotspot);
			void EndDrag();
			void DragEnter(HandleRef hwndLock, int x, int y);
			void DragLeave(HandleRef hwndLock);
			void DragMove(int x, int y);
			void SetDragCursorImage(IImageList punk, int iDrag, int dxHotspot, int dyHotspot);
			void DragShowNolock([MarshalAs(UnmanagedType.Bool)] bool fShow);
			void GetDragImage(out Point ppt, out Point pptHotspot, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IImageList ppv);
			IMAGELISTITEMFLAG GetItemFlags(int i);
			int GetOverlayImage(int iOverlay);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct IMAGEINFO
		{
			public IntPtr hbmImage;
			public IntPtr hbmMask;
			public int Unused1;
			public int Unused2;
			public RECT rcImage;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class IMAGELISTDRAWPARAMS
		{
			public int cbSize;
			public IntPtr himl;
			public int i;
			public IntPtr hdcDst;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public int xBitmap; // x offest from the upperleft of bitmap
			public int yBitmap; // y offset from the upperleft of bitmap
			public int rgbBk;
			public int rgbFg;
			public IMAGELISTDRAWFLAGS fStyle;
			public Gdi32.RasterOperationMode dwRop;
			public IMAGELISTSTATEFLAGS fState;
			public int Frame;
			public int crEffect;
		}
	}
}