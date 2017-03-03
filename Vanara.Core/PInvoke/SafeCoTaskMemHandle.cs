using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace Vanara.PInvoke
{
	/// <summary>Unmanaged memory methods for COM.</summary>
	/// <seealso cref="Vanara.PInvoke.IMemoryMethods"/>
	public sealed class CoTaskMemoryMethods : IMemoryMethods
	{
		/// <summary>Gets the allocation method.</summary>
		public Func<int, IntPtr> AllocMem => Marshal.AllocCoTaskMem;
		/// <summary>Gets the reallocation method.</summary>
		public Func<IntPtr, int, IntPtr> ReAllocMem => Marshal.ReAllocCoTaskMem;
		/// <summary>Gets the free method.</summary>
		public Action<IntPtr> FreeMem => Marshal.FreeCoTaskMem;
		/// <summary>Gets the Unicode string allocation method.</summary>
		public Func<string, IntPtr> AllocStringUni => Marshal.StringToCoTaskMemUni;
		/// <summary>Gets the Ansi string allocation method.</summary>
		public Func<string, IntPtr> AllocStringAnsi => Marshal.StringToCoTaskMemAnsi;
		/// <summary>Gets the Unicode <see cref="SecureString"/> allocation method.</summary>
		public Func<SecureString, IntPtr> AllocSecureStringUni => Marshal.SecureStringToCoTaskMemUnicode;
		/// <summary>Gets the Ansi <see cref="SecureString"/> allocation method.</summary>
		public Func<SecureString, IntPtr> AllocSecureStringAnsi => Marshal.SecureStringToCoTaskMemAnsi;
		/// <summary>Gets the Unicode <see cref="SecureString"/> free method.</summary>
		public Action<IntPtr> FreeSecureStringUni => Marshal.ZeroFreeCoTaskMemUnicode;
		/// <summary>Gets the Ansi <see cref="SecureString"/> free method.</summary>
		public Action<IntPtr> FreeSecureStringAnsi => Marshal.ZeroFreeCoTaskMemAnsi;
	}

	/// <summary>A <see cref="SafeHandle"/> for memory allocated via COM.</summary>
	/// <seealso cref="System.Runtime.InteropServices.SafeHandle"/>
	public class SafeCoTaskMemHandle : SafeMemoryHandleExt<CoTaskMemoryMethods>
	{
		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemHandle"/> class.</summary>
		/// <param name="handle">The handle.</param>
		/// <param name="size">The size of memory allocated to the handle, in bytes.</param>
		/// <param name="ownsHandle">if set to <c>true</c> if this class is responsible for freeing the memory on disposal.</param>
		public SafeCoTaskMemHandle(IntPtr handle, int size, bool ownsHandle = true) : base(handle, size, ownsHandle) { }

		/// <summary>Initializes a new instance of the <see cref="SafeCoTaskMemHandle"/> class.</summary>
		/// <param name="size">The size of memory to allocate, in bytes.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">size - The value of this argument must be non-negative</exception>
		public SafeCoTaskMemHandle(int size = 0) : base(size) { }

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="bytes">Array of unmanaged pointers</param>
		/// <returns>SafeCoTaskMemHandle object to an native (unmanaged) array of pointers</returns>
		public SafeCoTaskMemHandle(byte[] bytes) : base(bytes) { }

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="values">Array of unmanaged pointers</param>
		/// <returns>SafeCoTaskMemHandle object to an native (unmanaged) array of pointers</returns>
		public SafeCoTaskMemHandle(IntPtr[] values) : base(values) { }

		/// <summary>Allocates from unmanaged memory to represent a Unicode string (WSTR) and marshal this to a native PWSTR.</summary>
		/// <param name="s">The string value.</param>
		/// <returns>SafeCoTaskMemHandle object to an native (unmanaged) Unicode string</returns>
		public SafeCoTaskMemHandle(string s) : base(s) { }

		/// <summary>Represents a NULL memory pointer.</summary>
		public static SafeCoTaskMemHandle Null { get; } = new SafeCoTaskMemHandle(IntPtr.Zero, 0, false);

		/// <summary>Converts an <see cref="IntPtr"/> to a <see cref="SafeCoTaskMemHandle"/> where it owns the reference.</summary>
		/// <param name="ptr">The <see cref="IntPtr"/>.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator SafeCoTaskMemHandle(IntPtr ptr) => new SafeCoTaskMemHandle(ptr, 0, true);

		/// <summary>Allocates from unmanaged memory sufficient memory to hold an object of type T.</summary>
		/// <typeparam name="T">Native type</typeparam>
		/// <param name="value">The value.</param>
		/// <returns><see cref="SafeCoTaskMemHandle"/> object to an native (unmanaged) memory block the size of T.</returns>
		public static SafeCoTaskMemHandle Alloc<T>(T value = default(T)) where T : struct => Alloc<T, SafeCoTaskMemHandle>(value);

		/// <summary>
		/// Allocates from unmanaged memory to represent an array of structures and marshals the structure elements to the native array of structures. ONLY
		/// structures with attribute StructLayout of LayoutKind.Sequential are supported.
		/// </summary>
		/// <typeparam name="T">Native structure type</typeparam>
		/// <param name="values">Collection of structure objects</param>
		/// <returns><see cref="SafeCoTaskMemHandle"/> object to an native (unmanaged) array of structures</returns>
		public static SafeCoTaskMemHandle Alloc<T>(ICollection<T> values) where T : struct => Alloc<T, SafeCoTaskMemHandle>(values, values.Count, 0);

		/// <summary>
		/// Allocates from unmanaged memory to represent a structure with a variable length array at the end and marshal these structure elements. It is the
		/// callers responsibility to marshal what precedes the trailing array into the unmanaged memory. ONLY structures with attribute StructLayout of
		/// LayoutKind.Sequential are supported.
		/// </summary>
		/// <typeparam name="T">Type of the trailing array of structures</typeparam>
		/// <param name="values">Collection of structure objects</param>
		/// <param name="count">Number of items in <paramref name="values"/>.</param>
		/// <param name="prefixBytes">Number of bytes preceding the trailing array of structures</param>
		/// <returns><see cref="SafeCoTaskMemHandle"/> object to an native (unmanaged) structure with a trail array of structures</returns>
		public static SafeCoTaskMemHandle Alloc<T>(IEnumerable<T> values, int count, int prefixBytes = 0) where T : struct => Alloc<T, SafeCoTaskMemHandle>(values, count, prefixBytes);

		/// <summary>Allocates unmanaged memory to represent an array of pointers to strings that are then packed in memory behind the array.</summary>
		/// <param name="values">The list of strings.</param>
		/// <param name="charSet">The character set.</param>
		/// <param name="prefixBytes">Number of bytes preceding the trailing array of structures</param>
		/// <returns><see cref="SafeCoTaskMemHandle"/> object to a native (unmanaged) array of string pointers with trailing strings.</returns>
		public static SafeCoTaskMemHandle Alloc(IEnumerable<string> values, CharSet charSet = CharSet.Auto, int prefixBytes = 0) => Alloc<SafeCoTaskMemHandle>(values, charSet, prefixBytes);
	}
}