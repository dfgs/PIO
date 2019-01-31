using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class State
	{
		public static readonly Column<State, int> StateID = new Column<State, int>() { IsPrimaryKey = true, IsIdentity = false };
		public static readonly Column<State, string> Name = new Column<State, string>() {DefaultValue="New State" };
		//public static readonly Column<State, int> StateStateID = new Column<State, int>();
	}
}
