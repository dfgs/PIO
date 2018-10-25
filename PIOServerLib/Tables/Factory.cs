using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Factory
	{
		public static readonly Column<Factory, int> FactoryID = new Column<Factory, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Factory, int> PlanetID = new Column<Factory, int>();
		public static readonly Column<Factory, string> Name = new Column<Factory, string>() ;
		public static readonly Column<Factory, int> StatusID = new Column<Factory, int>();
	}
}
