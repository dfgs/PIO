using ModuleLib;
using NetORMLib;
using PIOServerLib.Rows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IStackModule:IDatabaseModule
	{
		StackRow GetStack(int StackID);
		StackRow GetStack(int FactoryID, int ResourceID);
		IEnumerable<StackRow> GetStacks(int FactoryID);
	}
}
