using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Vanara.Extensions
{
	public static partial class InteropExtensions
	{
		public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

		/// <summary>
		/// Marshals data from a managed list of specified type to an unmanaged block of memory.
		/// </summary>
		/// <typeparam name="T">A type of the enumerated managed object that holds the data to be marshaled. The object must be a structure or an instance of a formatted class.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="ptr">The PTR.</param>
		/// <param name="prefixBytes">The prefix bytes.</param>
		public static void MarshalToPtr<T>(this IEnumerable<T> items, IntPtr ptr, int prefixBytes = 0)
		{
			Debug.Assert(typeof(T).StructLayoutAttribute?.Value == LayoutKind.Sequential);
			var stSize = Marshal.SizeOf(typeof(T));
			int i = 0;
			foreach (var item in items)
				Marshal.StructureToPtr(item, new IntPtr(ptr.ToInt64() + prefixBytes + i++ * stSize), false);
		}

		/// <summary>
		/// Marshals data from a managed object to an unmanaged block of memory that is allocated using <see cref="Marshal.AllocCoTaskMem"/>.
		/// </summary>
		/// <typeparam name="T">The type of the managed object.</typeparam>
		/// <param name="value">A managed object that holds the data to be marshaled. The object must be a structure or an instance of a formatted class.</param>
		/// <returns>A pointer to an unmanaged block of memory, which has been allocated using <see cref="Marshal.AllocCoTaskMem"/>.</returns>
		public static IntPtr StructureToPtr<T>(this T value)
		{
			var ret = Marshal.AllocCoTaskMem(Marshal.SizeOf(value));
			Marshal.StructureToPtr(value, ret, false);
			return ret;
		}

		/// <summary>
		/// Converts a structure or null value to an <see cref="IntPtr" />. If memory has not been allocated for the <paramref name="ptr"/>, it will be via a call to <see cref="Marshal.AllocCoTaskMem(int)"/>.
		/// </summary>
		/// <typeparam name="T">Type of the structure.</typeparam>
		/// <param name="value">The structure to convert. If this value is <c>null</c>, <paramref name="ptr"/> will be set to <see cref="IntPtr.Zero"/> and memory will be released.</param>
		/// <param name="ptr">The <see cref="IntPtr" /> that will point to allocated memory holding the structure or <see cref="IntPtr.Zero"/>.</param>
		/// <param name="isEmpty">An optional predicate check to determine if the structure is non-essential and can be replaced with an empty pointer (<c>null</c>).</param>
		public static void StructureToPtr<T>(T value, ref IntPtr ptr, Predicate<T> isEmpty = null)
		{
			if (value == null || isEmpty != null && isEmpty(value))
			{
				if (ptr == IntPtr.Zero) return;
				Marshal.FreeCoTaskMem(ptr);
				ptr = IntPtr.Zero;
			}
			else
			{
				if (ptr == IntPtr.Zero)
					ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(T)));
				Marshal.StructureToPtr(value, ptr, false);
			}
		}

		/// <summary>
		/// Converts an <see cref="IntPtr"/> that points to a C-style array into a CLI array.
		/// </summary>
		/// <typeparam name="TS">Type of native structure used by the C-style array.</typeparam>
		/// <typeparam name="T">Output type for the CLI array. <typeparamref name="TS"/> must be able to convert to <typeparamref name="T"/>.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr"/> pointing to the native array.</param>
		/// <param name="count">The number of items in the native array.</param>
		/// <param name="prefixBytes">Bytes to skip before reading the array.</param>
		/// <returns>An array of type <typeparamref name="T"/> containing the converted elements of the native array.</returns>
		[Obsolete]
		public static T[] ToArray<TS, T>(this IntPtr ptr, int count, int prefixBytes = 0) where TS : IConvertible
		{
			var ret = new T[count];
			var stSize = Marshal.SizeOf(typeof(TS));
			for (var i = 0; i < count; i++)
			{
				var val = ToStructure<TS>(new IntPtr(ptr.ToInt64() + prefixBytes + i * stSize));
				ret[i] = (T)Convert.ChangeType(val, typeof(T));
			}
			return ret;
		}

		/// <summary>
		/// Converts an <see cref="IntPtr"/> that points to a C-style array into a CLI array.
		/// </summary>
		/// <typeparam name="T">Type of native structure used by the C-style array.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr"/> pointing to the native array.</param>
		/// <param name="count">The number of items in the native array.</param>
		/// <param name="prefixBytes">Bytes to skip before reading the array.</param>
		/// <returns>An array of type <typeparamref name="T"/> containing the elements of the native array.</returns>
		[Obsolete]
		public static T[] ToArray<T>(this IntPtr ptr, int count, int prefixBytes = 0)
		{
			var ret = new T[count];
			var stSize = Marshal.SizeOf(typeof(T));
			for (var i = 0; i < count; i++)
				ret[i] = ToStructure<T>(new IntPtr(ptr.ToInt64() + prefixBytes + i * stSize));
			return ret;
		}

		/// <summary>
		/// Converts an <see cref="IntPtr"/> that points to a C-style array into an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The output type. <typeparamref name="TNative"/> must implement <see cref="IConvertible"/> such that it can convert to <typeparamref name="T"/>.</typeparam>
		/// <typeparam name="TNative">Type of native structure used by the C-style array.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr"/> pointing to the native array.</param>
		/// <param name="count">The number of items in the native array.</param>
		/// <param name="prefixBytes">Bytes to skip before reading the array.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> exposing the elements of the native array.</returns>
		public static IEnumerable<T> ToIEnum<T, TNative>(this IntPtr ptr, int count, int prefixBytes = 0) where TNative : IConvertible
		{
			if (count == 0) yield break;
			var stSize = Marshal.SizeOf(typeof(TNative));
			for (var i = 0; i < count; i++)
				yield return (T)Convert.ChangeType(ToStructure<TNative>(new IntPtr(ptr.ToInt64() + prefixBytes + i * stSize)), typeof(T));
		}

		/// <summary>
		/// Converts an <see cref="IntPtr"/> that points to a C-style array into an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The output type. <typeparamref name="TNative"/> must implement <see cref="IConvertible"/> such that it can convert to <typeparamref name="T"/>.</typeparam>
		/// <typeparam name="TNative">Type of native structure used by the C-style array.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr"/> pointing to the native array.</param>
		/// <param name="count">The number of items in the native array.</param>
		/// <param name="converter"></param>
		/// <param name="prefixBytes">Bytes to skip before reading the array.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> exposing the elements of the native array.</returns>
		public static IEnumerable<T> ToIEnum<T, TNative>(this IntPtr ptr, int count, Converter<TNative, T> converter, int prefixBytes = 0)
		{
			if (converter == null) throw new ArgumentNullException(nameof(converter));
			if (count == 0) yield break;
			var stSize = Marshal.SizeOf(typeof(TNative));
			for (var i = 0; i < count; i++)
				yield return converter(ToStructure<TNative>(new IntPtr(ptr.ToInt64() + prefixBytes + i * stSize)));
		}

		/// <summary>
		/// Converts an <see cref="IntPtr"/> that points to a C-style array into an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type of native structure used by the C-style array.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr"/> pointing to the native array.</param>
		/// <param name="count">The number of items in the native array.</param>
		/// <param name="prefixBytes">Bytes to skip before reading the array.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> exposing the elements of the native array.</returns>
		public static IEnumerable<T> ToIEnum<T>(this IntPtr ptr, int count, int prefixBytes = 0)
		{
			if (count == 0) yield break;
			var stSize = Marshal.SizeOf(typeof(T));
			for (var i = 0; i < count; i++)
				yield return ToStructure<T>(new IntPtr(ptr.ToInt64() + prefixBytes + i * stSize));
		}

		/// <summary>
		/// Converts an <see cref="IntPtr" /> to a structure. If pointer has no value, <c>null</c> is returned.
		/// </summary>
		/// <typeparam name="T">Type of the structure.</typeparam>
		/// <param name="ptr">The <see cref="IntPtr" /> that points to allocated memory holding a structure or <see cref="IntPtr.Zero"/>.</param>
		/// <returns>The converted structure or <c>null</c>.</returns>
		public static T? ToNullableStructure<T>(this IntPtr ptr) where T : struct => ptr != IntPtr.Zero ? ptr.ToStructure<T>() : (T?)null;

		/// <summary>Marshals data from an unmanaged block of memory to a newly allocated managed object of the type specified by a generic type parameter.</summary>
		/// <typeparam name="T">The type of the object to which the data is to be copied. This must be a structure.</typeparam>
		/// <param name="ptr">A pointer to an unmanaged block of memory.</param>
		/// <returns>A managed object that contains the data that the <paramref name="ptr"/> parameter points to.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static T ToStructure<T>(this IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));

		/// <summary>Marshals data from an unmanaged block of memory to a managed object.</summary>
		/// <typeparam name="T">The type of the object to which the data is to be copied. This must be a formatted class.</typeparam>
		/// <param name="ptr">A pointer to an unmanaged block of memory.</param>
		/// <param name="instance">The object to which the data is to be copied. This must be an instance of a formatted class.</param>
		/// <returns>A managed object that contains the data that the <paramref name="ptr"/> parameter points to.</returns>
		public static T ToStructure<T>(this IntPtr ptr, [In] T instance) where T : class
		{
			Marshal.PtrToStructure(ptr, instance);
			return instance;
		}
	}
}
