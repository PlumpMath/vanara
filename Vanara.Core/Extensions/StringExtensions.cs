using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Vanara.Extensions
{
	public static partial class StringExtensions
	{
		internal const int cbBuffer = 256;

		/// <summary>
		/// Frees the unmanaged memory associated with a string and resets it size.
		/// </summary>
		/// <param name="ptr">The PTR.</param>
		/// <param name="size">The size.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void FreeString(ref IntPtr ptr, ref uint size)
		{
			if (ptr == IntPtr.Zero) return;
			Marshal.FreeHGlobal(ptr);
			ptr = IntPtr.Zero;
			size = 0;
		}

		/// <summary>
		/// Allocates the specified number of bytes and associates them with the supplied <c>IntPtr</c>. If the supplied <c>IntPtr</c> is initialized, it will attempt to free the memory first.
		/// </summary>
		/// <param name="ptr">A reference to an initialized or uninitialized memory pointer.</param>
		/// <param name="size">The size of the new unmanaged memory buffer in bytes.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void AllocString(ref IntPtr ptr, ref uint size)
		{
			FreeString(ref ptr, ref size);
			if (size == 0) size = cbBuffer;
			ptr = Marshal.AllocHGlobal(cbBuffer);
		}

		/// <summary>
		/// Gets the Unicode string pointed to by the pointer.
		/// </summary>
		/// <param name="pString">The address of the first character of the unmanaged string.</param>
		/// <returns>A managed string that holds a copy of the unmanaged string if the value of the <paramref name="pString"/> parameter is not null; otherwise, this method returns null.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static string GetString(this IntPtr pString) => Marshal.PtrToStringUni(pString);

		/// <summary>
		/// Gets an enumerated list of strings from a block of unmanaged memory where each string is separated by a single '\0' character and is terminated by two '\0' characters.
		/// </summary>
		/// <param name="pNullSepStringList">The address of the first character of the list of '\0' separated unmanaged strings.</param>
		/// <param name="charLength">Length in bytes of a single character in the string. If left at the default value of 0, the default character size for the assembly will be used.</param>
		/// <returns>An enumerated list of strings.</returns>
		public static IEnumerable<string> GetStrings(this IntPtr pNullSepStringList, int charLength = 0)
		{
			if (charLength == 0) charLength = Marshal.SystemDefaultCharSize;
			var chars = new char[charLength];
			Marshal.Copy(pNullSepStringList, chars, 0, charLength);
			var start = 0;
			for (var i = 0; i < charLength - 1; i++)
			{
				if (chars[i] != '\0') continue;
				if (i > 0 && chars[i - 1] == '\0') break;
				yield return new string(chars, start, i - start);
				start = i + 1;
			}
		}

		/// <summary>
		/// Looks at the current string assigned to the memory location and if different that the supplied value, will deallocate it and reallocate and copy the new Unicode string. If the new value is null, the pointer will be freed and assigned a null pointer (<c>IntPtr.Zero</c>).
		/// </summary>
		/// <param name="ptr">An empty pointer or a pointer to allocated memory holding a string.</param>
		/// <param name="size">On input, the size of the current string in bytes. On return, this value contains the number of bytes allocated for the new Unicode string.</param>
		/// <param name="value">The string value, which can be <c>null</c>.</param>
		/// <returns><c>true</c> if the value of <paramref name="ptr"/> changes; otherwise, <c>false</c>.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static bool SetString(ref IntPtr ptr, ref uint size, string value = null)
		{
			var s = GetString(ptr);
			if (value == string.Empty) value = null;
			if (string.CompareOrdinal(s, value) != 0)
			{
				FreeString(ref ptr, ref size);
				if (value != null)
				{
					ptr = Marshal.StringToHGlobalUni(value);
					size = (uint)(value.Length + 1) * 2;
				}
				return true;
			}
			return false;
		}
	}
}