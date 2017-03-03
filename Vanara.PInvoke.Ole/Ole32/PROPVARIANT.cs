using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using Vanara.Extensions;
using static Vanara.PInvoke.OleAut32;
using static Vanara.PInvoke.PropSys;
using static Vanara.PInvoke.Windows;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Vanara.PInvoke
{
	public static partial class Ole32
	{
		[Flags]
		public enum IntVarEnum : ushort
		{
			VT_EMPTY = 0,
			VT_NULL = 1,
			VT_I2 = 2,
			VT_I4 = 3,
			VT_R4 = 4,
			VT_R8 = 5,
			VT_CY = 6,
			VT_DATE = 7,
			VT_BSTR = 8,
			VT_DISPATCH = 9,
			VT_ERROR = 10,
			VT_BOOL = 11,
			VT_VARIANT = 12,
			VT_UNKNOWN = 13,
			VT_DECIMAL = 14,
			VT_I1 = 16,
			VT_UI1 = 17,
			VT_UI2 = 18,
			VT_UI4 = 19,
			VT_I8 = 20,
			VT_UI8 = 21,
			VT_INT = 22,
			VT_UINT = 23,
			VT_VOID = 24,
			VT_HRESULT = 25,
			VT_PTR = 26,
			VT_SAFEARRAY = 27,
			VT_CARRAY = 28,
			VT_USERDEFINED = 29,
			VT_LPSTR = 30,
			VT_LPWSTR = 31,
			VT_RECORD = 36,
			VT_FILETIME = 64,
			VT_BLOB = 65,
			VT_STREAM = 66,
			VT_STORAGE = 67,
			VT_STREAMED_OBJECT = 68,
			VT_STORED_OBJECT = 69,
			VT_BLOB_OBJECT = 70,
			VT_CF = 71,
			VT_CLSID = 72,
			VT_VECTOR = 0x1000,
			VT_ARRAY = 0x2000,
			VT_BYREF = 0x4000,
		}

		/// <summary>BLOB</summary>
		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct BLOB
		{
			public uint cbSize;
			public IntPtr pBlobData;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct CLIPDATA
		{
			public int cbSize;
			public int ulClipFmt;
			public IntPtr pClipData;

			public CLIPDATA(IntPtr fmtNamePtr, int fmtNameLength)
			{
				cbSize = Marshal.SystemDefaultCharSize * fmtNameLength + Marshal.SizeOf(typeof(CLIPDATA));
				ulClipFmt = fmtNameLength;
				pClipData = fmtNamePtr;
			}

			public CLIPDATA(uint fmtValue)
			{
				cbSize = Marshal.SizeOf(typeof(CLIPDATA));
				ulClipFmt = -1;
				pClipData = (IntPtr)fmtValue;
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 16)]
		public sealed class PROPVARIANT : ICloneable, IComparable<PROPVARIANT>, IDisposable, IEquatable<PROPVARIANT>
		{
			[FieldOffset(0)] public ushort vt;
			[FieldOffset(2)] public ushort wReserved1;
			[FieldOffset(4)] public ushort wReserved2;
			[FieldOffset(6)] public ushort wReserved3;
			[FieldOffset(0)] internal decimal _decimal;
			[FieldOffset(8)] internal IntPtr _ptr;
			[FieldOffset(8)] internal FILETIME _ft;
			[FieldOffset(8)] internal BLOB _blob;
			[FieldOffset(8)] internal ulong _ulong;

			public PROPVARIANT()
			{
			}

			public PROPVARIANT(object obj, VarEnum type = VarEnum.VT_EMPTY)
			{
				SetValue(obj, type);
			}

			public PROPVARIANT(object[] parray)
			{
				SetSafeArray(parray);
			}

			public PROPVARIANT(string[] strArray, VarEnum type = VarEnum.VT_LPWSTR)
			{
				SetStringVector(strArray, type);
			}

			public PROPVARIANT(PROPVARIANT sourceVar)
			{
				PropVariantCopy(this, sourceVar);
			}

			private PROPVARIANT(VarEnum type)
			{
				VarType = type;
			}

			public BLOB blob => GetRawValue<BLOB>().GetValueOrDefault();
			public bool boolVal => GetRawValue<bool>().GetValueOrDefault();
			public string bstrVal => GetString(VarType);
			public byte bVal => GetRawValue<byte>().GetValueOrDefault();
			public IEnumerable<bool> cabool => GetVector<short>().Select(s => s != 0);
			public IEnumerable<string> cabstr => GetStringVector();
			public IEnumerable<sbyte> cac => GetVector<sbyte>();
			public IEnumerable<CLIPDATA> caclipdata => GetVector<CLIPDATA>();
			public IEnumerable<decimal> cacy => GetVector<long>().Select(decimal.FromOACurrency);
			public IEnumerable<DateTime> cadate => GetVector<double>().Select(DateTime.FromOADate);
			public IEnumerable<double> cadbl => GetVector<double>();
			public IEnumerable<FILETIME> cafiletime => GetVector<FILETIME>();
			public IEnumerable<float> caflt => GetVector<float>();
			public IEnumerable<long> cah => GetVector<long>();
			public IEnumerable<short> cai => GetVector<short>();
			public IEnumerable<int> cal => GetVector<int>();
			public IEnumerable<string> calpstr => GetStringVector();
			public IEnumerable<string> calpwstr => GetStringVector();
			public IEnumerable<PROPVARIANT> capropvar => GetVector<PROPVARIANT>();
			public IEnumerable<Win32Error> cascode => GetVector<Win32Error>();
			public IEnumerable<byte> caub => GetVector<byte>();
			public IEnumerable<ulong> cauh => GetVector<ulong>();
			public IEnumerable<ushort> caui => GetVector<ushort>();
			public IEnumerable<uint> caul => GetVector<uint>();
			public IEnumerable<Guid> cauuid => GetVector<Guid>();
			public sbyte cVal => GetRawValue<sbyte>().GetValueOrDefault();
			public decimal cyVal => decimal.FromOACurrency(GetRawValue<long>().GetValueOrDefault());
			public DateTime date => DateTime.FromOADate(GetRawValue<double>().GetValueOrDefault());
			public double dblVal => GetRawValue<double>().GetValueOrDefault();
			public FILETIME filetime => _ft;
			public float fltVal => GetRawValue<float>().GetValueOrDefault();
			public long hVal => GetRawValue<long>().GetValueOrDefault();
			public int intVal => GetRawValue<int>().GetValueOrDefault();
			public short iVal => GetRawValue<short>().GetValueOrDefault();
			public int lVal => GetRawValue<int>().GetValueOrDefault();
			public IEnumerable<object> parray => GetSafeArray();
			public bool? pboolVal => GetRawValue<bool>();
			public string pbstrVal => GetString(VarType);
			public byte? pbVal => GetRawValue<byte>();
			public CLIPDATA pclipdata => GetRawValue<CLIPDATA>().GetValueOrDefault();
			public sbyte? pcVal => GetRawValue<sbyte>();

			public decimal? pcyVal
			{
				get
				{
					var d = GetRawValue<long>();
					return d.HasValue ? decimal.FromOACurrency(d.Value) : (decimal?)null;
				}
			}

			public DateTime? pdate
			{
				get
				{
					var d = GetRawValue<double>();
					return d.HasValue ? DateTime.FromOADate(d.Value) : (DateTime?)null;
				}
			}

			public double? pdblVal => GetRawValue<double>();
			public decimal? pdecVal => GetRawValue<decimal>();
			public IntPtr pdispVal => GetRawValue<IntPtr>().GetValueOrDefault();
			public float? pfltVal => GetRawValue<float>();
			public int? pintVal => GetRawValue<int>();
			public short? piVal => GetRawValue<short>();
			public int? plVal => GetRawValue<int>();
			public object ppdispVal => punkVal;
			public object ppunkVal => punkVal;

			public Win32Error? pscode
			{
				get
				{
					var r = GetRawValue<int>();
					return r.HasValue ? new Win32Error(r.Value) : (Win32Error?)null;
				}
			}

			public IStorage pStorage => _ptr == IntPtr.Zero ? null : (IStorage)Marshal.GetObjectForIUnknown(_ptr);
			public IStream pStream => _ptr == IntPtr.Zero ? null : (IStream)Marshal.GetObjectForIUnknown(_ptr);
			public string pszVal => GetString(VarType);
			public uint? puintVal => GetRawValue<uint>();
			public ushort? puiVal => GetRawValue<ushort>();
			public uint? pulVal => GetRawValue<uint>();
			public object punkVal => _ptr == IntPtr.Zero ? null : Marshal.GetObjectForIUnknown(_ptr);
			public Guid? puuid => GetRawValue<Guid>();
			public PROPVARIANT pvarVal => _ptr.ToStructure<PROPVARIANT>();
			public IntPtr pVersionedStream => GetRawValue<IntPtr>().GetValueOrDefault();
			public string pwszVal => GetString(VarType);
			public Win32Error scode => new Win32Error(GetRawValue<int>().GetValueOrDefault());
			public ulong uhVal => GetRawValue<ulong>().GetValueOrDefault();
			public uint uintVal => GetRawValue<uint>().GetValueOrDefault();
			public ushort uiVal => GetRawValue<ushort>().GetValueOrDefault();
			public uint ulVal => GetRawValue<uint>().GetValueOrDefault();
			//public IntPtr pparray { get { return GetRefValue<sbyte>(); } }

			public bool IsNullOrEmpty => VarType == VarEnum.VT_EMPTY || VarType == VarEnum.VT_NULL;

			public object Value
			{
				get
				{
					if (_vt.IsFlagSet(IntVarEnum.VT_ARRAY)) return GetSafeArray();
					var isVector = _vt.IsFlagSet(IntVarEnum.VT_VECTOR);
					var isRef = _vt.IsFlagSet(IntVarEnum.VT_BYREF);
					var elemType = (VarEnum)(vt & 0xFFF);
					switch (elemType)
					{
						case VarEnum.VT_EMPTY:
							return null;

						case VarEnum.VT_NULL:
							return DBNull.Value;

						case VarEnum.VT_I2:
							return isRef ? piVal : (isVector ? cai : (object)iVal);

						case VarEnum.VT_INT:
						case VarEnum.VT_I4:
							return isRef ? plVal : (isVector ? cal : (object)lVal);

						case VarEnum.VT_BSTR:
							return isRef ? pbstrVal : (isVector ? cabstr : (object)bstrVal);

						case VarEnum.VT_DISPATCH:
						case VarEnum.VT_UNKNOWN:
							return isRef
								? ppunkVal
								: (isVector ? GetVector<IntPtr>()?.Select(Marshal.GetObjectForIUnknown) : (object)IntPtr.Zero);

						case VarEnum.VT_BOOL:
							return isRef ? pboolVal : (isVector ? cabool : (object)boolVal);

						case VarEnum.VT_I1:
							return isRef ? pcVal : (isVector ? cac : (object)cVal);

						case VarEnum.VT_UI1:
							return isRef ? pbVal : (isVector ? caub : (object)bVal);

						case VarEnum.VT_UI2:
							return isRef ? puiVal : (isVector ? caui : (object)uiVal);

						case VarEnum.VT_UINT:
						case VarEnum.VT_UI4:
							return isRef ? pulVal : (isVector ? caul : (object)ulVal);

						case VarEnum.VT_ERROR:
							return isRef ? pscode : (isVector ? cascode : (object)scode);

						case VarEnum.VT_I8:
							return isRef ? hVal : (isVector ? cah : (object)hVal);

						case VarEnum.VT_UI8:
							return isRef ? uhVal : (isVector ? cauh : (object)uhVal);

						case VarEnum.VT_HRESULT:
							return isRef
								? (pulVal.HasValue ? new HRESULT(pulVal.Value) : (HRESULT?)null)
								: (isVector ? caul.Select(u => new HRESULT(u)) : (object)new HRESULT(ulVal));

						case VarEnum.VT_PTR:
						case VarEnum.VT_RECORD:
						case VarEnum.VT_USERDEFINED:
						case VarEnum.VT_VOID:
							return isRef ? IntPtr.Zero : (isVector ? GetVector<IntPtr>() : (object)_ptr);

						case VarEnum.VT_LPSTR:
							return isRef ? pszVal : (isVector ? calpstr : (object)pszVal);

						case VarEnum.VT_LPWSTR:
							return isRef ? pwszVal : (isVector ? calpwstr : (object)pwszVal);

						case VarEnum.VT_R8:
							return isRef ? dblVal : (isVector ? cadbl : (object)dblVal);

						case VarEnum.VT_DATE:
							return isRef ? pdate : (isVector ? cadate : (object)date);

						case VarEnum.VT_CY:
							return isRef ? pcyVal : (isVector ? cacy : (object)cyVal);

						case VarEnum.VT_DECIMAL:
							return isRef ? pdecVal : (isVector ? GetVector<decimal>() : (object)pdecVal.GetValueOrDefault());

						case VarEnum.VT_R4:
							return isRef ? pfltVal : (isVector ? caflt : (object)fltVal);

						case VarEnum.VT_FILETIME:
							return isRef ? filetime : (isVector ? cafiletime : (object)filetime);

						case VarEnum.VT_BLOB:
							return isRef ? _blob : (isVector ? null : (object)_blob);

						case VarEnum.VT_STREAM:
						case VarEnum.VT_STREAMED_OBJECT:
							return isRef ? pStream : (isVector ? null : pStream);

						case VarEnum.VT_STORAGE:
						case VarEnum.VT_STORED_OBJECT:
							return isRef ? pStorage : (isVector ? null : pStorage);

						case VarEnum.VT_CF:
							return isRef ? pclipdata : (isVector ? caclipdata : (object)pclipdata);

						case VarEnum.VT_CLSID:
							return isRef ? puuid : (isVector ? cauuid : (object)puuid.GetValueOrDefault());

						case VarEnum.VT_VARIANT:
							return isRef ? pvarVal : (isVector ? GetVector<PROPVARIANT>() : (object)pvarVal);

						default:
							return null;
					}
				}
				private set { SetValue(value); }
			}

			public VarEnum VarType
			{
				get { return (VarEnum)vt; }
				set { vt = (ushort)value; }
			}

			private IntVarEnum _vt => (IntVarEnum)vt;

			public static VarEnum GetVarEnum(Type type)
			{
				if (type == null)
					return VarEnum.VT_NULL;
				var elemtype = type.GetElementType() ?? type;

				if (type.IsArray && elemtype == typeof(object)) return VarEnum.VT_ARRAY | VarEnum.VT_VARIANT;

				var ret = type.IsArray ? VarEnum.VT_VECTOR : 0;
				if (elemtype.IsNullable()) ret |= VarEnum.VT_BYREF;

				if (elemtype == typeof(BLOB))
					return ret | VarEnum.VT_BLOB;
				if (elemtype == typeof(BStrWrapper))
					return ret | VarEnum.VT_BSTR;
				if (elemtype == typeof(CLIPDATA))
					return ret | VarEnum.VT_CF;
				if (elemtype == typeof(Guid))
					return ret | VarEnum.VT_CLSID;
				if (elemtype == typeof(CurrencyWrapper))
					return ret | VarEnum.VT_CY;
				if (elemtype == typeof(Win32Error))
					return ret | VarEnum.VT_ERROR;
				if (elemtype == typeof(FILETIME))
					return ret | VarEnum.VT_FILETIME;
				if (elemtype == typeof(HRESULT))
					return ret | VarEnum.VT_HRESULT;
				if (elemtype.IsCOMObject)
				{
					var intf = elemtype.GetInterfaces();
					if (intf.Contains(typeof(IStream))) return ret | VarEnum.VT_STREAM;
					if (intf.Contains(typeof(IStorage))) return ret | VarEnum.VT_STORAGE;
					return ret | VarEnum.VT_UNKNOWN;
				}
				if (elemtype == typeof(IntPtr))
					return VarEnum.VT_PTR;
				switch (Type.GetTypeCode(elemtype))
				{
					case TypeCode.DBNull:
						return ret | VarEnum.VT_NULL;

					case TypeCode.Boolean:
						return ret | VarEnum.VT_BOOL;

					case TypeCode.Char:
						return ret | VarEnum.VT_LPWSTR;

					case TypeCode.SByte:
						return ret | VarEnum.VT_I1;

					case TypeCode.Byte:
						return ret | VarEnum.VT_UI1;

					case TypeCode.Int16:
						return ret | VarEnum.VT_I2;

					case TypeCode.UInt16:
						return ret | VarEnum.VT_UI2;

					case TypeCode.Int32:
						return ret | VarEnum.VT_I4;

					case TypeCode.UInt32:
						return ret | VarEnum.VT_UI4;

					case TypeCode.Int64:
						return ret | VarEnum.VT_I8;

					case TypeCode.UInt64:
						return ret | VarEnum.VT_UI8;

					case TypeCode.Single:
						return ret | VarEnum.VT_R4;

					case TypeCode.Double:
						return ret | VarEnum.VT_R8;

					case TypeCode.Decimal:
						return type.IsArray ? VarEnum.VT_VECTOR | VarEnum.VT_DECIMAL : VarEnum.VT_DECIMAL | VarEnum.VT_BYREF;

					case TypeCode.DateTime:
						return ret | VarEnum.VT_DATE;

					case TypeCode.String:
						return ret | VarEnum.VT_LPWSTR;
				}
				return ret | VarEnum.VT_USERDEFINED;
			}

			[SecurityCritical, SecuritySafeCritical]
			public void Clear()
			{
				PropVariantClear(this);
			}

			public void Clone(out PROPVARIANT clone)
			{
				clone = new PROPVARIANT();
				PropVariantCopy(clone, this);
			}

			object ICloneable.Clone()
			{
				PROPVARIANT pv;
				Clone(out pv);
				return pv;
			}

			public int CompareTo(PROPVARIANT other) => PropVariantCompare(this, other);

			public override bool Equals(object obj)
			{
				var pv = obj as PROPVARIANT;
				return pv != null ? Equals(pv.Value) : base.Equals(obj);
			}

			public static PROPVARIANT FromNativeVariant(IntPtr pSrcNativeVariant)
			{
				var pv = new PROPVARIANT();
				VariantToPropVariant(pSrcNativeVariant, pv);
				return pv;
			}

			public bool Equals(PROPVARIANT other) => CompareTo(other) == 0;

			public override int GetHashCode() => _vt.GetHashCode();

			private void SetValue(object value, VarEnum vEnum = VarEnum.VT_EMPTY)
			{
				if (vEnum == VarEnum.VT_EMPTY)
					vEnum = GetVarEnum(value?.GetType());
				Clear();
				VarType = vEnum;

				// Finished if NULL or EMPTY
				if (vt <= 1) return;

				// Handle SAFEARRAY
				if (_vt.IsFlagSet(IntVarEnum.VT_ARRAY))
				{
					SetSafeArray((object[])value);
					return;
				}

				// Handle BYREF null value
				if (_vt.IsFlagSet(IntVarEnum.VT_BYREF) && value == null)
					return;

				// Handle case where element type is put in w/o specifying VECTOR
				if (value != null && value.GetType().IsArray) VarType |= VarEnum.VT_VECTOR;

				switch (VarType)
				{
					case VarEnum.VT_I1:
					case VarEnum.VT_BYREF | VarEnum.VT_I1:
						SetStruct((sbyte?)value, VarType);
						break;

					case VarEnum.VT_UI1:
					case VarEnum.VT_BYREF | VarEnum.VT_UI1:
						SetStruct((byte?)value, VarType);
						break;

					case VarEnum.VT_I2:
					case VarEnum.VT_BYREF | VarEnum.VT_I2:
						SetStruct((short?)value, VarType);
						break;

					case VarEnum.VT_UI2:
					case VarEnum.VT_BYREF | VarEnum.VT_UI2:
						SetStruct((ushort?)value, VarType);
						break;

					case VarEnum.VT_I4:
					case VarEnum.VT_INT:
					case VarEnum.VT_BYREF | VarEnum.VT_I4:
					case VarEnum.VT_BYREF | VarEnum.VT_INT:
						SetStruct((int?)value, VarType);
						break;

					case VarEnum.VT_UI4:
					case VarEnum.VT_UINT:
					case VarEnum.VT_BYREF | VarEnum.VT_UI4:
					case VarEnum.VT_BYREF | VarEnum.VT_UINT:
						SetStruct((uint?)value, VarType);
						break;

					case VarEnum.VT_I8:
					case VarEnum.VT_BYREF | VarEnum.VT_I8:
						SetStruct((long?)value, VarType);
						break;

					case VarEnum.VT_UI8:
					case VarEnum.VT_BYREF | VarEnum.VT_UI8:
						SetStruct((ulong?)value, VarType);
						break;

					case VarEnum.VT_R4:
					case VarEnum.VT_BYREF | VarEnum.VT_R4:
						SetStruct((float?)value, VarType);
						break;

					case VarEnum.VT_R8:
					case VarEnum.VT_BYREF | VarEnum.VT_R8:
						SetStruct((double?)value, VarType);
						break;

					case VarEnum.VT_BOOL:
					case VarEnum.VT_BYREF | VarEnum.VT_BOOL:
						SetStruct((bool?)value, VarType);
						break;

					case VarEnum.VT_ERROR:
					case VarEnum.VT_BYREF | VarEnum.VT_ERROR:
						{
							uint? i;
							if (value is Win32Error)
								i = (uint?)(int)(Win32Error)value;
							else
								i = (uint)Convert.ChangeType(value, typeof(uint));
							SetStruct(i, VarType);
						}
						break;

					case VarEnum.VT_HRESULT:
					case VarEnum.VT_BYREF | VarEnum.VT_HRESULT:
						{
							uint? i;
							if (value is HRESULT)
								i = (uint?)(int)(HRESULT)value;
							else
								i = (uint)Convert.ChangeType(value, typeof(uint));
							SetStruct(i, VarType);
						}
						break;

					case VarEnum.VT_CY:
					case VarEnum.VT_BYREF | VarEnum.VT_CY:
						{
							ulong? i;
							if (value is decimal)
								i = (ulong?)decimal.ToOACurrency((decimal)value);
							else
								i = (ulong)Convert.ChangeType(value, typeof(ulong));
							SetStruct(i, VarType);
						}
						break;

					case VarEnum.VT_DATE:
					case VarEnum.VT_BYREF | VarEnum.VT_DATE:
						{
							double? d = null;
							var dt = value as DateTime?;
							if (dt != null)
								d = dt.Value.ToOADate();
							var ft = value as FILETIME?;
							if (ft != null)
								d = ft.Value.ToDateTime().ToOADate();
							if (d == null)
								d = (double)Convert.ChangeType(value, typeof(double));
							SetStruct(d, VarType);
						}
						break;

					case VarEnum.VT_FILETIME:
						{
							FILETIME? ft;
							var dt = value as DateTime?;
							if (dt != null)
								ft = dt.Value.ToFileTimeStruct();
							else
								ft = value as FILETIME? ?? MakeFILETIME((ulong)Convert.ChangeType(value, typeof(ulong)));
							_ft = ft.GetValueOrDefault();
						}
						break;

					case VarEnum.VT_CLSID:
						SetStruct((Guid?)value, VarType);
						break;

					case VarEnum.VT_CF:
						SetStruct((CLIPDATA?)value, VarType);
						break;

					case VarEnum.VT_BLOB:
					case VarEnum.VT_BLOB_OBJECT:
						_blob = (BLOB)value;
						break;

					case VarEnum.VT_BSTR:
					case VarEnum.VT_BYREF | VarEnum.VT_BSTR:
						if (value is IntPtr)
							SetStruct((IntPtr?)value, VarType);
						else
							SetString(value?.ToString(), VarType);
						break;

					case VarEnum.VT_LPSTR:
					case VarEnum.VT_LPWSTR:
						SetString(value?.ToString(), VarType);
						break;

					case VarEnum.VT_UNKNOWN:
					case VarEnum.VT_BYREF | VarEnum.VT_UNKNOWN:
						{
							var p = value as IntPtr? ?? Marshal.GetIUnknownForObject(value);
							SetStruct<IntPtr>(p, VarType);
						}
						break;

					case VarEnum.VT_DISPATCH:
					case VarEnum.VT_BYREF | VarEnum.VT_DISPATCH:
						{
							var p = value as IntPtr? ?? Marshal.GetIDispatchForObject(value);
							SetStruct<IntPtr>(p, VarType);
						}
						break;

					case VarEnum.VT_STREAM:
					case VarEnum.VT_STREAMED_OBJECT:
						SetStruct<IntPtr>(Marshal.GetComInterfaceForObject(value, typeof(IStream)), VarType);
						break;

					case VarEnum.VT_STORAGE:
					case VarEnum.VT_STORED_OBJECT:
						SetStruct<IntPtr>(Marshal.GetComInterfaceForObject(value, typeof(IStorage)), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_I1:
						SetVector(ConvertToEnum<sbyte>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_UI1:
						SetVector(ConvertToEnum<byte>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_I2:
						SetVector(ConvertToEnum<short>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_UI2:
						SetVector(ConvertToEnum<ushort>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_I4:
					case VarEnum.VT_VECTOR | VarEnum.VT_INT:
						SetVector(ConvertToEnum<int>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_UI4:
					case VarEnum.VT_VECTOR | VarEnum.VT_UINT:
						SetVector(ConvertToEnum<uint>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_I8:
						SetVector(ConvertToEnum<long>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_UI8:
						SetVector(ConvertToEnum<ulong>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_R4:
						SetVector(ConvertToEnum<float>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_R8:
						SetVector(ConvertToEnum<double>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_BOOL:
						SetVector(ConvertToEnum<bool>(value).Select(b => (ushort)(b ? -1 : 0)), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_ERROR:
						{
							var ee = (value as IEnumerable<Win32Error>)?.Select(w => (uint)(int)w) ?? ConvertToEnum<uint>(value);
							SetVector(ee, VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_HRESULT:
						{
							var ee = (value as IEnumerable<HRESULT>)?.Select(w => (uint)(int)w) ?? ConvertToEnum<uint>(value);
							SetVector(ee, VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_CY:
						{
							var ecy = (value as IEnumerable<decimal>)?.Select(d => (ulong)decimal.ToOACurrency(d)) ??
									  ConvertToEnum<ulong>(value);
							SetVector(ecy, VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_DATE:
						{
							var ed = (value as IEnumerable<DateTime>)?.Select(d => d.ToOADate()) ??
									 (value as IEnumerable<FILETIME>)?.Select(ft => ft.ToDateTime().ToOADate()) ??
									 ConvertToEnum<double>(value);
							SetVector(ed, VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_FILETIME:
						{
							var ed = value as IEnumerable<FILETIME> ??
									 (value as IEnumerable<DateTime>)?.Select(d => d.ToFileTimeStruct()) ??
									 ConvertToEnum<ulong>(value)?.Select(MakeFILETIME);
							SetVector(ed, VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_CLSID:
						SetVector(ConvertToEnum<Guid>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_CF:
						SetVector(ConvertToEnum<CLIPDATA>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_BSTR:
						{
							var ep = value as IEnumerable<IntPtr>;
							if (ep != null)
								SetVector(ep, VarType);
							else
								SetStringVector(ConvertToEnum<string>(value), VarType);
						}
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_LPSTR:
					case VarEnum.VT_VECTOR | VarEnum.VT_LPWSTR:
						SetStringVector(ConvertToEnum<string>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_VARIANT:
						SetVector(ConvertToEnum<PROPVARIANT>(value), VarType);
						break;

					case VarEnum.VT_VECTOR | VarEnum.VT_DECIMAL:
						SetVector(ConvertToEnum<decimal>(value), VarType);
						break;

					case VarEnum.VT_BYREF | VarEnum.VT_DECIMAL:
						SetDecimal((decimal?)value);
						break;

					case VarEnum.VT_BYREF | VarEnum.VT_VARIANT:
						_ptr = this.StructureToPtr();
						break;

					case VarEnum.VT_VOID:
					case VarEnum.VT_PTR:
					case VarEnum.VT_USERDEFINED:
					case VarEnum.VT_RECORD:
						_ptr = (IntPtr)value;
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			public override string ToString()
			{
				var v = Value;
				if (v is FILETIME) v = ((FILETIME)v).ToString(null);
				return $"{_vt}={v ?? "null"}";
			}

			private static IEnumerable<T> ConvertToEnum<T>(object array, Func<object, T> conv = null)
			{
				if (array == null) return null;

				var iet = array as IEnumerable<T>;
				if (iet != null) return iet;

				if (conv == null) conv = o => (T)Convert.ChangeType(o, typeof(T));
				try
				{
					var ie = array as IEnumerable;
					return ie?.Cast<object>().Select(conv) ?? new[] { conv(array) };
				}
				catch { }
				return null;
			}

			private static string GetString(VarEnum ve, IntPtr ptr)
			{
				if (ve == VarEnum.VT_LPSTR)
					return Marshal.PtrToStringAnsi(ptr);
				if (ve == VarEnum.VT_LPWSTR)
					return Marshal.PtrToStringUni(ptr);
				return Marshal.PtrToStringBSTR(ptr);
			}

			private T? GetRawValue<T>() where T : struct
			{
				if ((vt <= 1 || (_vt & IntVarEnum.VT_BYREF) != 0) && _ptr == IntPtr.Zero)
					return null;

				var type = typeof(T);
				if (_vt == (IntVarEnum.VT_DECIMAL | IntVarEnum.VT_BYREF) && type != typeof(decimal))
					return null;
				if (type == typeof(IntPtr)) return (T)(object)_ptr;
				if (type == typeof(bool)) return (T)(object)((ushort)_ulong != 0);
				if (type.IsPrimitive)
				{
					unsafe
					{
						fixed (void* dataPtr = &_ulong)
						{
							/*if (_vt.IsFlagSet(IntVarEnum.VT_R4))
								try { return (T)Convert.ChangeType((float)Marshal.PtrToStructure(new IntPtr(dataPtr), typeof(float)), typeof(T)); } catch { return null; }
							if (_vt.IsFlagSet(IntVarEnum.VT_R8))
								try { return (T)Convert.ChangeType((double)Marshal.PtrToStructure(new IntPtr(dataPtr), typeof(double)), typeof(T)); } catch { return null; }*/
							return (T)Marshal.PtrToStructure(new IntPtr(dataPtr), typeof(T));
						}
					}
				}
				if (type == typeof(FILETIME)) return (T)(object)_ft;
				if (type == typeof(BLOB)) return (T)(object)_blob;
				if (type == typeof(decimal)) return (T)(object)_decimal;
				return _ptr.ToNullableStructure<T>();
			}

			private IEnumerable<object> GetSafeArray()
			{
				if (_ptr == IntPtr.Zero) return null;
				var dims = SafeArrayGetDim(_ptr);
				if (dims != 1) throw new NotSupportedException("Only single-dimensional arrays are supported");
				int lBound, uBound;
				SafeArrayGetLBound(_ptr, 1U, out lBound);
				SafeArrayGetUBound(_ptr, 1U, out uBound);
				var elemSz = SafeArrayGetElemsize(_ptr);
				if (elemSz == 0) throw new Win32Exception();
				IntPtr data;
				SafeArrayAccessData(_ptr, out data);
				try
				{
					return Marshal.GetObjectsForNativeVariants(data, uBound - lBound + 1);
				}
				finally
				{
					SafeArrayUnaccessData(_ptr);
				}
			}

			private string GetString(VarEnum ve) => GetString(ve, _ptr);

			private IntPtr GetStringPtr(string value, VarEnum ve)
			{
				if (value == null) return IntPtr.Zero;
				var ive = (IntVarEnum)ve;
				if (ive == IntVarEnum.VT_LPSTR)
					return Marshal.StringToCoTaskMemAnsi(value);
				if (ive == IntVarEnum.VT_LPWSTR)
					return Marshal.StringToCoTaskMemUni(value);
				if (ive.IsFlagSet(IntVarEnum.VT_BSTR))
					return Marshal.StringToBSTR(value);
				throw new ArgumentOutOfRangeException(nameof(ve));
			}

			private IEnumerable<string> GetStringVector()
			{
				var ve = (VarEnum)(vt & 0x0FFF);
				return _blob.pBlobData.ToIEnum<IntPtr>((int)_blob.cbSize).Select(p => GetString(ve, p));
			}

			private IEnumerable<T> GetVector<T>()
			{
				if ((vt & (ushort)VarEnum.VT_VECTOR) == 0)
					throw new InvalidCastException();
				return _blob.cbSize <= 0 ? new T[0] : _blob.pBlobData.ToIEnum<T>((int)_blob.cbSize);
			}

			private void SetDecimal(decimal? decVal)
			{
				var tempVt = vt;
				_decimal = decVal.GetValueOrDefault();
				vt = tempVt;
			}

			private void SetSafeArray(object[] array)
			{
				VarType = VarEnum.VT_ARRAY | VarEnum.VT_VARIANT;
				if (array == null || array.Length == 0) return;
				var psa = SafeArrayCreateVector((ushort)VarEnum.VT_VARIANT, 0, (uint)array.Length);
				if (psa == IntPtr.Zero) throw new Win32Exception();
				IntPtr p;
				SafeArrayAccessData(psa, out p);
				try
				{
					var elemSz = SafeArrayGetElemsize(psa);
					if (elemSz == 0) throw new Win32Exception();
					for (var i = 0; i < array.Length; ++i)
						Marshal.GetNativeVariantForObject(array[i], new IntPtr(p.ToInt64() + i * elemSz));
				}
				finally
				{
					SafeArrayUnaccessData(psa);
				}
				_ptr = psa;
			}

			private void SetString(string value, VarEnum ve)
			{
				VarType = ve;
				_ptr = GetStringPtr(value, ve);
				if (_ptr == IntPtr.Zero && ve == VarEnum.VT_BSTR)
					VarType = VarEnum.VT_BSTR | VarEnum.VT_BYREF;
			}

			[SecurityCritical, SecuritySafeCritical]
			private void SetStringVector(IEnumerable<string> value, VarEnum ve)
			{
				Clear();
				VarType = ve;

				var sc = value as IList<string> ?? value.ToList();
				var length = sc.Count;

				if (length <= 0) return;

				var destPtr = IntPtr.Zero;
				int sizeIntPtr;
				unsafe
				{
					sizeIntPtr = sizeof(IntPtr);
				}
				long size = sizeIntPtr * length;
				var index = 0;

				try
				{
					destPtr = Marshal.AllocCoTaskMem((int)size);

					for (index = 0; index < length; index++)
						Marshal.WriteIntPtr(destPtr, index * sizeIntPtr, GetStringPtr(sc[index], ve));

					_blob.cbSize = (uint)length;
					_blob.pBlobData = destPtr;
					destPtr = IntPtr.Zero;
				}
				finally
				{
					if (destPtr != IntPtr.Zero)
					{
						for (var i = 0; i < index; i++)
						{
							var pString = Marshal.ReadIntPtr(destPtr, i * sizeIntPtr);
							Marshal.FreeCoTaskMem(pString);
						}
						Marshal.FreeCoTaskMem(destPtr);
					}
				}
			}

			private void SetStruct<T>(T? value, VarEnum ve) where T : struct
			{
				Clear();
				VarType = ve;
				if (value.HasValue)
				{
					var type = typeof(T);
					if (type == typeof(IntPtr)) _ptr = (IntPtr)(object)value.Value;
					else if (type == typeof(bool)) _ptr = (IntPtr)(ushort)((bool)(object)value.Value ? -1 : 0);
					else if (value.Value.GetType().IsPrimitive)
						unsafe
						{
							fixed (void* ptr = &_ulong) Marshal.StructureToPtr(value.Value, new IntPtr(ptr), true);
						}
					else if (type == typeof(FILETIME)) _ft = (FILETIME)(object)value.Value;
					else if (type == typeof(BLOB)) _blob = (BLOB)(object)value.Value;
					else if (type == typeof(decimal)) SetDecimal((decimal)(object)value.Value);
					else _ptr = value.Value.StructureToPtr();
				}
				else
					vt |= (ushort)VarEnum.VT_BYREF;
			}

			private void SetVector<T>(IEnumerable<T> array, VarEnum varEnum)
			{
				Clear();
				VarType = varEnum | VarEnum.VT_VECTOR;
				if (array == null) return;
				var enumerable = array as ICollection<T> ?? array.ToList();
				_blob.cbSize = (uint)enumerable.Count;
				var sz = Marshal.SizeOf(typeof(T));
				_blob.pBlobData = Marshal.AllocCoTaskMem(sz * (int)_blob.cbSize);
				enumerable.MarshalToPtr(_blob.pBlobData);
			}

			public void Dispose()
			{
				PropVariantClear(this);
				GC.SuppressFinalize(this);
			}

			~PROPVARIANT()
			{
				Dispose();
			}
		}
	}
}