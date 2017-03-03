using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static class VirtDisk
	{
		public const uint VIRTUAL_STORAGE_TYPE_DEVICE_VHD = 2;

		public static readonly Guid VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT = new Guid("EC984AEC-A0F9-47e9-901F-71415A66345B");

		[Flags]
		public enum ATTACH_VIRTUAL_DISK_FLAG
		{
			ATTACH_VIRTUAL_DISK_FLAG_NONE = 0x00000000,
			ATTACH_VIRTUAL_DISK_FLAG_READ_ONLY = 0x00000001,
			ATTACH_VIRTUAL_DISK_FLAG_NO_DRIVE_LETTER = 0x00000002,
			ATTACH_VIRTUAL_DISK_FLAG_PERMANENT_LIFETIME = 0x00000004,
			ATTACH_VIRTUAL_DISK_FLAG_NO_LOCAL_HOST = 0x00000008
		}

		public enum ATTACH_VIRTUAL_DISK_VERSION
		{
			ATTACH_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
			ATTACH_VIRTUAL_DISK_VERSION_1 = 1
		}

		[Flags]
		public enum OPEN_VIRTUAL_DISK_FLAG
		{
			OPEN_VIRTUAL_DISK_FLAG_NONE = 0x00000000,
			OPEN_VIRTUAL_DISK_FLAG_NO_PARENTS = 0x00000001,
			OPEN_VIRTUAL_DISK_FLAG_BLANK_FILE = 0x00000002,
			OPEN_VIRTUAL_DISK_FLAG_BOOT_DRIVE = 0x00000004,
			OPEN_VIRTUAL_DISK_FLAG_CACHED_IO = 0x00000008,
			OPEN_VIRTUAL_DISK_FLAG_CUSTOM_DIFF_CHAIN = 0x00000010
		}

		public enum OPEN_VIRTUAL_DISK_VERSION
		{
			OPEN_VIRTUAL_DISK_VERSION_UNSPECIFIED = 0,
			OPEN_VIRTUAL_DISK_VERSION_1 = 1,
			OPEN_VIRTUAL_DISK_VERSION_2 = 2
		}

		[Flags]
		public enum VIRTUAL_DISK_ACCESS_MASK
		{
			VIRTUAL_DISK_ACCESS_NONE = 0x00000000,
			VIRTUAL_DISK_ACCESS_ATTACH_RO = 0x00010000,
			VIRTUAL_DISK_ACCESS_ATTACH_RW = 0x00020000,
			VIRTUAL_DISK_ACCESS_DETACH = 0x00040000,
			VIRTUAL_DISK_ACCESS_GET_INFO = 0x00080000,
			VIRTUAL_DISK_ACCESS_CREATE = 0x00100000,
			VIRTUAL_DISK_ACCESS_METAOPS = 0x00200000,
			VIRTUAL_DISK_ACCESS_READ = 0x000d0000,
			VIRTUAL_DISK_ACCESS_ALL = 0x003f0000,
			VIRTUAL_DISK_ACCESS_WRITABLE = 0x00320000
		}

		[DllImport(nameof(VirtDisk), ExactSpelling = true)]
		public static extern unsafe int AttachVirtualDisk(
			SafeFileHandle VirtualDiskHandle,
			IntPtr SecurityDescriptor,
			ATTACH_VIRTUAL_DISK_FLAG Flags,
			uint ProviderSpecificFlags,
			ref ATTACH_VIRTUAL_DISK_PARAMETERS Parameters,
			[In] NativeOverlapped* Overlapped);

		[DllImport(nameof(VirtDisk), ExactSpelling = true, ThrowOnUnmappableChar = true)]
		public static extern int OpenVirtualDisk(
			[In] ref VIRTUAL_STORAGE_TYPE VirtualStorageType,
			[MarshalAs(UnmanagedType.LPWStr)] string Path,
			VIRTUAL_DISK_ACCESS_MASK VirtualDiskAccessMask,
			OPEN_VIRTUAL_DISK_FLAG Flags,
			[In] ref OPEN_VIRTUAL_DISK_PARAMETERS Parameters,
			out SafeFileHandle Handle);

		[StructLayout(LayoutKind.Sequential)]
		public struct ATTACH_VIRTUAL_DISK_PARAMETERS
		{
			public ATTACH_VIRTUAL_DISK_VERSION Version;

			public uint Reserved;

			public static ATTACH_VIRTUAL_DISK_PARAMETERS Default => new ATTACH_VIRTUAL_DISK_PARAMETERS { Version = ATTACH_VIRTUAL_DISK_VERSION.ATTACH_VIRTUAL_DISK_VERSION_1 };
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct OPEN_VIRTUAL_DISK_PARAMETERS
		{
			[FieldOffset(0)]
			public OPEN_VIRTUAL_DISK_VERSION Version;

			[FieldOffset(4)]
			public uint RWDepth;

			[FieldOffset(4)]
			[MarshalAs(UnmanagedType.Bool)]
			public bool GetInfoOnly;

			[FieldOffset(8)]
			[MarshalAs(UnmanagedType.Bool)]
			public bool ReadOnly;

			[FieldOffset(12)]
			public Guid ResiliencyGuid;

			public OPEN_VIRTUAL_DISK_PARAMETERS(uint rwDepth) : this()
			{
				Version = OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_1;
				RWDepth = rwDepth;
			}

			public OPEN_VIRTUAL_DISK_PARAMETERS(bool getInfoOnly, bool readOnly, Guid resiliencyGuid) : this()
			{
				if (Environment.OSVersion.Version < new Version(6, 2))
					throw new InvalidOperationException();
				Version = OPEN_VIRTUAL_DISK_VERSION.OPEN_VIRTUAL_DISK_VERSION_2;
				GetInfoOnly = getInfoOnly;
				ReadOnly = readOnly;
				ResiliencyGuid = resiliencyGuid;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct VIRTUAL_STORAGE_TYPE
		{
			public uint DeviceId;
			public Guid VendorId;

			public static VIRTUAL_STORAGE_TYPE VHD => new VIRTUAL_STORAGE_TYPE { DeviceId = VIRTUAL_STORAGE_TYPE_DEVICE_VHD, VendorId = VIRTUAL_STORAGE_TYPE_VENDOR_MICROSOFT };
		}
	}
}