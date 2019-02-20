using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Stack
	{
		public static readonly Column<Stack, DbInt> StackID = new Column<Stack, DbInt>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Stack, DbInt> FactoryID = new Column<Stack, DbInt>();
		public static readonly Column<Stack, DbInt> ResourceID = new Column<Stack, DbInt>();
		public static readonly Column<Stack, DbInt> Quantity = new Column<Stack, DbInt>();
	}
}
