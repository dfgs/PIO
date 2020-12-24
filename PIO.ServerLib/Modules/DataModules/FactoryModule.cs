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
	public class FactoryModule : DatabaseModule,IFactoryModule
	{

		public FactoryModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Factory GetFactory(int FactoryID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (FactoryID={FactoryID})");
			query=new Select(
						BuildingTable.BuildingID,BuildingTable.PlanetID, BuildingTable.X,BuildingTable.Y,  BuildingTable.HealthPoints,BuildingTable.RemainingBuildSteps,
						FactoryTable.FactoryID, FactoryTable.FactoryTypeID
					)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));

			return TrySelectFirst<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Factory GetFactory(int PlanetID, int X,int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (X={X}, Y={Y})");
			query=new Select(
						BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps,
						FactoryTable.FactoryID, FactoryTable.FactoryTypeID
					)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.X.IsEqualTo(X).And(BuildingTable.Y.IsEqualTo(Y)).And(BuildingTable.PlanetID.IsEqualTo(PlanetID)));

			return TrySelectFirst<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Factory[] GetFactories(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (PlanetID={PlanetID})");
			query=new Select(
						BuildingTable.BuildingID, BuildingTable.PlanetID, BuildingTable.X, BuildingTable.Y, BuildingTable.HealthPoints, BuildingTable.RemainingBuildSteps,
						FactoryTable.FactoryID, FactoryTable.FactoryTypeID
					)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Factory CreateFactory(int PlanetID, int X, int Y, int RemainingBuildSteps,FactoryTypeIDs FactoryTypeID)
		{
			IInsert queryBuilding,queryFactory;
			Factory item;
			object result;

			LogEnter();

			//new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, planetID).Set(BuildingTable.X, 0).Set(BuildingTable.Y, 0).Set(BuildingTable.HealthPoints, 10).Set(BuildingTable.RemainingBuildSteps, 0);
			//new SelectIdentity((result) => buildingID = Convert.ToInt32(result));
			//new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, buildingID).Set(FactoryTable.FactoryTypeID, FactoryTypeIDs.Forest);

			item = new Factory() { PlanetID = PlanetID, X = X, Y = Y, RemainingBuildSteps = RemainingBuildSteps, FactoryTypeID = FactoryTypeID, };

			Log(LogLevels.Information, $"Inserting into Building table (PlanetID={PlanetID}, X={X}, Y={Y}, RemainingBuildingSteps={RemainingBuildSteps})");
			queryBuilding=new Insert().Into(PIODB.BuildingTable).Set(BuildingTable.PlanetID, item.PlanetID).Set(BuildingTable.X, item.X).Set(BuildingTable.Y, item.Y).Set(BuildingTable.HealthPoints, item.HealthPoints).Set(BuildingTable.RemainingBuildSteps, item.RemainingBuildSteps);
			result = Try(queryBuilding).OrThrow<PIODataException>("Failed to insert");
			item.BuildingID=Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into Factory table (BuildingID={item.BuildingID}, FactoryTypeID={FactoryTypeID})");
			queryFactory=new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, item.BuildingID).Set(FactoryTable.FactoryTypeID, item.FactoryTypeID);
			result = Try(queryFactory).OrThrow<PIODataException>("Failed to insert");
			item.FactoryID=Convert.ToInt32(result);
			return item;
		}



	}
}
