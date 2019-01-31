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
using PIOServerLib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public interface IPIOServer
	{
		bool IsInitialized
		{
			get;
		}

		Row BuildFactory(int PlanetID, int FactoryTypeID);
		Row GetPlanet(int PlanetID);
		IEnumerable<Row> GetPlanets();
		IEnumerable<Row> GetFactories(int PlanetID);
		IEnumerable<Row> GetStacks(int FactoryID);
		Row GetTask(int TaskID);
		Row GetState(int StateID);


	}
}
