using System.ComponentModel;
using System.Runtime.InteropServices;
using static Vanara.Interop.AdvApi32;

namespace Vanara.Extensions
{
	public static partial class ControlExtension
	{
		public static void SetStartType(this ServiceController svc, ServiceStartMode mode)
		{
			using (var serviceHandle = svc.ServiceHandle)
			{
				uint tag;
				if (!ChangeServiceConfig(serviceHandle.DangerousGetHandle(), ServiceTypes.SERVICE_NO_CHANGE, (ServiceStartType)mode, ServiceErrorControlType.SERVICE_NO_CHANGE, null, null, out tag, null, null, null, null))
					throw new ExternalException("Could not change service start type.", new Win32Exception());
			}
		}
	}
}
