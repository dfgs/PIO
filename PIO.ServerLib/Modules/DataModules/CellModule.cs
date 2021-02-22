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
using PIO.Models.Modules;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class CellModule : DatabaseModule,ICellModule
	{

		public CellModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}


		public Cell GetCell(int CellID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Cell table (CellID={CellID})");
			query=new Select(CellTable.CellID, CellTable.PlanetID, CellTable.X, CellTable.Y)
				.From(PIODB.CellTable)
				.Where(CellTable.CellID.IsEqualTo(CellID));

			return TrySelectFirst<CellTable, Cell>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Cell GetCell(int PlanetID, int X,int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Cell table (PlanetID={PlanetID}, X={X}, Y={Y})");
			query=new Select(CellTable.CellID, CellTable.PlanetID, CellTable.X, CellTable.Y)
				.From(PIODB.CellTable)
				.Where( CellTable.X.IsEqualTo(X).And(CellTable.Y.IsEqualTo(Y).And(CellTable.PlanetID.IsEqualTo(PlanetID))));

			return TrySelectFirst<CellTable, Cell>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Cell[] GetCells(int PlanetID, int X, int Y, int Width, int Height)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Cell table (PlanetID={PlanetID}, X={X}, Y={Y}, Width={Width}, Height={Height})");
			query=new Select(CellTable.CellID, CellTable.PlanetID, CellTable.X, CellTable.Y)
				.From(PIODB.CellTable)
				.Where(CellTable.X.IsGreaterOrEqualTo(X).And(CellTable.Y.IsGreaterOrEqualTo(Y)
				.And(CellTable.X.IsLowerThan(X+Width).And(CellTable.Y.IsLowerThan(Y+Height)
				.And(CellTable.PlanetID.IsEqualTo(PlanetID))))));
			return TrySelectMany<CellTable, Cell>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Cell CreateCell(int PlanetID, int X, int Y)
		{
			IInsert query;
			Cell item;
			object result;

			LogEnter();

			item = new Cell() { PlanetID=PlanetID, X= X, Y= Y, };

			Log(LogLevels.Information, $"Inserting into Cell table (Name={PlanetID}, Width={X}, Height={Y})");
			query = new Insert().Into(PIODB.CellTable).Set(CellTable.PlanetID, item.PlanetID).Set(CellTable.X, item.X).Set(CellTable.Y, item.Y);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.CellID = Convert.ToInt32(result);

			return item;
		}




	}
}
