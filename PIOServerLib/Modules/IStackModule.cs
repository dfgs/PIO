using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IStackModule:IDatabaseModule
	{
		Row<Stack> GetStack(int StackID);
		IEnumerable<Row<Stack>> GetStacks(int FactoryID);
	}
}
