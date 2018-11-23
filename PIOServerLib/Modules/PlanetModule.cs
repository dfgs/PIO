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
	public class PlanetModule : DatabaseModule,IPlanetModule
	{

		public PlanetModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row GetPlanet(int PlanetID)
		{
			ISelect query;

			query = new Select<Planet>(Planet.PlanetID, Planet.Name).Where(Planet.PlanetID.IsEqualTo(PlanetID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<Row> GetPlanets()
		{
			ISelect query;

			query = new Select<Planet>(Planet.PlanetID, Planet.Name);
			return Try(query).OrThrow("Failed to query");
		}

	}
}
