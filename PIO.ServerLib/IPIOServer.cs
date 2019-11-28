using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIO.Models;
using PIO.ServerLib.Modules;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PIO.ServerLib
{
	public interface IPIOServer
	{
		bool IsInitialized
		{
			get;
		}

		Row<Factory> BuildFactory(int PlanetID, int FactoryTypeID);
		Row<Planet> GetPlanet(int PlanetID);
		IEnumerable<Row<Planet>> GetPlanets();
		IEnumerable<Row<Factory>> GetFactories(int PlanetID);
		IEnumerable<Row<Stack>> GetStacks(int FactoryID);
		Row<Task> GetTask(int TaskID);
		Row<State> GetState(int StateID);


	}
}
