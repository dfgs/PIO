using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class FarmModule : DatabaseModule,IFarmModule
	{

		public FarmModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Farm GetFarm(int FarmID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Farm table (FarmID={FarmID})");
			query=new Select(
						BuildingTable.BuildingID,BuildingTable.PlanetID, BuildingTable.X,BuildingTable.Y,  BuildingTable.HealthPoints,BuildingTable.RemainingBuildSteps,
						FarmTable.FarmID, FarmTable.FarmTypeID
					)
				.From(PIODB.FarmTable.Join(PIODB.BuildingTable.On(FarmTable.BuildingID, BuildingTable.BuildingID)))
				.Where(FarmTable.FarmID.IsEqualTo(FarmID));

			return TrySelectFirst<FarmTable, Farm>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Farm GetFarm(int PlanetID, int X,int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Farm table (PlanetID={PlanetID}, X={X}, Y={Y})");
			query=new Select(
						BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps,
						FarmTable.FarmID, FarmTable.FarmTypeID
					)
				.From(PIODB.FarmTable.Join(PIODB.BuildingTable.On(FarmTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.X.IsEqualTo(X).And(BuildingTable.Y.IsEqualTo(Y)).And(BuildingTable.PlanetID.IsEqualTo(PlanetID)));

			return TrySelectFirst<FarmTable, Farm>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Farm[] GetFarms(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Farm table (PlanetID={PlanetID})");
			query=new Select(
						BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps,
						FarmTable.FarmID, FarmTable.FarmTypeID
					)
				.From(PIODB.FarmTable.Join(PIODB.BuildingTable.On(FarmTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<FarmTable, Farm>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Farm CreateFarm(int PlanetID, int X, int Y, int RemainingBuildSteps,FarmTypeIDs FarmTypeID)
		{
			IInsert queryBuilding,queryFarm;
			Farm item;
			object result;

			LogEnter();

			//new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.X, 0).Set(BuildingTable.Y, 0).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
			//new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
			//new Insert().Into(PIODB.FarmTable).Set(FarmTable.BuildingID, buildingID).Set(FarmTable.FarmTypeID, FarmTypeIDs.Forest);

			item = new Farm() { PlanetID = PlanetID, X = X, Y = Y, RemainingBuildSteps = RemainingBuildSteps, FarmTypeID = FarmTypeID, };

			Log(LogLevels.Information, $"Inserting into Building table (PlanetID={PlanetID}, X={X}, Y={Y}, RemainingBuildingSteps={RemainingBuildSteps})");
			queryBuilding=new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, item.PlanetID).Set(BuildingTable.X, item.X).Set(BuildingTable.Y, item.Y).Set(BuildingTable.HealthPoints, item.HealthPoints).Set(BuildingTable.RemainingBuildSteps, item.RemainingBuildSteps);
			result = Try(queryBuilding).OrThrow<PIODataException>("Failed to insert");
			item.BuildingID=Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into Farm table (BuildingID={item.BuildingID}, FarmTypeID={FarmTypeID})");
			queryFarm=new Insert().Into(PIODB.FarmTable).Set(FarmTable.BuildingID, item.BuildingID).Set(FarmTable.FarmTypeID, item.FarmTypeID);
			result = Try(queryFarm).OrThrow<PIODataException>("Failed to insert");
			item.FarmID=Convert.ToInt32(result);
			return item;
		}



	}
}
