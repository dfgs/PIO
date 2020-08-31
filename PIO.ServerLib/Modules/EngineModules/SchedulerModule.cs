using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.CommandBuilders;
using NetORMLib.ConnectionFactories;
using NetORMLib.Databases;
using NetORMLib.Sql.CommandBuilders;
using NetORMLib.Sql.ConnectionFactories;
using NetORMLib.Sql.Databases;
using NetORMLib.VersionControl;
using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
using PIO.ServerLib.Modules;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PIO.ServerLib.Modules
{
	public class SchedulerModule : AtModule<Task>,ISchedulerModule
	{
		private ITaskModule taskModule;

		public SchedulerModule(ILogger Logger,ITaskModule TaskModule) : base(Logger, ThreadPriority.Normal)
		{
			this.taskModule = TaskModule;
		}

		public void Add(Task Task)
		{
			this.Add(Task.ETA, Task);
		}

		protected override void OnTriggerEvent(Task Task)
		{
			Log(LogLevels.Information, $"Task finished (TaskID={Task.TaskID}, TaskTypeID={Task.TaskTypeID})");
			//taskModule.

			Log(LogLevels.Information, $"Deleting task (TaskID={Task.TaskID})");
			Try(() => taskModule.DeleteTask(Task.TaskID)).OrThrow<PIOInternalErrorException>("Failed to delete task");

		}
	}
}
