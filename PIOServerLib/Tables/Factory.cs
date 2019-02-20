using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Factory
	{
		public static readonly Column<Factory, DbInt> FactoryID = new Column<Factory, DbInt>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Factory, DbInt> FactoryTypeID = new Column<Factory, DbInt>();
		public static readonly Column<Factory, DbInt> PlanetID = new Column<Factory, DbInt>();
		public static readonly Column<Factory, DbString> Name = new Column<Factory, DbString>();
		public static readonly Column<Factory, DbInt> StateID = new Column<Factory, DbInt>();
		

	}
}
