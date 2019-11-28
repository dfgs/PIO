using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogLib;
using NetORMLib;
using PIO.Models;
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

		protected override Row<Planet> OnGetPlanet(int PlanetID)
		{
			return server.GetPlanet(PlanetID);
		}
		protected override IEnumerable<Row<Planet>> OnGetPlanets()
		{
			return server.GetPlanets();
		}
		protected override IEnumerable<Row<Factory>> OnGetFactories(int PlanetID)
		{
			return server.GetFactories(PlanetID);
		}
		protected override Row<Factory> OnBuildFactory(int PlanetID, int FactoryTypeID)
		{
			return server.BuildFactory(PlanetID, FactoryTypeID);
		}
		protected override IEnumerable<Row<Stack>> OnGetStacks(int FactoryID)
		{
			return server.GetStacks(FactoryID);
		}
		protected override Row<Task> OnGetTask(int TaskID)
		{
			return server.GetTask(TaskID);
		}
		protected override Row<State> OnGetState(int StateID)
		{
			return server.GetState(StateID);
		}


	}
}
