using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Transition
	{
		public static readonly Column<Transition, int> TransitionID = new Column<Transition, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Transition, int> StateID = new Column<Transition, int>();
		public static readonly Column<Transition, int> NextStateID = new Column<Transition, int>();
		public static readonly Column<Transition, int> EventID = new Column<Transition, int>();
	}
}
