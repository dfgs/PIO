using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class TransitionTable
	{
		public static readonly Column<TransitionTable, int> TransitionID = new Column<TransitionTable, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<TransitionTable, int> StateID = new Column<TransitionTable, int>();
		public static readonly Column<TransitionTable, int> NextStateID = new Column<TransitionTable, int>();
		public static readonly Column<TransitionTable, int> EventID = new Column<TransitionTable, int>();
	}
}
