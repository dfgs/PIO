using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public interface IStateModule:IDatabaseModule
	{
		State GetState(int StateID);
	}
}
