﻿using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class StackTable : Table
	{
		public static readonly Column<int> StackID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public static readonly Column<int> BuildingID = new Column<int>();
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>();
		public static readonly Column<int> Quantity = new Column<int>() {DefaultValue=0 } ;
	}
}
