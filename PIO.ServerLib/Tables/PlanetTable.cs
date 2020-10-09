using NetORMLib.Columns;
using NetORMLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Tables
{
	public class PlanetTable : Table
	{
		public static readonly Column<int> PlanetID = new Column<int>() { IsPrimaryKey = true, IsIdentity = true };
		public new static readonly Column<string> Name = new Column<string>() {DefaultValue="New planet" };
	}
}
