using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
	public static partial class AdvApi32
	{
		public class PSID : SafeHGlobalHandle
		{
			public PSID(IntPtr ptr, bool own = true) : base(ptr, GetLengthSid(ptr), own) { }

			public PSID(PSID psid) : base(GetLengthSid(psid.handle))
			{
				CopySid(Size, handle, psid.handle);
			}

			public PSID(int size) : base(size) { }

			public PSID(string sidValue) : base(IntPtr.Zero, 0, true)
			{
				IntPtr psid;
				if (ConvertStringSidToSid(sidValue, out psid))
					SetHandle(psid);
			}

			public PSID() { }

			public override bool IsInvalid => handle == IntPtr.Zero || !IsValidSid(this);

			public IntPtr Clone()
			{
				var len = GetLengthSid(handle);
				var p = Marshal.AllocHGlobal(len);
				CopySid(len, p, handle);
				return p;
			}

			public static PSID Copy(IntPtr psid)
			{
				var newSid = new PSID(GetLengthSid(psid));
				CopySid(newSid.Size, newSid.handle, psid);
				return newSid;
			}

			public static implicit operator PSID(IntPtr psid) => new PSID(psid, false);

			public static explicit operator IntPtr(PSID psid) => psid.DangerousGetHandle();

			/// <summary>
			/// Initializes the specified sid authority.
			/// </summary>
			/// <param name="sidAuthority">The sid authority.</param>
			/// <param name="subAuth0">The sub auth0.</param>
			/// <param name="subAuthorities1to7">The sub authorities1to7.</param>
			/// <returns></returns>
			/// <exception cref="System.ArgumentOutOfRangeException">
			/// sidAuthority
			/// or
			/// subAuthorities1to7
			/// </exception>
			public static PSID Init(byte[] sidAuthority, int subAuth0, params int[] subAuthorities1to7)
			{
				if (sidAuthority == null || sidAuthority.Length != 6)
					throw new ArgumentOutOfRangeException(nameof(sidAuthority));
				if (subAuthorities1to7.Length > 7)
					throw new ArgumentOutOfRangeException(nameof(subAuthorities1to7));
				var sia = new SID_IDENTIFIER_AUTHORITY(sidAuthority);
				IntPtr res = IntPtr.Zero;
				try
				{
					AllocateAndInitializeSid(ref sia, (byte)(subAuthorities1to7.Length + 1),
						subAuth0,
						subAuthorities1to7.Length > 0 ? subAuthorities1to7[0] : 0,
						subAuthorities1to7.Length > 1 ? subAuthorities1to7[1] : 0,
						subAuthorities1to7.Length > 2 ? subAuthorities1to7[2] : 0,
						subAuthorities1to7.Length > 3 ? subAuthorities1to7[3] : 0,
						subAuthorities1to7.Length > 4 ? subAuthorities1to7[4] : 0,
						subAuthorities1to7.Length > 5 ? subAuthorities1to7[5] : 0,
						subAuthorities1to7.Length > 6 ? subAuthorities1to7[6] : 0,
						out res);
					return Copy(res);
				}
				finally
				{
					FreeSid(res);
				}
			}

			/// <summary>
			/// Implements the operator !=.
			/// </summary>
			/// <param name="psid1">The psid1.</param>
			/// <param name="psid2">The psid2.</param>
			/// <returns>
			/// The result of the operator.
			/// </returns>
			public static bool operator !=(PSID psid1, PSID psid2) { return !psid1?.Equals(psid2) ?? false; }

			/// <summary>
			/// Implements the operator ==.
			/// </summary>
			/// <param name="psid1">The psid1.</param>
			/// <param name="psid2">The psid2.</param>
			/// <returns>
			/// The result of the operator.
			/// </returns>
			public static bool operator ==(PSID psid1, PSID psid2) { return psid1?.Equals(psid2) ?? false; }

			/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
			/// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
			/// <param name="obj">The object to compare with the current object. </param>
			/// <filterpriority>2</filterpriority>
			public override bool Equals(object obj)
			{
				var psid2 = obj as PSID;
				if (psid2 != null)
					return EqualSid(this, psid2);
				if (obj is IntPtr)
					return EqualSid(this.handle, (IntPtr)obj);
				return base.Equals(obj);
			}

			/// <summary>
			/// Returns a hash code for this instance.
			/// </summary>
			/// <returns>
			/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
			/// </returns>
			public override int GetHashCode() => base.GetHashCode();

			/// <summary>
			/// Returns a <see cref="System.String" /> that represents this instance.
			/// </summary>
			/// <returns>
			/// A <see cref="System.String" /> that represents this instance.
			/// </returns>
			public override string ToString() => ConvertSidToStringSid(this);
		}
	}
}
