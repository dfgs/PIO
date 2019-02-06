using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class FactoryType
	{
		public static readonly Column<FactoryType, int> FactoryTypeID = new Column<FactoryType, int>() { IsPrimaryKey = true};
		public static readonly Column<FactoryType, string> Name = new Column<FactoryType, string>() ;
	}
}
