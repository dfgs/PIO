using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIO.ServerLib.TaskHandler
{
	public abstract class TaskHandler : Module, ITaskHandler
	{
		public abstract int TaskTypeID { get; }

		public TaskHandler(ILogger Logger) : base(Logger)
		{
		}

		public abstract void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task);
	}
}
