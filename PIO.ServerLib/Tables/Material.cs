using NetORMLib.Columns;
using NetORMLib.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Tables
{
	public class Material
	{
		public static readonly Column<Material, DbInt> MaterialID = new Column<Material, DbInt>() { IsPrimaryKey = true, IsIdentity = true };
		public static readonly Column<Material, DbInt> FactoryTypeID = new Column<Material, DbInt>();
		public static readonly Column<Material, DbInt> ResourceID = new Column<Material, DbInt>();
		public static readonly Column<Material, DbInt> Quantity = new Column<Material, DbInt>();
	}
}
