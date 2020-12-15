using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class BuildingTypeModule : DatabaseModule,IBuildingTypeModule
	{

		public BuildingTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildingType table (BuildingTypeID={BuildingTypeID})");
			query = new Select(BuildingTypeTable.BuildingTypeID, BuildingTypeTable.Name).From(PIODB.BuildingTypeTable).Where(BuildingTypeTable.BuildingTypeID.IsEqualTo(BuildingTypeID));
			return TrySelectFirst<BuildingTypeTable,BuildingType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public BuildingType[] GetBuildingTypes()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildingType table");
			query = new Select(BuildingTypeTable.BuildingTypeID, BuildingTypeTable.Name).From(PIODB.BuildingTypeTable);
			return TrySelectMany<BuildingTypeTable,BuildingType>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
