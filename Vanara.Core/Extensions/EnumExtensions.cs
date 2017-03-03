﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Vanara.Extensions
{
	/// <summary>
	/// Extensions for enumerated types.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Checks if <typeparam name="T"/> represents an enumeration and throws an exception if not.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> to validate.</typeparam>
		/// <param name="checkHasFlags">if set to <c>true</c> the check with also assert that the enumeration has the <see cref="FlagsAttribute"/> set and will throw an exception if not.</param>
		/// <exception cref="System.ArgumentException"></exception>
		private static void CheckIsEnum<T>(bool checkHasFlags = false)
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException($"Type '{typeof(T).FullName}' is not an enum");
			if (checkHasFlags && !IsFlags<T>())
				throw new ArgumentException($"Type '{typeof(T).FullName}' doesn't have the 'Flags' attribute");
		}

		/// <summary>
		/// Determines whether this enumerations has the <see cref="FlagsAttribute"/> set.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <returns>
		/// <c>true</c> if this instance has the <see cref="FlagsAttribute"/> set; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsFlags<T>() => Attribute.IsDefined(typeof(T), typeof(FlagsAttribute));

		/// <summary>
		/// Throws an exception if a flag value does not exist in a specified enumeration.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="value">The value to check.</param>
		/// <param name="argName">Name of the argument to display in the exception. "value" is used if no value or <c>null</c> is supplied.</param>
		/// <exception cref="InvalidEnumArgumentException"></exception>
		public static void CheckHasValue<T>(T value, string argName = null)
		{
			CheckIsEnum<T>();
			if (IsFlags<T>())
			{
				var allFlags = 0L;
				foreach (T flag in Enum.GetValues(typeof(T)))
					allFlags |= Convert.ToInt64(flag);
				if ((allFlags & Convert.ToInt64(value)) != 0L)
					return;
			}
			else if (Enum.IsDefined(typeof(T), value))
				return;
			throw new InvalidEnumArgumentException(argName ?? "value", Convert.ToInt32(value), typeof(T));
		}

		/// <summary>
		/// Determines whether the enumerated flag value has the specified flag set.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">The enumerated flag value.</param>
		/// <param name="flag">The flag value to check.</param>
		/// <returns><c>true</c> if is flag set; otherwise, <c>false</c>.</returns>
		public static bool IsFlagSet<T>(this T flags, T flag) where T : struct, IConvertible
		{
			CheckIsEnum<T>(true);
			var flagValue = Convert.ToInt64(flag);
			return (Convert.ToInt64(flags) & flagValue) == flagValue;
		}

		/// <summary>
		/// Set or unsets flags in a referenced enumerated value.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">A reference to an enumerated value.</param>
		/// <param name="flag">The flag to set or unset.</param>
		/// <param name="set">if set to <c>true</c> sets the flag; otherwise the flag is unset.</param>
		public static void SetFlags<T>(ref T flags, T flag, bool set = true) where T : struct, IConvertible
		{
			CheckIsEnum<T>(true);
			var flagsValue = Convert.ToInt64(flags);
			var flagValue = Convert.ToInt64(flag);
			if (set)
				flagsValue |= flagValue;
			else
				flagsValue &= (~flagValue);
			flags = (T)Enum.ToObject(typeof(T), flagsValue);
		}

		/// <summary>
		/// Set or unsets flags in an enumerated value and returns the new value.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">The enumerated value.</param>
		/// <param name="flag">The flag to set or unset.</param>
		/// <param name="set">if set to <c>true</c> sets the flag; otherwise the flag is unset.</param>
		/// <returns>
		/// The resulting enumerated value after the <paramref name="flag"/> has been set or unset.
		/// </returns>
		public static T SetFlags<T>(this T flags, T flag, bool set = true) where T : struct, IConvertible
		{
			var ret = flags;
			SetFlags(ref ret, flag, set);
			return ret;
		}

		/// <summary>
		/// Clears the specified flags from an enumerated value and returns the new value.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">The enumerated value.</param>
		/// <param name="flag">The flags to clear or unset.</param>
		/// <returns>
		/// The resulting enumerated value after the <paramref name="flag"/> has been unset.
		/// </returns>
		public static T ClearFlags<T>(this T flags, T flag) where T : struct, IConvertible => flags.SetFlags(flag, false);

		/// <summary>
		/// Gets the flags of an enumerated value as an enumerated list.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="value">The enumerated value.</param>
		/// <returns>An enumeration of individual flags that compose the <paramref name="value"/>.</returns>
		public static IEnumerable<T> GetFlags<T>(this T value) where T : struct, IConvertible
		{
			CheckIsEnum<T>(true);
			foreach (T flag in Enum.GetValues(typeof(T)))
			{
				if (value.IsFlagSet(flag))
					yield return flag;
			}
		}

		/// <summary>
		/// Combines enumerated list of values into a single enumerated value.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">The flags to combine.</param>
		/// <returns>A single enumerated value.</returns>
		public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct, IConvertible
		{
			CheckIsEnum<T>(true);
			long lValue = 0;
			foreach (var flag in flags)
			{
				var lFlag = Convert.ToInt64(flag);
				lValue |= lFlag;
			}
			return (T)Enum.ToObject(typeof(T), lValue);
		}

		/// <summary>
		/// Gets the description supplied by a <see cref="DescriptionAttribute"/> if one is set.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="value">The enumerated value.</param>
		/// <returns>The description, or <c>null</c> if one is not set.</returns>
		public static string GetDescription<T>(this T value) where T : struct, IConvertible
		{
			CheckIsEnum<T>();
			var name = Enum.GetName(typeof(T), value);
			if (name != null)
			{
				var field = typeof(T).GetField(name);
				if (field != null)
				{
					var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
					if (attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Returns an indication if the enumerated value is either defined or can be defined by a set of known flags.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="value">The enumerated value.</param>
		/// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
		public static bool IsValid<T>(this T value) where T : struct, IConvertible
		{
			CheckIsEnum<T>();
			if (IsFlags<T>())
			{
				long mask = 0, lValue = Convert.ToInt64(value);
				foreach (T flag in Enum.GetValues(typeof(T)))
					mask |= Convert.ToInt64(flag);
				return (mask & lValue) == lValue;
			}
			return Enum.IsDefined(typeof(T), value);
		}

		/// <summary>
		/// Returns an indication if the enumerated value is either defined or can be defined by a set of known flags.
		/// </summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="value">The enumerated value.</param>
		/// <param name="validValues">The valid values.</param>
		/// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
		private static bool IsValid<T>(this T value, params T[] validValues) where T : struct, IConvertible
		{
			CheckIsEnum<T>();
			foreach (var vval in validValues)
				if (value.Equals(vval))
					return true;
			return false;
		}

		/// <summary>
		/// Checks the valid property argument enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="argName">Name of the argument.</param>
		/// <exception cref="InvalidEnumArgumentException"></exception>
		private static void CheckValidPropArgEnum<T>(this T value, string argName = "value") where T : struct, IConvertible
		{
			if (!value.IsValid())
				throw new InvalidEnumArgumentException(argName, Convert.ToInt32(value), typeof(T));
		}
	}
}