using System;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
	/// <summary>
	/// Specifies a date and time, using individual members for the month, day, year, weekday, hour, minute, second, and millisecond. The time is either in
	/// coordinated universal time (UTC) or local time, depending on the function that is being called.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	// ReSharper disable once InconsistentNaming
	public struct SYSTEMTIME : IEquatable<SYSTEMTIME>, IComparable<SYSTEMTIME>
	{
		/// <summary>The year. The valid values for this member are 1601 through 30827.</summary>
		public ushort wYear;
		/// <summary>
		/// The month. This member can be one of the following values.
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <term>Meaning</term>
		/// </listheader>
		/// <item>
		/// <term>1</term>
		/// <term>January</term>
		/// </item>
		/// <item>
		/// <term>2</term>
		/// <term>February</term>
		/// </item>
		/// <item>
		/// <term>3</term>
		/// <term>March</term>
		/// </item>
		/// <item>
		/// <term>4</term>
		/// <term>April</term>
		/// </item>
		/// <item>
		/// <term>5</term>
		/// <term>May</term>
		/// </item>
		/// <item>
		/// <term>6</term>
		/// <term>June</term>
		/// </item>
		/// <item>
		/// <term>7</term>
		/// <term>July</term>
		/// </item>
		/// <item>
		/// <term>8</term>
		/// <term>August</term>
		/// </item>
		/// <item>
		/// <term>9</term>
		/// <term>September</term>
		/// </item>
		/// <item>
		/// <term>10</term>
		/// <term>October</term>
		/// </item>
		/// <item>
		/// <term>11</term>
		/// <term>November</term>
		/// </item>
		/// <item>
		/// <term>12</term>
		/// <term>December</term>
		/// </item>
		/// </list>
		/// </summary>
		public ushort wMonth;
		/// <summary>
		/// The day of the week. This member can be one of the following values.
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <term>Meaning</term>
		/// </listheader>
		/// <item>
		/// <term>0</term>
		/// <term>Sunday</term>
		/// </item>
		/// <item>
		/// <term>1</term>
		/// <term>Monday</term>
		/// </item>
		/// <item>
		/// <term>2</term>
		/// <term>Tuesday</term>
		/// </item>
		/// <item>
		/// <term>3</term>
		/// <term>Wednesday</term>
		/// </item>
		/// <item>
		/// <term>4</term>
		/// <term>Thursday</term>
		/// </item>
		/// <item>
		/// <term>5</term>
		/// <term>Friday</term>
		/// </item>
		/// <item>
		/// <term>6</term>
		/// <term>Saturday</term>
		/// </item>
		/// </list>
		/// </summary>
		public ushort wDayOfWeek;
		/// <summary>The day of the month. The valid values for this member are 1 through 31.</summary>
		public ushort wDay;
		/// <summary>The hour. The valid values for this member are 0 through 23.</summary>
		public ushort wHour;
		/// <summary>The minute. The valid values for this member are 0 through 59.</summary>
		public ushort wMinute;
		/// <summary>The second. The valid values for this member are 0 through 59.</summary>
		public ushort wSecond;
		/// <summary>The millisecond. The valid values for this member are 0 through 999.</summary>
		public ushort wMilliseconds;

		private static readonly int[] DaysToMonth365 = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
		private static readonly int[] DaysToMonth366 = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

		/// <summary>Initializes a new instance of the <see cref="SYSTEMTIME"/> struct with a <see cref="DateTime"/>.</summary>
		/// <param name="dt">The <see cref="DateTime"/> value.</param>
		public SYSTEMTIME(DateTime dt)
		{
			dt = dt.ToLocalTime();
			wYear = Convert.ToUInt16(dt.Year);
			if (wYear < 1601) throw new ArgumentOutOfRangeException(nameof(dt), @"Year value must be 1601 through 30827");
			wMonth = Convert.ToUInt16(dt.Month);
			wDayOfWeek = Convert.ToUInt16(dt.DayOfWeek);
			wDay = Convert.ToUInt16(dt.Day);
			wHour = Convert.ToUInt16(dt.Hour);
			wMinute = Convert.ToUInt16(dt.Minute);
			wSecond = Convert.ToUInt16(dt.Second);
			wMilliseconds = Convert.ToUInt16(dt.Millisecond);
		}

		/// <summary>Initializes a new instance of the <see cref="SYSTEMTIME"/> struct.</summary>
		/// <param name="year">The year.</param>
		/// <param name="month">The month.</param>
		/// <param name="day">The day.</param>
		/// <param name="hour">The hour.</param>
		/// <param name="minute">The minute.</param>
		/// <param name="second">The second.</param>
		/// <param name="millisecond">The millisecond.</param>
		public SYSTEMTIME(ushort year, ushort month, ushort day, ushort hour = 0, ushort minute = 0, ushort second = 0, ushort millisecond = 0)
		{
			if (year < 1601) throw new ArgumentOutOfRangeException(nameof(year), @"year value must be 1601 through 30827");
		    // ReSharper disable once ObjectCreationAsStatement
			new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
			wYear = year;
			wMonth = month;
			wDay = day;
			wHour = hour;
			wMinute = minute;
			wSecond = second;
			wMilliseconds = millisecond;
			wDayOfWeek = 0;
		}

		/// <summary>Gets or sets the day of the week.</summary>
		/// <value>The day of the week.</value>
		public DayOfWeek DayOfWeek { get { return (DayOfWeek)wDayOfWeek; } set { wDayOfWeek = (ushort)value; } }

		/// <summary>Gets the number of ticks that represent the date and time of this instance.</summary>
		public long Ticks
		{
			get
			{
				var leap = wYear % 4 == 0 && (wYear % 100 != 0 || wYear % 400 == 0);
				var days = leap ? DaysToMonth366 : DaysToMonth365;
				var y = wYear - 1;
				var n = y * 365 + y / 4 - y / 100 + y / 400 + days[wMonth - 1] + wDay - 1;
				return new TimeSpan(n, wHour, wMinute, wSecond, wMilliseconds).Ticks;
			}
		}

		/// <summary>Performs an implicit conversion from <see cref="SYSTEMTIME"/> to <see cref="DateTime"/>.</summary>
		/// <param name="st">The <see cref="SYSTEMTIME"/> value to convert.</param>
		/// <returns>The <see cref="DateTime"/> equivalent.</returns>
		public static implicit operator DateTime(SYSTEMTIME st)
		{
			if (st.wYear == 0 || st == MinValue)
				return DateTime.MinValue;
			if (st == MaxValue)
				return DateTime.MaxValue;
			return new DateTime(st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds, DateTimeKind.Local);
		}

		/// <summary>Performs an implicit conversion from <see cref="DateTime"/> to <see cref="SYSTEMTIME"/>.</summary>
		/// <param name="dt">The dt.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator SYSTEMTIME(DateTime dt) => new SYSTEMTIME(dt);

		/// <summary>Indicates if two <see cref="SYSTEMTIME"/> values are equal.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if both values are equal; otherwise <c>false</c>.</returns>
		public static bool operator ==(SYSTEMTIME s1, SYSTEMTIME s2) => s1.wYear == s2.wYear && s1.wMonth == s2.wMonth && s1.wDay == s2.wDay &&
			s1.wHour == s2.wHour && s1.wMinute == s2.wMinute && s1.wSecond == s2.wSecond && s1.wMilliseconds == s2.wMilliseconds;

		/// <summary>Indicates if two <see cref="SYSTEMTIME"/> values are not equal.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if both values are not equal; otherwise <c>false</c>.</returns>
		public static bool operator !=(SYSTEMTIME s1, SYSTEMTIME s2) => !(s1 == s2);

		/// <summary>Determines whether one specified <see cref="SYSTEMTIME"/> is greater than another specified <see cref="SYSTEMTIME"/>.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if <paramref name="s1"/> is greater than <paramref name="s2"/>; otherwise, <c>false</c>.</returns>
		public static bool operator >(SYSTEMTIME s1, SYSTEMTIME s2) => s1.Ticks > s2.Ticks;

		/// <summary>Determines whether one specified <see cref="SYSTEMTIME"/> is greater than or equal to another specified <see cref="SYSTEMTIME"/>.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if <paramref name="s1"/> is greater than or equal to <paramref name="s2"/>; otherwise, <c>false</c>.</returns>
		public static bool operator >=(SYSTEMTIME s1, SYSTEMTIME s2) => s1.Ticks >= s2.Ticks;

		/// <summary>Determines whether one specified <see cref="SYSTEMTIME"/> is less than another specified <see cref="SYSTEMTIME"/>.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if <paramref name="s1"/> is less than <paramref name="s2"/>; otherwise, <c>false</c>.</returns>
		public static bool operator <(SYSTEMTIME s1, SYSTEMTIME s2) => s1.Ticks < s2.Ticks;

		/// <summary>Determines whether one specified <see cref="SYSTEMTIME"/> is less than or equal to another specified <see cref="SYSTEMTIME"/>.</summary>
		/// <param name="s1">The first <see cref="SYSTEMTIME"/> value.</param>
		/// <param name="s2">The second <see cref="SYSTEMTIME"/> value.</param>
		/// <returns><c>true</c> if <paramref name="s1"/> is less than or equal to <paramref name="s2"/>; otherwise, <c>false</c>.</returns>
		public static bool operator <=(SYSTEMTIME s1, SYSTEMTIME s2) => s1.Ticks <= s2.Ticks;

		/// <summary>The minimum value supported by <see cref="SYSTEMTIME"/>.</summary>
		public static readonly SYSTEMTIME MinValue = new SYSTEMTIME(1601, 1, 1);

		/// <summary>The maximum value supported by <see cref="SYSTEMTIME"/>.</summary>
		public static readonly SYSTEMTIME MaxValue = new SYSTEMTIME(30827, 12, 31, 23, 59, 59, 999);

		/// <summary>Compares two instances of <see cref="SYSTEMTIME"/> and returns an integer that indicates whether the first instance is earlier than, the same as, or later than the second instance.</summary>
		/// <param name="t1">The first object to compare. </param>
		/// <param name="t2">The second object to compare. </param>
		/// <returns>A signed number indicating the relative values of t1 and t2.
		/// <list type="table">
		/// <listheader><term>Value Type</term><term>Condition</term></listheader>
		/// <item><term>Less than zero</term><term>t1 is earlier than t2.</term></item>
		/// <item><term>Zero</term><term>t1 is the same as t2.</term></item>
		/// <item><term>Greater than zero</term><term>t1 is later than t2.</term></item>
		/// </list>
		///</returns>
		public static int Compare(SYSTEMTIME t1, SYSTEMTIME t2)
		{
			var ticks1 = t1.Ticks;
			var ticks2 = t2.Ticks;
			if (ticks1 > ticks2) return 1;
			if (ticks1 < ticks2) return -1;
			return 0;
		}

		/// <summary>Compares the current object with another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero
		/// This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object
		/// is greater than <paramref name="other"/>.
		/// </returns>
		public int CompareTo(SYSTEMTIME other) => Compare(this, other);

		/// <summary>Determines whether the specified <see cref="System.Object"/>, is equal to this instance.</summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is SYSTEMTIME)
				return (SYSTEMTIME)obj == this;
			return base.Equals(obj);
		}

		/// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(SYSTEMTIME other) => this == other;

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode() => base.GetHashCode();

		/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString() => ((DateTime)this).ToString();
	}
}