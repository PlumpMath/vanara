using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Vanara.Extensions;
using Vanara.PInvoke;

namespace Vanara.Security
{
	/// <summary>
	/// Safe container for an authentication buffer. Allows for creation using native <c>CredPackAuthenticationBuffer</c> method or assignment from an existing
	/// <c>IntPtr</c>. Can unpack to <see cref="string"/> or <see cref="SecureString"/> values.
	/// </summary>
	public class AuthenticationBuffer : IDisposable
	{
		private IntPtr buffer = IntPtr.Zero;
		private int bufferSize;

		public AuthenticationBuffer(string userName, string password)
		{
			var pUserName = new SafeCoTaskMemString(userName ?? "");
			var pPassword = new SafeCoTaskMemString(password ?? "");
			Init(0, pUserName, pPassword);
		}

		public AuthenticationBuffer(SecureString userName, SecureString password)
		{
			var pUserName = new SafeCoTaskMemString(userName);
			var pPassword = new SafeCoTaskMemString(password);
			Init(0, pUserName, pPassword);
		}

		public AuthenticationBuffer(IntPtr authBuffer, int authBufferSize)
		{
			buffer = authBuffer;
			bufferSize = authBufferSize;
		}

		public IntPtr DangerousHandle => buffer;

		public int Size => bufferSize;

		private void Init(CredUI.CredPackFlags flags, SafeCoTaskMemString pUserName, SafeCoTaskMemString pPassword)
		{
			if (!CredUI.CredPackAuthenticationBuffer(flags, (IntPtr)pUserName, (IntPtr)pPassword, IntPtr.Zero, ref bufferSize) && Marshal.GetLastWin32Error() == 122) /*ERROR_INSUFFICIENT_BUFFER*/
			{
				buffer = Marshal.AllocCoTaskMem(bufferSize);
				if (!CredUI.CredPackAuthenticationBuffer(flags, (IntPtr)pUserName, (IntPtr)pPassword, buffer, ref bufferSize))
					throw new Win32Exception();
			}
			else
				throw new Win32Exception();
		}

		public void UnPack(bool decryptProtectedCredentials, out string userName, out string domainName, out string password)
		{
			var pUserName = new StringBuilder(CredUI.CRED_MAX_USERNAME_LENGTH);
			var pDomainName = new StringBuilder(CredUI.CRED_MAX_USERNAME_LENGTH);
			var pPassword = new StringBuilder(CredUI.CREDUI_MAX_PASSWORD_LENGTH);
			int userNameSize = pUserName.Capacity;
			int domainNameSize = pDomainName.Capacity;
			int passwordSize = pPassword.Capacity;

			if (!CredUI.CredUnPackAuthenticationBuffer(decryptProtectedCredentials ? 0x1 : 0x0, buffer, bufferSize,
				pUserName, ref userNameSize, pDomainName, ref domainNameSize, pPassword, ref passwordSize))
				throw new Win32Exception();

			userName = pUserName.ToString();
			domainName = pDomainName.ToString();
			password = pPassword.ToString();
		}

		public void UnPack(bool decryptProtectedCredentials, out SecureString userName, out SecureString domainName, out SecureString password)
		{
			var pUserName = new SafeCoTaskMemString(CredUI.CRED_MAX_USERNAME_LENGTH);
			var pDomainName = new SafeCoTaskMemString(CredUI.CRED_MAX_USERNAME_LENGTH);
			var pPassword = new SafeCoTaskMemString(CredUI.CREDUI_MAX_PASSWORD_LENGTH);
			int userNameSize = pUserName.Size;
			int domainNameSize = pDomainName.Size;
			int passwordSize = pPassword.Size;

			if (!CredUI.CredUnPackAuthenticationBuffer(decryptProtectedCredentials ? 0x1 : 0x0, buffer, bufferSize,
				(IntPtr)pUserName, ref userNameSize, (IntPtr)pDomainName, ref domainNameSize, (IntPtr)pPassword, ref passwordSize))
				throw new Win32Exception();

			userName = pUserName.DangerousGetHandle().ToSecureString();
			domainName = pDomainName.DangerousGetHandle().ToSecureString();
			password = pPassword.DangerousGetHandle().ToSecureString();
		}

		public void Dispose()
		{
			if (buffer != IntPtr.Zero)
				Marshal.FreeCoTaskMem(buffer);
		}

		public static implicit operator IntPtr(AuthenticationBuffer b) => b.buffer;
	}
}