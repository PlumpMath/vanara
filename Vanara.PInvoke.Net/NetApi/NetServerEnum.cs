using System;
using System.Collections.Generic;
using System.Linq;
using Vanara.Extensions;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class NetApi32
	{
		private static IEnumerable<SERVER_INFO_101> GetNetworkComputerInfo(NetServerEnumFilter netServerEnumFilter = NetServerEnumFilter.SV_TYPE_WORKSTATION | NetServerEnumFilter.SV_TYPE_SERVER, string domain = null) =>
			NetServerEnum<SERVER_INFO_101>(netServerEnumFilter, domain, 101);

		private static IEnumerable<string> GetNetworkComputerNames(NetServerEnumFilter netServerEnumFilter = NetServerEnumFilter.SV_TYPE_WORKSTATION | NetServerEnumFilter.SV_TYPE_SERVER, string domain = null) =>
			NetServerEnum<SERVER_INFO_100>(netServerEnumFilter, domain, 100).Select(si => si.sv100_name);

		/// <summary>
		/// The NetServerEnum function lists all servers of the specified type that are visible in a domain.
		/// </summary>
		/// <typeparam name="T">The type of the structure to have filled in for each server. This must be SERVER_INFO_100, SERVER_INFO_101, or SERVER_INFO_102.</typeparam>
		/// <param name="netServerEnumFilter">A value that filters the server entries to return from the enumeration.</param>
		/// <param name="domain">A string that specifies the name of the domain for which a list of servers is to be returned. The domain name must be a NetBIOS domain name (for example, Microsoft). The NetServerEnum function does not support DNS-style names (for example, microsoft.com). If this parameter is NULL, the primary domain is implied.</param>
		/// <param name="level">The information level of the data requested. If this value is 0, then the method will extract all digits to form the level (e.g. SERVER_INFO_101 produces 101).</param>
		/// <returns>A managed array of the requested type.</returns>
		public static IEnumerable<T> NetServerEnum<T>(NetServerEnumFilter netServerEnumFilter = NetServerEnumFilter.SV_TYPE_WORKSTATION | NetServerEnumFilter.SV_TYPE_SERVER, string domain = null, int level = 0) where T : struct, INetServerInfo
		{
			if (level == 0) level = GetLevelFromStructure<T>();

			SafeNetApiBuffer bufptr;
			int entriesRead, totalEntries;
			var resumeHandle = IntPtr.Zero;
			var ret = NetServerEnum(null, level, out bufptr, MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries, netServerEnumFilter, domain, resumeHandle);
			if (ret != 0)
				throw new System.ComponentModel.Win32Exception(ret);
			return ((IntPtr)bufptr).ToIEnum<T>(entriesRead);
		}

		/// <summary>
		/// The NetServerGetInfo function retrieves current configuration information for the specified server.
		/// </summary>
		/// <typeparam name="T">The type of the structure to have filled in for each server. This must be SERVER_INFO_100, SERVER_INFO_101, or SERVER_INFO_102.</typeparam>
		/// <param name="serverName">A string that specifies the name of the remote server on which the function is to execute. If this parameter is NULL, the local computer is used.</param>
		/// <param name="level">The information level of the data requested. If this value is 0, then the method will extract all digits to form the level (e.g. SERVER_INFO_101 produces 101).</param>
		/// <returns>The requested type with returned information about the server.</returns>
		public static T NetServerGetInfo<T>(string serverName, int level = 0) where T : struct, INetServerInfo
		{
			if (level == 0) level = GetLevelFromStructure<T>();

			SafeNetApiBuffer ptr;
			var ret = NetServerGetInfo(serverName, level, out ptr);
			if (ret != 0)
				throw new System.ComponentModel.Win32Exception(ret);
			return ((IntPtr)ptr).ToStructure<T>();
		}

		private static int GetLevelFromStructure<T>()
		{
			int i;
			int.TryParse(System.Text.RegularExpressions.Regex.Replace(typeof(T).Name, @"[^\d]", ""), out i);
			return i;
		}

		public interface INetServerInfo { }
	}
}