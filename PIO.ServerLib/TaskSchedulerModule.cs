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

		public bool Initialize()
		{
			Task[] tasks;

			Log(LogLevels.Information, "Retrieving tasks");
			if (!Try<Task[]>( ()=>taskModule.GetTasks().ToArray() ).OrAlert(out tasks,"Failed to retrieve tasks")) return false;

			foreach(Task task in tasks)
			{
				Add(task.ETA, task);
			}

			return true;
		}

		public bool Register(ITaskHandler TaskHandler)
		{
			Log(LogLevels.Information, $"Registering TaskHandler with TaskTypeID {TaskHandler.TaskTypeID}");
			return Try(() => taskHandlers.Add(TaskHandler.TaskTypeID, TaskHandler)).OrAlert("Failed to register TaskHandler");
		}

		protected override void OnTriggerEvent(Task Task)
		{
			ITaskHandler handler;

			LogEnter();

			Log(LogLevels.Information, $"Trying to find TaskHandler with TaskTypeID {Task.TaskTypeID}");
			if (!taskHandlers.TryGetValue(Task.TaskTypeID, out handler))
			{
				Log(LogLevels.Fatal, $"Failed to find TaskHandler with TaskTypeID {Task.TaskTypeID}");
				return;
			}


			/*
			int eventID;


			if (!Try<int>(() => ScheduledExecutor.Executor.Execute(ScheduledExecutor.FactoryID)).OrAlert(out eventID, "Unexpected error occured in executor")) return;
			// post event
			Try(() => taskModule.DeleteScheduledTask(ScheduledExecutor.ScheduledTaskID)).OrThrow("Failed to delete scheduled task");
			Try(() => messageBrokerModule.Post(new Message(ScheduledExecutor.FactoryID, eventID))).OrAlert("Failed to post message to broker");
			*/
		}

		





	}
}
