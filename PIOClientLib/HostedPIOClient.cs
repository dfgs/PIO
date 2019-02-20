using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using NetORMLib;
using PIOServerLib;
using PIOServerLib.Modules;
using PIOServerLib.Rows;

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

		protected override PlanetRow OnGetPlanet(int PlanetID)
		{
			return server.GetPlanet(PlanetID);
		}
		protected override IEnumerable<PlanetRow> OnGetPlanets()
		{
			return server.GetPlanets();
		}
		protected override IEnumerable<FactoryRow> OnGetFactories(int PlanetID)
		{
			return server.GetFactories(PlanetID);
		}
		protected override FactoryRow OnBuildFactory(int PlanetID, int FactoryTypeID)
		{
			return server.BuildFactory(PlanetID, FactoryTypeID);
		}
		protected override IEnumerable<StackRow> OnGetStacks(int FactoryID)
		{
			return server.GetStacks(FactoryID);
		}
		protected override TaskRow OnGetTask(int TaskID)
		{
			return server.GetTask(TaskID);
		}
		protected override StateRow OnGetState(int StateID)
		{
			return server.GetState(StateID);
		}


	}
}
