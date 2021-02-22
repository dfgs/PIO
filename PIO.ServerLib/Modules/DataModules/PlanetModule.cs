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
using System;

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
			query=new Select(PlanetTable.PlanetID, PlanetTable.Name,PlanetTable.Width,PlanetTable.Height).From(PIODB.PlanetTable).Where(PlanetTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectFirst<PlanetTable,Planet>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Planet[] GetPlanets()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Planet table");
			query=new Select(PlanetTable.PlanetID, PlanetTable.Name, PlanetTable.Width, PlanetTable.Height).From(PIODB.PlanetTable);
			return TrySelectMany<PlanetTable,Planet>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Planet CreatePlanet(string Name,int Width,int Height)
		{
			IInsert query;
			Planet item;
			object result;

			LogEnter();

			item = new Planet() {Name=Name, Width=Width, Height= Height, };

			Log(LogLevels.Information, $"Inserting into Planet table (Name={Name}, Width={Width}, Height={Height})");
			query = new Insert().Into(PIODB.PlanetTable).Set(PlanetTable.Name, item.Name).Set(PlanetTable.Width, item.Width).Set(PlanetTable.Height, item.Height);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.PlanetID = Convert.ToInt32(result);

			return item;
		}

	}
}
