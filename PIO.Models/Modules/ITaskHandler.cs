using ModuleLib;
using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Models.Modules
{
	public interface ITaskHandler:IPIOModule
	{
		int TaskTypeID
		{
			get;
		}

		void Execute(ITaskSchedulerModule TaskSchedulerModule,Task Task);

	}
}
