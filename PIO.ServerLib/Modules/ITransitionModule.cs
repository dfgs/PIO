using ModuleLib;
using NetORMLib;
using PIO.Models;
using PIO.WebServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public interface ITransitionModule:IDatabaseModule
	{
		Transition GetTransition(int TransitionID);
		Transition GetTransition(int StateID,int EventID);
	}
}
