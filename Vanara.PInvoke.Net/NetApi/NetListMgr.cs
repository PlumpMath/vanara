using System;
using System.Collections;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Vanara.PInvoke
{
	public static partial class NetListMgr
	{
		[Flags]
		public enum NLM_CONNECTION_COST
		{
			NLM_CONNECTION_COST_UNKNOWN = 0,
			NLM_CONNECTION_COST_UNRESTRICTED = 1,
			NLM_CONNECTION_COST_FIXED = 2,
			NLM_CONNECTION_COST_VARIABLE = 4,
			NLM_CONNECTION_COST_OVERDATALIMIT = 65536,
			NLM_CONNECTION_COST_CONGESTED = 131072,
			NLM_CONNECTION_COST_ROAMING = 262144,
			NLM_CONNECTION_COST_APPROACHINGDATALIMIT = 524288,
		}

		[Flags]
		public enum NLM_CONNECTIVITY
		{
			NLM_CONNECTIVITY_DISCONNECTED = 0,
			NLM_CONNECTIVITY_IPV4_NOTRAFFIC = 1,
			NLM_CONNECTIVITY_IPV6_NOTRAFFIC = 2,
			NLM_CONNECTIVITY_IPV4_SUBNET = 16,
			NLM_CONNECTIVITY_IPV4_LOCALNETWORK = 32,
			NLM_CONNECTIVITY_IPV4_INTERNET = 64,
			NLM_CONNECTIVITY_IPV6_SUBNET = 256,
			NLM_CONNECTIVITY_IPV6_LOCALNETWORK = 512,
			NLM_CONNECTIVITY_IPV6_INTERNET = 1024,
		}

		public enum NLM_DOMAIN_TYPE
		{
			NLM_DOMAIN_TYPE_NON_DOMAIN_NETWORK,
			NLM_DOMAIN_TYPE_DOMAIN_NETWORK,
			NLM_DOMAIN_TYPE_DOMAIN_AUTHENTICATED,
		}

		public enum NLM_ENUM_NETWORK
		{
			NLM_ENUM_NETWORK_CONNECTED = 0x01,
			NLM_ENUM_NETWORK_DISCONNECTED = 0x02,
			NLM_ENUM_NETWORK_ALL = 0x03
		}

		[ComImport, Guid("DCB00006-570F-4A9B-8D69-199FDBA5723B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IEnumNetworkConnections : IEnumerator { }

		[ComImport, Guid("DCB00003-570F-4A9B-8D69-199FDBA5723B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface IEnumNetworks : IEnumerable { }

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("DCB00002-570F-4A9B-8D69-199FDBA5723B")]
		public interface INetwork
		{
			string GetName();

			void SetName(string szNetworkNewName);

			string GetDescription();

			void SetDescription(string szDescription);

			Guid GetNetworkId();
		}

		[ComImport, Guid("DCB00005-570F-4A9B-8D69-199FDBA5723B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface INetworkConnection
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			INetwork GetNetwork();

			bool IsConnectedToInternet { get; }

			bool IsConnected { get; }

			NLM_CONNECTIVITY GetConnectivity();

			Guid GetConnectionId();

			Guid GetAdapterId();

			NLM_DOMAIN_TYPE GetDomainType();
		}

		[ComImport, Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
		public interface INetworkListManager
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			IEnumNetworks GetNetworks([In] NLM_ENUM_NETWORK Flags);

			[return: MarshalAs(UnmanagedType.Interface)]
			INetwork GetNetwork([In] Guid gdNetworkId);

			[return: MarshalAs(UnmanagedType.Interface)]
			IEnumNetworkConnections GetNetworkConnections();

			[return: MarshalAs(UnmanagedType.Interface)]
			INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);

			bool IsConnectedToInternet { get; }

			bool IsConnected { get; }

			NLM_CONNECTIVITY GetConnectivity();

			void SetSimulatedProfileInfo([In] ref NLM_SIMULATED_PROFILE_INFO pSimulatedInfo);

			void ClearSimulatedProfileInfo();
		}

		[ComImport, CoClass(typeof(NetworkListManager)), Guid("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
		public interface NetworkListManager : INetworkListManager { }

		[ComImport, Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B"), TypeLibType(TypeLibTypeFlags.FCanCreate), ClassInterface(ClassInterfaceType.None), System.Security.SuppressUnmanagedCodeSecurity]
		public class NetworkListManagerClass : NetworkListManager
		{
			public virtual extern IEnumNetworks GetNetworks([In] NLM_ENUM_NETWORK Flags);
			public virtual extern INetwork GetNetwork([In] Guid gdNetworkId);
			public virtual extern IEnumNetworkConnections GetNetworkConnections();
			public virtual extern INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);
			public virtual extern bool IsConnectedToInternet { get; }
			public virtual extern bool IsConnected { get; }
			public virtual extern NLM_CONNECTIVITY GetConnectivity();
			public virtual extern void SetSimulatedProfileInfo([In] ref NLM_SIMULATED_PROFILE_INFO pSimulatedInfo);
			public virtual extern void ClearSimulatedProfileInfo();
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct NLM_SIMULATED_PROFILE_INFO
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = 0)]
			public ushort[] ProfileName;

			public NLM_CONNECTION_COST cost;
			public uint UsageInMegabytes;
			public uint DataLimitInMegabytes;
		}
	}
}