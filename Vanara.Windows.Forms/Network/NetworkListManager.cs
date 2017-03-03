using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.NetListMgr;

namespace Vanara.Network
{
	public static class NetworkListManager
	{
		private static INetworkListManager manager;

		private static INetworkListManager Manager
		{
			get
			{
				if (manager != null) return manager;
				try
				{
					manager = new NetworkListManagerClass();
				}
				catch (UnauthorizedAccessException) { }
				catch (ExternalException) { }
				return manager;
			}
		}

		public static IEnumerable<NetworkProfile> GetNetworkList()
		{
			try
			{
				return GetNetworkEnumerator().Cast<INetwork>().Select(n => new NetworkProfile(n.GetNetworkId(), n.GetName()));
			}
			catch { }
			return new NetworkProfile[0];
		}

		private static IEnumNetworks GetNetworkEnumerator()
		{
			try
			{
				if (Manager != null)
					return Manager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_ALL);
			}
			catch (UnauthorizedAccessException) { }
			catch (ExternalException) { }
			return null;
		}
	}
}