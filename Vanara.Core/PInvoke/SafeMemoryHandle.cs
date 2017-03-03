using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Vanara.Extensions;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	/// <summary>Interface to capture unmanaged memory methods.</summary>
	public interface IMemoryMethods
	{
		/// <summary>Gets the allocation method.</summary>
		Func<int, IntPtr> AllocMem { get; }

		/// <summary>Gets the Ansi <see cref="SecureString"/> allocation method.</summary>
		Func<SecureString, IntPtr> AllocSecureStringAnsi { get; }

		/// <summary>Gets the Unicode <see cref="SecureString"/> allocation method.</summary>
		Func<SecureString, IntPtr> AllocSecureStringUni { get; }

		/// <summary>Gets the Ansi string allocation method.</summary>
		Func<string, IntPtr> AllocStringAnsi { get; }

		/// <summary>Gets the Unicode string allocation method.</summary>
		Func<string, IntPtr> AllocStringUni { get; }

		/// <summary>Gets the free method.</summary>
		Action<IntPtr> FreeMem { get; }

		/// <summary>Gets the Ansi <see cref="SecureString"/> free method.</summary>
		Action<IntPtr> FreeSecureStringAnsi { get; }

		/// <summary>Gets the Unicode <see cref="SecureString"/> free method.</summary>
		Action<IntPtr> FreeSecureStringUni { get; }

		/// <summary>Gets the reallocation method.</summary>
		Func<IntPtr, int, IntPtr> ReAllocMem { get; }
	}

	public abstract class SafeMemoryHandle<TMem> : SafeHandle where TMem : IMemoryMethods, new()
	{
		protected static TMem mm = new TMem();

		/// <summary>Initializes a new instance of the <see cref="SafeMemoryHandle{T}"/> class.</summary>
		/// <param name="size">The size of memory to allocate, in bytes.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">size - The value of this argument must be non-negative</exception>
		protected SafeMemoryHandle(int size = 0) : base(IntPtr.Zero, true)
		{
			if (size < 0)
				throw new ArgumentOutOfRangeException(nameof(size), "The value of this argument must be non-negative");
			Size = size;
			if (size == 0) return;
			RuntimeHelpers.PrepareConstrainedRegions();
			SetHandle(mm.AllocMem(size));
		}

		/// <summary>Initializes a new instance of the <see cref="SafeMemoryHandle{T}"/> class.</summary>
		/// <param name="handle">The handle.</param>
		/// <param name="size">The size of memory allocated to the handle, in bytes.</param>
		/// <param name="ownsHandle">if set to <c>true</c> if this class is responsible for freeing the memory on disposal.</param>
		protected SafeMemoryHandle(IntPtr handle, int size, bool ownsHandle = true) : base(IntPtr.Zero, ownsHandle)
		{
			if (handle != IntPtr.Zero)
				SetHandle(handle);
			Size = size;
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the handle value is invalid.</summary>
		public override bool IsInvalid => handle == IntPtr.Zero;

		/// <summary>Gets the size of the allocated memory block.</summary>
		/// <value>The size of the allocated memory block.</value>
		public int Size { get; protected set; }

		/// <summary>Returns the <see cref="SafeMemoryHandle{T}"/> as an <see cref="IntPtr"/>. This is a dangerous call as the value is mutable.</summary>
		/// <param name="h">The <see cref="SafeMemoryHandle{T}"/> instance.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator IntPtr(SafeMemoryHandle<TMem> h) => h.DangerousGetHandle();

		/// <summary>Reallocates the memory to the new specified size.</summary>
		/// <param name="newSize">The new size.</param>
		public virtual void Resize(int newSize)
		{
			mm.ReAllocMem(handle, newSize);
			Size = newSize;
		}

		protected static T GetDerivedTypeInstance<T>(int size = 0) where T : SafeMemoryHandle<TMem> => Activator.CreateInstance(typeof(T), size) as T;

		/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
		/// releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			mm.FreeMem(handle);
			return true;
		}
	}

	/// <summary>A <see cref="SafeHandle"/> for memory allocated via COM.</summary>
	/// <seealso cref="System.Runtime.InteropServices.SafeHandle"/>
	public abstract class SafeMemoryHandleExt<TMem> : SafeMemoryHandle<TMem> where TMem : IMemoryMethods, new()
	{
		/// <summary>
		/// Maintains reference to other SafeMemoryHandleExt objects, the pointer to which are referred to by this object. This is to ensure that such objects
		/// being referred to wouldn't be unreferenced until this object is active.
		/// </summary>
		private List<SafeMemoryHandle<TMem>> references;

		/// <summary>Initializes a new instance of the <see cref="SafeMemoryHandleExt{T}"/> class.</summary>
		/// <param name="size">The size of memory to allocate, in bytes.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">size - The value of this argument must be non-negative</exception>
		protected SafeMemoryHandleExt(int size = 0) : base(size) { }

		/// <summary>Initializes a new instance of the <see cref="SafeMemoryHandleExt{T}"/> class.</summary>
		/// <param name="handle">The handle.</param>
		/// <param name="size">The size of memory allocated to the handle, in bytes.</param>
		/// <param name="ownsHandle">if set to <c>true</c> if this class is responsible for freeing the memory on disposal.</param>
		protected SafeMemoryHandleExt(IntPtr handle, int size, bool ownsHandle = true) : base(handle, size, ownsHandle) { }

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="bytes">Array of unmanaged pointers</param>
		/// <returns>SafeHGlobalHandle object to an native (unmanaged) array of pointers</returns>
		protected SafeMemoryHandleExt(byte[] bytes) : this(bytes.Length)
		{
			Marshal.Copy(bytes, 0, handle, bytes.Length);
		}

		/// <summary>Allocates from unmanaged memory to represent an array of pointers and marshals the unmanaged pointers (IntPtr) to the native array equivalent.</summary>
		/// <param name="values">Array of unmanaged pointers</param>
		/// <returns>SafeMemoryHandleExt object to an native (unmanaged) array of pointers</returns>
		protected SafeMemoryHandleExt(IntPtr[] values) : this(IntPtr.Size * values.Length)
		{
			Marshal.Copy(values, 0, handle, values.Length);
		}

		/// <summary>Allocates from unmanaged memory to represent a Unicode string (WSTR) and marshal this to a native PWSTR.</summary>
		/// <param name="s">The string value.</param>
		/// <returns>SafeMemoryHandleExt object to an native (unmanaged) Unicode string</returns>
		protected SafeMemoryHandleExt(string s) : this(s == null ? IntPtr.Zero : mm.AllocStringUni(s), (s?.Length + 1) * 2 ?? 0) { }

		/// <summary>Gets the structures.</summary>
		/// <value>The structures.</value>
		private Stack<Tuple<IntPtr, Type>> Structures { get; } = new Stack<Tuple<IntPtr, Type>>();

		/// <summary>
		/// Adds reference to other SafeMemoryHandle objects, the pointer to which are referred to by this object. This is to ensure that such objects being
		/// referred to wouldn't be unreferenced until this object is active. For e.g. when this object is an array of pointers to other objects
		/// </summary>
		/// <param name="children">Collection of SafeMemoryHandle objects referred to by this object.</param>
		public void AddSubReference(IEnumerable<SafeMemoryHandle<TMem>> children)
		{
			if (references == null)
				references = new List<SafeMemoryHandle<TMem>>();
			references.AddRange(children);
		}

		/// <summary>Extracts an enumeration of structures of <typeparamref name="T"/> containing <paramref name="count"/> items.
		/// <note type="note">This call can cause memory exceptions if the pointer does not have sufficient allocated memory to retrieve all the structures.</note>
		/// </summary>
		/// <typeparam name="T">The type of the structures to retrieve.</typeparam>
		/// <param name="count">The number of structures to retrieve.</param>
		/// <param name="prefixBytes">The number of bytes to skip before reading the structures.</param>
		/// <returns>An enumeration of structures of <typeparamref name="T"/>.</returns>
		public IEnumerable<T> ToIEnum<T>(int count, int prefixBytes = 0) where T : struct
		{
			if (IsInvalid) return null;
			//if (Size < Marshal.SizeOf(typeof(T)) * count + prefixBytes)
			//	throw new InsufficientMemoryException("Requested array is larger than the memory allocated.");
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

		/// <summary>Marshals data from this block of memory to a newly allocated managed object of the type specified by a generic type parameter.</summary>
		/// <typeparam name="T">The type of the object to which the data is to be copied. This must be a structure.</typeparam>
		/// <returns>A managed object that contains the data that this <see cref="SafeMemoryHandleExt{T}"/> holds.</returns>
		public T ToStructure<T>() where T : struct
		{
			if (IsInvalid) return default(T);
			//if (Size < Marshal.SizeOf(typeof(T)))
			//	throw new InsufficientMemoryException("Requested structure is larger than the memory allocated.");
			return handle.ToStructure<T>();
		}

		/// <summary>Allocates from unmanaged memory sufficient memory to hold an object of type T.</summary>
		/// <typeparam name="T">Native type</typeparam>
		/// <typeparam name="TRet">The return type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>SafeMemoryHandleExt object to an native (unmanaged) memory block the size of T.</returns>
		protected static TRet Alloc<T, TRet>(T value = default(T)) where T : struct where TRet : SafeMemoryHandleExt<TMem>
		{
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			var ret = GetDerivedTypeInstance<TRet>(Marshal.SizeOf(typeof(T)));
			Marshal.StructureToPtr(value, ret.handle, false);
			return ret;
		}

		/// <summary>
		/// Allocates from unmanaged memory to represent a structure with a variable length array at the end and marshal these structure elements. It is the
		/// callers responsibility to marshal what precedes the trailing array into the unmanaged memory. ONLY structures with attribute StructLayout of
		/// LayoutKind.Sequential are supported.
		/// </summary>
		/// <typeparam name="T">Type of the trailing array of structures</typeparam>
		/// <typeparam name="TRet">The return type.</typeparam>
		/// <param name="values">Collection of structure objects</param>
		/// <param name="count">Number of items in <paramref name="values"/>.</param>
		/// <param name="prefixBytes">Number of bytes preceding the trailing array of structures</param>
		/// <returns>SafeMemoryHandleExt object to an native (unmanaged) structure with a trail array of structures</returns>
		protected static TRet Alloc<T, TRet>(IEnumerable<T> values, int count, int prefixBytes = 0) where T : struct where TRet : SafeMemoryHandleExt<TMem>
		{
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			var sz = Marshal.SizeOf(typeof(T));
			var result = GetDerivedTypeInstance<TRet>(prefixBytes + sz * count);
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
		/// <typeparam name="TRet">The return type.</typeparam>
		/// <returns>SafeMemoryHandleExt object to a native (unmanaged) array of string pointers with trailing strings.</returns>
		protected static TRet Alloc<TRet>(IEnumerable<string> values, CharSet charSet = CharSet.Auto, int prefixBytes = 0) where TRet : SafeMemoryHandleExt<TMem>
		{
			var coll = values as IList<string> ?? new List<string>(values);
			var chSz = charSet == CharSet.Unicode ? 2 : (charSet == CharSet.Auto ? Marshal.SystemDefaultCharSize : 1);
			var sz = coll.Sum(s => ((s?.Length + 1) * chSz ?? 0) + IntPtr.Size);
			var result = GetDerivedTypeInstance<TRet>(prefixBytes + sz);
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
			return base.ReleaseHandle();
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