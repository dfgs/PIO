using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Tables
{
	public class Planet
	{
		public static readonly Column<Planet, DbInt> PlanetID = new Column<Planet, DbInt>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Planet, DbString> Name = new Column<Planet, DbString>();
	}
}
