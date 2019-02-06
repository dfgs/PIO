using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Material
	{
		public static readonly Column<Material, int> MaterialID = new Column<Material, int>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Material, int> FactoryTypeID = new Column<Material, int>();
		public static readonly Column<Material, int> ResourceID = new Column<Material, int>();
		public static readonly Column<Material, int> Quantity = new Column<Material, int>() {DefaultValue=0 } ;
	}
}
