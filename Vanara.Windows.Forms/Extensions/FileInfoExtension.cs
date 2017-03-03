using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Vanara.PInvoke;

namespace Vanara.Extensions
{
	public static class FileInfoExtension
	{
		private static readonly SafeFileHandle nullSafeHandle = new SafeFileHandle(IntPtr.Zero, false);

		public static SafeFileHandle GetFileHandle(this FileSystemInfo fi, bool readOnly = true, bool overlapped = false)
		{
			var fa = readOnly ? FileAccess.Read : FileAccess.ReadWrite;
			var fs = readOnly ? FileShare.Read : FileShare.None;
			var ff = Kernel32.FileFlagsAndAttributes.FILE_ATTRIBUTE_NORMAL;
			if (overlapped) ff |= Kernel32.FileFlagsAndAttributes.FILE_FLAG_OVERLAPPED;
			if (fi is DirectoryInfo) ff |= Kernel32.FileFlagsAndAttributes.FILE_FLAG_BACKUP_SEMANTICS;
			return Kernel32.CreateFile(fi.FullName, fa, fs, null, FileMode.Open, ff, nullSafeHandle);
		}

		public static Kernel32.FileSystemFlags GetFileSystemFlags(this DriveInfo di)
		{
			string volName, fsName;
			int volSn, compLen;
			Kernel32.FileSystemFlags fsFlags;
			Kernel32.GetVolumeInformation(di.Name, out volName, out volSn, out compLen, out fsFlags, out fsName);
			return fsFlags;
		}

		public static int GetMaxFileNameLength(this DriveInfo di)
		{
			string volName, fsName;
			int volSn, compLen;
			Kernel32.FileSystemFlags fsFlags;
			Kernel32.GetVolumeInformation(di.Name, out volName, out volSn, out compLen, out fsFlags, out fsName);
			return compLen;
		}

		public static bool GetNtfsCompression(this FileSystemInfo fi)
		{
			using (var fs = GetFileHandle(fi))
			{
				ushort outVal;
				if (!Kernel32.DeviceIoControl(fs, Kernel32.IOControlCode.FSCTL_GET_COMPRESSION, out outVal))
					throw new Win32Exception();
				return outVal != 0;
			}
			//return (fi.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
		}

		public static Task<bool> GetNtfsCompressionAsync(this FileSystemInfo fi)
		{
			using (var fs = GetFileHandle(fi, true, true))
				return ConvertTask(Kernel32.DeviceIoControlAsync<uint>(fs, Kernel32.IOControlCode.FSCTL_GET_COMPRESSION), u => u.GetValueOrDefault() > 0);
		}

		/// <summary>
		/// Gets the length of the file on the disk. If the file is compressed, this will return the compressed size.
		/// </summary>
		/// <param name="fi">The <see cref="FileInfo"/> instance.</param>
		/// <returns>The actual size of the file on the disk in bytes, compressed or uncompressed.</returns>
		public static ulong GetPhysicalLength(this FileInfo fi)
		{
			var high = 0;
			var low = Kernel32.GetCompressedFileSize(fi.FullName, ref high);
			var error = Marshal.GetLastWin32Error();
			if (error == Win32Error.ERROR_SHARING_VIOLATION)
				return (ulong)fi.Length;
			if (high == 0 && low == Kernel32.INVALID_FILE_SIZE && error != 0)
				throw new Win32Exception(error);
			return ((ulong)high << 32) | (uint)low;
		}

		public static void SetNtfsCompression(this FileSystemInfo fi, bool compressed)
		{
			using (var fs = GetFileHandle(fi, false))
			{
				if (!Kernel32.DeviceIoControl(fs, Kernel32.IOControlCode.FSCTL_SET_COMPRESSION, (ushort)(compressed ? 1 : 0)))
					throw new Win32Exception();
			}
		}

		public static Task SetNtfsCompressionAsync(this FileSystemInfo fi, bool compressed)
		{
			using (var fs = GetFileHandle(fi, false, true))
				return Kernel32.DeviceIoControlAsync(fs, Kernel32.IOControlCode.FSCTL_SET_COMPRESSION, (ushort)(compressed ? 1 : 0));
		}

		private static Task<TNew> ConvertTask<TCurrent, TNew>(Task<TCurrent> task, Converter<TCurrent, TNew> converter = null)
		{
			var tret = new TaskCompletionSource<TNew>();
			if (task.IsCanceled) tret.TrySetCanceled();
			else if (task.IsFaulted && task.Exception != null) tret.TrySetException(task.Exception);
			else
			{
				if (converter == null)
					tret.TrySetResult((TNew)Convert.ChangeType(task.Result, typeof(TNew)));
				else
					tret.TrySetResult(converter(task.Result));
			}
			return tret.Task;
		}
	}
}
