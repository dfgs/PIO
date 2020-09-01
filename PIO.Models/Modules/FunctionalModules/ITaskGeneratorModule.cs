using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public delegate void TaskCreatedHandler(ITaskGeneratorModule Module,Task Task);
	public interface ITaskGeneratorModule : IFunctionalModule
	{
		event TaskCreatedHandler TaskCreated;
	}
}
