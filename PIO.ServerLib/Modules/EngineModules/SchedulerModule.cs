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

	// should not throw exceptions, in order to prevent module to stop
	public class SchedulerModule : AtModule<Task>,ISchedulerModule
	{
		private ITaskModule taskModule;
		private IIdlerModule idlerModule;
		private IProducerModule producerModule;
		private IMoverModule moverModule;
		private IFactoryBuilderModule factoryBuilderModule;
		private ITakerModule takerModule;
		private IStorerModule storerModule;

		public event TaskEventHandler TaskStarted;
		public event TaskEventHandler TaskEnded;


		public SchedulerModule(ILogger Logger,ITaskModule TaskModule, IIdlerModule IdlerModule, IProducerModule ProducerModule,IMoverModule MoverModule,  ITakerModule TakerModule,IStorerModule StorerModule, IFactoryBuilderModule FactoryBuilderModule) : base(Logger, ThreadPriority.Normal)
		{
			this.taskModule = TaskModule;this.idlerModule = IdlerModule; this.producerModule = ProducerModule;this.moverModule = MoverModule;this.takerModule = TakerModule;this.storerModule = StorerModule;
			this.factoryBuilderModule = FactoryBuilderModule;


			idlerModule.TaskCreated += TaskGeneratorModule_TaskCreated;
			producerModule.TaskCreated += TaskGeneratorModule_TaskCreated;
			moverModule.TaskCreated += TaskGeneratorModule_TaskCreated;
			takerModule.TaskCreated += TaskGeneratorModule_TaskCreated;
			storerModule.TaskCreated += TaskGeneratorModule_TaskCreated;


			factoryBuilderModule.TaskCreated += TaskGeneratorModule_TaskCreated;
		}

		
		private void TaskGeneratorModule_TaskCreated(ITaskGeneratorModule Module, Task Task)
		{
			Add(Task);
		}

		private void Add(Task Task)
		{
			LogEnter();

			Log(LogLevels.Information, $"Adding new task (TaskID={Task.TaskID}, WorkerID={Task.WorkerID}, ETA={Task.ETA})");
			Try(() => this.Add(Task.ETA, Task)).OrAlert("Failed to enqueue task");

			Log(LogLevels.Information, $"Triggering callbacks");
			if (TaskStarted!=null) TaskStarted(this, new TaskEventArgs(Task));
		}

		protected override void OnStarting()
		{
			Task[] items;

			LogEnter();

			Log(LogLevels.Information, $"Loading existing tasks");
			if (!Try(() => taskModule.GetTasks()).OrAlert(out items, "Failed to load tasks")) return;
		
			foreach(Task item in items)
			{
				Add(item.ETA, item);
			}
		}

		protected override void OnTriggerEvent(Task Task)
		{
			Log(LogLevels.Information, $"Task finished (TaskID={Task.TaskID}, TaskTypeID={Task.TaskTypeID})");

			Log(LogLevels.Information, $"Terminating task (TaskID={Task.TaskID})");
			switch (Task.TaskTypeID)
			{
				case TaskTypeIDs.Idle:
					Try(() => idlerModule.EndIdle(Task.WorkerID)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.Produce:
					Try(() => producerModule.EndProduce(Task.WorkerID)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.MoveTo:
					Try(() => moverModule.EndMoveTo(Task.WorkerID, Task.X,Task.Y)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.Take:
					Try(() => takerModule.EndTake(Task.WorkerID, Task.ResourceTypeID.Value)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.Store:
					Try(() => storerModule.EndStore(Task.WorkerID, Task.ResourceTypeID.Value)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.CreateBuilding:
					Try(() => factoryBuilderModule.EndCreateBuilding(Task.WorkerID, Task.FactoryTypeID.Value)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				case TaskTypeIDs.Build:
					Try(() => factoryBuilderModule.EndBuild(Task.WorkerID)).OrAlert($"Failed to terminate task (TaskID={Task.TaskID})");
					break;
				default:
					Log(LogLevels.Warning, $"Unhandled task type (TaskTypeID={Task.TaskTypeID})");
					break;
			}

			Log(LogLevels.Information, $"Deleting task (TaskID={Task.TaskID})");
			Try(() => taskModule.DeleteTask(Task.TaskID)).OrAlert("Failed to delete task");

			Log(LogLevels.Information, $"Triggering callbacks");
			if (TaskEnded != null) TaskEnded(this, new TaskEventArgs(Task));



		}








	}
}
