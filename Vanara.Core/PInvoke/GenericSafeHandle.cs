using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
	/// <summary>A <see cref="SafeHandle"/> that takes a delegate in the constructor that closes the supplied handle.</summary>
	/// <seealso cref="SafeHandle"/>
	public class GenericSafeHandle : SafeHandle
	{
		private readonly HandleCloser closeMethod;

		/// <summary>A delegate definition for a method that closes a handle.</summary>
		/// <param name="ptr">The handle.</param>
		/// <returns><c>true</c> if the handle is successfully closed; otherwise <c>false</c>.</returns>
		public delegate bool HandleCloser(IntPtr ptr);

		/// <summary>Initializes a new instance of the <see cref="GenericSafeHandle"/> class.</summary>
		/// <param name="closeMethod">The delegate method for closing the handle.</param>
		protected GenericSafeHandle(HandleCloser closeMethod) : this(IntPtr.Zero, closeMethod, true) { }

		/// <summary>Initializes a new instance of the <see cref="GenericSafeHandle"/> class.</summary>
		/// <param name="ptr">The PTR.</param>
		/// <param name="closeMethod">The delegate method for closing the handle.</param>
		/// <param name="ownsHandle">if set to <c>true</c> [owns handle].</param>
		/// <exception cref="System.ArgumentNullException">closeMethod</exception>
		public GenericSafeHandle(IntPtr ptr, HandleCloser closeMethod, bool ownsHandle = true) : base(ptr, ownsHandle)
		{
			if (closeMethod == null)
				throw new ArgumentNullException(nameof(closeMethod));
			this.closeMethod = closeMethod;
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the handle value is invalid.</summary>
		public override bool IsInvalid => handle == IntPtr.Zero;

		/// <summary>Performs an implicit conversion from <see cref="GenericSafeHandle"/> to <see cref="IntPtr"/>.</summary>
		/// <param name="h">The <see cref="GenericSafeHandle"/> instance.</param>
		/// <returns>The value of the handle. Use caution when using this value as it can be closed by the disposal of the parent <see cref="GenericSafeHandle"/>.</returns>
		public static implicit operator IntPtr(GenericSafeHandle h) => h.DangerousGetHandle();

		/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
		/// releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle() => IsInvalid || closeMethod(handle);
	}
}