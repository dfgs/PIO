using LogLib;
using ModuleLib;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public class ServiceHostModule : ThreadModule, IWebServiceModule
	{
		private IPIOService service;

		public ServiceHostModule(ILogger Logger,IPIOService Service) : base(Logger)
		{
			this.service = Service;	
		}

		protected override void ThreadLoop()
		{
			ServiceHost host;


			Log(LogLib.LogLevels.Information, "Creating ServiceHost");
			if (!Try<ServiceHost>(() => new ServiceHost(service)).OrAlert(out host, "Failed to create ServiceHost")) return;

			Log(LogLib.LogLevels.Information, "Opening ServiceHost");
			if (!Try(() => host.Open()).OrAlert("Failed to open ServiceHost")) return;

			Log(LogLib.LogLevels.Information, "ServiceHost ready, waiting for notifications");
			WaitHandles(-1, QuitEvent);

			Log(LogLib.LogLevels.Information, "Closing ServiceHost");
			Try(() => host.Close()).OrWarn("Failed to close ServiceHost");
			
		}



	}
}
