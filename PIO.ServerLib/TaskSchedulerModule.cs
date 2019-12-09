using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.Models;
using PIO.ServerLib.TaskHandler;

namespace PIO.ServerLib
{
	public class TaskSchedulerModule : AtModule<Task>,ITaskSchedulerModule
	{
		private readonly ITaskModule taskModule;
		private Dictionary<int, ITaskHandler> taskHandlers;

		public TaskSchedulerModule(ILogger Logger, ITaskModule TaskModule) : base(Logger)
		{

			this.taskModule = TaskModule;
			taskHandlers = new Dictionary<int, ITaskHandler>();
		}

		public void EnqueueTask(int FactoryID, int TaskTypeID, int? TargetResourceTypeID, int DelayInSec)
		{
			Task task;
			LogEnter();

			Log(LogLevels.Information, $"Enqueuing task (FactoryID={FactoryID}, TaskTypeID={TaskTypeID},TargetResourceTypeID={TargetResourceTypeID}, DelayInSec={DelayInSec})");
			task = Try<Task>(()=> taskModule.CreateTask(FactoryID, TaskTypeID, TargetResourceTypeID, DateTime.Now.AddSeconds( DelayInSec))).OrThrow("Failed to enqueue task");
			Add(task.ETA, task);
		}

		public bool Initialize()
		{
			Task[] tasks;
			LogEnter();

			Log(LogLevels.Information, "Loading tasks");
			if (!Try<Task[]>( ()=>taskModule.GetTasks() ).OrAlert(out tasks,"Failed to load tasks")) return false;

			foreach(Task task in tasks)
			{
				Add(task.ETA, task);
			}

			return true;
		}

		public bool Register(ITaskHandler TaskHandler)
		{
			LogEnter();

			Log(LogLevels.Information, $"Registering TaskHandler (TaskTypeID={TaskHandler.TaskTypeID})");
			return Try(() => taskHandlers.Add(TaskHandler.TaskTypeID, TaskHandler)).OrAlert("Failed to register TaskHandler");
		}

		protected override void OnTriggerEvent(Task Task)
		{
			ITaskHandler handler;

			LogEnter();

			if (!Try(()=>taskModule.RemoveTask(Task.TaskID)).OrAlert("Failed to remove task"))
			{
				return;
			}

			Log(LogLevels.Information, $"Trying to find TaskHandler with TaskTypeID {Task.TaskTypeID}");
			if (!taskHandlers.TryGetValue(Task.TaskTypeID, out handler))
			{
				Log(LogLevels.Fatal, $"Failed to find TaskHandler with TaskTypeID {Task.TaskTypeID}");
				return;
			}
			Try(() => handler.Execute(this, Task)).OrAlert("Failed to execute task handler");

			
		}

		





	}
}
