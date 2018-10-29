using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Stack
	{
		public static readonly Column<Stack, int> StackID = new Column<Stack, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Stack, int> FactoryID = new Column<Stack, int>();
		public static readonly Column<Stack, int> ResourceID = new Column<Stack, int>();
		public static readonly Column<Stack, int> Quantity = new Column<Stack, int>() {DefaultValue=0 } ;
	}
}
