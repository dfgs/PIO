using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public interface ITaskSchedulerModule:IThreadModule
	{
		bool Initialize();
		//void StartTask(int FactoryID, int TaskID,DateTime ETA);

	}
}
