using NetORMLib.Columns;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Tables
{
	// No identity for enum tables
	public class TaskTypeTable
	{
		public static readonly Column<TaskTypeTable, TaskTypeIDs> TaskTypeID = new Column<TaskTypeTable, TaskTypeIDs>() { IsPrimaryKey = true };
		public static readonly Column<TaskTypeTable, string> Name = new Column<TaskTypeTable, string>();
	}
}
