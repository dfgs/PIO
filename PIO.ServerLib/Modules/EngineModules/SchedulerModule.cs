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
			LogEnter();

			Log(LogLevels.Information, $"Adding new task (TaskID={Task.TaskID}, WorkerID={Task.WorkerID}, ETA={Task.ETA})");

			Try(() => this.Add(Task.ETA, Task)).OrThrow<PIOInternalErrorException>("Failed to enqueue task");
		}

		protected override void OnStarting()
		{
			Task[] items;

			LogEnter();

			Log(LogLevels.Information, $"Loading existing tasks");
			items=Try(() => taskModule.GetTasks()).OrThrow<PIOInternalErrorException>("Failed to load tasks");
		
			foreach(Task item in items)
			{
				Add(item.ETA, item);
			}
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
