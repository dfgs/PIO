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
	public class FactoryModule : Module,IFactoryModule
	{
		private IDatabase database;

		public FactoryModule(ILogger Logger,IDatabase Database) : base(Logger)
		{
			this.database = Database;
		}

		public Row GetFactory(int FactoryID)
		{
			return this.database.Execute(new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.FactoryID.IsEqualTo(FactoryID))).FirstOrDefault();
		}

		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			return this.database.Execute(new Select<Factory>(Factory.FactoryID, Factory.Name).Where(Factory.PlanetID.IsEqualTo(PlanetID)) );
		}

		public Row BuildFactory(int PlanetID,int FactoryTypeID)
		{
			dynamic item;

			item = new Row(Table<Factory>.Columns);
			item.PlanetID = PlanetID;
			item.Name = "New";
			item.FactoryStateID = 0;

			this.database.Execute(new Insert<Factory>().Set(Factory.PlanetID,PlanetID).Set(Factory.Name,"New").Set(Factory.FactoryStateID,0), new SelectIdentity<Factory>((key) => item.FactoryID = Convert.ToInt32(key)));
			
			return item;
		}


	}
}
