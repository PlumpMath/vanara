using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;
using static Vanara.PInvoke.AdvApi32;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

#if !NET_40_OR_GREATER
namespace Microsoft.Win32.SafeHandles
{
	/// <summary>
	/// Represents a safe handle to the Windows registry.
	/// </summary>
	[SecurityCritical]
	public sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeRegistryHandle"/> class.
		/// </summary>
		/// <param name="preexistingHandle">The preexisting handle.</param>
		/// <param name="ownsHandle">if set to <c>true</c> [owns handle].</param>
		[SecurityCritical]
		public SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
		{
			SetHandle(preexistingHandle);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SafeRegistryHandle"/> class.
		/// </summary>
		[SecurityCritical]
		internal SafeRegistryHandle() : base(true) { }

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		[SecurityCritical]
		protected override bool ReleaseHandle() => RegCloseKey(handle) == 0;
	}
}
#endif

namespace Vanara.PInvoke
{
	public static partial class AdvApi32
	{
		public const int KEY_QUERY_VALUE = 0x0001;
		public const int KEY_NOTIFY = 0x0010;
		public const int STANDARD_RIGHTS_READ = 0x00020000;

		public static readonly SafeRegistryHandle HKEY_CLASSES_ROOT = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000000)), false);
		public static readonly SafeRegistryHandle HKEY_CURRENT_USER = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000001)), false);
		public static readonly SafeRegistryHandle HKEY_LOCAL_MACHINE = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000002)), false);
		public static readonly SafeRegistryHandle HKEY_USERS = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000003)), false);
		public static readonly SafeRegistryHandle HKEY_PERFORMANCE_DATA = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000004)), false);
		public static readonly SafeRegistryHandle HKEY_CURRENT_CONFIG = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000005)), false);
		public static readonly SafeRegistryHandle HKEY_DYN_DATA = new SafeRegistryHandle(new IntPtr(unchecked((int)0x80000006)), false);

		/// <summary>Filter for notifications reported by <see cref="RegNotifyChangeKeyValue" />.</summary>
		[Flags]
		public enum RegNotifyChangeFilter
		{
			/// <summary>Notify the caller if a subkey is added or deleted.</summary>
			REG_NOTIFY_CHANGE_NAME = 1,

			/// <summary>Notify the caller of changes to the attributes of the key, such as the security descriptor information.</summary>
			REG_NOTIFY_CHANGE_ATTRIBUTES = 2,

			/// <summary>
			/// Notify the caller of changes to a value of the key. This can include adding or deleting a value, or changing
			/// an existing value.
			/// </summary>
			REG_NOTIFY_CHANGE_LAST_SET = 4,

			/// <summary>Notify the caller of changes to the security descriptor of the key.</summary>
			REG_NOTIFY_CHANGE_SECURITY = 8,

			/// <summary>
			/// Indicates that the lifetime of the registration must not be tied to the lifetime of the thread issuing the RegNotifyChangeKeyValue call. <note
			/// type="note">This flag value is only supported in Windows 8 and later.</note>
			/// </summary>
			REG_NOTIFY_THREAD_AGNOSTIC = 0x10000000
		}

		[SuppressUnmanagedCodeSecurity, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern int RegCloseKey(IntPtr hKey);

		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern int RegNotifyChangeKeyValue(SafeRegistryHandle hKey, bool bWatchSubtree, RegNotifyChangeFilter dwFilter, IntPtr hEvent, bool fAsynchronous);

		[DllImport(nameof(AdvApi32), SetLastError = true)]
		public static extern int RegOpenKeyEx(SafeRegistryHandle hKey, string subKey, uint options, int samDesired, out SafeRegistryHandle phkResult);
	}
}