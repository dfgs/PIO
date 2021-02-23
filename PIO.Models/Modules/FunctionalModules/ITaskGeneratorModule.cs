using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.FunctionalModules;

namespace PIO.Models.Modules
{
	public delegate void TaskCreatedHandler(ITaskGeneratorModule Module,Task[] Tasks);
	public interface ITaskGeneratorModule : IFunctionalModule
	{
		event TaskCreatedHandler TaskCreated;
	}
}
