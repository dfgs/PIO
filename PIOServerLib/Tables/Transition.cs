using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Transition
	{
		public static readonly Column<Transition, DbInt> TransitionID = new Column<Transition, DbInt>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Transition, DbInt> StateID = new Column<Transition, DbInt>();
		public static readonly Column<Transition, DbInt> NextStateID = new Column<Transition, DbInt>();
		public static readonly Column<Transition, DbInt> EventID = new Column<Transition, DbInt>();
	}
}
