using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class PropSys
	{
		public enum PROPVAR_CHANGE_FLAGS
		{
			PVCHF_DEFAULT = 0x00000000,
			PVCHF_NOVALUEPROP = 0x00000001,       // Maps to VARIANT_NOVALUEPROP for VariantChangeType
			PVCHF_ALPHABOOL = 0x00000002,       // Maps to VARIANT_ALPHABOOL for VariantChangeType
			PVCHF_NOUSEROVERRIDE = 0x00000004,       // Maps to VARIANT_NOUSEROVERRIDE for VariantChangeType
			PVCHF_LOCALBOOL = 0x00000008,       // Maps to VARIANT_LOCALBOOL for VariantChangeType
			PVCHF_NOHEXSTRING = 0x00000010,       // Don't convert a string that looks like hexadecimal (0xABCD) to the numerical equivalent
		}

		[Flags]
		public enum PROPVAR_COMPARE_FLAGS
		{
			/// <summary>When comparing strings, use StrCmpLogical</summary>
			PVCF_DEFAULT = 0x00000000,
			/// <summary>Empty/null values are greater-than non-empty values</summary>
			PVCF_TREATEMPTYASGREATERTHAN = 0x00000001,
			/// <summary>When comparing strings, use StrCmp</summary>
			PVCF_USESTRCMP = 0x00000002,
			/// <summary>When comparing strings, use StrCmpC</summary>
			PVCF_USESTRCMPC = 0x00000004,
			/// <summary>When comparing strings, use StrCmpI</summary>
			PVCF_USESTRCMPI = 0x00000008,
			/// <summary>When comparing strings, use StrCmpIC</summary>
			PVCF_USESTRCMPIC = 0x00000010,
			/// <summary>When comparing strings, use CompareStringEx with LOCALE_NAME_USER_DEFAULT and SORT_DIGITSASNUMBERS.  This corresponds to the linguistically correct order for UI lists.</summary>
			PVCF_DIGITSASNUMBERS_CASESENSITIVE = 0x00000020,
		}

		public enum PROPVAR_COMPARE_UNIT
		{
			PVCU_DEFAULT = 0,
			PVCU_SECOND = 1,
			PVCU_MINUTE = 2,
			PVCU_HOUR = 3,
			PVCU_DAY = 4,
			PVCU_MONTH = 5,
			PVCU_YEAR = 6
		}
		
		public enum PSTIME_FLAGS
		{
			PSTF_UTC = 0x00000000,
			PSTF_LOCAL = 0x00000001
		}

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure from a specified Boolean vector.
		/// </summary>
		/// <param name="prgf">Pointer to the Boolean vector used to initialize the structure. If this parameter is NULL, the elements pointed to by the cabool.pElems structure member are initialized with VARIANT_FALSE.</param>
		/// <param name="cElems">The number of vector elements.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromBooleanVector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.Bool)] bool[] prgf, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure using the contents of a buffer.
		/// </summary>
		/// <param name="pv">Pointer to the buffer.</param>
		/// <param name="cb">The length of the buffer, in bytes.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromBuffer([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.U1)] byte[] pv, uint cb, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a class identifier (CLSID).
		/// </summary>
		/// <param name="clsid">Reference to the CLSID.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromCLSID([In] ref Guid clsid, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a specified vector of double values.
		/// </summary>
		/// <param name="prgn">Pointer to a double vector. If this value is NULL, the elements pointed to by the cadbl.pElems structure member are initialized with 0.0.</param>
		/// <param name="cElems">The number of vector elements.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromDoubleVector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.R8)] double[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on information stored in a FILETIME structure.
		/// </summary>
		/// <param name="pftIn">Pointer to the date and time as a FILETIME structure.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromFileTime([In] ref FILETIME pftIn, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure from a specified vector of <see cref="FILETIME"/> values.
		/// </summary>
		/// <param name="prgft">Pointer to the date and time as a <see cref="FILETIME"/> vector. If this value is NULL, the elements pointed to by the cafiletime.pElems structure member is initialized with (FILETIME)0.</param>
		/// <param name="cElems">The number of vector elements.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromFileTimeVector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] FILETIME[] prgft, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a specified vector of 16-bit integer values.
		/// </summary>
		/// <param name="prgn">Pointer to a source vector of SHORT values. If this parameter is NULL, the vector is initialized with zeros.</param>
		/// <param name="cElems">The number of elements in the vector.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromInt16Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.I2)] short[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a specified vector of 32-bit integer values.
		/// </summary>
		/// <param name="prgn">Pointer to a source vector of LONG values. If this parameter is NULL, the vector is initialized with zeros.</param>
		/// <param name="cElems">The number of elements in the vector.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromInt32Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.I4)] int[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a specified vector of 64-bit integer values.
		/// </summary>
		/// <param name="prgn">Pointer to a source vector of LONGLONG values. If this parameter is NULL, the vector is initialized with zeros.</param>
		/// <param name="cElems">The number of elements in the vector.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromInt64Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.I8)] long[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Initializes a <see cref="Ole32.PROPVARIANT"/> structure based on a specified PROPVARIANT vector element.
		/// </summary>
		/// <param name="propvarIn">The source <see cref="Ole32.PROPVARIANT"/> structure.</param>
		/// <param name="iElem">The index of the source <see cref="Ole32.PROPVARIANT"/> structure element.</param>
		/// <param name="ppropvar">When this function returns, contains the initialized <see cref="Ole32.PROPVARIANT"/> structure.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromPropVariantVectorElem([In] Ole32.PROPVARIANT propvarIn, uint iElem, [In, Out] Ole32.PROPVARIANT ppropvar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromStringVector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.LPWStr)] string[] prgsz, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromUInt16Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.U2)] ushort[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromUInt32Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.U4)] uint[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT InitPropVariantFromUInt64Vector([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1, ArraySubType = UnmanagedType.U8)] ulong[] prgn, uint cElems, [In, Out] Ole32.PROPVARIANT ppropvar);

		/// <summary>
		/// Retrieves the property's canonical name given its PROPERTYKEY.
		/// </summary>
		/// <param name="propkey">A pointer to a PROPERTYKEY structure containing the property's identifiers.</param>
		/// <param name="ppszCanonicalName">The address of a pointer to a buffer that receives the property name as a null-terminated Unicode string. It is the responsibility of the caller to release this string through a call to CoTaskMemFree once it is no longer needed.</param>
		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PSGetNameFromPropertyKey(ref Ole32.PROPERTYKEY propkey, out SafeCoTaskMemString ppszCanonicalName);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantChangeType(Ole32.PROPVARIANT ppropvarDest, [In] Ole32.PROPVARIANT propvarSrc, PROPVAR_CHANGE_FLAGS flags, ushort vt);

		[DllImport(nameof(PropSys), ExactSpelling = true, PreserveSig = true)]
		public static extern int PropVariantCompare(Ole32.PROPVARIANT propvar1, Ole32.PROPVARIANT propvar2);

		[DllImport(nameof(PropSys), ExactSpelling = true, PreserveSig = true)]
		public static extern int PropVariantCompareEx(Ole32.PROPVARIANT propvar1, Ole32.PROPVARIANT propvar2, PROPVAR_COMPARE_UNIT unit, PROPVAR_COMPARE_FLAGS flags);

		[DllImport(nameof(PropSys), ExactSpelling = true, PreserveSig = true)]
		[return: MarshalAs(UnmanagedType.I4)]
		public static extern int PropVariantGetElementCount([In] Ole32.PROPVARIANT propVar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToBoolean([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.Bool)] out bool pfRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToBooleanVectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToBSTR([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.BStr)] out BStrWrapper pbstrOut);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToBuffer([In] Ole32.PROPVARIANT propVar, [In, Out] SafeCoTaskMemHandle pv, uint cb);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToDouble([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.R8)] out double pdblRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToDoubleVectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToFileTime([In] Ole32.PROPVARIANT propVar, PSTIME_FLAGS pstfOut, out FILETIME pftOut);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToFileTimeVectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToGUID([In] Ole32.PROPVARIANT propVar, out Guid pguid);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt16([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.I2)] out short piRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt16VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt32([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.I4)] out int plRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt32VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt64([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.I8)] out long pllRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToInt64VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToStringAlloc([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.LPWStr)] out SafeCoTaskMemString ppszOut);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToStringVectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt16([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.U2)] out ushort puiRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt16VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt32([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.U4)] out uint pulRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt32VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt64([In] Ole32.PROPVARIANT propVar, [MarshalAs(UnmanagedType.U8)] out ulong pullRet);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToUInt64VectorAlloc([In] Ole32.PROPVARIANT propVar, out SafeCoTaskMemHandle pprgf, out uint pcElem);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT PropVariantToVariant([In] Ole32.PROPVARIANT pPropVar, IntPtr pVar);

		[DllImport(nameof(PropSys), ExactSpelling = true)]
		public static extern HRESULT VariantToPropVariant([In] IntPtr pVar, Ole32.PROPVARIANT pPropVar);
	}
}
