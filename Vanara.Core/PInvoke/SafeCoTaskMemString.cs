using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Vanara.PInvoke
{
	/// <summary>Safely handles an unmanaged memory allocated Unicode string.</summary>
	/// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid"/>
	public class SafeCoTaskMemString : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemString"/> class.</summary>
		/// <param name="s">The s.</param>
		public SafeCoTaskMemString(string s) : this(Marshal.StringToCoTaskMemUni(s))
		{
			Length = s?.Length ?? 0;
			Size = Length == 0 ? 0 : (Length + 1) * 2;
		}

		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemString"/> class.</summary>
		/// <param name="s">The s.</param>
		public SafeCoTaskMemString(SecureString s) : this(Marshal.SecureStringToCoTaskMemUnicode(s))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemString"/> class.</summary>
		/// <param name="size">The size of the buffer in bytes. This must be more than 2 and an even number.</param>
		public SafeCoTaskMemString(int size) : this(Marshal.AllocCoTaskMem(size))
		{
			if (size < 2) throw new ArgumentOutOfRangeException(nameof(size), @"A string must, at a minimum, occupy 2 bytes of allocated memory.");
			if (size % 2 != 0) throw new ArgumentOutOfRangeException(nameof(size), @"A Unicode string buffer must be a multiple of 2.");
			Size = size;
			Length = 0;
			// Write '\0' into the first character.
			Marshal.Copy(new byte[] {0, 0}, 0, handle, 2);
		}

		/// <summary>Prevents a default instance of the <see cref="SafeCoTaskMemString"/> class from being created.</summary>
		private SafeCoTaskMemString() : base(true) { }

		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemString"/> class.</summary>
		/// <param name="ptr">The PTR.</param>
		private SafeCoTaskMemString(IntPtr ptr) : base(true)
		{
			SetHandle(ptr);
		}

		/// <summary>Gets the length of the string or -1 if the length is unknown (for example if it is holding a <see cref="SecureString"/>.</summary>
		/// <value>The string length.</value>
		public int Length { get; } = -1;

		/// <summary>Gets the number of allocated bytes or -1 if the size is unknown (for example if it is holding a <see cref="SecureString"/>.</summary>
		/// <value>The number of allocated bytes.</value>
		public int Size { get; } = -1;

		/// <summary>Returns the value of the <see cref="SafeHandle.handle"/> field. This</summary>
		/// <param name="s">The <see cref="SafeCoTaskMemString"/> instance.</param>
		/// <returns>
		/// An <see cref="IntPtr"/> representing the value of the handle field. If the handle has been marked invalid with <see
		/// cref="SafeHandle.SetHandleAsInvalid"/>, this method still returns the original handle value, which can be a stale value.
		/// </returns>
		public static explicit operator IntPtr(SafeCoTaskMemString s) => s.DangerousGetHandle();

		/// <summary>Returns the string value held by a <see cref="SafeCoTaskMemString"/>.</summary>
		/// <param name="s">The <see cref="SafeCoTaskMemString"/> instance.</param>
		/// <returns>A <see cref="System.String"/> value held by the <see cref="SafeCoTaskMemString"/> or <c>null</c> if the handle or value is invalid.</returns>
		public static implicit operator string(SafeCoTaskMemString s)
		{
			try { return s == null || s.IsInvalid ? null : Marshal.PtrToStringUni(s.DangerousGetHandle()); }
			catch { return null; }
		}

		/// <summary>Returns the string value held by this instance.</summary>
		/// <returns>A <see cref="System.String"/> value held by this instance or <c>null</c> if the handle is invalid.</returns>
		public override string ToString() => this;

		/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
		/// releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			Marshal.ZeroFreeCoTaskMemUnicode(handle);
			return true;
		}
	}
}