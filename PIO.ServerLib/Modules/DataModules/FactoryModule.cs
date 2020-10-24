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
			query = new Select(FactoryTable.BuildingID,FactoryTable.FactoryID, FactoryTable.FactoryTypeID)
				.From(PIODB.FactoryTable.Join(PIODB.BuildingTable.On(FactoryTable.BuildingID,BuildingTable.BuildingID)) )
				.Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));

			return TrySelectFirst<FactoryTable,Factory>(query).OrThrow<PIODataException>("Failed to query");
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

		/*public void SetHealthPoints(int FactoryID,int HealthPoints)
		{
			IUpdate update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Factory table (FactoryID={FactoryID}, HealthPoints={HealthPoints})");
			update = new Update(PIODB.FactoryTable).Set(FactoryTable.HealthPoints, HealthPoints).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}*/


	}
}
