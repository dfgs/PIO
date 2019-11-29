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

		Factory BuildFactory(int PlanetID, int FactoryTypeID);
		Planet GetPlanet(int PlanetID);
		IEnumerable<Planet> GetPlanets();
		IEnumerable<Factory> GetFactories(int PlanetID);
		IEnumerable<Stack> GetStacks(int FactoryID);
		TaskType GetTask(int TaskID);
		State GetState(int StateID);


	}
}
