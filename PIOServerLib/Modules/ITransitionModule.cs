using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface ITransitionModule:IDatabaseModule
	{
		Row GetTransition(int TransitionID);
		Row GetTransition(int StateID,int EventID);
	}
}
