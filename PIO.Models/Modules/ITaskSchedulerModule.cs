using ModuleLib;
using PIO.Models.Modules;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models.Modules
{
	public interface ITaskSchedulerModule:IPIOModule,IThreadModule
	{
		bool Initialize();
		bool Register(ITaskHandler TaskHandler);
		void EnqueueTask(int FactoryID,int TaskTypeID,int? TargetResourceTypeID,int DelayInSec);

	}
}
