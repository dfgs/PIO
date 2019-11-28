using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models
{
	public class State
	{
		public static readonly Column<State, DbInt> StateID = new Column<State, DbInt>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<State, DbString> Name = new Column<State, DbString>() {DefaultValue="New State" };
		public static readonly Column<State, DbInt> TaskID = new Column<State, DbInt>();
		public static readonly Column<State, DbInt> Duration = new Column<State, DbInt>();
	}
}
