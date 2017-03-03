using System;
using System.Runtime.InteropServices;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class OleAut32
	{
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public class SAFEARRAY : IDisposable
		{
			public ushort cDims;
			public ushort fFeatures;
			public uint cbElements;
			public uint cLocks;
			public IntPtr pvData;
			public IntPtr rgsabound;

			public void Dispose()
			{
				throw new NotImplementedException();
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct SAFEARRAYBOUND
		{
			public uint cElements;
			public int lLbound;
		}

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // returns hresult
		public static extern HRESULT SafeArrayAccessData(IntPtr psa, out IntPtr ppvData);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // psa is actually returned, not hresult
		public static extern IntPtr SafeArrayCreate(ushort vt, uint cDims, ref SAFEARRAYBOUND rgsabound);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // psa is actually returned, not hresult
		public static extern IntPtr SafeArrayCreateVector(ushort vt, int lowerBound, uint cElems);

		[DllImport(nameof(OleAut32), ExactSpelling = true)]
		public static extern HRESULT SafeArrayDestroy(IntPtr psa);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // retuns uint32
		public static extern uint SafeArrayGetDim(IntPtr psa);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // returns hresult
		public static extern HRESULT SafeArrayGetElement(IntPtr psa, ref int rgIndices, out IntPtr pv);

		[DllImport(nameof(OleAut32), ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern int SafeArrayGetElemsize(IntPtr pSafeArray);

		[DllImport(nameof(OleAut32), ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern HRESULT SafeArrayGetLBound(IntPtr psa, uint nDim, out int plLbound);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // returns hresult
		public static extern HRESULT SafeArrayGetUBound(IntPtr psa, uint nDim, out int plUbound);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // returns hresult
		public static extern HRESULT SafeArrayPutElement(IntPtr psa, [MarshalAs(UnmanagedType.LPArray)] int[] rgIndicies, object pv);

		[DllImport(nameof(OleAut32), ExactSpelling = true)] // returns hresult
		public static extern HRESULT SafeArrayUnaccessData(IntPtr psa);
	}
}