using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
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

		public Row GetFactory(int FactoryID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.PlanetID.IsEqualTo(PlanetID));
			return Try(query).OrThrow("Failed to query");
		}

		public int CreateFactory(int PlanetID,int FactoryTypeID)
		{
			IQuery[] queries;
			int result=-1;
			LogEnter();

			queries = new IQuery[] { new Insert<Factory>().Set(Factory.PlanetID, PlanetID).Set(Factory.Name, "New"), new SelectIdentity<Factory>((key) => result = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return result;
		}





	}
}
