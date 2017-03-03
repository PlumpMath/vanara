using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	/// <summary>Represents a Win32 Error Code. This can be used in place of a return value.</summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public partial struct Win32Error : IEquatable<Win32Error>, IEquatable<int>
	{
		private readonly int value;

		/// <summary>Initializes a new instance of the <see cref="Win32Error"/> struct with an error value.</summary>
		/// <param name="i">The i.</param>
		public Win32Error(int i)
		{
			value = i;
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Equals(int other) => other == value;

		/// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			try
			{
				return (int)obj == value;
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool Equals(Win32Error other) => other.value == value;

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode() => value.GetHashCode();

		/// <summary>Gets the last error.</summary>
		/// <returns></returns>
		[SecurityCritical]
		public static Win32Error GetLastError() => new Win32Error(Marshal.GetLastWin32Error());

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString()
		{
			return Marshal.GetExceptionForHR((int)(HRESULT)this).Message;
		}

		/// <summary>To the hresult.</summary>
		/// <returns></returns>
		public HRESULT ToHRESULT() => (HRESULT)this;

		/// <summary>Throws if failed.</summary>
		/// <param name="message">The message.</param>
		/// <exception cref="Win32Exception"></exception>
		public void ThrowIfFailed(string message = null)
		{
			if (value != ERROR_SUCCESS) throw new Win32Exception(value, message);
		}

		/// <summary>Throws if failed.</summary>
		/// <param name="err">The error.</param>
		/// <param name="message">The message.</param>
		public static void ThrowIfFailed(Win32Error err, string message = null)
		{
			err.ThrowIfFailed(message);
		}

		/// <summary>Performs an explicit conversion from <see cref="System.Int32"/> to <see cref="Win32Error"/>.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator Win32Error(int value) => new Win32Error(value);

		/// <summary>Performs an explicit conversion from <see cref="Win32Error"/> to <see cref="System.Int32"/>.</summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator int(Win32Error value) => value.value;

		/// <summary>Performs an explicit conversion from <see cref="Win32Error"/> to <see cref="HRESULT"/>.</summary>
		/// <param name="error">The error.</param>
		/// <returns>The result of the conversion.</returns>
		public static explicit operator HRESULT(Win32Error error)
			=>
				error.value <= 0
					? new HRESULT((uint)error.value)
					: HRESULT.Make(true, HRESULT.FacilityCode.FACILITY_WIN32, (uint)error.value & 0xffff);

		/// <summary>Implements the operator ==.</summary>
		/// <param name="errLeft">The error left.</param>
		/// <param name="errRight">The error right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(Win32Error errLeft, Win32Error errRight) => errLeft.Equals(errRight);

		/// <summary>Implements the operator !=.</summary>
		/// <param name="errLeft">The error left.</param>
		/// <param name="errRight">The error right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(Win32Error errLeft, Win32Error errRight) => !errLeft.Equals(errRight);
	}
}