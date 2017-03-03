using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace Vanara.PInvoke
{
	public static partial class Ole32
	{
		[ComImport, Guid("0000000D-0000-0000-C000-000000000046"), InterfaceType((short)1)]
		public interface IEnumSTATSTG
		{
			[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int Next([In] uint celt,
				[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] STATSTG[] rgelt,
				out uint pceltFetched);

			[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int Skip([In] uint celt);

			[PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			int Reset();

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumSTATSTG ppEnum);
		}

		[ComImport, InterfaceType((short)1), ComConversionLoss, Guid("0000000B-0000-0000-C000-000000000046")]
		public interface IStorage
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void CreateStream([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
				[In] uint grfMode,
				[In] uint reserved1,
				[In] uint reserved2,
				[MarshalAs(UnmanagedType.Interface)] out IStream ppstm);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void OpenStream([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName, [In] IntPtr reserved1,
				[In] uint grfMode,
				[In] uint reserved2,
				[MarshalAs(UnmanagedType.Interface)] out IStream ppstm);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void CreateStorage([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
				[In] uint grfMode,
				[In] uint reserved1,
				[In] uint reserved2,
				[MarshalAs(UnmanagedType.Interface)] out IStorage ppstg);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void OpenStorage([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
				[In, MarshalAs(UnmanagedType.Interface)] IStorage pstgPriority,
				[In] uint grfMode,
				[In] IntPtr snbExclude,
				[In] uint reserved,
				[MarshalAs(UnmanagedType.Interface)] out IStorage ppstg);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void CopyTo([In] uint ciidExclude,
				[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgiidExclude,
				[In] IntPtr snbExclude,
				[In, MarshalAs(UnmanagedType.Interface)] IStorage pstgDest);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void MoveElementTo([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
				[In, MarshalAs(UnmanagedType.Interface)] IStorage pstgDest, [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName,
				[In] uint grfFlags);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Commit([In] uint grfCommitFlags);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Revert();

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void EnumElements([In] uint reserved1,
				[In] IntPtr reserved2, [In] uint reserved3,
				[MarshalAs(UnmanagedType.Interface)] out IEnumSTATSTG ppEnum);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void DestroyElement([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void RenameElement([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName,
				[In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void SetElementTimes([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
				[In, MarshalAs(UnmanagedType.LPArray)] FILETIME[]
					pctime,
				[In, MarshalAs(UnmanagedType.LPArray)] FILETIME[]
					patime,
				[In, MarshalAs(UnmanagedType.LPArray)] FILETIME[]
					pmtime);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void SetClass([In] ref Guid clsid);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void SetStateBits([In] uint grfStateBits,
				[In] uint grfMask);

			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			void Stat(
				[Out, MarshalAs(UnmanagedType.LPArray)] STATSTG[]
					pstatstg, [In] uint grfStatFlag);
		}
	}
}