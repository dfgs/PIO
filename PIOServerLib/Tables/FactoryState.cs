using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class FactoryState
	{
		public static readonly Column<FactoryState, int> FactoryStateID = new Column<FactoryState, int>() { IsPrimaryKey = true};
		public static readonly Column<FactoryState, string> Name = new Column<FactoryState, string>() ;
	}
}
