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
	public class FarmTypeModule : DatabaseModule,IFarmTypeModule
	{

		public FarmTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public FarmType GetFarmType(FarmTypeIDs FarmTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying FarmType table (FarmTypeID={FarmTypeID})");
			query=new Select(FarmTypeTable.FarmTypeID, FarmTypeTable.Name,FarmTypeTable.HealthPoints,FarmTypeTable.BuildSteps).From(PIODB.FarmTypeTable).Where(FarmTypeTable.FarmTypeID.IsEqualTo(FarmTypeID));
			return TrySelectFirst<FarmTypeTable,FarmType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public FarmType[] GetFarmTypes()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying FarmType table");
			query=new Select(FarmTypeTable.FarmTypeID, FarmTypeTable.Name, FarmTypeTable.HealthPoints, FarmTypeTable.BuildSteps).From(PIODB.FarmTypeTable);
			return TrySelectMany<FarmTypeTable,FarmType>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
