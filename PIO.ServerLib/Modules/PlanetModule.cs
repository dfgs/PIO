using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.WebServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class PlanetModule : DatabaseModule,IPlanetModule
	{

		public PlanetModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Planet GetPlanet(int PlanetID)
		{
			ISelect<PlanetTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying planet with ID {PlanetID}");
			query = new Select<PlanetTable>(PlanetTable.PlanetID, PlanetTable.Name).Where(PlanetTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectFirst<PlanetTable,Planet>(query).OrThrow("Failed to query");
		}

		public IEnumerable<Planet> GetPlanets()
		{
			ISelect<PlanetTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying planets");
			query = new Select<PlanetTable>(PlanetTable.PlanetID, PlanetTable.Name);
			return TrySelectMany<PlanetTable,Planet>(query).OrThrow("Failed to query");
		}

	}
}
