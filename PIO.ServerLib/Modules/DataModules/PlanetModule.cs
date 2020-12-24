using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ServerLib.Tables;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;


namespace PIO.ServerLib.Modules
{
	public class PlanetModule : DatabaseModule,IPlanetModule
	{

		public PlanetModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Planet GetPlanet(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Planet table (PlanetID={PlanetID})");
			query=new Select(PlanetTable.PlanetID, PlanetTable.Name).From(PIODB.PlanetTable).Where(PlanetTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectFirst<PlanetTable,Planet>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Planet[] GetPlanets()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Planet table");
			query=new Select(PlanetTable.PlanetID, PlanetTable.Name).From(PIODB.PlanetTable);
			return TrySelectMany<PlanetTable,Planet>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
