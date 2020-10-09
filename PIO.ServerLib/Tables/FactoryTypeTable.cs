using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class FactoryTypeTable : Table
	{
		public static readonly Column<FactoryTypeIDs> FactoryTypeID = new Column<FactoryTypeIDs>() { IsPrimaryKey = true };
		public new static readonly Column<string> Name = new Column<string>();
		public static readonly Column<int> HealthPoints = new Column<int>();
	}
}
