using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	public class StateRow
	{
		public int StateID {get;set;}
		public string Name {get;set;}
		public int TaskID {get;set;}
		public int Duration {get;set;}
	}
}
