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
		private Dictionary<int, ITask> states;
		private IFactoryModule factoryModule;

		public TaskSchedulerModule(ILogger Logger, IFactoryModule FactoryModule) : base(Logger)
		{
			ITask Task;

			this.factoryModule = FactoryModule;
			states = new Dictionary<int, ITask>();

			Task = new CollectMaterialTask(Logger); states.Add(Task.TaskID, Task);
			Task = new ProgressBuildingTask(Logger); states.Add(Task.TaskID, Task);
		}

		private ITask GetTask(int TaskID)
		{
			return Try( ()=>states[TaskID]).OrThrow($"TaskID {TaskID} is not registed");
		}

		public void SetTask(int FactoryID,int TaskID)
		{
			ITask state;
			int currentTaskID;
			
			LogEnter();

			currentTaskID = Try(() => factoryModule.GetTaskID(FactoryID)).OrThrow($"Failed to retrieve current TaskID for FactoryID {FactoryID}");

			state = Try(() => GetTask(currentTaskID)).OrThrow($"Cannot find valid factory state with ID {currentTaskID}");
			state.Leave();

			Try(() => factoryModule.SetTaskID(FactoryID, TaskID)).OrThrow($"Failed to set TaskID for FactoryID {FactoryID}");

			state = Try(() => GetTask(TaskID)).OrThrow($"Cannot find valid factory state with ID {TaskID}");
			state.Enter();
		}


	}
}
