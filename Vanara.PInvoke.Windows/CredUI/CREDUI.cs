using System;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static class CredUI
	{
		public const int CRED_MAX_DOMAIN_TARGET_NAME_LENGTH = 256 + 1 + 80;
		public const int CRED_MAX_USERNAME_LENGTH = (256 + 1 + 256);
		public const int CREDUI_MAX_CAPTION_LENGTH = 128;
		public const int CREDUI_MAX_DOMAIN_TARGET_LENGTH = CREDUI_MAX_USERNAME_LENGTH;
		public const int CREDUI_MAX_MESSAGE_LENGTH = 32767;
		public const int CREDUI_MAX_PASSWORD_LENGTH = (512 / 2);
		public const int CREDUI_MAX_USERNAME_LENGTH = CRED_MAX_USERNAME_LENGTH;

		/// <summary>
		/// Options for the display of the <see cref="CredUIPromptForCredentials"/> and its functionality.
		/// </summary>
		[Flags]
		public enum CredentialsDialogOptions
		{
			/// <summary>
			/// Default flags settings These are the following values:
			/// <see cref="CREDUI_FLAGS_GENERIC_CREDENTIALS"/>, <see cref="CREDUI_FLAGS_ALWAYS_SHOW_UI"/> and <see cref="CREDUI_FLAGS_EXPECT_CONFIRMATION"/>
			/// </summary>
			CREDUI_FLAGS_DEFAULT = CREDUI_FLAGS_GENERIC_CREDENTIALS | CREDUI_FLAGS_ALWAYS_SHOW_UI | CREDUI_FLAGS_EXPECT_CONFIRMATION,
			/// <summary>No options are set.</summary>
			CREDUI_FLAGS_NONE = 0,
			/// <summary>Notify the user of insufficient credentials by displaying the "Logon unsuccessful" balloon tip.</summary>
			CREDUI_FLAGS_INCORRECT_PASSWORD = 0x00001,
			/// <summary>Do not store credentials or display check boxes. You can pass ShowSaveCheckBox with this newDS to display the Save check box only, and the result is returned in the <see cref="System.Windows.Forms.CredentialsDialog.SaveChecked"/> property.</summary>
			CREDUI_FLAGS_DO_NOT_PERSIST = 0x00002,
			/// <summary>Populate the combo box with local administrators only.</summary>
			CREDUI_FLAGS_REQUEST_ADMINISTRATOR = 0x00004,
			/// <summary>Populate the combo box with user name/password only. Do not display certificates or smart cards in the combo box.</summary>
			CREDUI_FLAGS_EXCLUDE_CERTIFICATES = 0x00008,
			/// <summary>Populate the combo box with certificates and smart cards only. Do not allow a user name to be entered.</summary>
			CREDUI_FLAGS_REQUIRE_CERTIFICATE = 0x00010,
			/// <summary>If the check box is selected, show the Save check box and return <c>true</c> in the <see cref="System.Windows.Forms.CredentialsDialog.SaveChecked"/> property, otherwise, return <c>false</c>. Check box uses the value in the <see cref="System.Windows.Forms.CredentialsDialog.SaveChecked"/> property by default.</summary>
			CREDUI_FLAGS_SHOW_SAVE_CHECK_BOX = 0x00040,
			/// <summary>Specifies that a user interface will be shown even if the credentials can be returned from an existing credential in credential manager. This newDS is permitted only if GenericCredentials is also specified.</summary>
			CREDUI_FLAGS_ALWAYS_SHOW_UI = 0x00080,
			/// <summary>Populate the combo box with certificates or smart cards only. Do not allow a user name to be entered.</summary>
			CREDUI_FLAGS_REQUIRE_SMARTCARD = 0x00100,
			/// <summary></summary>
			CREDUI_FLAGS_PASSWORD_ONLY_OK = 0x00200,
			/// <summary></summary>
			CREDUI_FLAGS_VALIDATE_USERNAME = 0x00400,
			/// <summary></summary>
			CREDUI_FLAGS_COMPLETE_USERNAME = 0x00800,
			/// <summary>Do not show the Save check box, but the credential is saved as though the box were shown and selected.</summary>
			CREDUI_FLAGS_PERSIST = 0x01000,
			/// <summary>This newDS is meaningful only in locating a matching credential to pre-fill the dialog box, should authentication fail. When this newDS is specified, wildcard credentials will not be matched. It has no effect when writing a credential. CredUI does not create credentials that contain wildcard characters. Any found were either created explicitly by the user or created programmatically, as happens when a RAS connection is made.</summary>
			CREDUI_FLAGS_SERVER_CREDENTIAL = 0x04000,
			/// <summary>Specifies that the caller will call ConfirmCredentials after checking to determine whether the returned credentials are actually valid. This mechanism ensures that credentials that are not valid are not saved to the credential manager. Specify this newDS in all cases unless DoNotPersist is specified.</summary>
			CREDUI_FLAGS_EXPECT_CONFIRMATION = 0x20000,
			/// <summary>Consider the credentials entered by the user to be generic credentials, rather than windows credentials.</summary>
			CREDUI_FLAGS_GENERIC_CREDENTIALS = 0x40000,
			/// <summary>The credential is a "RunAs" credential. The TargetName parameter specifies the name of the command or program being run. It is used for prompting purposes only.</summary>
			CREDUI_FLAGS_USERNAME_TARGET_CREDENTIALS = 0x80000,
			/// <summary></summary>
			CREDUI_FLAGS_KEEP_USERNAME = 0x100000
		}

		[Flags]
		public enum CredPackFlags
		{
			CRED_PACK_PROTECTED_CREDENTIALS   = 0x1,
			CRED_PACK_WOW_BUFFER              = 0x2,
			CRED_PACK_GENERIC_CREDENTIALS     = 0x4,
			CRED_PACK_ID_PROVIDER_CREDENTIALS = 0x8
		}

		/// <summary>
		/// Options for the display of the <see cref="CredUI.CredUIPromptForWindowsCredentials"/> and its functionality.
		/// </summary>
		[Flags]
		public enum WindowsCredentialsDialogOptions
		{
			/// <summary>No options are set.</summary>
			CREDUIWIN_NONE = 0,
			/// <summary>
			/// The caller is requesting that the credential provider return the user name and password in plain text. This value cannot be combined with SecurePrompt.
			/// </summary>
			CREDUIWIN_GENERIC = 0x00000001,
			/// <summary>The Save check box is displayed in the dialog box.</summary>
			CREDUIWIN_CHECKBOX = 0x00000002,
			/// <summary>
			/// Only credential providers that support the authentication package specified by the authPackage parameter should be enumerated. This value cannot
			/// be combined with InAuthBufferCredentialsOnly.
			/// </summary>
			CREDUIWIN_AUTHPACKAGE_ONLY = 0x00000010,
			/// <summary>
			/// Only the credentials specified by the InAuthBuffer parameter for the authentication package specified by the authPackage parameter should be
			/// enumerated. If this flag is set, and the InAuthBuffer parameter is NULL, the function fails. This value cannot be combined with AuthPackageOnly.
			/// </summary>
			CREDUIWIN_IN_CRED_ONLY = 0x00000020,
			/// <summary>
			/// Credential providers should enumerate only administrators. This value is intended for User Account Control (UAC) purposes only. We recommend that
			/// external callers not set this flag.
			/// </summary>
			CREDUIWIN_ENUMERATE_ADMINS = 0x00000100,
			/// <summary>Only the incoming credentials for the authentication package specified by the authPackage parameter should be enumerated.</summary>
			CREDUIWIN_ENUMERATE_CURRENT_USER = 0x00000200,
			/// <summary>
			/// The credential dialog box should be displayed on the secure desktop. This value cannot be combined with Generic. Windows Vista: This value is not
			/// supported until Windows Vista with SP1.
			/// </summary>
			CREDUIWIN_SECURE_PROMPT = 0x00001000,
			/// <summary>
			/// The credential provider should align the credential BLOB pointed to by the refOutAuthBuffer parameter to a 32-bit boundary, even if the provider
			/// is running on a 64-bit system.
			/// </summary>
			CREDUIWIN_PACK_32_WOW = 0x10000000,
			/// <summary>
			/// The credential dialog box is invoked by the SspiPromptForCredentials function, and the client is prompted before a prior handshake. If
			/// SSPIPFC_NO_CHECKBOX is passed in the pvInAuthBuffer parameter, then the credential provider should not display the check box.
			/// </summary>
			CREDUIWIN_PREPROMPTING = 0X00002000
		}

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CredPackAuthenticationBuffer(CredPackFlags dwFlags, IntPtr pszUserName, IntPtr pszPassword, IntPtr pPackedCredentials, ref int pcbPackedCredentials);

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode, EntryPoint = "CredUIConfirmCredentialsW")]
		public static extern int CredUIConfirmCredentials(string targetName, [MarshalAs(UnmanagedType.Bool)] bool confirm);

		[DllImport(nameof(CredUI), CharSet = CharSet.Auto)]
		public static extern int CredUIParseUserName(string pszUserName, StringBuilder pszUser, int ulUserMaxChars, StringBuilder pszDomain, int ulDomainMaxChars);

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode, EntryPoint = "CredUIPromptForCredentialsW")]
		public static extern int CredUIPromptForCredentials(ref CREDUI_INFO creditUR, string targetName, IntPtr reserved1, int iError, StringBuilder userName, int maxUserName, StringBuilder password, int maxPassword, [MarshalAs(UnmanagedType.Bool)] ref bool pfSave, CredentialsDialogOptions flags);

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode)]
		public static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere, int authError, ref uint authPackage, IntPtr InAuthBuffer, uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, [MarshalAs(UnmanagedType.Bool)] ref bool pfSave, WindowsCredentialsDialogOptions flags);

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, int cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

		[DllImport(nameof(CredUI), CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, int cbAuthBuffer, IntPtr pszUserName, ref int pcchMaxUserName, IntPtr pszDomainName, ref int pcchMaxDomainame, IntPtr pszPassword, ref int pcchMaxPassword);

		[StructLayout(LayoutKind.Sequential)]
		public struct CREDUI_INFO
		{
			public int cbSize;
			public IntPtr hwndParent;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszMessageText;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszCaptionText;
			public IntPtr hbmBanner;

			public CREDUI_INFO(IntPtr hwndOwner, string caption, string message)
			{
				cbSize = Marshal.SizeOf(typeof(CREDUI_INFO));
				hwndParent = hwndOwner;
				if (caption?.Length > CREDUI_MAX_CAPTION_LENGTH)
					throw new ArgumentOutOfRangeException(nameof(caption), $"The caption may not be longer than {CREDUI_MAX_CAPTION_LENGTH}.");
				pszCaptionText = caption ?? string.Empty;
				if (message?.Length > CREDUI_MAX_MESSAGE_LENGTH)
					throw new ArgumentOutOfRangeException(nameof(message), $"The message may not be longer than {CREDUI_MAX_MESSAGE_LENGTH}.");
				pszMessageText = message;
				hbmBanner = IntPtr.Zero;
			}
		}
	}
}