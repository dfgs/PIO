using NetORMLib.Columns;
using NetORMLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Tables
{
	public class PhraseTable : Table
	{
		public static readonly Column<int> PhraseID = new Column<int>() {Constraint = NetORMLib.ColumnConstraints.PrimaryKey };
		public static readonly Column<string> Key = new Column<string>();
		public static readonly Column<string> CountryCode = new Column<string>() ;
		public static readonly Column<string> Value = new Column<string>();
	}
}
