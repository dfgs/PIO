using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.ServerLib.Modules
{
	public interface IWorkerScheduler:IThreadModule
	{
		void Add(int WorkerID);
	
	}
}
