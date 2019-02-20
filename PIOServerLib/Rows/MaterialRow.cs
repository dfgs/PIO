using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	public class MaterialRow
	{
		public int MaterialID {get;set;}
		public int FactoryTypeID {get;set;}
		public int ResourceID {get;set;}
		public int Quantity {get;set;}
	}
}
