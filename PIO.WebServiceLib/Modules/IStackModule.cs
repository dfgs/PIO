using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServerLib.Modules
{
	public interface IStackModule:IDatabaseModule
	{
		Stack GetStack(int StackID);
		IEnumerable<Stack> GetStacks(int FactoryID);
	}
}
