using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class FactoryModule : DatabaseModule,IFactoryModule
	{

		public FactoryModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public FactoryRow GetFactory(int FactoryID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Factory>(Factory.FactoryID, Factory.Name,Factory.StateID,Factory.FactoryTypeID).Where(Factory.FactoryID.IsEqualTo(FactoryID));
			return Try<FactoryRow>(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<FactoryRow> GetFactories(int PlanetID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Factory>(Factory.FactoryID, Factory.Name, Factory.StateID, Factory.FactoryTypeID).Where(Factory.PlanetID.IsEqualTo(PlanetID));
			return Try<FactoryRow>(query).OrThrow("Failed to query");
		}

		public int CreateFactory(int PlanetID,int FactoryTypeID, int StateID)
		{
			IQuery[] queries;
			int result=-1;
			LogEnter();

			queries = new IQuery[] { new Insert<Factory>().Set(Factory.PlanetID, PlanetID).Set(Factory.Name, "New").Set(Factory.FactoryTypeID,FactoryTypeID).Set(Factory.StateID,StateID), new SelectIdentity<Factory>((key) => result = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return result;
		}

		public void SetState(int FactoryID, int StateID)
		{
			IUpdate query;
			LogEnter();

			query = new Update<Factory>().Set(Factory.StateID,StateID).Where(Factory.FactoryID.IsEqualTo(FactoryID));
			Try(query).OrThrow("Failed to query");
		}


	}
}
