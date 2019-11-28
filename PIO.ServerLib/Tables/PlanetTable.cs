using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Tables
{
	public class PlanetTable
	{
		public static readonly Column<PlanetTable, int> PlanetID = new Column<PlanetTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<PlanetTable, string> Name = new Column<PlanetTable, string>() {DefaultValue="New planet" };
	}
}
