using System;
using System.Runtime.InteropServices;
// ReSharper disable FieldCanBeMadeReadOnly.Global

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class ComCtl32
	{
		public const string WC_IPADDRESS = "SysIPAddress32";
		public const int IPN_FIRST = -860;

		public enum IPAddressMessage
		{
			IPM_CLEARADDRESS = User32.WindowMessage.WM_USER+100, // no parameters
			IPM_SETADDRESS   = User32.WindowMessage.WM_USER+101, // lparam = TCP/IP address
			IPM_GETADDRESS   = User32.WindowMessage.WM_USER+102, // lresult = # of non black fields.  lparam = LPDWORD for TCP/IP address
			IPM_SETRANGE     = User32.WindowMessage.WM_USER+103, // wparam = field, lparam = range
			IPM_SETFOCUS     = User32.WindowMessage.WM_USER+104, // wparam = field
			IPM_ISBLANK      = User32.WindowMessage.WM_USER+105, // no parameters
		}

		public enum IPAddressNotification
		{
			IPN_FIELDCHANGED = IPN_FIRST - 0
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMIPADDRESS
		{
			public User32.NMHDR hdr;
			public int iField;
			public int iValue;
		}

		public static UIntPtr MAKEIPRANGE(byte low, byte high) => (UIntPtr)((high << 8) + low);

		public static UIntPtr MAKEIPADDRESS(byte b1, byte b2, byte b3, byte b4) => new UIntPtr(((uint)b1 << 24) | ((uint)b2 << 16) | ((uint)b3 << 8) | b4);

		public static UIntPtr MAKEIPADDRESS(byte[] bytes)
		{
			if (bytes != null && bytes.Length != 4)
				throw new ArgumentOutOfRangeException(nameof(bytes), "Array must contain exactly four items.");
			return bytes == null ? UIntPtr.Zero : MAKEIPADDRESS(bytes[0], bytes[1], bytes[2], bytes[3]);
		}

		public static byte[] GET_IPADDRESS(UIntPtr ipAddress)
		{
			var i = ipAddress.ToUInt32();
			return new[] { (byte)((i >> 24) & 0xff), (byte)((i >> 16) & 0xff), (byte)((i >> 8) & 0xff), (byte)(i & 0xff) };
		}
	}
}