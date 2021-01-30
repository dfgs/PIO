using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models.Modules
{
	public interface IBotSchedulerModule:IThreadModule
	{
		Bot CreateBot(int WorkerID);


	}
}
