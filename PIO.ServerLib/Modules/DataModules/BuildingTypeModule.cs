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
			query=new Select(BuildingTypeTable.BuildingTypeID,  BuildingTypeTable.PhraseKey,  BuildingTypeTable.HealthPoints,BuildingTypeTable.BuildSteps,BuildingTypeTable.IsFactory,BuildingTypeTable.IsFarm ).From(PIODB.BuildingTypeTable).Where(BuildingTypeTable.BuildingTypeID.IsEqualTo(BuildingTypeID));
			return TrySelectFirst<BuildingTypeTable,BuildingType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public BuildingType[] GetBuildingTypes()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildingType table");
			query=new Select(BuildingTypeTable.BuildingTypeID,  BuildingTypeTable.PhraseKey,  BuildingTypeTable.HealthPoints, BuildingTypeTable.BuildSteps, BuildingTypeTable.IsFactory, BuildingTypeTable.IsFarm).From(PIODB.BuildingTypeTable);
			return TrySelectMany<BuildingTypeTable,BuildingType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public BuildingType CreateBuildingType(BuildingTypeIDs BuildingTypeID, string PhraseKey, int BuildSteps,int HealthPoints, bool IsFactory, bool IsFarm)
		{
			IInsert query;
			BuildingType item;
			object result;

			LogEnter();

			item = new BuildingType() { BuildingTypeID = BuildingTypeID, PhraseKey = PhraseKey, BuildSteps=BuildSteps,HealthPoints=HealthPoints, IsFactory=IsFactory,IsFarm=IsFarm,};

			Log(LogLevels.Information, $"Inserting into BuildingType table (BuildingTypeID={BuildingTypeID}, PhraseKey={PhraseKey})");
			query = new Insert().Into(PIODB.BuildingTypeTable).Set(BuildingTypeTable.BuildingTypeID, item.BuildingTypeID)
				.Set(BuildingTypeTable.PhraseKey, item.PhraseKey).Set(BuildingTypeTable.BuildSteps,item.BuildSteps)
				.Set(BuildingTypeTable.HealthPoints,item.HealthPoints)
				.Set(BuildingTypeTable.IsFactory, item.IsFactory).Set(BuildingTypeTable.IsFarm, item.IsFarm)
				;
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			//item.BuildingTypeID = Convert.ToInt32(result);

			return item;
		}


	}
}
