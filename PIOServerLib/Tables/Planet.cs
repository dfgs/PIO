using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Tables
{
	public class Planet
	{
		public static readonly Column<Planet, int> PlanetID = new Column<Planet, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Planet, string> Name = new Column<Planet, string>() {DefaultValue="New planet" };
	}
}
