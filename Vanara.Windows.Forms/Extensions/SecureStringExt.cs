using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		public static string ToInsecureString(this SecureString s)
		{
			if (s == null) return null;
			var p = IntPtr.Zero;
			try
			{
				p = Marshal.SecureStringToCoTaskMemUnicode(s);
				return Marshal.PtrToStringUni(p);
			}
			finally
			{
				if (p != IntPtr.Zero)
					Marshal.ZeroFreeCoTaskMemUnicode(p);
			}
		}

		public static SecureString ToSecureString(this IntPtr p)
		{
			if (p == IntPtr.Zero) return null;
			var s = new SecureString();
			var i = 0;
			while (true)
			{
				var c = (char)Marshal.ReadInt16(p, ((i++) * sizeof(short)));
				if (c == '\u0000')
					break;
				s.AppendChar(c);
			}
			s.MakeReadOnly();
			return s;
		}

		public static SecureString ToSecureString(this IntPtr p, int length)
		{
			if (p == IntPtr.Zero) return null;
			var s = new SecureString();
			for (var i = 0; i < length; i++)
				s.AppendChar((char)Marshal.ReadInt16(p, i * sizeof(short)));
			s.MakeReadOnly();
			return s;
		}

		public static SecureString ToSecureString(this string s)
		{
			if (s == null) return null;
			var ss = new SecureString();
			foreach (var c in s)
				ss.AppendChar(c);
			ss.MakeReadOnly();
			return ss;
		}
	}
}