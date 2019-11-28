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
	public interface ITransitionModule:IDatabaseModule
	{
		Row<Transition> GetTransition(int TransitionID);
		Row<Transition> GetTransition(int StateID,int EventID);
	}
}
