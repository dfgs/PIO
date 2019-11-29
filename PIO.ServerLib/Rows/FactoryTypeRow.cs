using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	// No identity for enum tables
	public class FactoryTypeRow
	{
		public int FactoryTypeID {get;set;}
		public string Name {get;set;}
	}
}
