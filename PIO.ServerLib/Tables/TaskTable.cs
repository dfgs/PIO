using NetORMLib.Columns;
using NetORMLib.Tables;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	public class TaskTable : Table
	{
		public static readonly Column<int> TaskID = new Column<int>() { IsIdentity = true, Constraint = NetORMLib.ColumnConstraints.PrimaryKey };
		public static readonly Column<TaskTypeIDs> TaskTypeID = new Column<TaskTypeIDs>() ;
		public static readonly Column<int> WorkerID = new Column<int>();
		public static readonly Column<int> X = new Column<int>();
		public static readonly Column<int> Y = new Column<int>();
		public static readonly Column<int> BuildingID = new Column<int>() { IsNullable = true };
		//public static readonly Column<int> TargetBuildingID = new Column<int>() { IsNullable = true };
		public static readonly Column<ResourceTypeIDs> ResourceTypeID = new Column<ResourceTypeIDs>() { IsNullable = true };
		public static readonly Column<BuildingTypeIDs> BuildingTypeID = new Column<BuildingTypeIDs>() { IsNullable = true };
		public static readonly Column<DateTime> ETA = new Column<DateTime>() ;
	}


	


}
