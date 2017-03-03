using System;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	/// <summary>Represents methods in <c>windows.h</c>.</summary>
	public static partial class Windows
	{
		/// <summary>Gets an array of unmanaged bytes that hold an empty <see cref="Guid"/> value.</summary>
		private static SafeByteArray EmptyGuidBytes { get; } = new SafeByteArray(Guid.Empty.ToByteArray());

		/// <summary>Gets a pointer to an empty GUID value.</summary>
		public static IntPtr EmptyGuidPtr => (IntPtr)EmptyGuidBytes;

		/// <summary>Retrieves the high-order byte from the given 16-bit value.</summary>
		/// <param name="wValue">The value to be converted.</param>
		/// <returns>The return value is the high-order byte of the specified value.</returns>
		public static byte HIBYTE(ushort wValue) => (byte)((wValue >> 8) & 0xff);

		/// <summary>Retrieves the high-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the high-order word of the specified value.</returns>
		public static ushort HIWORD(uint dwValue) => (ushort)((dwValue >> 16) & 0xffff);

		/// <summary>Retrieves the high-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the high-order word of the specified value.</returns>
		public static ushort HIWORD(IntPtr dwValue) => HIWORD((uint)Convert.ToUInt64(dwValue.ToInt64()));

		/// <summary>Retrieves the high-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the high-order word of the specified value.</returns>
		public static ushort HIWORD(UIntPtr dwValue) => HIWORD((uint)dwValue.ToUInt64());

		/// <summary>Retrieves the low-order byte from the given 16-bit value.</summary>
		/// <param name="wValue">The value to be converted.</param>
		/// <returns>The return value is the low-order byte of the specified value.</returns>
		public static byte LOBYTE(ushort wValue) => (byte)(wValue & 0xff);

		/// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the low-order word of the specified value.</returns>
		public static ushort LOWORD(uint dwValue) => (ushort)(dwValue & 0xffff);

		/// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the low-order word of the specified value.</returns>
		public static ushort LOWORD(IntPtr dwValue) => LOWORD((uint)Convert.ToUInt64(dwValue.ToInt64()));

		/// <summary>Retrieves the low-order word from the specified 32-bit value.</summary>
		/// <param name="dwValue">The value to be converted.</param>
		/// <returns>The return value is the low-order word of the specified value.</returns>
		public static ushort LOWORD(UIntPtr dwValue) => LOWORD((uint)dwValue.ToUInt64());

		/// <summary>
		/// Converts an integer value to a resource type compatible with the resource-management functions. This macro is used in place of a string containing
		/// the name of the resource.
		/// </summary>
		/// <param name="id">The integer value to be converted.</param>
		/// <returns>The return value is string representation of the integer value.</returns>
		public static ResourceName MAKEINTRESOURCE(int id) => new ResourceName(id);

		/// <summary>Creates a LONG value by concatenating the specified values.</summary>
		/// <param name="wLow">The low-order word of the new value.</param>
		/// <param name="wHigh">The high-order word of the new value.</param>
		/// <returns>The return value is a LONG value.</returns>
		public static uint MAKELONG(ushort wLow, ushort wHigh) => ((uint)wHigh << 16) | ((uint)wLow & 0xffff);

		/// <summary>Creates a LONG value by concatenating the specified values.</summary>
		/// <param name="wLow">The low-order word of the new value.</param>
		/// <param name="wHigh">The high-order word of the new value.</param>
		/// <returns>The return value is a LONG value.</returns>
		public static int MAKELONG(short wLow, short wHigh) => ((int)wHigh << 16) | ((int)wLow & 0xffff);

		/// <summary>Creates a LONG64 value by concatenating the specified values.</summary>
		/// <param name="dwLow">The low-order double word of the new value.</param>
		/// <param name="dwHigh">The high-order double word of the new value.</param>
		/// <returns>The return value is a LONG64 value.</returns>
		public static ulong MAKELONG64(uint dwLow, uint dwHigh) => ((ulong)dwHigh << 32) | ((ulong)dwLow & 0xffffffff);

		/// <summary>Creates a LONG64 value by concatenating the specified values.</summary>
		/// <param name="dwLow">The low-order double word of the new value.</param>
		/// <param name="dwHigh">The high-order double word of the new value.</param>
		/// <returns>The return value is a LONG64 value.</returns>
		public static long MAKELONG64(int dwLow, int dwHigh) => ((long)dwHigh << 32) | ((long)dwLow & 0xffffffff);

		/// <summary>Creates a value for use as an lParam parameter in a message. The macro concatenates the specified values.</summary>
		/// <param name="wLow">The low-order word of the new value.</param>
		/// <param name="wHigh">The high-order word of the new value.</param>
		/// <returns>The return value is an LPARAM value.</returns>
		public static IntPtr MAKELPARAM(int wLow, int wHigh) => (IntPtr)((wHigh << 16) | (wLow & 0xffff));

		/// <summary>Creates a WORD value by concatenating the specified values.</summary>
		/// <param name="bLow">The low-order byte of the new value.</param>
		/// <param name="bHigh">The high-order byte of the new value.</param>
		/// <returns>The return value is a WORD value.</returns>
		public static ushort MAKEWORD(byte bLow, byte bHigh) => (ushort)(bHigh << 8 | bLow & 0xff);

		/// <summary>Retrieves the high-order 16-bit value from the specified 32-bit value.</summary>
		/// <param name="iValue">The value to be converted.</param>
		/// <returns>The return value is the high-order 16-bit value of the specified value.</returns>
		public static short SignedHIWORD(int iValue) => (short)((iValue >> 16) & 0xffff);

		/// <summary>Retrieves the high-order 16-bit value from the specified 32-bit value.</summary>
		/// <param name="iValue">The value to be converted.</param>
		/// <returns>The return value is the high-order 16-bit value of the specified value.</returns>
		public static short SignedHIWORD(IntPtr iValue) => SignedHIWORD((int)iValue.ToInt64());

		/// <summary>Retrieves the low-order 16-bit value from the specified 32-bit value.</summary>
		/// <param name="iValue">The value to be converted.</param>
		/// <returns>The return value is the low-order 16-bit value of the specified value.</returns>
		public static short SignedLOWORD(int iValue) => (short)(iValue & 0xffff);

		/// <summary>Retrieves the low-order 16-bit value from the specified 32-bit value.</summary>
		/// <param name="iValue">The value to be converted.</param>
		/// <returns>The return value is the low-order 16-bit value of the specified value.</returns>
		public static short SignedLOWORD(IntPtr iValue) => SignedLOWORD((int)iValue.ToInt64());

		/// <summary>Creates a <see cref="FILETIME"/> from a 64-bit value.</summary>
		/// <param name="ul">The value to be converted.</param>
		/// <returns>The return value is a <see cref="FILETIME"/> created from the supplied 64-bit value.</returns>
		public static FILETIME MakeFILETIME(ulong ul) => new FILETIME { dwHighDateTime = (int)(ul >> 32), dwLowDateTime = (int)(ul & 0xFFFFFFFF) };

		/// <summary>Converts a <see cref="FILETIME"/> structure to its 64-bit representation.</summary>
		/// <param name="ft">The value to be converted.</param>
		/// <returns>The return value is a 64-bit value that represented the <see cref="FILETIME"/>.</returns>
		public static ulong ToUInt64(this FILETIME ft) => ((ulong)ft.dwHighDateTime << 32) | (uint)ft.dwLowDateTime;

		/// <summary>Returns a <see cref="string"/> that represents the <see cref="FILETIME"/> instance.</summary>
		/// <param name="ft">The <see cref="FILETIME"/> to convert.</param>
		/// <param name="format">A standard or custom date and time format string. See notes for <a href="https://msdn.microsoft.com/en-us/library/8tfzyc64(v=vs.110).aspx">DateTime.ToString()</a>.</param>
		/// <param name="provider">An object that supplies culture-specific formatting information.</param>
		/// <returns>
		/// A string representation of value of the current <see cref="FILETIME"/> object as specified by <paramref name="format"/> and <paramref name="provider"/>.
		/// </returns>
		public static string ToString(this FILETIME ft, string format, IFormatProvider provider = null) => DateTime.FromFileTimeUtc((long)ft.ToUInt64()).ToString(format, provider);
	}

	//public static T GetLParam<T>(this System.Windows.Forms.Message msg) => (T)msg.GetLParam(typeof(T));

	/*private static int GetEmbeddedNullStringLengthAnsi(string s)
	{
		int index = s.IndexOf('\0');
		if (index > -1)
		{
			string str = s.Substring(0, index);
			string str2 = s.Substring(index + 1);
			return ((GetPInvokeStringLength(str) + GetEmbeddedNullStringLengthAnsi(str2)) + 1);
		}
		return GetPInvokeStringLength(s);
	}

	public static int GetPInvokeStringLength(string s)
	{
		if (string.IsNullOrEmpty(s))
			return 0;
		if (Marshal.SystemDefaultCharSize == 2)
			return s.Length;
		if (s.IndexOf('\0') > -1)
			return GetEmbeddedNullStringLengthAnsi(s);
		return lstrlen(s);
	}*/
}