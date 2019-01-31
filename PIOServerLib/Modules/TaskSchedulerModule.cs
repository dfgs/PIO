using LogLib;
using ModuleLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Modules.Tasks;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class TaskSchedulerModule : Module,ITaskSchedulerModule
	{
		//private IDatabase database;
		private Dictionary<int, ITask> tasks;
		private IFactoryModule factoryModule;
		private ITaskModule taskModule;

		public TaskSchedulerModule(ILogger Logger, IFactoryModule FactoryModule,ITaskModule TaskModule) : base(Logger)
		{
			ITask Task;

			this.factoryModule = FactoryModule;
			this.taskModule = TaskModule;
			tasks = new Dictionary<int, ITask>();

			Task = new CollectMaterialTask(Logger); tasks.Add(Task.TaskID, Task);
			Task = new ProgressBuildingTask(Logger); tasks.Add(Task.TaskID, Task);
		}

		private ITask GetTask(int TaskID)
		{
			return Try( ()=>tasks[TaskID]).OrThrow($"TaskID {TaskID} is not registed");
		}

		public void SetTask(int FactoryID,int TaskID)
		{
			dynamic row;
			ITask task;
			int currentTaskID;
			
			LogEnter();

			row = Try(() => taskModule.GetTasks(FactoryID)).OrThrow($"Failed to retrieve current task for FactoryID {FactoryID}");
			if (row != null)
			{
				currentTaskID = row.TaskID;
				task = Try(() => GetTask(currentTaskID)).OrThrow($"Cannot find valid task with ID {currentTaskID}");
				Try(task.Leave).OrThrow($"Failed to leave current task with ID {currentTaskID}");
			}

			task = Try(() => GetTask(TaskID)).OrThrow($"Cannot find valid task with ID {TaskID}");
			Try(() => taskModule.SetTask(FactoryID, TaskID)).OrThrow($"Failed to create task for FactoryID {FactoryID}");
			Try(task.Enter).OrThrow($"Failed to start task with ID {TaskID}");
		}


	}
}
