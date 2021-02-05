using NetORMLib.Columns;
using NetORMLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.ServerLib.Tables
{
	public class BotTable:Table
	{
		public static readonly Column<int> BotID = new Column<int>() { Constraint = NetORMLib.ColumnConstraints.PrimaryKey, IsIdentity = true };
		public static readonly Column<int> WorkerID = new Column<int>() { Constraint=NetORMLib.ColumnConstraints.Unique };
	}
}
