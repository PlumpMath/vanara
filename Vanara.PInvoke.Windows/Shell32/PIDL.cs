using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.Extensions;
using static Vanara.PInvoke.Shell32.PIDLUtil;

namespace Vanara.PInvoke
{
	public static partial class Shell32
	{
		public static class PIDLUtil
		{
			// Clones an ITEMIDLIST structure
			public static PIDL ILClone(IntPtr pidl) => new PIDL(IntILClone(pidl));

			// Clones the first SHITEMID structure in an ITEMIDLIST structure
			public static PIDL ILCloneFirst(IntPtr pidl)
			{
				var size = ItemIdSize(pidl);

				var bytes = new byte[size + 2];
				Marshal.Copy(pidl, bytes, 0, size);

				var newPidl = Marshal.AllocCoTaskMem(size + 2);
				Marshal.Copy(bytes, 0, newPidl, size + 2);

				return new PIDL(newPidl);
			}

			public static PIDL ILCombine(IntPtr pidl1, IntPtr pidl2) => new PIDL(IntILCombine(pidl1, pidl2));

			// Returns a pointer to the last SHITEMID structure in an ITEMIDLIST structure
			public static IntPtr ILFindLastId(IntPtr pidl)
			{
				var ptr1 = pidl;
				var ptr2 = ILGetNext(ptr1);

				while (ItemIdSize(ptr2) > 0)
				{
					ptr1 = ptr2;
					ptr2 = ILGetNext(ptr1);
				}

				return ptr1;
			}

			// Gets the next SHITEMID structure in an ITEMIDLIST structure
			public static IntPtr ILGetNext(IntPtr pidl)
			{
				var size = ItemIdSize(pidl);
				return size == 0 ? IntPtr.Zero : new IntPtr(pidl.ToInt64() + size);
			}

			// Removes the last SHITEMID structure from an ITEMIDLIST structure
			public static bool ILRemoveLastId(IntPtr pidl)
			{
				var lastPidl = ILFindLastId(pidl);

				if (lastPidl == pidl) return false;

				var newSize = (int)lastPidl - (int)pidl + 2;
				Marshal.ReAllocCoTaskMem(pidl, newSize);
				Marshal.Copy(new byte[] { 0, 0 }, 0, new IntPtr(pidl.ToInt64() + newSize - 2), 2);
				return true;
			}

			public static bool IsEmpty(IntPtr pidl) => ItemIdListSize(pidl) == 0;

			public static bool SplitPidl(IntPtr pidl, out IntPtr parent, out IntPtr child)
			{
				parent = IntILClone(pidl);
				child = IntILClone(ILFindLastId(pidl));

				if (!ILRemoveLastId(parent))
				{
					Marshal.FreeCoTaskMem(parent);
					Marshal.FreeCoTaskMem(child);
					return false;
				}
				return true;
			}

			// Clones an ITEMIDLIST structure
			internal static IntPtr IntILClone(IntPtr pidl)
			{
				var size = ItemIdListSize(pidl);

				var bytes = new byte[size + 2];
				Marshal.Copy(pidl, bytes, 0, size);

				var newPidl = Marshal.AllocCoTaskMem(size + 2);
				Marshal.Copy(bytes, 0, newPidl, size + 2);

				return newPidl;
			}

			// Combines two ITEMIDLIST structures
			internal static IntPtr IntILCombine(IntPtr pidl1, IntPtr pidl2)
			{
				var size1 = ItemIdListSize(pidl1);
				var size2 = ItemIdListSize(pidl2);

				var newPidl = Marshal.AllocCoTaskMem(size1 + size2 + 2);
				var bytes = new byte[size1 + size2 + 2];

				Marshal.Copy(pidl1, bytes, 0, size1);
				Marshal.Copy(pidl2, bytes, size1, size2);

				Marshal.Copy(bytes, 0, newPidl, bytes.Length);

				return newPidl;
			}

			/*public static void WriteBytes(IntPtr handle)
			{
				var size = Marshal.ReadByte(handle, 0) + Marshal.ReadByte(handle, 1)*256 - 2;

				for (var i = 0; i < size; i++)
				{
					Console.Out.WriteLine(Marshal.ReadByte(handle, i + 2));
				}

				Console.Out.WriteLine(Marshal.ReadByte(handle, size + 2));
				Console.Out.WriteLine(Marshal.ReadByte(handle, size + 3));
			}*/

			internal static int ItemIdListSize(IntPtr handle)
			{
				if (handle.Equals(IntPtr.Zero))
					return 0;
				var size = ItemIdSize(handle);
				var nextSize = Marshal.ReadInt16(handle, size);
				while (nextSize > 0)
				{
					size += nextSize;
					nextSize = Marshal.ReadInt16(handle, size);
				}
				return size;
			}

			internal static int ItemIdSize(IntPtr handle) => handle.Equals(IntPtr.Zero) ? 0 : Marshal.ReadInt16(handle);
		}

		// ReSharper disable once InconsistentNaming
		/// <summary>
		/// Represents a managed pointer to an ITEMIDLIST.
		/// </summary>
		/// <seealso cref="Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid" />
		/// <seealso cref="System.Collections.Generic.IEnumerable{PIDL}" />
		public class PIDL : SafeHandleZeroOrMinusOneIsInvalid, IEnumerable<PIDL>, IEquatable<PIDL>, IEquatable<IntPtr>
		{
			/// <summary>Initializes a new instance of the <see cref="PIDL"/> class.</summary>
			/// <param name="pidl">The raw pointer to a native ITEMIDLIST.</param>
			/// <param name="clone">if set to <c>true</c> clone the list before storing it.</param>
			/// <param name="own">if set to <c>true</c><see cref="PIDL"/> will release the memory associated with the ITEMIDLIST when done.</param>
			public PIDL(IntPtr pidl, bool clone = false, bool own = true) : base(clone || own)
			{
				SetHandle(clone ? IntILClone(pidl) : pidl);
			}

			/// <summary>Initializes a new instance of the <see cref="PIDL"/> class.</summary>
			/// <param name="pidl">An existing <see cref="PIDL"/> that will be copied and managed.</param>
			public PIDL(PIDL pidl) : this(pidl.handle, true) { }

			/// <summary>Initializes a new instance of the <see cref="PIDL"/> class.</summary>
			internal PIDL() : base(true) { }

			/// <summary>Gets a value indicating whether this list is empty.</summary>
			/// <value><c>true</c> if this list is empty; otherwise, <c>false</c>.</value>
			public bool IsEmpty => IsEmpty(handle);

			public PIDL LastId => new PIDL(ILFindLastId(handle), true);

			public static PIDL Null { get; } = new PIDL();

			/// <summary>Performs an explicit conversion from <see cref="PIDL"/> to <see cref="IntPtr"/>.</summary>
			/// <param name="pidl">The current <see cref="PIDL"/>.</param>
			/// <returns>The result of the conversion.</returns>
			public static explicit operator IntPtr(PIDL pidl) => pidl.handle;

			/// <summary>Appends the specified <see cref="PIDL"/> to the existing list.</summary>
			/// <param name="appendPidl">The <see cref="PIDL"/> to append.</param>
			public void Append(PIDL appendPidl)
			{
				var newPidl = IntILCombine(handle, appendPidl.DangerousGetHandle());
				Marshal.FreeCoTaskMem(handle);
				SetHandle(newPidl);
			}

			/// <summary>Combines the specified <see cref="PIDL"/> instances to create a new one.</summary>
			/// <param name="firstPidl">The first <see cref="PIDL"/>.</param>
			/// <param name="secondPidl">The second <see cref="PIDL"/>.</param>
			/// <returns>A managed <see cref="PIDL"/> instance that contains both supplied lists in their respective order.</returns>
			public static PIDL Combine(PIDL firstPidl, PIDL secondPidl) => ILCombine(firstPidl.handle, secondPidl.handle);

			/// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
			/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
			/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
			public override bool Equals(object obj)
			{
				if (obj == null && IsEmpty) return true;
				if (obj is PIDL)
					return Equals((PIDL)obj);
				if (obj is IntPtr)
					return Equals((IntPtr)obj);
				return base.Equals(obj);
			}

			public bool Equals(IntPtr other) { return ILIsEqual(handle, other); }

			public bool Equals(PIDL other) { return Equals(other?.handle ?? IntPtr.Zero); }

			//public static bool operator ==(PIDL a, PIDL b) => (a == null && b == null) || (a?.Equals(b) ?? false);

			//public static bool operator !=(PIDL a, PIDL b) => !(a == b);

			/// <summary>Returns an enumerator that iterates through the collection.</summary>
			/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator{PIDL}"/> that can be used to iterate through the collection.</returns>
			public IEnumerator<PIDL> GetEnumerator() => new InternalEnumerator(handle);

			/// <summary>Returns an enumerator that iterates through a collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			/// <summary>Returns a hash code for this instance.</summary>
			/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
			public override int GetHashCode() => base.GetHashCode();

			/// <summary>Gets the icon associated with this ITEMIDLIST, if one does.</summary>
			/// <param name="small">if set to <c>true</c> retrieves the small (usually 16x16) icon; other retrieves the large icon (usually 32x32).</param>
			/// <returns>Icon of the specified size, or <c>null</c> if no icon is associated with this ITEMIDLIST.</returns>
			public System.Drawing.Icon GetIcon(bool small)
			{
				if (IsInvalid) return null;
				var shfi = new SHFILEINFO();
				var shgfi = SHGFI.SHGFI_ICON | SHGFI.SHGFI_PIDL;
				shgfi |= small ? SHGFI.SHGFI_SMALLICON : SHGFI.SHGFI_LARGEICON;
				SHGetFileInfo(this, 0, ref shfi, Marshal.SizeOf(shfi), shgfi);
				var ico = (System.Drawing.Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
				User32.DestroyIcon(shfi.hIcon);
				return ico;
			}

			/// <summary>Inserts the specified <see cref="PIDL"/> before the existing list.</summary>
			/// <param name="insertPidl">The <see cref="PIDL"/> to insert.</param>
			public void Insert(PIDL insertPidl)
			{
				var newPidl = IntILCombine(insertPidl.handle, handle);
				Marshal.FreeCoTaskMem(handle);
				SetHandle(newPidl);
			}

			/// <summary>
			/// Removes the last identifier from the list.
			/// </summary>
			public bool RemoveLastId() => ILRemoveLastId(handle);

			/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
			/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
			public override string ToString() => ToString(SIGDN.SIGDN_NORMALDISPLAY);

			/// <summary>Returns a <see cref="System.String"/> that represents this instance base according to the format provided by <paramref name="displayNameFormat"/>.</summary>
			/// <param name="displayNameFormat">The desired display name format.</param>
			/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
			public string ToString(SIGDN displayNameFormat)
			{
				try
				{
					SafeCoTaskMemHandle name;
					SHGetNameFromIDList(this, displayNameFormat, out name);
					return name.ToString(-1);
				}
				catch (Exception ex) { Debug.WriteLine($"Error: PIDL:ToString = {ex}"); }
				return null;
			}

			public string Dump()
			{
				var sb = new StringBuilder();
				foreach (var pidl in this)
				{
					var sz = ItemIdSize(pidl.handle);
					var bytes = pidl.handle.ToIEnum<byte>(sz);
					sb.AppendLine(string.Join(" ", bytes.Select(b => $"{b:X2}")));
				}
				return sb.ToString();
			}

			/// <summary>When overridden in a derived class, executes the code required to free the handle.</summary>
			/// <returns>
			/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a
			/// releaseHandleFailed MDA Managed Debugging Assistant.
			/// </returns>
			protected override bool ReleaseHandle()
			{
				try { Marshal.FreeCoTaskMem(handle); } catch { }
				handle = IntPtr.Zero;
				return true;
			}

			private class InternalEnumerator : IEnumerator<PIDL>
			{
				private readonly IntPtr pidl;
				private IntPtr currentPidl;
				private bool start;

				public InternalEnumerator(IntPtr handle)
				{
					start = true;
					pidl = handle;
					currentPidl = IntPtr.Zero;
				}

				public PIDL Current => currentPidl == IntPtr.Zero ? null : ILCloneFirst(currentPidl);

				object IEnumerator.Current => Current;

				public void Dispose() { }

				public bool MoveNext()
				{
					if (start)
					{
						start = false;
						currentPidl = pidl;
						return true;
					}

					var newPidl = ILGetNext(currentPidl);
					if (IsEmpty(newPidl)) return false;
					currentPidl = newPidl;
					return true;
				}

				public void Reset()
				{
					start = true;
					currentPidl = IntPtr.Zero;
				}
			}
		}
	}
}