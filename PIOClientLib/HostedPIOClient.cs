using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using NetORMLib;
using PIOServerLib;
using PIOServerLib.Modules;

namespace PIOClientLib
{
	public class HostedPIOClient : PIOClient
	{
		private IPIOServer server;

		public HostedPIOClient(ILogger Logger, IPIOServer Server):base(Logger)
		{
			this.server = Server;
		}

		protected override void OnConnect()
		{
			if (!server.IsInitialized) throw (new Exception("Cannot connect to server"));
		}

		protected override void OnDisconnect()
		{
			
		}

		protected override IEnumerable<Row> OnGetFactories(int PlanetID)
		{
			throw new NotImplementedException();
		}
		protected override Row OnGetPlanet(int PlanetID)
		{
			return server.PlanetModule.GetPlanet(PlanetID);
		}
		protected override IEnumerable<Row> OnGetPlanets()
		{
			return server.PlanetModule.GetPlanets();
		}



	}
}
