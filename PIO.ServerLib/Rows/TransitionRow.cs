using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	public class TransitionRow
	{
		public int TransitionID {get;set;}
		public int StateID {get;set;}
		public int NextStateID {get;set;}
		public int EventID {get;set;}
	}
}
