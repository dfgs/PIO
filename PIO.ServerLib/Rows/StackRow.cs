using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	public class StackRow
	{
		public int StackID {get;set;}
		public int FactoryID {get;set;}
		public int ResourceID {get;set;}
		public int Quantity {get;set;}
	}
}
