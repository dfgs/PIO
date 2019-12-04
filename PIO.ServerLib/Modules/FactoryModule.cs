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

namespace PIO.ServerLib.Modules
{
	public class FactoryModule : DatabaseModule,IFactoryModule
	{

		public FactoryModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Factory GetFactory(int FactoryID)
		{
			ISelect<FactoryTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (FactoryID={FactoryID})");
			query = new Select<FactoryTable>(FactoryTable.FactoryID, FactoryTable.FactoryTypeID,FactoryTable.HealthPoints).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectFirst<FactoryTable,Factory>(query).OrThrow("Failed to query");
		}

		public Factory[] GetFactories(int PlanetID)
		{
			ISelect<FactoryTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (PlanetID={PlanetID})");
			query = new Select<FactoryTable>(FactoryTable.FactoryID, FactoryTable.FactoryTypeID, FactoryTable.HealthPoints).Where(FactoryTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<FactoryTable, Factory>(query).OrThrow("Failed to query");
		}

		public void Build(int FactoryID)
		{
			Factory factory;
			ISelect<FactoryTable> query;
			IUpdate<FactoryTable> update;

			LogEnter();

			Log(LogLevels.Information, $"Querying Factory table (FactoryID={FactoryID})");
			query = new Select<FactoryTable>(FactoryTable.FactoryID, FactoryTable.HealthPoints ).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			factory = TrySelectFirst<FactoryTable, Factory>(query).OrThrow("Failed to query");
			if ((factory == null) ) throw new InvalidOperationException($"Invalid factory");

			factory.HealthPoints++;
			Log(LogLevels.Information, $"Updating Factory table (FactoryID={factory.FactoryID}, HealthPoints={factory.HealthPoints})");
			update = new Update<FactoryTable>().Set(FactoryTable.HealthPoints, factory.HealthPoints).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			Try(update).OrThrow("Failed to update");
		}


	}
}
