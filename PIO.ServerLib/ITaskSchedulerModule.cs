using ModuleLib;
using PIO.Models;
using PIO.ServerLib.TaskHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib
{
	public interface ITaskSchedulerModule:IThreadModule
	{
		bool Initialize();
		bool Register(ITaskHandler TaskHandler);
		void EnqueueTask(int FactoryID,int TaskTypeID,int DelayInSec);

	}
}
