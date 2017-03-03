using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class User32
	{
		public const int OCM_NOTIFY = 0x204E; // WM_NOTIFY + WM_REFLECT

		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// For use with ChildWindowFromPointEx 
		/// </summary>
		[Flags]
		public enum ChildWindowSkipOptions
		{
			/// <summary>
			/// Does not skip any child windows
			/// </summary>
			CWP_ALL = 0x0000,
			/// <summary>
			/// Skips invisible child windows
			/// </summary>
			CWP_SKIPINVISIBLE = 0x0001,
			/// <summary>
			/// Skips disabled child windows
			/// </summary>
			CWP_SKIPDISABLED = 0x0002,
			/// <summary>
			/// Skips transparent child windows
			/// </summary>
			CWP_SKIPTRANSPARENT = 0x0004
		}

		[Flags]
		public enum CopyImageOptions
		{
			/// <summary>Returns the original hImage if it satisfies the criteria for the copy—that is, correct dimensions and color depth—in which case the LR_COPYDELETEORG flag is ignored. If this flag is not specified, a new object is always created.</summary>
			LR_COPYRETURNORG = 0x00000004,
			/// <summary>Deletes the original image after creating the copy.</summary>
			LR_COPYDELETEORG = 0x00000008,
			/// <summary>Tries to reload an icon or cursor resource from the original resource file rather than simply copying the current image. This is useful for creating a different-sized copy when the resource file contains multiple sizes of the resource. Without this flag, CopyImage stretches the original image to the new size. If this flag is set, CopyImage uses the size in the resource file closest to the desired size. This will succeed only if hImage was loaded by LoadIcon or LoadCursor, or by LoadImage with the LR_SHARED flag.</summary>
			LR_COPYFROMRESOURCE = 0x00004000,
			/// <summary>When the uType parameter specifies IMAGE_BITMAP, causes the function to return a DIB section bitmap rather than a compatible bitmap. This flag is useful for loading a bitmap without mapping it to the colors of the display device.</summary>
			LR_CREATEDIBSECTION = 0x00002000,
			/// <summary>Uses the width or height specified by the system metric values for cursors or icons, if the cxDesired or cyDesired values are set to zero. If this flag is not specified and cxDesired and cyDesired are set to zero, the function uses the actual resource size. If the resource contains multiple images, the function uses the size of the first image.</summary>
			LR_DEFAULTSIZE = 0x00000040,
			/// <summary>Loads the image in black and white.</summary>
			LR_MONOCHROME = 0x00000001,
		}

		/// <summary>
		/// The formatting options for <see cref="DrawText"/>.
		/// </summary>
		[Flags]
		public enum DrawTextFlags
		{
			/// <summary>Justifies the text to the top of the rectangle.</summary>
			DT_TOP = 0x00000000,
			/// <summary>Aligns text to the left.</summary>
			DT_LEFT = 0x00000000,
			/// <summary>Centers text horizontally in the rectangle.</summary>
			DT_CENTER = 0x00000001,
			/// <summary>Aligns text to the right.</summary>
			DT_RIGHT = 0x00000002,
			/// <summary>Centers text vertically. This value is used only with the DT_SINGLELINE value.</summary>
			DT_VCENTER = 0x00000004,
			/// <summary>Justifies the text to the bottom of the rectangle. This value is used only with the DT_SINGLELINE value.</summary>
			DT_BOTTOM = 0x00000008,
			/// <summary>Breaks words. Lines are automatically broken between words if a word extends past the edge of the rectangle specified by the lprc parameter. A carriage return-line feed sequence also breaks the line.</summary>
			DT_WORDBREAK = 0x00000010,
			/// <summary>Displays text on a single line only. Carriage returns and line feeds do not break the line.</summary>
			DT_SINGLELINE = 0x00000020,
			/// <summary>Expands tab characters. The default number of characters per tab is eight.</summary>
			DT_EXPANDTABS = 0x00000040,
			/// <summary>Sets tab stops. The DRAWTEXTPARAMS structure pointed to by the lpDTParams parameter specifies the number of average character widths per tab stop.</summary>
			DT_TABSTOP = 0x00000080,
			/// <summary>Draws without clipping. DrawTextEx is somewhat faster when DT_NOCLIP is used.</summary>
			DT_NOCLIP = 0x00000100,
			/// <summary>Includes the font external leading in line height. Normally, external leading is not included in the height of a line of text.</summary>
			DT_EXTERNALLEADING = 0x00000200,
			/// <summary>Determines the width and height of the rectangle. If there are multiple lines of text, DrawTextEx uses the width of the rectangle pointed to by the lprc parameter and extends the base of the rectangle to bound the last line of text. If there is only one line of text, DrawTextEx modifies the right side of the rectangle so that it bounds the last character in the line. In either case, DrawTextEx returns the height of the formatted text, but does not draw the text.</summary>
			DT_CALCRECT = 0x00000400,
			/// <summary>Turns off processing of prefix characters. Normally, DrawTextEx interprets the ampersand (&) mnemonic-prefix character as a directive to underscore the character that follows, and the double-ampersand (&&) mnemonic-prefix characters as a directive to print a single ampersand. By specifying DT_NOPREFIX, this processing is turned off. Compare with DT_HIDEPREFIX and DT_PREFIXONLY</summary>
			DT_NOPREFIX = 0x00000800,
			/// <summary>Uses the system font to calculate text metrics.</summary>
			DT_INTERNAL = 0x00001000,
			/// <summary>Duplicates the text-displaying characteristics of a multiline edit control. Specifically, the average character width is calculated in the same manner as for an edit control, and the function does not display a partially visible last line.</summary>
			DT_EDITCONTROL = 0x00002000,
			/// <summary>For displayed text, replaces characters in the middle of the string with ellipses so that the result fits in the specified rectangle. If the string contains backslash (\) characters, DT_PATH_ELLIPSIS preserves as much as possible of the text after the last backslash. The string is not modified unless the DT_MODIFYSTRING flag is specified.</summary>
			DT_PATH_ELLIPSIS = 0x00004000,
			/// <summary>For displayed text, replaces the end of a string with ellipses so that the result fits in the specified rectangle. Any word (not at the end of the string) that goes beyond the limits of the rectangle is truncated without ellipses. The string is not modified unless the DT_MODIFYSTRING flag is specified.</summary>
			DT_END_ELLIPSIS = 0x00008000,
			/// <summary>Modifies the specified string to match the displayed text. This value has no effect unless DT_END_ELLIPSIS or DT_PATH_ELLIPSIS is specified.</summary>
			DT_MODIFYSTRING = 0x00010000,
			/// <summary>Layout in right-to-left reading order for bidirectional text when the font selected into the hdc is a Hebrew or Arabic font. The default reading order for all text is left-to-right.</summary>
			DT_RTLREADING = 0x00020000,
			/// <summary>Truncates any word that does not fit in the rectangle and adds ellipses.</summary>
			DT_WORD_ELLIPSIS = 0x00040000,
			/// <summary>Prevents a line break at a DBCS (double-wide character string), so that the line-breaking rule is equivalent to SBCS strings. For example, this can be used in Korean windows, for more readability of icon labels. This value has no effect unless DT_WORDBREAK is specified.</summary>
			DT_NOFULLWIDTHCHARBREAK = 0x00080000,
			/// <summary>Ignores the ampersand (&) prefix character in the text. The letter that follows will not be underlined, but other mnemonic-prefix characters are still processed.</summary>
			DT_HIDEPREFIX = 0x00100000,
			/// <summary>Draws only an underline at the position of the character following the ampersand (&) prefix character. Does not draw any character in the string.</summary>
			DT_PREFIXONLY = 0x00200000,
		}

		/// <summary>
		/// The shutdown type for the <see cref="ExitWindowsEx"/> method.
		/// </summary>
		[Flags]
		public enum ExitWindowsFlags
		{
			/// <summary>
			/// Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. Then it logs the user off.
			/// <para>This flag can be used only by processes running in an interactive user's logon session.</para>
			/// </summary>
			EWX_LOGOFF = 0x00000000,
			/// <summary>
			/// Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped.
			/// <para>The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.</para>
			/// <para>Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.</para>
			/// <para>Windows XP with SP1:  If the system supports the power-off feature, specifying this flag turns off the power.</para>
			/// </summary>
			EWX_SHUTDOWN = 0x00000001,
			/// <summary>
			/// Shuts down the system and then restarts the system.
			/// <para>The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.</para>
			/// </summary>
			EWX_REBOOT = 0x00000002,
			/// <summary>
			/// This flag has no effect if terminal services is enabled. Otherwise, the system does not send the WM_QUERYENDSESSION message. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.
			/// </summary>
			EWX_FORCE = 0x00000004,
			/// <summary>
			/// Shuts down the system and turns off the power. The system must support the power-off feature.
			/// <para>The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.</para>
			/// </summary>
			EWX_POWEROFF = 0x00000008,
			/// <summary>
			/// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval. For more information, see the Remarks.
			/// </summary>
			EWX_FORCEIFHUNG = 0x00000010,
			/// <summary>
			/// The ewx quickresolve
			/// </summary>
			EWX_QUICKRESOLVE = 0x00000020,
			/// <summary>
			/// Shuts down the system and then restarts it, as well as any applications that have been registered for restart using the RegisterApplicationRestart function. These application receive the WM_QUERYENDSESSION message with lParam set to the ENDSESSION_CLOSEAPP value. For more information, see Guidelines for Applications.
			/// </summary>
			EWX_RESTARTAPPS = 0x00000040,
			/// <summary>
			/// Beginning with Windows 8:  You can prepare the system for a faster startup by combining the EWX_HYBRID_SHUTDOWN flag with the EWX_SHUTDOWN flag.
			/// </summary>
			EWX_HYBRID_SHUTDOWN = 0x00400000,
			/// <summary>
			/// When combined with the EWX_REBOOT flag, will reboot to the boot options.
			/// </summary>
			EWX_BOOTOPTIONS = 0x01000000,
		}

		/// <summary>
		/// Values to use a return codes when handling the WM_HCHITTEST message.
		/// </summary>
		public enum HitTestValues
		{
			/// <summary>In the border of a window that does not have a sizing border.</summary>
			HTBORDER = 18,

			/// <summary>In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).</summary>
			HTBOTTOM = 15,

			/// <summary>In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).</summary>
			HTBOTTOMLEFT = 16,

			/// <summary>In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).</summary>
			HTBOTTOMRIGHT = 17,

			/// <summary>In a title bar.</summary>
			HTCAPTION = 2,

			/// <summary>In a client area.</summary>
			HTCLIENT = 1,

			/// <summary>In a Close button.</summary>
			HTCLOSE = 20,

			/// <summary>
			/// On the screen background or on a dividing line between windows (same as HTNOWHERE, except that the DefWindowProc function produces a system beep
			/// to indicate an error).
			/// </summary>
			HTERROR = -2,

			/// <summary>In a size box (same as HTSIZE).</summary>
			HTGROWBOX = 4,

			/// <summary>In a Help button.</summary>
			HTHELP = 21,

			/// <summary>In a horizontal scroll bar.</summary>
			HTHSCROLL = 6,

			/// <summary>In the left border of a resizable window (the user can click the mouse to resize the window horizontally).</summary>
			HTLEFT = 10,

			/// <summary>In a menu.</summary>
			HTMENU = 5,

			/// <summary>In a Maximize button.</summary>
			HTMAXBUTTON = 9,

			/// <summary>In a Minimize button.</summary>
			HTMINBUTTON = 8,

			/// <summary>On the screen background or on a dividing line between windows.</summary>
			HTNOWHERE = 0,

			/// <summary>Not implemented.</summary>
			/* HTOBJECT = 19, */

			/// <summary>In a Minimize button.</summary>
			HTREDUCE = HTMINBUTTON,

			/// <summary>In the right border of a resizable window (the user can click the mouse to resize the window horizontally).</summary>
			HTRIGHT = 11,

			/// <summary>In a size box (same as HTGROWBOX).</summary>
			HTSIZE = HTGROWBOX,

			/// <summary>In a window menu or in a Close button in a child window.</summary>
			HTSYSMENU = 3,

			/// <summary>In the upper-horizontal border of a window.</summary>
			HTTOP = 12,

			/// <summary>In the upper-left corner of a window border.</summary>
			HTTOPLEFT = 13,

			/// <summary>In the upper-right corner of a window border.</summary>
			HTTOPRIGHT = 14,

			/// <summary>
			/// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one
			/// of them returns a code that is not HTTRANSPARENT).
			/// </summary>
			HTTRANSPARENT = -1,

			/// <summary>In the vertical scroll bar.</summary>
			HTVSCROLL = 7,

			/// <summary>In a Maximize button.</summary>
			HTZOOM = HTMAXBUTTON,
		}

		public enum HookType
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		[Flags]
		public enum HotKeyModifiers
		{
			MOD_NONE = 0,
			MOD_ALT = 0x0001,
			MOD_CONTROL = 0x0002,
			MOD_SHIFT = 0x0004,
			MOD_WIN = 0x0008,
			MOD_NOREPEAT = 0x4000,
		}

		/// <summary>
		/// Specifies the load options for <see cref="LoadImage"/>.
		/// </summary>
		[Flags]
		public enum LoadImageOptions : uint
		{
			/// <summary>The default flag; it does nothing. All it means is "not LR_MONOCHROME".</summary>
			LR_DEFAULTCOLOR = 0x00000000,
			/// <summary>Loads the image in black and white.</summary>
			LR_MONOCHROME = 0x00000001,
			/// <summary>Undocumented</summary>
			LR_COLOR = 0x00000002,
			/// <summary>Loads the stand-alone image from the file specified by lpszName (icon, cursor, or bitmap file).</summary>
			LR_LOADFROMFILE = 0x00000010,
			/// <summary>Retrieves the color value of the first pixel in the image and replaces the corresponding entry in the color table with the default window color (COLOR_WINDOW). All pixels in the image that use that entry become the default window color. This value applies only to images that have corresponding color tables.
			/// <para>Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.</para>
			/// <para>If fuLoad includes both the LR_LOADTRANSPARENT and LR_LOADMAP3DCOLORS values, LR_LOADTRANSPARENT takes precedence. However, the color table entry is replaced with COLOR_3DFACE rather than COLOR_WINDOW.</para></summary>
			LR_LOADTRANSPARENT = 0x00000020,
			/// <summary>Uses the width or height specified by the system metric values for cursors or icons, if the cxDesired or cyDesired values are set to zero. If this flag is not specified and cxDesired and cyDesired are set to zero, the function uses the actual resource size. If the resource contains multiple images, the function uses the size of the first image.</summary>
			LR_DEFAULTSIZE = 0x00000040,
			/// <summary>Uses true VGA colors.</summary>
			LR_VGACOLOR = 0x00000080,
			/// <summary>Searches the color table for the image and replaces the following shades of gray with the corresponding 3-D color.
			/// <list type="bullet">
			/// <item><description>Dk Gray, RGB(128,128,128) with COLOR_3DSHADOW</description></item>
			/// <item><description>Gray, RGB(192,192,192) with COLOR_3DFACE</description></item>
			/// <item><description>Lt Gray, RGB(223,223,223) with COLOR_3DLIGHT</description></item>
			/// </list>
			/// <para>Do not use this option if you are loading a bitmap with a color depth greater than 8bpp.</para></summary>
			LR_LOADMAP3DCOLORS = 0x00001000,
			/// <summary>When the uType parameter specifies IMAGE_BITMAP, causes the function to return a DIB section bitmap rather than a compatible bitmap. This flag is useful for loading a bitmap without mapping it to the colors of the display device.</summary>
			LR_CREATEDIBSECTION = 0x00002000,
			/// <summary>Shares the image handle if the image is loaded multiple times. If LR_SHARED is not set, a second call to LoadImage for the same resource will load the image again and return a different handle.
			/// <para>When you use this flag, the system will destroy the resource when it is no longer needed.</para>
			/// <para>Do not use LR_SHARED for images that have non-standard sizes, that may change after loading, or that are loaded from a file.</para>
			/// <para>When loading a system icon or cursor, you must use LR_SHARED or the function will fail to load the resource.</para>
			/// <para>This function finds the first image in the cache with the requested resource name, regardless of the size requested.</para></summary>
			LR_SHARED = 0x00008000
		}

		/// <summary>
		/// Specifies the type of image to be loaded by <see cref="LoadImage"/>.
		/// </summary>
		public enum LoadImageType : uint
		{
			/// <summary>Loads a bitmap.</summary>
			IMAGE_BITMAP = 0,

			/// <summary>Loads an icon.</summary>
			IMAGE_ICON = 1,

			/// <summary>Loads a cursor.</summary>
			IMAGE_CURSOR = 2,

			/// <summary>Loads an enhanced metafile.</summary>
			IMAGE_ENHMETAFILE = 3
		}

		/// <summary>Window sizing and positioning flags.</summary>
		[Flags]
		public enum SetWindowPosFlags : uint
		{
			/// <summary>
			/// If the calling thread and the thread that owns the window are attached to different input queues, the
			/// system posts the request to the thread that owns the window. This prevents the calling thread from
			/// blocking its execution while other threads process the request.
			/// </summary>
			SWP_ASYNCWINDOWPOS = 0x4000,

			/// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
			SWP_DEFERERASE = 0x2000,

			/// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
			SWP_DRAWFRAME = 0x0020,

			/// <summary>
			/// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the
			/// window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is
			/// sent only when the window's size is being changed.
			/// </summary>
			SWP_FRAMECHANGED = 0x0020,

			/// <summary>Hides the window.</summary>
			SWP_HIDEWINDOW = 0x0080,

			/// <summary>
			/// Does not activate the window. If this flag is not set, the window is activated and moved to the top of
			/// either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
			/// </summary>
			SWP_NOACTIVATE = 0x0010,

			/// <summary>
			/// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the
			/// client area are saved and copied back into the client area after the window is sized or repositioned.
			/// </summary>
			SWP_NOCOPYBITS = 0x0100,

			/// <summary>Retains the current position (ignores X and Y parameters).</summary>
			SWP_NOMOVE = 0x0002,

			/// <summary>Does not change the owner window's position in the Z order.</summary>
			SWP_NOOWNERZORDER = 0x0200,

			/// <summary>
			/// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the
			/// client area, the nonclient area (including the title bar and scroll bars), and any part of the parent
			/// window uncovered as a result of the window being moved. When this flag is set, the application must
			/// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
			/// </summary>
			SWP_NOREDRAW = 0x0008,

			/// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
			SWP_NOREPOSITION = 0x0200,

			/// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
			SWP_NOSENDCHANGING = 0x0400,

			/// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
			SWP_NOSIZE = 0x0001,

			/// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
			SWP_NOZORDER = 0x0004,

			/// <summary>Displays the window.</summary>
			SWP_SHOWWINDOW = 0x0040,
		}

		/// <summary>Flags used for <see cref="GetWindowLong"/> and <see cref="SetWindowLong"/> methods to retrieve information about a window.</summary>
		[Flags]
		public enum WindowLongFlags
		{
			/// <summary>The extended window styles</summary>
			GWL_EXSTYLE = -20,
			/// <summary>The application instance handle</summary>
			GWL_HINSTANCE = -6,
			/// <summary>The parent window handle</summary>
			GWL_HWNDPARENT = -8,
			/// <summary>The window identifier</summary>
			GWL_ID = -12,
			/// <summary>The window styles</summary>
			GWL_STYLE = -16,
			/// <summary>The window user data</summary>
			GWL_USERDATA = -21,
			/// <summary>The window procedure address or handle</summary>
			GWL_WNDPROC = -4,
			/// <summary>The dialog user data</summary>
			DWLP_USER = 0x8,
			/// <summary>The dialog procedure message result</summary>
			DWLP_MSGRESULT = 0x0,
			/// <summary>The dialog procedure address or handle</summary>
			DWLP_DLGPROC = 0x4
		}

		/// <summary>
		/// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this function either before or after
		/// processing the hook information.
		/// <para>See [ https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974%28v=vs.85%29.aspx ] for more information.</para>
		/// </summary>
		/// <param name="hhk">C++ ( hhk [in, optional]. Type: HHOOK ) <br/> This parameter is ignored.</param>
		/// <param name="nCode">
		/// C++ ( nCode [in]. Type: int ) <br/> The hook code passed to the current hook procedure. The next hook procedure uses this code to determine how to
		/// process the hook information.
		/// </param>
		/// <param name="wParam">
		/// C++ ( wParam [in]. Type: WPARAM ) <br/> The wParam value passed to the current hook procedure. The meaning of this parameter depends on the type of
		/// hook associated with the current hook chain.
		/// </param>
		/// <param name="lParam">
		/// C++ ( lParam [in]. Type: LPARAM ) <br/> The lParam value passed to the current hook procedure. The meaning of this parameter depends on the type of
		/// hook associated with the current hook chain.
		/// </param>
		/// <returns>
		/// C++ ( Type: LRESULT ) <br/> This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value.
		/// The meaning of the return value depends on the hook type. For more information, see the descriptions of the individual hook procedures.
		/// </returns>
		/// <remarks>
		/// <para>Hook procedures are installed in chains for particular hook types. <see cref="CallNextHookEx"/> calls the next hook in the chain.</para>
		/// <para>
		/// Calling CallNextHookEx is optional, but it is highly recommended; otherwise, other applications that have installed hooks will not receive hook
		/// notifications and may behave incorrectly as a result. You should call <see cref="CallNextHookEx"/> unless you absolutely need to prevent the
		/// notification from being seen by other applications.
		/// </para>
		/// </remarks>
		[DllImport(nameof(User32))]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Determines which, if any, of the child windows belonging to the specified parent window contains the specified point. The function can ignore invisible, disabled, and transparent child windows. The search is restricted to immediate child windows. Grandchildren and deeper descendants are not searched.
		/// </summary>
		/// <param name="hwndParent">A handle to the parent window.</param>
		/// <param name="pt">A structure that defines the client coordinates (relative to hwndParent) of the point to be checked.</param>
		/// <param name="uFlags">The child windows to be skipped. This parameter can be one or more of the following values.</param>
		/// <returns>The return value is a handle to the first child window that contains the point and meets the criteria specified by uFlags. If the point is within the parent window but not within any child window that meets the criteria, the return value is a handle to the parent window. If the point lies outside the parent window or if the function fails, the return value is NULL.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern IntPtr ChildWindowFromPointEx(HandleRef hwndParent, ref Point pt, ChildWindowSkipOptions uFlags);

		/// <summary>
		/// Destroys an icon and frees any memory the icon occupied.
		/// </summary>
		/// <param name="hIcon">A handle to the icon to be destroyed. The icon must not be in use.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool DestroyIcon(IntPtr hIcon);

		/// <summary>
		/// The DrawText function draws formatted text in the specified rectangle. It formats the text according to the specified method (expanding tabs, justifying characters, breaking lines, and so forth).
		/// </summary>
		/// <param name="hDC">A handle to the device context.</param>
		/// <param name="lpchText">A pointer to the string that specifies the text to be drawn. If the nCount parameter is -1, the string must be null-terminated. If uFormat includes DT_MODIFYSTRING, the function could add up to four additional characters to this string. The buffer containing the string should be large enough to accommodate these extra characters.</param>
		/// <param name="nCount">The length, in characters, of the string. If nCount is -1, then the lpchText parameter is assumed to be a pointer to a null-terminated string and DrawText computes the character count automatically.</param>
		/// <param name="lpRect">A pointer to a RECT structure that contains the rectangle (in logical coordinates) in which the text is to be formatted.</param>
		/// <param name="uFormat">The method of formatting the text.</param>
		/// <returns>If the function succeeds, the return value is the height of the text in logical units. If DT_VCENTER or DT_BOTTOM is specified, the return value is the offset from lpRect->top to the bottom of the drawn text. If the function fails, the return value is zero.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DrawText(Gdi32.SafeDCHandle hDC, string lpchText, int nCount, ref RECT lpRect, DrawTextFlags uFormat);

		/// <summary>
		/// The ExitWindowsEx function either logs off the current user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
		/// </summary>
		/// <param name="uFlags">Specifies the type of shutdown.</param>
		/// <param name="dwReason">The reason for initiating the shutdown.</param>
		/// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error.</br></returns>
		[DllImport(nameof(User32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ExitWindowsEx(ExitWindowsFlags uFlags, AdvApi32.SystemShutDownReason dwReason);

		/// <summary>
		/// Retrieves the window handle to the active window attached to the calling thread's message queue.
		/// </summary>
		/// <returns>The return value is the handle to the active window attached to the calling thread's message queue. Otherwise, the return value is NULL.</returns>
		[DllImport(nameof(User32), ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern IntPtr GetActiveWindow();

		/// <summary>
		/// Retrieves the coordinates of a window's client area. The client coordinates specify the upper-left and lower-right corners of the client area. Because client coordinates are relative to the upper-left corner of a window's client area, the coordinates of the upper-left corner are (0,0).
		/// </summary>
		/// <param name="hWnd">A handle to the window whose client coordinates are to be retrieved.</param>
		/// <param name="lpRect">A pointer to a RECT structure that receives the client coordinates. The left and top members are zero. The right and bottom members contain the width and height of the window.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool GetClientRect(HandleRef hWnd, [In, Out] ref RECT lpRect);

		/// <summary>
		/// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To retrieve any other value, specify one of the following values.</param>
		/// <returns>If the function succeeds, the return value is the requested value. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
		{
			IntPtr ret;
			if (IntPtr.Size == 4)
				ret = (IntPtr)GetWindowLong32(hWnd, nIndex);
			else
				ret = GetWindowLongPtr(hWnd, nIndex);
			if (ret == IntPtr.Zero)
				throw new System.ComponentModel.Win32Exception();
			return ret;
		}

		/// <summary>
		/// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus four; for example, if you specified 12 or more bytes of extra memory, a value of 8 would be an index to the third 32-bit integer. To retrieve any other value, specify one of the following values.</param>
		/// <returns>If the function succeeds, the return value is the requested value. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), EntryPoint = "GetWindowLong", SetLastError = true)]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
		[System.Security.SecurityCritical]
		public static extern int GetWindowLong32(HandleRef hWnd, int nIndex);

		/// <summary>
		/// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">The zero-based offset to the value to be retrieved. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To retrieve any other value, specify one of the following values.</param>
		/// <returns>If the function succeeds, the return value is the requested value. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), EntryPoint = "GetWindowLongPtr", SetLastError = true)]
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
		[System.Security.SecurityCritical]
		public static extern IntPtr GetWindowLongPtr(HandleRef hWnd, int nIndex);

		/// <summary>
		/// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

		/// <summary>
		/// The InvalidateRect function adds a rectangle to the specified window's update region. The update region represents the portion of the window's client area that must be redrawn.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose update region has changed. If this parameter is NULL, the system invalidates and redraws all windows, not just the windows for this application, and sends the WM_ERASEBKGND and WM_NCPAINT messages before the function returns. Setting this parameter to NULL is not recommended.</param>
		/// <param name="rect">A pointer to a RECT structure that contains the client coordinates of the rectangle to be added to the update region. If this parameter is NULL, the entire client area is added to the update region.</param>
		/// <param name="bErase">Specifies whether the background within the update region is to be erased when the update region is processed. If this parameter is TRUE, the background is erased when the BeginPaint function is called. If this parameter is FALSE, the background remains unchanged.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool InvalidateRect(HandleRef hWnd, [In] PRECT rect, [MarshalAs(UnmanagedType.Bool)] bool bErase);

		/// <summary>
		/// Loads an icon, cursor, animated cursor, or bitmap.
		/// </summary>
		/// <param name="hinst">A handle to the module of either a DLL or executable (.exe) that contains the image to be loaded. For more information, see GetModuleHandle. Note that as of 32-bit Windows, an instance handle (HINSTANCE), such as the application instance handle exposed by system function call of WinMain, and a module handle (HMODULE) are the same thing.
		/// <para>To load an OEM image, set this parameter to NULL.</para>
		/// <para>To load a stand-alone resource (icon, cursor, or bitmap file)—for example, c:\myimage.bmp—set this parameter to NULL.</para></param>
		/// <param name="lpszName">The image to be loaded. If the hinst parameter is non-NULL and the fuLoad parameter omits LR_LOADFROMFILE, lpszName specifies the image resource in the hinst module. If the image resource is to be loaded by name from the module, the lpszName parameter is a pointer to a null-terminated string that contains the name of the image resource. If the image resource is to be loaded by ordinal from the module, use the MAKEINTRESOURCE macro to convert the image ordinal into a form that can be passed to the LoadImage function. For more information, see the Remarks section below.
		/// <para>If the hinst parameter is NULL and the fuLoad parameter omits the LR_LOADFROMFILE value, the lpszName specifies the OEM image to load.</para>
		/// <para>To pass these constants to the LoadImage function, use the MAKEINTRESOURCE macro. For example, to load the OCR_NORMAL cursor, pass MAKEINTRESOURCE(OCR_NORMAL) as the lpszName parameter, NULL as the hinst parameter, and LR_SHARED as one of the flags to the fuLoad parameter.</para>
		/// <para>If the fuLoad parameter includes the LR_LOADFROMFILE value, lpszName is the name of the file that contains the stand-alone resource (icon, cursor, or bitmap file). Therefore, set hinst to NULL.</para></param>
		/// <param name="uType">The type of image to be loaded. This parameter can be one of the following values.</param>
		/// <param name="cxDesired">The width, in pixels, of the icon or cursor. If this parameter is zero and the fuLoad parameter is LR_DEFAULTSIZE, the function uses the SM_CXICON or SM_CXCURSOR system metric value to set the width. If this parameter is zero and LR_DEFAULTSIZE is not used, the function uses the actual resource width.</param>
		/// <param name="cyDesired">The height, in pixels, of the icon or cursor. If this parameter is zero and the fuLoad parameter is LR_DEFAULTSIZE, the function uses the SM_CYICON or SM_CYCURSOR system metric value to set the height. If this parameter is zero and LR_DEFAULTSIZE is not used, the function uses the actual resource height.</param>
		/// <param name="fuLoad">Loading options.</param>
		/// <returns>If the function succeeds, the return value is the handle of the newly loaded image. If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true, CharSet = CharSet.Auto)]
		[System.Security.SecurityCritical]
		public static extern IntPtr LoadImage(Kernel32.SafeLibraryHandle hinst, ResourceName lpszName, LoadImageType uType, int cxDesired, int cyDesired, LoadImageOptions fuLoad);

		/// <summary>
		/// Loads a string resource from the executable file associated with a specified module, copies the string into a buffer, and appends a terminating null character.
		/// </summary>
		/// <param name="hInstance">A handle to an instance of the module whose executable file contains the string resource. To get the handle to the application itself, call the GetModuleHandle function with NULL.</param>
		/// <param name="uID">The identifier of the string to be loaded.</param>
		/// <param name="lpBuffer">The buffer is to receive the string. Must be of sufficient length to hold a pointer (8 bytes).</param>
		/// <param name="nBufferMax">The size of the buffer, in characters. The string is truncated and null-terminated if it is longer than the number of characters specified. If this parameter is 0, then lpBuffer receives a read-only pointer to the resource itself.</param>
		/// <returns>If the function succeeds, the return value is the number of characters copied into the buffer, not including the terminating null character, or zero if the string resource does not exist. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = true)]
		[System.Security.SecurityCritical]
		public static extern int LoadString(Kernel32.SafeLibraryHandle hInstance, int uID, System.Text.StringBuilder lpBuffer, int nBufferMax);

		/// <summary>
		/// Locks the workstation's display, protecting it from unauthorized use.
		/// </summary>
		/// <returns>0 on failure, non-zero for success</returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LockWorkStation();

		/// <summary>
		/// The MapWindowPoints function converts (maps) a set of points from a coordinate space relative to one window to a coordinate space relative to another window.
		/// </summary>
		/// <param name="hWndFrom">A handle to the window from which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are presumed to be in screen coordinates.</param>
		/// <param name="hWndTo">A handle to the window to which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are converted to screen coordinates.</param>
		/// <param name="lpPoints">A pointer to an array of POINT structures that contain the set of points to be converted. The points are in device units. This parameter can also point to a RECT structure, in which case the cPoints parameter should be set to 2.</param>
		/// <param name="cPoints">The number of POINT structures in the array pointed to by the lpPoints parameter.</param>
		/// <returns>If the function succeeds, the low-order word of the return value is the number of pixels added to the horizontal coordinate of each source point in order to compute the horizontal coordinate of each destination point. (In addition to that, if precisely one of hWndFrom and hWndTo is mirrored, then each resulting horizontal coordinate is multiplied by -1.) The high-order word is the number of pixels added to the vertical coordinate of each source point in order to compute the vertical coordinate of each destination point.
		/// <para>If the function fails, the return value is zero. Call SetLastError prior to calling this method to differentiate an error return value from a legitimate "0" return value.</para></returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In, Out] ref RECT lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

		/// <summary>
		/// The MapWindowPoints function converts (maps) a set of points from a coordinate space relative to one window to a coordinate space relative to another window.
		/// </summary>
		/// <param name="hWndFrom">A handle to the window from which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are presumed to be in screen coordinates.</param>
		/// <param name="hWndTo">A handle to the window to which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are converted to screen coordinates.</param>
		/// <param name="lpPoints">A pointer to an array of POINT structures that contain the set of points to be converted. The points are in device units. This parameter can also point to a RECT structure, in which case the cPoints parameter should be set to 2.</param>
		/// <param name="cPoints">The number of POINT structures in the array pointed to by the lpPoints parameter.</param>
		/// <returns>If the function succeeds, the low-order word of the return value is the number of pixels added to the horizontal coordinate of each source point in order to compute the horizontal coordinate of each destination point. (In addition to that, if precisely one of hWndFrom and hWndTo is mirrored, then each resulting horizontal coordinate is multiplied by -1.) The high-order word is the number of pixels added to the vertical coordinate of each source point in order to compute the vertical coordinate of each destination point.
		/// <para>If the function fails, the return value is zero. Call SetLastError prior to calling this method to differentiate an error return value from a legitimate "0" return value.</para></returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In, Out] ref Point lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

		/// <summary>
		/// The MapWindowPoints function converts (maps) a set of points from a coordinate space relative to one window to a coordinate space relative to another window.
		/// </summary>
		/// <param name="hWndFrom">A handle to the window from which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are presumed to be in screen coordinates.</param>
		/// <param name="hWndTo">A handle to the window to which points are converted. If this parameter is NULL or HWND_DESKTOP, the points are converted to screen coordinates.</param>
		/// <param name="lpPoints">A pointer to an array of POINT structures that contain the set of points to be converted. The points are in device units. This parameter can also point to a RECT structure, in which case the cPoints parameter should be set to 2.</param>
		/// <param name="cPoints">The number of POINT structures in the array pointed to by the lpPoints parameter.</param>
		/// <returns>If the function succeeds, the low-order word of the return value is the number of pixels added to the horizontal coordinate of each source point in order to compute the horizontal coordinate of each destination point. (In addition to that, if precisely one of hWndFrom and hWndTo is mirrored, then each resulting horizontal coordinate is multiplied by -1.) The high-order word is the number of pixels added to the vertical coordinate of each source point in order to compute the vertical coordinate of each destination point.
		/// <para>If the function fails, the return value is zero. Call SetLastError prior to calling this method to differentiate an error return value from a legitimate "0" return value.</para></returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] Point[] lpPoints, [MarshalAs(UnmanagedType.U4)] int cPoints);

		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int RealGetWindowClass(HandleRef hwnd, System.Text.StringBuilder pszType, int cchType);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern int RegisterHotKey(HandleRef hWnd, int id, HotKeyModifiers fsModifiers, uint vk);

		/// <summary>
		/// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
		/// </summary>
		/// <param name="lpString">The message to be registered.</param>
		/// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true, CharSet = CharSet.Auto)]
		[System.Security.SecurityCritical]
		public static extern uint RegisterWindowMessage(string lpString);

		/// <summary>
		/// The ScreenToClient function converts the screen coordinates of a specified point on the screen to client-area coordinates.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose client area will be used for the conversion.</param>
		/// <param name="lpPoint">A pointer to a POINT structure that specifies the screen coordinates to be converted.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool ScreenToClient(HandleRef hWnd, [In, Out] ref Point lpPoint);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, UIntPtr wParam, UIntPtr lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		public static IntPtr SendMessage(HandleRef hWnd, uint msg, int wParam = 0, int lParam = 0) => SendMessage(hWnd, msg, (IntPtr)wParam, (IntPtr)lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		public static IntPtr SendMessage(HandleRef hWnd, uint msg, int wParam, string lParam) => SendMessage(hWnd, msg, (IntPtr)wParam, lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="rect">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, IntPtr wParam, ref RECT rect);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, IntPtr wParam, [In, MarshalAs(UnmanagedType.LPWStr)] string lParam);

		/// <summary>
		/// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
		/// <param name="msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, ref int wParam, [In, Out] System.Text.StringBuilder lParam);

		/// <summary>
		/// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs. The SetWindowLongPtr function fails if the process that owns the window specified by the hWnd parameter is at a higher process privilege in the UIPI hierarchy than the process the calling thread resides in.</param>
		/// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. Alternately, this can be a value from <see cref="WindowLongFlags"/>.</param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified offset. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// <para>If the previous value is zero and the function succeeds, the return value is zero, but the function does not clear the last error information. To determine success or failure, clear the last error information by calling SetLastError with 0, then call SetWindowLongPtr. Function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</para></returns>
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			IntPtr ret;
			if (IntPtr.Size == 4)
				ret = (IntPtr)SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			else
				ret = SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			if (ret == IntPtr.Zero)
				throw new System.ComponentModel.Win32Exception();
			return ret;
		}

		/// <summary>
		/// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
		/// </summary>
		/// <param name="hWnd">A handle to the window.</param>
		/// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. This parameter must be a window handle or one of the following values.
		/// <list type="table">
		/// <listheader><term>Value</term><description>Meaning</description></listheader>
		/// <item><term><c>HWND_BOTTOM</c> (HWND)1</term><description>Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.</description></item>
		/// <item><term><c>HWND_NOTOPMOST</c> (HWND)-2</term><description>Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.</description></item>
		/// <item><term><c>HWND_TOP</c> (HWND)0</term><description>Places the window at the top of the Z order.</description></item>
		/// <item><term><c>HWND_TOPMOST</c> (HWND)-1</term><description>Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.</description></item>
		/// </list>
		/// </param>
		/// <param name="X">The new position of the left side of the window, in client coordinates.</param>
		/// <param name="Y">The new position of the top of the window, in client coordinates.</param>
		/// <param name="cx">The new width of the window, in pixels.</param>
		/// <param name="cy">The new height of the window, in pixels.</param>
		/// <param name="uFlags">The window sizing and positioning flags.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool SetWindowPos(HandleRef hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

		/// <summary>
		/// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to monitor the system for certain types of
		/// events. These events are associated either with a specific thread or with all threads in the same desktop as the calling thread.
		/// <para>See https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990%28v=vs.85%29.aspx for more information</para>
		/// </summary>
		/// <param name="idHook">
		/// C++ ( idHook [in]. Type: int ) <br/> The type of hook procedure to be installed. This parameter can be one of the following values.
		/// <list type="table">
		/// <listheader>
		/// <term>Possible Hook Types</term>
		/// </listheader>
		/// <item>
		/// <term>WH_CALLWNDPROC (4)</term>
		/// <description>
		/// Installs a hook procedure that monitors messages before the system sends them to the destination window procedure. For more information, see the
		/// CallWndProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_CALLWNDPROCRET (12)</term>
		/// <description>
		/// Installs a hook procedure that monitors messages after they have been processed by the destination window procedure. For more information, see the
		/// CallWndRetProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_CBT (5)</term>
		/// <description>
		/// Installs a hook procedure that receives notifications useful to a CBT application. For more information, see the CBTProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_DEBUG (9)</term>
		/// <description>Installs a hook procedure useful for debugging other hook procedures. For more information, see the DebugProc hook procedure.</description>
		/// </item>
		/// <item>
		/// <term>WH_FOREGROUNDIDLE (11)</term>
		/// <description>
		/// Installs a hook procedure that will be called when the application's foreground thread is about to become idle. This hook is useful for performing
		/// low priority tasks during idle time. For more information, see the ForegroundIdleProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_GETMESSAGE (3)</term>
		/// <description>
		/// Installs a hook procedure that monitors messages posted to a message queue. For more information, see the GetMsgProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_JOURNALPLAYBACK (1)</term>
		/// <description>
		/// Installs a hook procedure that posts messages previously recorded by a WH_JOURNALRECORD hook procedure. For more information, see the
		/// JournalPlaybackProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_JOURNALRECORD (0)</term>
		/// <description>
		/// Installs a hook procedure that records input messages posted to the system message queue. This hook is useful for recording macros. For more
		/// information, see the JournalRecordProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD (2)</term>
		/// <description>Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure.</description>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD_LL (13)</term>
		/// <description>
		/// Installs a hook procedure that monitors low-level keyboard input events. For more information, see the LowLevelKeyboardProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE (7)</term>
		/// <description>Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure.</description>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE_LL (14)</term>
		/// <description>
		/// Installs a hook procedure that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_MSGFILTER (-1)</term>
		/// <description>
		/// Installs a hook procedure that monitors messages generated as a result of an input event in a dialog box, message box, menu, or scroll bar. For more
		/// information, see the MessageProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_SHELL (10)</term>
		/// <description>
		/// Installs a hook procedure that receives notifications useful to shell applications. For more information, see the ShellProc hook procedure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>WH_SYSMSGFILTER (6)</term>
		/// <description></description>
		/// </item>
		/// </list>
		/// </param>
		/// <param name="lpfn">
		/// C++ ( lpfn [in]. Type: HOOKPROC ) <br/> A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a thread
		/// created by a different process, the lpfn parameter must point to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the code
		/// associated with the current process.
		/// </param>
		/// <param name="hMod">
		/// C++ ( hMod [in]. Type: HINSTANCE ) <br/> A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The hMod parameter must
		/// be set to NULL if the dwThreadId parameter specifies a thread created by the current process and if the hook procedure is within the code associated
		/// with the current process.
		/// </param>
		/// <param name="dwThreadId">
		/// C++ ( dwThreadId [in]. Type: DWORD ) <br/> The identifier of the thread with which the hook procedure is to be associated. For desktop apps, if this
		/// parameter is zero, the hook procedure is associated with all existing threads running in the same desktop as the calling thread. For Windows Store
		/// apps, see the Remarks section.
		/// </param>
		/// <returns>
		/// C++ ( Type: HHOOK ) <br/> If the function succeeds, the return value is the handle to the hook procedure. If the function fails, the return value is NULL.
		/// <para>To get extended error information, call GetLastError.</para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// SetWindowsHookEx can be used to inject a DLL into another process. A 32-bit DLL cannot be injected into a 64-bit process, and a 64-bit DLL cannot be
		/// injected into a 32-bit process. If an application requires the use of hooks in other processes, it is required that a 32-bit application call
		/// SetWindowsHookEx to inject a 32-bit DLL into 32-bit processes, and a 64-bit application call SetWindowsHookEx to inject a 64-bit DLL into 64-bit
		/// processes. The 32-bit and 64-bit DLLs must have different names.
		/// </para>
		/// <para>
		/// Because hooks run in the context of an application, they must match the "bitness" of the application. If a 32-bit application installs a global hook
		/// on 64-bit Windows, the 32-bit hook is injected into each 32-bit process (the usual security boundaries apply). In a 64-bit process, the threads are
		/// still marked as "hooked." However, because a 32-bit application must run the hook code, the system executes the hook in the hooking app's context;
		/// specifically, on the thread that called SetWindowsHookEx. This means that the hooking application must continue to pump messages or it might block
		/// the normal functioning of the 64-bit processes.
		/// </para>
		/// <para>
		/// If a 64-bit application installs a global hook on 64-bit Windows, the 64-bit hook is injected into each 64-bit process, while all 32-bit processes
		/// use a callback to the hooking application.
		/// </para>
		/// <para>
		/// To hook all applications on the desktop of a 64-bit Windows installation, install a 32-bit global hook and a 64-bit global hook, each from
		/// appropriate processes, and be sure to keep pumping messages in the hooking application to avoid blocking normal functioning. If you already have a
		/// 32-bit global hooking application and it doesn't need to run in each application's context, you may not need to create a 64-bit version.
		/// </para>
		/// <para>
		/// An error may occur if the hMod parameter is NULL and the dwThreadId parameter is zero or specifies the identifier of a thread created by another process.
		/// </para>
		/// <para>
		/// Calling the CallNextHookEx function to chain to the next hook procedure is optional, but it is highly recommended; otherwise, other applications that
		/// have installed hooks will not receive hook notifications and may behave incorrectly as a result. You should call CallNextHookEx unless you absolutely
		/// need to prevent the notification from being seen by other applications.
		/// </para>
		/// <para>Before terminating, an application must call the UnhookWindowsHookEx function to free system resources associated with the hook.</para>
		/// <para>
		/// The scope of a hook depends on the hook type. Some hooks can be set only with global scope; others can also be set for only a specific thread, as
		/// shown in the following table.
		/// </para>
		/// <list type="table">
		/// <listheader>
		/// <term>Possible Hook Types</term>
		/// </listheader>
		/// <item>
		/// <term>WH_CALLWNDPROC (4)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_CALLWNDPROCRET (12)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_CBT (5)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_DEBUG (9)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_FOREGROUNDIDLE (11)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_GETMESSAGE (3)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_JOURNALPLAYBACK (1)</term>
		/// <description>Global only</description>
		/// </item>
		/// <item>
		/// <term>WH_JOURNALRECORD (0)</term>
		/// <description>Global only</description>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD (2)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD_LL (13)</term>
		/// <description>Global only</description>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE (7)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE_LL (14)</term>
		/// <description>Global only</description>
		/// </item>
		/// <item>
		/// <term>WH_MSGFILTER (-1)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_SHELL (10)</term>
		/// <description>Thread or global</description>
		/// </item>
		/// <item>
		/// <term>WH_SYSMSGFILTER (6)</term>
		/// <description>Global only</description>
		/// </item>
		/// </list>
		/// <para>
		/// For a specified hook type, thread hooks are called first, then global hooks. Be aware that the WH_MOUSE, WH_KEYBOARD, WH_JOURNAL*, WH_SHELL, and
		/// low-level hooks can be called on the thread that installed the hook rather than the thread processing the hook. For these hooks, it is possible that
		/// both the 32-bit and 64-bit hooks will be called if a 32-bit hook is ahead of a 64-bit hook in the hook chain.
		/// </para>
		/// <para>
		/// The global hooks are a shared resource, and installing one affects all applications in the same desktop as the calling thread. All global hook
		/// functions must be in libraries. Global hooks should be restricted to special-purpose applications or to use as a development aid during application
		/// debugging. Libraries that no longer need a hook should remove its hook procedure.
		/// </para>
		/// <para>
		/// Windows Store app development If dwThreadId is zero, then window hook DLLs are not loaded in-process for the Windows Store app processes and the
		/// Windows Runtime broker process unless they are installed by either UIAccess processes (accessibility tools). The notification is delivered on the
		/// installer's thread for these hooks:
		/// </para>
		/// <list type="bullet">
		/// <item>
		/// <term>WH_JOURNALPLAYBACK</term>
		/// </item>
		/// <item>
		/// <term>WH_JOURNALRECORD</term>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD</term>
		/// </item>
		/// <item>
		/// <term>WH_KEYBOARD_LL</term>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE</term>
		/// </item>
		/// <item>
		/// <term>WH_MOUSE_LL</term>
		/// </item>
		/// </list>
		/// <para>
		/// This behavior is similar to what happens when there is an architecture mismatch between the hook DLL and the target application process, for example,
		/// when the hook DLL is 32-bit and the application process 64-bit.
		/// </para>
		/// <para>
		/// For an example, see Installing and <see
		/// cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms644960%28v=vs.85%29.aspx#installing_releasing"> Releasing Hook Procedures. </see>
		/// [ https://msdn.microsoft.com/en-us/library/windows/desktop/ms644960%28v=vs.85%29.aspx#installing_releasing ]
		/// </para>
		/// </remarks>
		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(HookType idHook, HookProc lpfn, Kernel32.SafeLibraryHandle hMod, int dwThreadId);

		/// <summary>
		/// Changes the text of the specified window's title bar (if it has one). If the specified window is a control, the text of the control is changed. However, SetWindowText cannot change the text of a control in another application.
		/// </summary>
		/// <param name="hWnd">A handle to the window or control whose text is to be changed.</param>
		/// <param name="lpString">The new title or control text.</param>
		/// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool SetWindowText(HandleRef hWnd, string lpString);

		/// <summary>
		/// Indicates that the system cannot be shut down and sets a reason string to be displayed to the user if system shutdown is initiated.
		/// </summary>
		/// <param name="hWnd">A handle to the main window of the application.</param>
		/// <param name="reason">The reason the application must block system shutdown. This string will be truncated for display purposes after MAX_STR_BLOCKREASON characters.</param>
		/// <returns>If the call succeeds, the return value is nonzero. If the call fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShutdownBlockReasonCreate(HandleRef hWnd, [MarshalAs(UnmanagedType.LPWStr)] string reason);

		/// <summary>
		/// Indicates that the system can be shut down and frees the reason string.
		/// </summary>
		/// <param name="hWnd">A handle to the main window of the application.</param>
		/// <returns>If the call succeeds, the return value is nonzero. If the call fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport(nameof(User32), SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShutdownBlockReasonDestroy(HandleRef hWnd);

		/// <summary>Retrieves the reason string set by the <see cref="ShutdownBlockReasonCreate"/> function.</summary>
		/// <param name="hWnd">A handle to the main window of the application.</param>
		/// <param name="pwszBuff">
		/// A pointer to a buffer that receives the reason string. If this parameter is NULL, the function retrieves the number of characters in the reason string.
		/// </param>
		/// <param name="pcchBuff">
		/// A pointer to a variable that specifies the size of the pwszBuff buffer, in characters. If the function succeeds, this variable receives the number of
		/// characters copied into the buffer, including the null-terminating character. If the buffer is too small, the variable receives the required buffer
		/// size, in characters, not including the null-terminating character.
		/// </param>
		/// <returns>
		/// If the call succeeds, the return value is nonzero. If the call fails, the return value is zero. To get extended error information, call GetLastError.
		/// </returns>
		[DllImport(nameof(User32), SetLastError = true, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShutdownBlockReasonQuery(HandleRef hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, ref uint pcchBuff);

		/// <summary>Retrieves the reason string set by the <see cref="ShutdownBlockReasonCreate"/> function.</summary>
		/// <param name="hWnd">A handle to the main window of the application.</param>
		/// <param name="reason">On success, receives the reason string.</param>
		/// <returns>If the call succeeds, the return value is nonzero. If the call fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		public static bool ShutdownBlockReasonQuery(HandleRef hWnd, out string reason)
		{
			uint sz = 0;
			reason = null;
			if (!ShutdownBlockReasonQuery(hWnd, null, ref sz)) return false;
			var sb = new StringBuilder((int)sz);
			if (ShutdownBlockReasonQuery(hWnd, sb, ref sz))
			{
				reason = sb.ToString();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
		/// <para>See [ https://msdn.microsoft.com/en-us/library/windows/desktop/ms644993%28v=vs.85%29.aspx ] for more information</para>
		/// </summary>
		/// <param name="hhk">
		/// C++ ( hhk [in]. Type: HHOOK ) <br/> A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to <see cref="SetWindowsHookEx"/>.
		/// </param>
		/// <returns>
		/// C++ ( Type: BOOL ) <c>true</c> or nonzero if the function succeeds, <c>false</c> or zero if the function fails.
		/// <para>
		/// To get extended error information, call <see
		/// cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms679360%28v=vs.85%29.aspx">GetLastError</see> .
		/// </para>
		/// <para>The return value is the calling thread's last-error code.</para>
		/// The Return Value section of the documentation for each function that sets the last-error code notes the conditions under which the function sets the
		/// last-error code. Most functions that set the thread's last-error code set it when they fail. However, some functions also set the last-error code
		/// when they succeed. If the function is not documented to set the last-error code, the value returned by this function is simply the most recent
		/// last-error code to have been set; some functions set the last-error code to 0 on success and others do not.
		/// <para></para>
		/// </returns>
		/// <remarks>
		/// <para>
		/// The hook procedure can be in the state of being called by another thread even after UnhookWindowsHookEx returns. If the hook procedure is not being
		/// called concurrently, the hook procedure is removed immediately before <see cref="UnhookWindowsHookEx"/> returns.
		/// </para>
		/// <para>
		/// For an example, see <see cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms644960%28v=vs.85%29.aspx#system_events"> Monitoring
		/// System Events </see> .
		/// </para>
		/// </remarks>
		[DllImport(nameof(User32), SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport(nameof(User32), SetLastError = true)]
		public static extern int UnregisterHotKey(HandleRef hWnd, int id);

		/// <summary>Retrieves a handle to the window that contains the specified point.</summary>
		/// <param name="Point">The point to be checked.</param>
		/// <returns>
		/// The return value is a handle to the window that contains the point. If no window exists at the given point, the return value is NULL. If the point is
		/// over a static text control, the return value is a handle to the window under the static text control.
		/// </returns>
		[DllImport(nameof(User32), SetLastError = true)]
		public static extern IntPtr WindowFromPoint(Point Point);

		/// <summary>
		/// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs. The SetWindowLongPtr function fails if the process that owns the window specified by the hWnd parameter is at a higher process privilege in the UIPI hierarchy than the process the calling thread resides in.</param>
		/// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. Alternately, this can be a value from <see cref="WindowLongFlags"/>.</param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified offset. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// <para>If the previous value is zero and the function succeeds, the return value is zero, but the function does not clear the last error information. To determine success or failure, clear the last error information by calling SetLastError with 0, then call SetWindowLongPtr. Function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</para></returns>
		[DllImport(nameof(User32), SetLastError = true, EntryPoint = "SetWindowLong")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
		private static extern int SetWindowLongPtr32(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		/// <summary>
		/// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs. The SetWindowLongPtr function fails if the process that owns the window specified by the hWnd parameter is at a higher process privilege in the UIPI hierarchy than the process the calling thread resides in.</param>
		/// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. Alternately, this can be a value from <see cref="WindowLongFlags"/>.</param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified offset. If the function fails, the return value is zero. To get extended error information, call GetLastError.
		/// <para>If the previous value is zero and the function succeeds, the return value is zero, but the function does not clear the last error information. To determine success or failure, clear the last error information by calling SetLastError with 0, then call SetWindowLongPtr. Function failure will be indicated by a return value of zero and a GetLastError result that is nonzero.</para></returns>
		[DllImport(nameof(User32), SetLastError = true, EntryPoint = "SetWindowLongPtr")]
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
		private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct GRPICONDIR
		{
			/// <summary>Reserved (must be 0)</summary>
			public ushort idReserved;
			/// <summary>Resource type</summary>
			public ResourceType idType;
			/// <summary>Icon count</summary>
			public ushort idCount;
		}

		/// <summary>Represents an icon as stored in a resource</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct GRPICONDIRENTRY
		{
			/// <summary>Width, in pixels, of the image</summary>
			public byte bWidth;
			/// <summary>Height, in pixels, of the image</summary>
			public byte bHeight;
			/// <summary>Number of colors in image (0 if &gt;= 8bpp)</summary>
			public byte bColorCount;
			/// <summary>Reserved</summary>
			public byte bReserved;
			/// <summary>Color Planes</summary>
			public ushort wPlanes;
			/// <summary>Bits per pixel</summary>
			public ushort wBitCount;
			/// <summary>How many bytes in this resource?</summary>
			public uint dwBytesInRes;
			/// <summary>The ID</summary>
			public ushort nId;
		}

		/// <summary>Contains information about a window's maximized size and position and its minimum and maximum tracking size.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			/// <summary>Reserved; do not use.</summary>
			public Point reserved;
			/// <summary>
			/// The maximized width (x member) and the maximized height (y member) of the window. For top-level windows, this value is based on the width of the
			/// primary monitor.
			/// </summary>
			public Size maxSize;
			/// <summary>
			/// The position of the left side of the maximized window (x member) and the position of the top of the maximized window (y member). For top-level
			/// windows, this value is based on the position of the primary monitor.
			/// </summary>
			public Point maxPosition;
			/// <summary>
			/// The minimum tracking width (x member) and the minimum tracking height (y member) of the window. This value can be obtained programmatically from
			/// the system metrics SM_CXMINTRACK and SM_CYMINTRACK (see the GetSystemMetrics function).
			/// </summary>
			public Size minTrackSize;
			/// <summary>
			/// The maximum tracking width (x member) and the maximum tracking height (y member) of the window. This value is based on the size of the virtual
			/// screen and can be obtained programmatically from the system metrics SM_CXMAXTRACK and SM_CYMAXTRACK (see the GetSystemMetrics function).
			/// </summary>
			public Size maxTrackSize;
		}

		/// <summary>
		/// Contains information about a notification message.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			/// <summary>A window handle to the control sending the message.</summary>
			public IntPtr hwndFrom;
			/// <summary>An identifier of the control sending the message.</summary>
			public IntPtr idFrom;
			/// <summary>A notification code. This member can be one of the common notification codes (see Notifications under General Control Reference), or it can be a control-specific notification code.</summary>
			public int code;
		}

		/// <summary>
		/// Contains information about the size and position of a window.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPOS
		{
			/// <summary>A handle to the window.</summary>
			public IntPtr hwnd;
			/// <summary>The position of the window in Z order (front-to-back position). This member can be a handle to the window behind which this window is placed, or can be one of the special values listed with the SetWindowPos function.</summary>
			public IntPtr hwndInsertAfter;
			/// <summary>The position of the left edge of the window.</summary>
			public int x;
			/// <summary>The position of the top edge of the window.</summary>
			public int y;
			/// <summary>The window width, in pixels.</summary>
			public int cx;
			/// <summary>The window height, in pixels.</summary>
			public int cy;
			/// <summary>The window position. This member can be one or more of the following values.</summary>
			public SetWindowPosFlags flags;
		}

		/// <summary>Special window handles</summary>
		public static class SpecialWindowHandles
		{
			/// <summary>
			/// Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the
			/// window loses its topmost status and is placed at the bottom of all other windows.
			/// </summary>
			public static IntPtr HWND_BOTTOM = new IntPtr(1);

			/// <summary>
			/// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no
			/// effect if the window is already a non-topmost window.
			/// </summary>
			public static IntPtr HWND_NOTOPMOST = new IntPtr(-2);

			/// <summary>Places the window at the top of the Z order.</summary>
			public static IntPtr HWND_TOP = new IntPtr(0);

			/// <summary>
			/// Places the window above all non-topmost windows. The window maintains its topmost position even when it
			/// is deactivated.
			/// </summary>
			public static IntPtr HWND_TOPMOST = new IntPtr(-1);
		}
	}
}