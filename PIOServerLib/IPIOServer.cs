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
using PIOServerLib.Rows;
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

		FactoryRow BuildFactory(int PlanetID, int FactoryTypeID);
		PlanetRow GetPlanet(int PlanetID);
		IEnumerable<PlanetRow> GetPlanets();
		IEnumerable<FactoryRow> GetFactories(int PlanetID);
		IEnumerable<StackRow> GetStacks(int FactoryID);
		TaskRow GetTask(int TaskID);
		StateRow GetState(int StateID);


	}
}
