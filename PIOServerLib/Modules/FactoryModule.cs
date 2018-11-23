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

			query = new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			ISelect query;

			query=new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.PlanetID.IsEqualTo(PlanetID));
			return Try(query).OrThrow("Failed to query");
		}

		public Row BuildFactory(int PlanetID,int FactoryTypeID)
		{
			IQuery[] queries;
			dynamic item;

			item = new Row(Table<Factory>.Columns);
			item.PlanetID = PlanetID;
			item.Name = "New";
			item.FactoryStateID = 0;

			queries=new IQuery[] { new Insert<Factory>().Set(Factory.PlanetID, PlanetID).Set(Factory.Name, "New").Set(Factory.FactoryStateID, 0), new SelectIdentity<Factory>((key) => item.FactoryID = Convert.ToInt32(key)) };
			Try(queries).OrThrow("Failed to query");

			return item;
		}

		



	}
}
