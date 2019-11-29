using NetORMLib.Columns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Rows
{
	// No identity for enum tables
	public class ResourceRow
	{
		public int ResourceID {get;set;}
		public string Name {get;set;}
	}
}
