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
	public class PlanetModule : Module,IPlanetModule
	{
		private IDatabase database;

		public PlanetModule(ILogger Logger,IDatabase Database) : base("PlanetModule", Logger)
		{
			this.database = Database;
		}

		public IEnumerable<Row> GetPlanets()
		{
			return this.database.Execute(new Select<Planet>(Planet.PlanetID, Planet.Name));
		}

	}
}
