using System;
using System.Linq;
using System.DirectoryServices.ActiveDirectory;
using Vanara.PInvoke;

namespace Vanara.Extensions
{
	public static class ActiveDirectoryExtension
	{
		public static string[] CrackNames(this DomainController dc, string[] names = null,
			NTDSApi.DS_NAME_FLAGS flags = NTDSApi.DS_NAME_FLAGS.DS_NAME_NO_FLAGS,
			NTDSApi.DS_NAME_FORMAT formatOffered = NTDSApi.DS_NAME_FORMAT.DS_NT4_ACCOUNT_NAME,
			NTDSApi.DS_NAME_FORMAT formatDesired = NTDSApi.DS_NAME_FORMAT.DS_USER_PRINCIPAL_NAME)
		{
			lock (dc)
				using (var ds = dc.GetHandle())
					return NTDSApi.DsCrackNames(ds, names, flags, formatOffered, formatDesired).Select(r => r.pName).ToArray();
		}

		public static NTDSApi.SafeDsHandle GetHandle(this DomainController dc)
		{
			var hDc = dc.GetPropertyValue("Handle", IntPtr.Zero);
			if (hDc == IntPtr.Zero) throw new InvalidOperationException();
			return new NTDSApi.SafeDsHandle(hDc);
		}
	}
}
