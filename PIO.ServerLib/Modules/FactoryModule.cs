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
using PIO.WebServerLib.Modules;

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

			Log(LogLevels.Information, $"Querying factory with FactoryID {FactoryID}");
			query = new Select<FactoryTable>(FactoryTable.FactoryID, FactoryTable.FactoryTypeID,FactoryTable.HealthPoints).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectFirst<FactoryTable,Factory>(query).OrThrow("Failed to query");
		}

		public IEnumerable<Factory> GetFactories(int PlanetID)
		{
			ISelect<FactoryTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying factories with PlanetID {PlanetID}");
			query = new Select<FactoryTable>(FactoryTable.FactoryID, FactoryTable.FactoryTypeID, FactoryTable.HealthPoints).Where(FactoryTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<FactoryTable, Factory>(query).OrThrow("Failed to query");
		}

		/*public int CreateFactory(int PlanetID,int FactoryTypeID, int StateID)
		{
			IQuery[] queries;
			int result=-1;
			LogEnter();

			//Log(LogLevels.Information, $"Creating factory");
			queries = new IQuery[] { new Insert<FactoryTable>().Set(FactoryTable.PlanetID, PlanetID).Set(FactoryTable.Name, "New").Set(FactoryTable.StateID,StateID), new SelectIdentity<Factory>((key) => result = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return result;
		}

		public void SetState(int FactoryID, int StateID)
		{
			IUpdate query;
			LogEnter();

			query = new Update<FactoryTable>().Set(FactoryTable.StateID,StateID).Where(FactoryTable.FactoryID.IsEqualTo(FactoryID));
			Try(query).OrThrow("Failed to query");
		}*/


	}
}
