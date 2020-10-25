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
using PIO.Models.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class BuildingModule : DatabaseModule,IBuildingModule
	{

		public BuildingModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}


		public Building GetBuilding(int BuildingID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Building table (BuildingID={BuildingID})");
			query = new Select(BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.BuildingTypeID, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps)
				.From(PIODB.BuildingTable)
				.Where(BuildingTable.BuildingID.IsEqualTo(BuildingID));

			return TrySelectFirst<BuildingTable, Building>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Building GetBuilding(int X,int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Building table (X={X}, Y={Y})");
			query = new Select(BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.BuildingTypeID, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps)
				.From(PIODB.BuildingTable)
				.Where(BuildingTable.X.IsEqualTo(X).And(BuildingTable.Y.IsEqualTo(Y)));

			return TrySelectFirst<BuildingTable, Building>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Building[] GetBuildings(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Building table (PlanetID={PlanetID})");
			query = new Select(BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.BuildingTypeID, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps)
				.From(PIODB.BuildingTable)
				.Where(BuildingTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<BuildingTable, Building>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Building CreateBuilding(int PlanetID, int X, int Y, BuildingTypeIDs BuildingTypeID, int RemainingBuildSteps)
		{
			IInsert query;
			Building item;
			object result;

			LogEnter();

			Log(LogLevels.Information, $"Inserting into Building table (PlanetID={PlanetID}, X={X}, Y={Y}, BuildingTypeID={BuildingTypeID}, RemainingBuildingSteps={RemainingBuildSteps})");
			item = new Building() { PlanetID = PlanetID, X = X, Y = Y, BuildingTypeID = BuildingTypeID, RemainingBuildSteps = RemainingBuildSteps, HealthPoints = 0 };
			query = new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, item.PlanetID).Set(BuildingTable.X,item.X).Set(BuildingTable.Y,item.Y).Set(BuildingTable.BuildingTypeID, item.BuildingTypeID).Set(BuildingTable.HealthPoints, item.HealthPoints).Set(BuildingTable.RemainingBuildSteps, item.RemainingBuildSteps);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.BuildingID = Convert.ToInt32(result);
			return item;
		}

		
		/*public void SetHealthPoints(int BuildingID,int HealthPoints)
		{
			IUpdate update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Building table (BuildingID={BuildingID}, HealthPoints={HealthPoints})");
			update = new Update(PIODB.BuildingTable).Set(BuildingTable.HealthPoints, HealthPoints).Where(BuildingTable.BuildingID.IsEqualTo(BuildingID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}*/


	}
}
