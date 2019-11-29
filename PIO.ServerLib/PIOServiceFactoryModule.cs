using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using PIO.ServerLib.Modules;
using PIO.WebServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public class PIOServiceFactoryModule : Module, IPIOServiceFactoryModule
	{
		private IDatabase database;

		public PIOServiceFactoryModule(ILogger Logger,IDatabase Database) : base(Logger)
		{
			this.database = Database;
		}
		public IPIOService CreateService()
		{
			PIOService service;
			
			Log(LogLib.LogLevels.Information, "Creating Service");
			Try<PIOService>(() =>
				new PIOService(new PlanetModule(Logger, database), new FactoryModule(Logger, database), new StackModule(Logger, database))
			).OrAlert(out service, "Failed to create Service");
			
			return service;
		}
	}
}
