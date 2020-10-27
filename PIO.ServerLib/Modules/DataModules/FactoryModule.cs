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
			query = new Select(FactoryTable.BuildingID, FactoryTable.FactoryID, FactoryTable.FactoryTypeID)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));

			return TrySelectFirst<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Factory GetFactory(int PlanetID, int X,int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (X={X}, Y={Y})");
			query = new Select(FactoryTable.BuildingID, FactoryTable.FactoryID, FactoryTable.FactoryTypeID)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.X.IsEqualTo(X).And(BuildingTable.Y.IsEqualTo(Y)).And(BuildingTable.PlanetID.IsEqualTo(PlanetID)));

			return TrySelectFirst<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Factory[] GetFactories(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (PlanetID={PlanetID})");
			query = new Select(FactoryTable.BuildingID, FactoryTable.FactoryID, FactoryTable.FactoryTypeID)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID, BuildingTable.BuildingID)))
				.Where(BuildingTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<FactoryTable, Factory>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Factory CreateFactory(int BuildingID, FactoryTypeIDs FactoryTypeID)
		{
			IInsert query;
			Factory item;
			object result;

			LogEnter();

			Log(LogLevels.Information, $"Inserting into Factory table (BuildingID={BuildingID}, FactoryTypeID={FactoryTypeID})");
			item = new Factory() { BuildingID = BuildingID,  FactoryTypeID = FactoryTypeID, };
			query = new Insert().Into(PIODB.FactoryTable).Set(FactoryTable.BuildingID, item.BuildingID).Set(FactoryTable.FactoryTypeID, item.FactoryTypeID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.FactoryID = Convert.ToInt32(result);
			return item;
		}



	}
}
