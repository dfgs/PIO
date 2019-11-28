using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class StateTable
	{
		public static readonly Column<StateTable, int> StateID = new Column<StateTable, int>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<StateTable, string> Name = new Column<StateTable, string>() {DefaultValue="New State" };
		public static readonly Column<StateTable, int> TaskID = new Column<StateTable, int>();
		public static readonly Column<StateTable, int> Duration = new Column<StateTable, int>();
	}
}
