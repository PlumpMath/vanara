using System;

namespace Vanara.Extensions
{
	public static partial class DateTimeExtensions
	{
		/// <summary>
		/// Converts a <see cref="DateTime"/> structure to a <see cref="System.Runtime.InteropServices.ComTypes.FILETIME"/> structure.
		/// </summary>
		/// <param name="dt">The <see cref="DateTime"/> value to convert.</param>
		/// <returns>The resulting <see cref="System.Runtime.InteropServices.ComTypes.FILETIME"/> structure.</returns>
		public static System.Runtime.InteropServices.ComTypes.FILETIME ToFileTimeStruct(this DateTime dt)
		{
			var l = dt.Kind == DateTimeKind.Utc ? dt.ToFileTimeUtc() : dt.ToFileTime();
			return new System.Runtime.InteropServices.ComTypes.FILETIME
			{
				dwHighDateTime = (int) (l >> 32),
				dwLowDateTime = (int) (l & 0xFFFFFFFF)
			};
		}

		/// <summary>
		/// Converts a <see cref="System.Runtime.InteropServices.ComTypes.FILETIME"/> structure to a <see cref="DateTime"/> structure.
		/// </summary>
		/// <param name="ft">The <see cref="System.Runtime.InteropServices.ComTypes.FILETIME"/> value to convert.</param>
		/// <param name="kind">The <see cref="DateTimeKind"/> value to use to determine local or UTC time.</param>
		/// <returns>The resulting <see cref="DateTime"/> structure.</returns>
		public static DateTime ToDateTime(this System.Runtime.InteropServices.ComTypes.FILETIME ft, DateTimeKind kind = DateTimeKind.Utc)
		{
			unchecked
			{
				var hFT2 = ((long) ft.dwHighDateTime << 32) | (uint) ft.dwLowDateTime;
				return kind == DateTimeKind.Utc ? DateTime.FromFileTimeUtc(hFT2) : DateTime.FromFileTime(hFT2);
			}
		}
	}
}