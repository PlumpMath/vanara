using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using Vanara.PInvoke;
using static Vanara.PInvoke.AdvApi32;

namespace Vanara.Security.AccessControl
{
	public class PinnedSid : PinnedObject
	{
		private readonly byte[] bytes;

		public PinnedSid(SecurityIdentifier sid)
		{
			bytes = new byte[sid.BinaryLength];
			sid.GetBinaryForm(bytes, 0);
			SetObject(bytes);
		}

		public PSID PSID => PSID.Copy(this);
	}

	public class PinnedAcl : PinnedObject
	{
		private readonly byte[] bytes;

		public PinnedAcl(RawAcl acl)
		{
			bytes = new byte[acl.BinaryLength];
			acl.GetBinaryForm(bytes, 0);
			SetObject(bytes);
		}
	}

	public static class AccessControlHelper
	{
		public static ACCESS_ALLOWED_ACE GetAce(IntPtr pAcl, int aceIndex)
		{
			IntPtr acePtr;
			if (AdvApi32.GetAce(pAcl, aceIndex, out acePtr))
				return (ACCESS_ALLOWED_ACE)Marshal.PtrToStructure(acePtr, typeof(ACCESS_ALLOWED_ACE));
			throw new System.ComponentModel.Win32Exception();
		}

		public static uint GetAceCount(IntPtr pAcl) => GetAclInfo(pAcl).AceCount;

		public static ACL_SIZE_INFORMATION GetAclInfo(IntPtr pAcl)
		{
			var si = new ACL_SIZE_INFORMATION();
			if (!GetAclInformation(pAcl, ref si, (uint)Marshal.SizeOf(si), 2))
				throw new System.ComponentModel.Win32Exception();
			return si;
		}

		public static uint GetAclSize(IntPtr pAcl) => GetAclInfo(pAcl).AclBytesInUse;

		public static uint GetEffectiveRights(PSID pSid, IntPtr pSD)
		{
			var t = new TRUSTEE(pSid);

			bool daclPresent, daclDefaulted;
			var pDacl = IntPtr.Zero;
			GetSecurityDescriptorDacl(pSD, out daclPresent, ref pDacl, out daclDefaulted);

			uint access = 0;
			GetEffectiveRightsFromAcl(pDacl, t, ref access);

			return access;
		}

		public static uint GetEffectiveRights(this RawSecurityDescriptor sd, SecurityIdentifier sid)
		{
			var t = new TRUSTEE(GetPSID(sid));
			uint access = 0;
			using (var pDacl = new PinnedAcl(sd.DiscretionaryAcl))
				GetEffectiveRightsFromAcl(pDacl, t, ref access);

			return access;
		}

		public static IEnumerable<INHERITED_FROM> GetInheritanceSource(string objectName, ResourceType objectType,
			SECURITY_INFORMATION securityInfo, bool container, IntPtr pAcl, ref GENERIC_MAPPING pGenericMapping)
		{
			var objSize = Marshal.SizeOf(typeof(INHERITED_FROM));
			var aceCount = GetAceCount(pAcl);
			using (var pInherit = new SafeCoTaskMemHandle(objSize * (int)aceCount))
			{
				var hr = 0;
				try
				{
					hr = AdvApi32.GetInheritanceSource(objectName, objectType, securityInfo, container, null, 0, pAcl, IntPtr.Zero, ref pGenericMapping, (IntPtr)pInherit);
					if (hr != 0)
						throw new System.ComponentModel.Win32Exception(hr);
					return pInherit.ToIEnum<INHERITED_FROM>((int)aceCount);
				}
				finally
				{
					if (hr != 0)
						FreeInheritedFromArray((IntPtr)pInherit, (ushort)aceCount, IntPtr.Zero);
				}
			}
		}

		public static PSID GetPSID(this SecurityIdentifier sid) { using (var ps = new PinnedSid(sid)) return ps.PSID; }

		public static IntPtr GetPrivateObjectSecurity(IntPtr pSD, SECURITY_INFORMATION si)
		{
			var pResSD = IntPtr.Zero;
			uint ret = 0;
			AdvApi32.GetPrivateObjectSecurity(pSD, si, IntPtr.Zero, 0, ref ret);
			if (ret > 0)
			{
				pResSD = Marshal.AllocHGlobal((int)ret);
				if (pResSD != IntPtr.Zero && !AdvApi32.GetPrivateObjectSecurity(pSD, si, pResSD, ret, ref ret))
				{
					var hres = Marshal.GetLastWin32Error();
					Marshal.FreeHGlobal(pResSD);
					pResSD = IntPtr.Zero;
					Marshal.ThrowExceptionForHR(hres);
				}
			}
			return pResSD;
		}

		public static RawAcl RawAclFromPtr(IntPtr pAcl)
		{
			var len = GetAclSize(pAcl);
			var dest = new byte[len];
			Marshal.Copy(pAcl, dest, 0, (int)len);
			return new RawAcl(dest, 0);
		}

		public static string SecurityDescriptorPtrToSdll(IntPtr pSD, SECURITY_INFORMATION si)
		{
			IntPtr ssd, ssdLen;
			if (ConvertSecurityDescriptorToStringSecurityDescriptor(pSD, 1, si, out ssd, out ssdLen))
			{
				var s = Marshal.PtrToStringAuto(ssd);
				Marshal.FreeHGlobal(ssd);
				return s;
			}
			return null;
		}
	}
}