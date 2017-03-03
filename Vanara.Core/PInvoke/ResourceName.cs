using System;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	/// <summary>Predefined resource types.</summary>
	public enum ResourceType : ushort
	{
		/// <summary>Accelerator table.</summary>
		RT_ACCELERATOR = 9,
		/// <summary>Animated cursor.</summary>
		RT_ANICURSOR = 21,
		/// <summary>Animated icon.</summary>
		RT_ANIICON = 22,
		/// <summary>Bitmap resource.</summary>
		RT_BITMAP = 2,
		/// <summary>Hardware-dependent cursor resource.</summary>
		RT_CURSOR = 1,
		/// <summary>Dialog box.</summary>
		RT_DIALOG = 5,
		/// <summary>Allows a resource editing tool to associate a string with an .rc file. Typically, the string is the name of the header file that provides symbolic names. The resource compiler parses the string but otherwise ignores the value.</summary>
		RT_DLGINCLUDE = 17,
		/// <summary>Font resource.</summary>
		RT_FONT = 8,
		/// <summary>Font directory resource.</summary>
		RT_FONTDIR = 7,
		/// <summary>Hardware-independent cursor resource.</summary>
		RT_GROUP_CURSOR = 12,
		/// <summary>Hardware-independent icon resource.</summary>
		RT_GROUP_ICON = 14,
		/// <summary>HTML resource.</summary>
		RT_HTML = 23,
		/// <summary>Hardware-dependent icon resource.</summary>
		RT_ICON = 3,
		/// <summary>Side-by-Side Assembly Manifest.</summary>
		RT_MANIFEST = 24,
		/// <summary>Menu resource.</summary>
		RT_MENU = 4,
		/// <summary>Message-table entry.</summary>
		RT_MESSAGETABLE = 11,
		/// <summary>Plug and Play resource.</summary>
		RT_PLUGPLAY = 19,
		/// <summary>Application-defined resource (raw data).</summary>
		RT_RCDATA = 10,
		/// <summary>String-table entry.</summary>
		RT_STRING = 6,
		/// <summary>Version resource.</summary>
		RT_VERSION = 16,
		/// <summary>VXD.</summary>
		RT_VXD = 20,
	}

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	/// <summary>Represents a system resource name that can identify as a string, integer, or pointer.</summary>
	/// <seealso cref="SafeHandle"/>
	/// <seealso cref="System.IEquatable{String}"/>
	/// <seealso cref="System.IEquatable{Int32}"/>
	/// <seealso cref="System.IEquatable{ResourceName}"/>
	/// <seealso cref="System.IEquatable{IntPtr}"/>
	public class ResourceName : SafeHandle, IEquatable<string>, IEquatable<int>, IEquatable<ResourceName>, IEquatable<IntPtr>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	{
		/// <summary>Initializes a new instance of the <see cref="ResourceName"/> class.</summary>
		/// <param name="resName">Name of the resource.</param>
		public ResourceName(string resName) : base(IntPtr.Zero, true)
		{
			SetHandle(Marshal.StringToCoTaskMemUni(resName));
		}

		/// <summary>Initializes a new instance of the <see cref="ResourceName"/> class.</summary>
		/// <param name="resId">The resource identifier.</param>
		public ResourceName(int resId) : base(IntPtr.Zero, true)
		{
			SetHandle((IntPtr)(ushort)resId);
		}

		/// <summary>Initializes a new instance of the <see cref="ResourceName"/> class.</summary>
		/// <param name="resType">Type of the resource.</param>
		public ResourceName(ResourceType resType) : this((int)resType)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="ResourceName"/> class.</summary>
		/// <param name="ptr">The PTR.</param>
		public ResourceName(IntPtr ptr) : base(IntPtr.Zero, true)
		{
			if (IsIntResource(ptr))
				SetHandle(ptr);
			else
			{
				var s = (string)Marshal.PtrToStringUni(ptr)?.Clone();
				if (s != null)
					SetHandle(Marshal.StringToCoTaskMemUni(s));
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the handle value is invalid.</summary>
		/// <PermissionSet>
		/// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
		/// version="1" Flags="UnmanagedCode"/>
		/// </PermissionSet>
		public override bool IsInvalid => handle == IntPtr.Zero;

		/// <summary>Performs an implicit conversion from <see cref="ResourceName"/> to <see cref="System.Int32"/>.</summary>
		/// <param name="r">The r.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator int(ResourceName r)
			=> IsIntResource(r.handle) ? (ushort)r.handle.ToInt32() : 0;

		/// <summary>Performs an implicit conversion from <see cref="ResourceName"/> to <see cref="IntPtr"/>.</summary>
		/// <param name="r">The r.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator IntPtr(ResourceName r) => r.handle;

		/// <summary>Performs an implicit conversion from <see cref="string"/> to <see cref="ResourceName"/>.</summary>
		/// <param name="resName">Name of the resource.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator ResourceName(string resName) => new ResourceName(resName);

		/// <summary>Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="ResourceName"/>.</summary>
		/// <param name="resId">The resource identifier.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator ResourceName(int resId) => new ResourceName(resId);

		/// <summary>Performs an implicit conversion from <see cref="ResourceType"/> to <see cref="ResourceName"/>.</summary>
		/// <param name="resType">Type of the resource.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator ResourceName(ResourceType resType) => new ResourceName(resType);

		/// <summary>Performs an implicit conversion from <see cref="IntPtr"/> to <see cref="ResourceName"/>.</summary>
		/// <param name="ptr">The PTR.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator ResourceName(IntPtr ptr) => new ResourceName(ptr);

		/// <summary>Performs an implicit conversion from <see cref="ResourceName"/> to <see cref="string"/>.</summary>
		/// <param name="r">The r.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator string(ResourceName r) => r.ToString();

		/// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			var s = obj as string;
			if (s != null) return Equals(s);
			if (obj != null && obj.GetType().IsPrimitive)
				try
				{
					return Equals(Convert.ToInt32(obj));
				}
				catch { }
			return base.Equals(obj);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(string other) => string.Equals(ToString(), other);

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(int other) => other == handle.ToInt32();

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(ResourceName other)
			=> IsIntResource(handle) && other != null ? handle == other.handle : Equals(other?.ToString());

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(IntPtr other) => new ResourceName(other).Equals(this);

		/// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
		/// <returns>A <see cref="string"/> that represents this instance.</returns>
		public override string ToString()
			=> IsIntResource(handle) ? $"#{handle.ToInt32()}" : Marshal.PtrToStringUni(handle);

		/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
		/// releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override bool ReleaseHandle()
		{
			if (!IsInvalid && !IsIntResource(handle)) Marshal.FreeCoTaskMem(handle);
			return true;
		}

		private static bool IsIntResource(IntPtr ptr) => ptr.ToInt64() >> 16 == 0;
	}
}