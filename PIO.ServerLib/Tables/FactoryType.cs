using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	// No identity for enum tables
	public class FactoryType
	{
		public static readonly Column<FactoryType, DbInt> FactoryTypeID = new Column<FactoryType, DbInt>() { IsPrimaryKey = true};
		public static readonly Column<FactoryType, DbString> Name = new Column<FactoryType, DbString>() ;
	}
}
