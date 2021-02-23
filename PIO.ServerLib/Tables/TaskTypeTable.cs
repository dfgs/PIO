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
	public class TaskTypeTable : Table
	{
		public static readonly Column<TaskTypeIDs> TaskTypeID = new Column<TaskTypeIDs>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey};
		public static readonly Column<string> PhraseKey = new Column<string>();
	}
}
