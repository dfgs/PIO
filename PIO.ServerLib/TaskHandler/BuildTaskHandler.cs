using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogLib;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;

namespace PIO.ServerLib.TaskHandler
{
	public class BuildTaskHandler : TaskHandler
	{
		private IFactoryBuilderModule factoryBuilderModule;

		public override int TaskTypeID => (int)TaskTypeIDs.Build;
		public BuildTaskHandler(ILogger Logger, IFactoryBuilderModule FactoryBuilderModule) : base(Logger)
		{
			this.factoryBuilderModule = FactoryBuilderModule;
		}

		public override void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task)
		{
			
			LogEnter();

			Log(LogLevels.Information, $"Building Factory (FactoryID={Task.FactoryID})");
			Try(() => factoryBuilderModule.Build(Task.FactoryID)).OrThrow("Failed to build");
			
			Try(()=>TaskSchedulerModule.EnqueueTask(Task.FactoryID,(int)TaskTypeIDs.CheckMaterials, null, 1)).OrAlert("Failed to enqueue new task");
		}


	}
}
