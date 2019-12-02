using ModuleLib;
using PIO.ServerLib.TaskHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib
{
	public interface ITaskSchedulerModule:IThreadModule
	{
		bool Initialize();
		bool Register(ITaskHandler TaskHandler);
		//void StartTask(int FactoryID, int TaskID,DateTime ETA);

	}
}
