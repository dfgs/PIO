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
		public static readonly Column<int> PlanetID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public new static readonly Column<string> Name = new Column<string>() { DefaultValue = "New planet" };
		public static readonly Column<int> Width = new Column<int>() { DefaultValue = 50 };
		public static readonly Column<int> Height = new Column<int>() { DefaultValue = 50 };
	}
}
