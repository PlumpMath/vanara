using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Vanara.Extensions;

namespace Vanara.PInvoke
{
	/// <summary>A <see cref="SafeHandle"/> for memory allocated via LocalAlloc.</summary>
	/// <seealso cref="System.Runtime.InteropServices.SafeHandle"/>
	public class SafeHGlobalHandle : SafeHandle
	{
		/// <summary>
		/// Maintains reference to other SafeHGlobalHandle objects, the pointer to which are referred to by this object. This is to ensure that such objects
		/// being referred to wouldn't be unreferenced until this object is active.
		/// </summary>
		private List<SafeHGlobalHandle> references;

		/// <summary>Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.</summary>
		public SafeHGlobalHandle() : this(IntPtr.Zero, 0, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.</summary>
		/// <param name="handle">The handle.</param>
		/// <param name="size">The size.</param>
		/// <param name="ownsHandle">if set to <c>true</c> [owns handle].</param>
		public SafeHGlobalHandle(IntPtr handle, int size, bool ownsHandle = true) :
			base(IntPtr.Zero, ownsHandle)
		{
			if (handle != IntPtr.Zero)
				SetHandle(handle);
			Size = size;
		}

		/// <summary>Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.</summary>
		/// <param name="size">The size.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">size - The value of this argument must be non-negative</exception>
		public SafeHGlobalHandle(int size) : this()
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size), "The value of this argument must be non-negative");
			Size = size;
			if (size == 0) return;
			RuntimeHelpers.PrepareConstrainedRegions();
			SetHandle(Marshal.AllocHGlobal(size));
		}

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="bytes">Array of unmanaged pointers</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) array of pointers</returns>
		public SafeHGlobalHandle(byte[] bytes) : this(bytes.Length)
		{
			Marshal.Copy(bytes, 0, handle, bytes.Length);
		}

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="values">Array of unmanaged pointers</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) array of pointers</returns>
		public SafeHGlobalHandle(IntPtr[] values) : this(IntPtr.Size * values.Length)
		{
			Marshal.Copy(values, 0, handle, values.Length);
		}

		/// <summary>Allocates from unmanaged memory to represent a Unicode string (WSTR) and marshal this to a native PWSTR.</summary>
		/// <param name="s">String</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) Unicode string</returns>
		public SafeHGlobalHandle(string s)
			: this(s == null ? IntPtr.Zero : Marshal.StringToHGlobalUni(s), (s?.Length + 1) * 2 ?? 0) { }

		/*
		/// <summary>Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.</summary>
		/// <param name="s">The secure string.</param>
		public SafeHGlobalHandle(Security.SecureString s) :
			base(IntPtr.Zero, p => { Marshal.ZeroFreeGlobalAllocUnicode(p); return true; }, true)
		{
			if (s != null)
			{
				s.MakeReadOnly();
				SetHandle(Marshal.SecureStringToGlobalAllocUnicode(s));
				Size = s.Length;
			}
		}
		*/

		/// <summary>Represents a NULL memory pointer.</summary>
		public static SafeHGlobalHandle Null { get; } = new SafeHGlobalHandle(IntPtr.Zero, 0, false);

		/// <summary>When overridden in a derived class, gets a value indicating whether the handle value is invalid.</summary>
		public override bool IsInvalid => handle == IntPtr.Zero;

		/// <summary>Gets the size of the allocated memory block.</summary>
		/// <value>The sizeof the allocated memory block.</value>
		public int Size { get; private set; }

		/// <summary>Gets the structures.</summary>
		/// <value>The structures.</value>
		private Stack<Tuple<IntPtr, Type>> Structures { get; } = new Stack<Tuple<IntPtr, Type>>();

		/// <summary>Allocates from unmanaged memory sufficient memory to hold an object of type T.</summary>
		/// <typeparam name="T">Native type</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) memory block the size of T.</returns>
		public static SafeHGlobalHandle AllocHGlobal<T>(T value = default(T)) where T : struct
		{
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			var ret = new SafeHGlobalHandle(Marshal.SizeOf(typeof(T)));
			Marshal.StructureToPtr(value, ret.handle, false);
			return ret;
		}

		/// <summary>
		/// Allocates unmanaged memory to represent an array of structures and marshals the structure elements to the native array of structures. ONLY
		/// structures with attribute StructLayout of LayoutKind.Sequential are supported.
		/// </summary>
		/// <typeparam name="T">Native structure type</typeparam>
		/// <param name="values">Collection of structure objects</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) array of structures</returns>
		public static SafeHGlobalHandle AllocHGlobal<T>(ICollection<T> values) where T : struct
		{
			return AllocHGlobal(values, values.Count);
		}

		/// <summary>
		/// Allocates unmanaged memory to represent a structure with a variable length array at the end and marshal these structure elements. It is the
		/// callers responsibility to marshal what precedes the trailing array into the unmanaged memory. ONLY structures with attribute StructLayout of
		/// LayoutKind.Sequential are supported.
		/// </summary>
		/// <typeparam name="T">Type of the trailing array of structures</typeparam>
		/// <param name="values">Collection of structure objects</param>
		/// <param name="count">Number of items in <paramref name="values"/>.</param>
		/// <param name="prefixBytes">Number of bytes preceding the trailing array of structures</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) structure with a trail array of structures</returns>
		public static SafeHGlobalHandle AllocHGlobal<T>(IEnumerable<T> values, int count, int prefixBytes = 0) where T : struct
		{
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			var sz = Marshal.SizeOf(typeof(T));
			var result = new SafeHGlobalHandle(prefixBytes + sz * count);
			var ptr = new IntPtr(result.handle.ToInt64() + prefixBytes);
			foreach (var value in values)
			{
				result.StructureToPtr(value, ptr, false);
				ptr = new IntPtr(ptr.ToInt64() + sz);
			}
			return result;
		}

		/// <summary>Allocates unmanaged memory to represent an array of pointers to strings that are then packed in memory behind the array.</summary>
		/// <param name="values">The list of strings.</param>
		/// <param name="charSet">The character set.</param>
		/// <param name="prefixBytes">Number of bytes preceding the trailing array of structures</param>
		/// <returns>SafeHGlobalHandle object to a native (unmanaged) array of string pointers with trailing strings.</returns>
		public static SafeHGlobalHandle AllocHGlobal(IEnumerable<string> values, CharSet charSet = CharSet.Auto, int prefixBytes = 0)
		{
			var coll = values as IList<string> ?? new List<string>(values);
			var chSz = charSet == CharSet.Unicode ? 2 : (charSet == CharSet.Auto ? Marshal.SystemDefaultCharSize : 1);
			var sz = coll.Sum(s => ((s?.Length + 1) * chSz ?? 0) + IntPtr.Size);
			var result = new SafeHGlobalHandle(prefixBytes + sz);
			var ptr = new IntPtr(result.handle.ToInt64() + prefixBytes);
			var enc = chSz == 1 ? System.Text.Encoding.ASCII : System.Text.Encoding.Unicode;
			var sptr = new IntPtr(ptr.ToInt64() + IntPtr.Size * coll.Count);
			var iptr = ptr;
			foreach (var s in coll)
			{
				var bytes = enc.GetByteCount(s);
				Marshal.Copy(enc.GetBytes(s), 0, sptr, bytes);
				Marshal.WriteIntPtr(iptr, sptr);
				iptr = new IntPtr(iptr.ToInt64() + IntPtr.Size);
				sptr = new IntPtr(sptr.ToInt64() + bytes);
			}
			return result;
		}

		/// <summary>Returns the <see cref="SafeHGlobalHandle"/> as an <see cref="IntPtr"/>. This is a dangerous call as the value is mutable.</summary>
		/// <param name="h">The <see cref="SafeHGlobalHandle"/> instance.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator IntPtr(SafeHGlobalHandle h) => h.DangerousGetHandle();

		/// <summary>Converts an <see cref="IntPtr"/> to a <see cref="SafeHGlobalHandle"/> where it owns the reference.</summary>
		/// <param name="ptr">The <see cref="IntPtr"/>.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator SafeHGlobalHandle(IntPtr ptr) => new SafeHGlobalHandle(ptr, 0, true);

		/// <summary>
		/// Adds reference to other SafeHGlobalHandle objects, the pointer to which are referred to by this object. This is to ensure that such objects being
		/// referred to wouldn't be unreferenced until this object is active. For e.g. when this object is an array of pointers to other objects
		/// </summary>
		/// <param name="children">Collection of SafeHGlobalHandle objects referred to by this object.</param>
		public void AddSubReference(IEnumerable<SafeHGlobalHandle> children)
		{
			if (references == null)
				references = new List<SafeHGlobalHandle>();
			references.AddRange(children);
		}

		/// <summary>Resizes the specified new size.</summary>
		/// <param name="newSize">The new size.</param>
		[SecurityCritical]
		public void Resize(int newSize)
		{
			Marshal.ReAllocHGlobal(handle, (IntPtr)newSize);
			Size = newSize;
		}

		/// <summary>To the i enum.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="count">The count.</param>
		/// <param name="prefixBytes">The prefix bytes.</param>
		/// <returns></returns>
		/// <exception cref="System.InsufficientMemoryException">Requested array is larger than the memory allocated.</exception>
		public IEnumerable<T> ToIEnum<T>(int count, int prefixBytes = 0) where T : struct
		{
			if (IsInvalid)
				return null;
			if (Size < Marshal.SizeOf(typeof(T)) * count + prefixBytes)
				throw new InsufficientMemoryException("Requested array is larger than the memory allocated.");
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			return handle.ToIEnum<T>(count, prefixBytes);
		}

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <param name="len">The length.</param>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public string ToString(int len) => len == -1 ? Marshal.PtrToStringUni(handle) : Marshal.PtrToStringUni(handle, len);

		/// <summary>Returns an enumeration of strings from memory where each string is pointed to by a preceding list of pointers of length <paramref name="count"/>.</summary>
		/// <param name="count">The count of expected strings.</param>
		/// <param name="charSet">The character set of the strings.</param>
		/// <param name="prefixBytes">Number of bytes preceding the array of string pointers.</param>
		/// <returns>Enumeration of strings.</returns>
		public IEnumerable<string> ToStringEnum(int count, CharSet charSet = CharSet.Auto, int prefixBytes = 0)
		{
			var chSz = charSet == CharSet.Unicode ? 2 : (charSet == CharSet.Auto ? Marshal.SystemDefaultCharSize : 1);
			var lPtrVal = handle.ToInt64();
			for (var i = 0; i < count; i++)
			{
				var iptr = new IntPtr(lPtrVal + prefixBytes + i * IntPtr.Size);
				var sptr = Marshal.ReadIntPtr(iptr);
				yield return chSz == 1 ? Marshal.PtrToStringAnsi(sptr) : Marshal.PtrToStringUni(sptr);
			}
		}

		/// <summary>To the structure.</summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		/// <exception cref="System.InsufficientMemoryException">Requested structure is larger than the memory allocated.</exception>
		public T ToStructure<T>() where T : struct
		{
			if (IsInvalid)
				return default(T);
			if (Size < Marshal.SizeOf(typeof(T)))
				throw new InsufficientMemoryException("Requested structure is larger than the memory allocated.");
			return handle.ToStructure<T>();
		}

		/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
		/// releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			while (Structures.Count > 0)
			{
				var t = Structures.Pop();
				Marshal.DestroyStructure(t.Item1, t.Item2);
			}
			Marshal.FreeHGlobal(handle);
			return true;
		}

		/// <summary>Structures to PTR.</summary>
		/// <param name="structure">The structure.</param>
		/// <param name="ptr">The PTR.</param>
		/// <param name="fDeleteOld">if set to <c>true</c> [f delete old].</param>
		private void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld)
		{
			Marshal.StructureToPtr(structure, ptr, fDeleteOld);
			Structures.Push(new Tuple<IntPtr, Type>(ptr, structure.GetType()));
		}
	}
}