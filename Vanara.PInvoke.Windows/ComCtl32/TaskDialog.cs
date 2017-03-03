using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.User32;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		/// <summary>The signature of the callback that receives messages from the Task Dialog when various events occur.</summary>
		/// <param name="hwnd">The window handle of the</param>
		/// <param name="msg">The message being passed.</param>
		/// <param name="wParam">wParam which is interpreted differently depending on the message.</param>
		/// <param name="lParam">wParam which is interpreted differently depending on the message.</param>
		/// <param name="refData">The reference data that was set to TaskDialog.CallbackData.</param>
		/// <returns>A HRESULT value. The return value is specific to the message being processed.</returns>
		public delegate int TaskDialogCallbackProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, IntPtr refData);

		/// <summary>The TaskDialog common button flags used to specify the built in buttons to show in the TaskDialog.</summary>
		[Flags]
		public enum TaskDialogCommonButtonFlags
		{
			/// <summary>No common buttons.</summary>
			None = 0,

			/// <summary>OK common button. If selected Task Dialog will return DialogResult.OK.</summary>
			TDCBF_OK_BUTTON = 0x0001,

			/// <summary>Yes common button. If selected Task Dialog will return DialogResult.Yes.</summary>
			TDCBF_YES_BUTTON = 0x0002,

			/// <summary>No common button. If selected Task Dialog will return DialogResult.No.</summary>
			TDCBF_NO_BUTTON = 0x0004,

			/// <summary>
			/// Cancel common button. If selected Task Dialog will return DialogResult.Cancel. If this button is specified, the dialog box will respond to
			/// typical cancel actions (Alt-F4 and Escape).
			/// </summary>
			TDCBF_CANCEL_BUTTON = 0x0008,

			/// <summary>Retry common button. If selected Task Dialog will return DialogResult.Retry.</summary>
			TDCBF_RETRY_BUTTON = 0x0010,

			/// <summary>Close common button. If selected Task Dialog will return this value.</summary>
			TDCBF_CLOSE_BUTTON = 0x0020,
		}

		/// <summary>TASKDIALOG_ELEMENTS taken from CommCtrl.h</summary>
		public enum TaskDialogElement
		{
			/// <summary>The content element.</summary>
			TDE_CONTENT,

			/// <summary>Expanded Information.</summary>
			TDE_EXPANDED_INFORMATION,

			/// <summary>Footer.</summary>
			TDE_FOOTER,

			/// <summary>Main Instructions</summary>
			TDE_MAIN_INSTRUCTION
		}

		/// <summary>TASKDIALOG_FLAGS taken from CommCtrl.h.</summary>
		[Flags]
		public enum TaskDialogFlags
		{
			/// <summary>Enable hyperlinks.</summary>
			TDF_ENABLE_HYPERLINKS = 0x0001,

			/// <summary>Use icon handle for main icon.</summary>
			TDF_USE_HICON_MAIN = 0x0002,

			/// <summary>Use icon handle for footer icon.</summary>
			TDF_USE_HICON_FOOTER = 0x0004,

			/// <summary>Allow dialog to be canceled, even if there is no cancel button.</summary>
			TDF_ALLOW_DIALOG_CANCELLATION = 0x0008,

			/// <summary>Use command links rather than buttons.</summary>
			TDF_USE_COMMAND_LINKS = 0x0010,

			/// <summary>Use command links with no icons rather than buttons.</summary>
			TDF_USE_COMMAND_LINKS_NO_ICON = 0x0020,

			/// <summary>Show expanded info in the footer area.</summary>
			TDF_EXPAND_FOOTER_AREA = 0x0040,

			/// <summary>Expand by default.</summary>
			TDF_EXPANDED_BY_DEFAULT = 0x0080,

			/// <summary>Start with verification flag already checked.</summary>
			TDF_VERIFICATION_FLAG_CHECKED = 0x0100,

			/// <summary>Show a progress bar.</summary>
			TDF_SHOW_PROGRESS_BAR = 0x0200,

			/// <summary>Show a marquee progress bar.</summary>
			TDF_SHOW_MARQUEE_PROGRESS_BAR = 0x0400,

			/// <summary>Callback every 200 milliseconds.</summary>
			TDF_CALLBACK_TIMER = 0x0800,

			/// <summary>Center the dialog on the owner window rather than the monitor.</summary>
			TDF_POSITION_RELATIVE_TO_WINDOW = 0x1000,

			/// <summary>Right to Left Layout.</summary>
			TDF_RTL_LAYOUT = 0x2000,

			/// <summary>No default radio button.</summary>
			TDF_NO_DEFAULT_RADIO_BUTTON = 0x4000,

			/// <summary>Task Dialog can be minimized.</summary>
			TDF_CAN_BE_MINIMIZED = 0x8000,

			/// <summary>Don't call SetForegroundWindow() when activating the dialog.</summary>
			TDF_NO_SET_FOREGROUND = 0x00010000,

			/// <summary>
			/// Indicates that the width of the task dialog is determined by the width of its content area. This flag is ignored if cxWidth is not set to 0.
			/// </summary>
			TDF_SIZE_TO_CONTENT = 0x01000000
		}

		/// <summary>TASKDIALOG_ICON_ELEMENTS taken from CommCtrl.h</summary>
		public enum TaskDialogIconElement
		{
			/// <summary>Main instruction icon.</summary>
			TDIE_ICON_MAIN,

			/// <summary>Footer icon.</summary>
			TDIE_ICON_FOOTER
		}

		/// <summary>TaskDialogMessage taken from CommCtrl.h.</summary>
		public enum TaskDialogMessage : uint
		{
			/// <summary>Navigate page.</summary>
			TDM_NAVIGATE_PAGE = User32.WindowMessage.WM_USER + 101,

			/// <summary>Click button.</summary>
			TDM_CLICK_BUTTON = User32.WindowMessage.WM_USER + 102, // wParam = Button ID

			/// <summary>Set Progress bar to be marquee mode.</summary>
			TDM_SET_MARQUEE_PROGRESS_BAR = User32.WindowMessage.WM_USER + 103, // wParam = 0 (nonMarque) wParam != 0 (Marquee)

			/// <summary>Set Progress bar state.</summary>
			TDM_SET_PROGRESS_BAR_STATE = User32.WindowMessage.WM_USER + 104, // wParam = new progress state

			/// <summary>Set progress bar range.</summary>
			TDM_SET_PROGRESS_BAR_RANGE = User32.WindowMessage.WM_USER + 105, // lParam = MAKELPARAM(nMinRange, nMaxRange)

			/// <summary>Set progress bar position.</summary>
			TDM_SET_PROGRESS_BAR_POS = User32.WindowMessage.WM_USER + 106, // wParam = new position

			/// <summary>Set progress bar marquee (animation).</summary>
			TDM_SET_PROGRESS_BAR_MARQUEE = User32.WindowMessage.WM_USER + 107, // wParam = 0 (stop marquee), wParam != 0 (start marquee), lparam = speed (milliseconds between repaints)

			/// <summary>Set a text element of the Task Dialog.</summary>
			TDM_SET_ELEMENT_TEXT = User32.WindowMessage.WM_USER + 108, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)

			/// <summary>Click a radio button.</summary>
			TDM_CLICK_RADIO_BUTTON = User32.WindowMessage.WM_USER + 110, // wParam = Radio Button ID

			/// <summary>Enable or disable a button.</summary>
			TDM_ENABLE_BUTTON = User32.WindowMessage.WM_USER + 111, // lParam = 0 (disable), lParam != 0 (enable), wParam = Button ID

			/// <summary>Enable or disable a radio button.</summary>
			TDM_ENABLE_RADIO_BUTTON = User32.WindowMessage.WM_USER + 112, // lParam = 0 (disable), lParam != 0 (enable), wParam = Radio Button ID

			/// <summary>Check or uncheck the verification checkbox.</summary>
			TDM_CLICK_VERIFICATION = User32.WindowMessage.WM_USER + 113, // wParam = 0 (unchecked), 1 (checked), lParam = 1 (set key focus)

			/// <summary>Update the text of an element (no effect if originally set as null).</summary>
			TDM_UPDATE_ELEMENT_TEXT = User32.WindowMessage.WM_USER + 114, // wParam = element (TASKDIALOG_ELEMENTS), lParam = new element text (LPCWSTR)

			/// <summary>Designate whether a given Task Dialog button or command link should have a User Account Control (UAC) shield icon.</summary>
			TDM_SET_BUTTON_ELEVATION_REQUIRED_STATE = User32.WindowMessage.WM_USER + 115, // wParam = Button ID, lParam = 0 (elevation not required), lParam != 0 (elevation required)

			/// <summary>Refreshes the icon of the task dialog.</summary>
			TDM_UPDATE_ICON = User32.WindowMessage.WM_USER + 116 // wParam = icon element (TASKDIALOG_ICON_ELEMENTS), lParam = new icon (hIcon if TDF_USE_HICON_* was set, PCWSTR otherwise)
		}

		/// <summary>The System icons the TaskDialog supports.</summary>
		[SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")] // Type comes from CommCtrl.h
		public enum TaskDialogIcon : uint
		{
			/// <summary>No Icon.</summary>
			None = 0,

			/// <summary>System warning icon.</summary>
			TD_WARNING_ICON = 0xFFFF, // MAKEINTRESOURCEW(-1)

			/// <summary>System Error icon.</summary>
			TD_ERROR_ICON = 0xFFFE, // MAKEINTRESOURCEW(-2)

			/// <summary>System Information icon.</summary>
			TD_INFORMATION_ICON = 0xFFFD, // MAKEINTRESOURCEW(-3)

			/// <summary>Shield icon.</summary>
			TD_SHIELD_ICON = 0xFFFC, // MAKEINTRESOURCEW(-4)

			/// <summary>Shield icon on a blue background. Only available on Windows 8 and later.</summary>
			TD_SHIELDBUILD_ICON = 0xFFFB, // MAKEINTRESOURCEW(-5)

			/// <summary>Warning Shield icon on a yellow background. Only available on Windows 8 and later.</summary>
			TD_SECURITYWARNING_ICON = 0xFFFA, // MAKEINTRESOURCEW(-6)

			/// <summary>Error Shield icon on a red background. Only available on Windows 8 and later.</summary>
			TD_SECURITYERROR_ICON = 0xFFF9, // MAKEINTRESOURCEW(-7)

			/// <summary>Success Shield icon on a green background. Only available on Windows 8 and later.</summary>
			TD_SECURITYSUCCESS_ICON = 0xFFF8, // MAKEINTRESOURCEW(-8)

			/// <summary>Shield icon on a gray background. Only available on Windows 8 and later.</summary>
			TD_SHIELDGRAY_ICON = 0xFFF7, // MAKEINTRESOURCEW(-9)
		}

		/// <summary>Task Dialog callback notifications.</summary>
		public enum TaskDialogNotification
		{
			/// <summary>Sent by the Task Dialog once the dialog has been created and before it is displayed. The value returned by the callback is ignored.</summary>
			TDN_CREATED = 0,

			// Spec is not clear what this is so not supporting it.
			/// <summary>
			/// Sent by the Task Dialog when a navigation has occurred.
			/// The value returned by the callback is ignored.
			/// </summary>
			TDN_NAVIGATED = 1,

			/// <summary>
			/// Sent by the Task Dialog when the user selects a button or command link in the task dialog. The button ID corresponding to the button selected
			/// will be available in the TaskDialogNotificationArgs. To prevent the Task Dialog from closing, the application must return true, otherwise the
			/// Task Dialog will be closed and the button ID returned to via the original application call.
			/// </summary>
			TDN_BUTTON_CLICKED = 2, // WPARAM = BUTTON ID

			/// <summary>
			/// Sent by the Task Dialog when the user clicks on a hyper-link in the Task Dialog’s content. The string containing the HREF of the hyper-link will
			/// be available in the TaskDialogNotificationArgs. To prevent the TaskDialog from shell executing the hyper-link, the application must return TRUE,
			/// otherwise ShellExecute will be called.
			/// </summary>
			TDN_HYPERLINK_CLICKED = 3, // LPARAM = (LPCWSTR)PSZHREF

			/// <summary>
			/// Sent by the Task Dialog approximately every 200 milliseconds when TaskDialog.CallbackTimer has been set to true. The number of milliseconds since
			/// the dialog was created or the notification returned true is available on the TaskDialogNotificationArgs. To reset the tick count, the application
			/// must return true, otherwise the tick count will continue to increment.
			/// </summary>
			TDN_TIMER = 4, // WPARAM = MILLISECONDS SINCE DIALOG CREATED OR TIMER RESET

			/// <summary>Sent by the Task Dialog when it is destroyed and its window handle no longer valid. The value returned by the callback is ignored.</summary>
			TDN_DESTROYED = 5,

			/// <summary>
			/// Sent by the Task Dialog when the user selects a radio button in the task dialog. The button ID corresponding to the button selected will be
			/// available in the TaskDialogNotificationArgs. The value returned by the callback is ignored.
			/// </summary>
			TDN_RADIO_BUTTON_CLICKED = 6, // WPARAM = RADIO BUTTON ID

			/// <summary>Sent by the Task Dialog once the dialog has been constructed and before it is displayed. The value returned by the callback is ignored.</summary>
			TDN_DIALOG_CONSTRUCTED = 7,

			/// <summary>
			/// Sent by the Task Dialog when the user checks or unchecks the verification checkbox. The verificationFlagChecked value is available on the
			/// TaskDialogNotificationArgs. The value returned by the callback is ignored.
			/// </summary>
			TDN_VERIFICATION_CLICKED = 8, // WPARAM = 1 IF CHECKBOX CHECKED, 0 IF NOT, LPARAM IS UNUSED AND ALWAYS 0

			/// <summary>
			/// Sent by the Task Dialog when the user presses F1 on the keyboard while the dialog has focus. The value returned by the callback is ignored.
			/// </summary>
			TDN_HELP = 9,

			/// <summary>
			/// Sent by the task dialog when the user clicks on the dialog's expando button. The expanded value is available on the TaskDialogNotificationArgs.
			/// The value returned by the callback is ignored.
			/// </summary>
			TDN_EXPANDO_BUTTON_CLICKED = 10 // WPARAM = 0 (DIALOG IS NOW COLLAPSED), WPARAM != 0 (DIALOG IS NOW EXPANDED)
		}

		/// <summary>
		/// The TaskDialog function creates, displays, and operates a task dialog. The task dialog contains application-defined message text and title, icons, and any combination of predefined push buttons. This function does not support the registration of a callback function to receive notifications.
		/// </summary>
		/// <param name="hwndParent">Handle to the owner window of the task dialog to be created. If this parameter is NULL, the task dialog has no owner window.</param>
		/// <param name="hInstance">Handle to the module that contains the icon resource identified by the pszIcon member, and the string resources identified by the pszWindowTitle and pszMainInstruction members. If this parameter is NULL, pszIcon must be NULL or a pointer to a null-terminated, Unicode string that contains a system resource identifier, for example, TD_ERROR_ICON.</param>
		/// <param name="pszWindowTitle">Pointer to the string to be used for the task dialog title. This parameter is a null-terminated, Unicode string that contains either text, or an integer resource identifier passed through the MAKEINTRESOURCE macro. If this parameter is NULL, the filename of the executable program is used.</param>
		/// <param name="pszMainInstruction">Pointer to the string to be used for the main instruction. This parameter is a null-terminated, Unicode string that contains either text, or an integer resource identifier passed through the MAKEINTRESOURCE macro. This parameter can be NULL if no main instruction is wanted.</param>
		/// <param name="pszContent">Pointer to a string used for additional text that appears below the main instruction, in a smaller font. This parameter is a null-terminated, Unicode string that contains either text, or an integer resource identifier passed through the MAKEINTRESOURCE macro. Can be NULL if no additional text is wanted.</param>
		/// <param name="dwCommonButtons">Specifies the push buttons displayed in the dialog box. This parameter may be a combination of flags from the following group.</param>
		/// <param name="pszIcon">Pointer to a string that identifies the icon to display in the task dialog. This parameter must be an integer resource identifier passed to the MAKEINTRESOURCE macro or one of the following predefined values. If this parameter is NULL, no icon will be displayed. If the hInstance parameter is NULL and one of the predefined values is not used, the TaskDialog function fails.</param>
		/// <param name="pnButton">When this function returns, contains a pointer to an integer location that receives one of the standard button result values.</param>
		/// <returns>This function can return one of these values.
		/// <list type="table">
		/// <listheader><term>Return code</term><term>Description</term></listheader>
		/// <item><term>S_OK</term><term>The operation completed successfully.</term></item>
		/// <item><term>E_OUTOFMEMORY</term><term>There is insufficient memory to complete the operation.</term></item>
		/// <item><term>E_INVALIDARG</term><term>One or more arguments are not valid.</term></item>
		/// <item><term>E_FAIL</term><term>The operation failed.</term></item>
		/// </list>
		/// </returns>
		[DllImport(nameof(ComCtl32), CharSet = CharSet.Unicode)]
		public static extern HRESULT TaskDialog(HandleRef hwndParent, Kernel32.SafeLibraryHandle hInstance, string pszWindowTitle, string pszMainInstruction, string pszContent, TaskDialogCommonButtonFlags dwCommonButtons, IntPtr pszIcon, out int pnButton);

		/// <summary>The TaskDialogIndirect function creates, displays, and operates a task dialog. The task dialog contains application-defined icons, messages, title, verification check box, command links, push buttons, and radio buttons. This function can register a callback function to receive notification messages.</summary>
		/// <param name="pTaskConfig">Pointer to a TASKDIALOGCONFIG structure that contains information used to display the task dialog.</param>
		/// <param name="pnButton">Address of a variable that receives one of the button IDs specified in the pButtons member of the pTaskConfig parameter or a standard button ID value.</param>
		/// <param name="pnRadioButton">Address of a variable that receives one of the button IDs specified in the pRadioButtons member of the pTaskConfig parameter. If this parameter is NULL, no value is returned.</param>
		/// <param name="pfVerificationFlagChecked">Address of a variable that receives a value indicating if the verification checkbox was checked when the dialog was dismissed.</param>
		/// <returns>This function can return one of these values.
		/// <list type="table">
		/// <listheader><term>Return code</term><term>Description</term></listheader>
		/// <item><term>S_OK</term><term>The operation completed successfully.</term></item>
		/// <item><term>E_OUTOFMEMORY</term><term>There is insufficient memory to complete the operation.</term></item>
		/// <item><term>E_INVALIDARG</term><term>One or more arguments are not valid.</term></item>
		/// <item><term>E_FAIL</term><term>The operation failed.</term></item>
		/// </list>
		/// </returns>
		[DllImport(nameof(ComCtl32), CharSet = CharSet.Unicode)]
		public static extern HRESULT TaskDialogIndirect(ref TASKDIALOGCONFIG pTaskConfig, out int pnButton, out int pnRadioButton, [MarshalAs(UnmanagedType.Bool)] out bool pfVerificationFlagChecked);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TASKDIALOG_BUTTON
		{
			public int nButtonID;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszButtonText;
		}

		/// <summary>The TASKDIALOGCONFIG structure contains information used to display a task dialog. The TaskDialogIndirect function uses this structure.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		public struct TASKDIALOGCONFIG
		{
			/// <summary>Size of the structure in bytes.</summary>
			public uint cbSize;

			/// <summary>Parent window handle.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr hwndParent;

			/// <summary>Module instance handle for resources.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr hInstance;

			/// <summary>Flags.</summary>
			public TaskDialogFlags dwFlags; // TASKDIALOG_FLAGS (TDF_XXX) flags

			/// <summary>Bit flags for commonly used buttons.</summary>
			public TaskDialogCommonButtonFlags dwCommonButtons; // TASKDIALOG_COMMON_BUTTON (TDCBF_XXX) flags

			/// <summary>Window title.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszWindowTitle; // string or MAKEINTRESOURCE()

			/// <summary>The Main icon. Overloaded member. Can be string, a handle, a special value or a resource ID.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr MainIcon;

			/// <summary>Main Instruction.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszMainInstruction;

			/// <summary>Content.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszContent;

			/// <summary>Count of custom Buttons.</summary>
			public uint cButtons;

			/// <summary>Array of custom buttons.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr pButtons;

			/// <summary>ID of default button.</summary>
			public int nDefaultButton;

			/// <summary>Count of radio Buttons.</summary>
			public uint cRadioButtons;

			/// <summary>Array of radio buttons.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr pRadioButtons;

			/// <summary>ID of default radio button.</summary>
			public int nDefaultRadioButton;

			/// <summary>Text for verification check box. often "Don't ask be again".</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszVerificationText;

			/// <summary>Expanded Information.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszExpandedInformation;

			/// <summary>Text for expanded control.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszExpandedControlText;

			/// <summary>Text for expanded control.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszCollapsedControlText;

			/// <summary>Icon for the footer. An overloaded member link MainIcon.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr FooterIcon;

			/// <summary>Footer Text.</summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszFooter;

			/// <summary>Function pointer for callback.</summary>
			public TaskDialogCallbackProc pfCallbackProc;

			/// <summary>Data that will be passed to the call back.</summary>
			[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
			// Managed code owns actual resource. Passed to native in synchronous call. No lifetime issues.
			public IntPtr lpCallbackData;

			/// <summary>Width of the Task Dialog's area in DLU's.</summary>
			public uint cxWidth; // width of the Task Dialog's client area in DLU's. If 0, Task Dialog will calculate the ideal width.
		}
	}
}