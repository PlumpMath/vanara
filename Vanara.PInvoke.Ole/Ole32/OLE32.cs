using System;
using System.Runtime.InteropServices;
using System.Security;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Ole32
	{
		/// <summary>
		/// Enumeration used when initializing the COM library
		/// </summary>
		public enum COINIT
		{
			COINIT_APARTMENTTHREADED = 0x2,
			COINIT_MULTITHREADED = 0x0,
			COINIT_DISABLE_OLE1DDE = 0x4,
			COINIT_SPEED_OVER_MEMORY = 0x8
		}

        /// <summary>
        /// Specifies the FMTID/PID identifier that programmatically identifies a property. Replaces SHCOLUMNID.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
	    public struct PROPERTYKEY
	    {
            /// <summary>
            /// A unique GUID for the property.
            /// </summary>
            public Guid fmtid;
            /// <summary>
            /// A property identifier (PID). This parameter is not used as in SHCOLUMNID. It is recommended that you set this value to PID_FIRST_USABLE. Any value greater than or equal to 2 is acceptable.
            /// <note>Values of 0 and 1 are reserved and should not be used.</note>
            /// </summary>
            public uint pid;
	    }

		/// <summary>
		/// Initializes the COM library on the current apartment, identifies the concurrency model as single-thread apartment (STA), and enables additional functionality described in the Remarks section below. Applications must initialize the COM library before they can call COM library functions other than CoGetMalloc and memory allocation functions.
		/// </summary>
		/// <param name="pvReserved">This parameter is reserved and must be NULL.</param>
		[DllImport(nameof(Ole32), ExactSpelling = true, SetLastError = false)]
		public static extern void OleInitialize(IntPtr pvReserved);

		/// <summary>
		/// Initializes the COM library for use by the calling thread, sets the thread's concurrency model, and creates a new apartment for the thread if one is required.
		/// <para>You should call Windows::Foundation::Initialize to initialize the thread instead of CoInitializeEx if you want to use the Windows Runtime APIs or if you want to use both COM and Windows Runtime components. Windows::Foundation::Initialize is sufficient to use for COM components.</para>
		/// </summary>
		/// <param name="pvReserved">This parameter is reserved and must be NULL.</param>
		/// <param name="coInit">The concurrency model and initialization options for the thread. Values for this parameter are taken from the COINIT enumeration. Any combination of values from COINIT can be used, except that the COINIT_APARTMENTTHREADED and COINIT_MULTITHREADED flags cannot both be set. The default is COINIT_MULTITHREADED.</param>
		/// <returns>
		/// <list type="table">
		/// <listheader><term>Return code</term><term>Description</term></listheader>
		/// <item><term>S_OK</term><defintion>The COM library was initialized successfully on this thread.</defintion></item>
		/// <item><term>S_FALSE</term><defintion>The COM library is already initialized on this thread.</defintion></item>
		/// <item><term>RPC_E_CHANGED_MODE</term><defintion>A previous call to CoInitializeEx specified the concurrency model for this thread as multithread apartment (MTA). This could also indicate that a change from neutral-threaded apartment to single-threaded apartment has occurred.</defintion></item>
		/// </list>
		/// </returns>
		[DllImport(nameof(Ole32), ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = false)]
		public static extern HRESULT CoInitializeEx(IntPtr pvReserved, COINIT coInit);

		/// <summary>
		/// Closes the COM library on the current thread, unloads all DLLs loaded by the thread, frees any other resources that the thread maintains, and forces all RPC connections on the thread to close.
		/// </summary>
		[DllImport(nameof(Ole32), ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = false)]
		public static extern void CoUninitialize();

		/// <summary>
		/// The PropVariantClear function frees all elements that can be freed in a given PROPVARIANT structure. For complex elements with known element pointers, the underlying elements are freed prior to freeing the containing element.
		/// </summary>
		/// <param name="pvar">A pointer to an initialized PROPVARIANT structure for which any deallocatable elements are to be freed. On return, all zeroes are written to the PROPVARIANT structure.</param>
		/// <returns>
		/// <list type="definition">
		/// <item><term>S_OK</term><definition>The VT types are recognized and all items that can be freed have been freed.</definition></item>
		/// <item><term>STG_E_INVALID_PARAMETER</term><definition>The variant has an unknown VT type.</definition></item>
		/// </list>
		/// </returns>
		[SecurityCritical, SuppressUnmanagedCodeSecurity]
		[DllImport(nameof(Ole32), ExactSpelling = true, SetLastError = false)]
		public static extern HRESULT PropVariantClear(PROPVARIANT pvar);

		/// <summary>
		/// The PropVariantCopy function copies the contents of one PROPVARIANT structure to another.
		/// </summary>
		/// <param name="pDst">Pointer to an uninitialized PROPVARIANT structure that receives the copy.</param>
		/// <param name="pSrc">Pointer to the PROPVARIANT structure to be copied.</param>
		/// <returns>
		/// <list type="definition">
		/// <item><term>S_OK</term><definition>The VT types are recognized and all items that can be freed have been freed.</definition></item>
		/// <item><term>STG_E_INVALID_PARAMETER</term><definition>The variant has an unknown VT type.</definition></item>
		/// </list>
		/// </returns>
		[DllImport(nameof(Ole32))]
		public static extern HRESULT PropVariantCopy([In, Out] PROPVARIANT pDst, [In] PROPVARIANT pSrc);

		/// <summary>
		/// Frees the specified storage medium.
		/// </summary>
		/// <param name="pMedium">Pointer to the storage medium that is to be freed.</param>
		[DllImport(nameof(Ole32))]
		public static extern void ReleaseStgMedium([In] ref STGMEDIUM pMedium);

		[StructLayout(LayoutKind.Sequential)]
		public struct STGMEDIUM
		{
			[MarshalAs(UnmanagedType.U4)]
			public int tymed;
			public IntPtr data;
			[MarshalAs(UnmanagedType.IUnknown)]
			public object pUnkForRelease;
		}
	}
}
