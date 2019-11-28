using LogLib;
using PIO.ServerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerHost
{
	class Program
	{
		static void Main(string[] args)
		{
			ILogger logger;
			VersionControlModule versionControlModule;
			WebServiceModule webServiceModule;

			logger = new ConsoleLogger(new DefaultLogFormatter());

			versionControlModule = new VersionControlModule(logger,Properties.Settings.Default.Server);
			if (!versionControlModule.InitializeDatabase(false))
			{
				Console.ReadLine();
				return;
			}

			webServiceModule = new WebServiceModule(logger);
			
   
			webServiceModule.Start();
			Console.ReadLine();
			webServiceModule.Stop();
			
		}


	}
}
