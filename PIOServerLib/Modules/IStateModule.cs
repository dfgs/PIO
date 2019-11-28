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
	public interface IStateModule:IDatabaseModule
	{
		Row<State> GetState(int StateID);
	}
}
