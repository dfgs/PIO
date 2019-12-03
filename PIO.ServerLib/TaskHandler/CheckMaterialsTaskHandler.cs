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
	public class CheckMaterialsTaskHandler : TaskHandler
	{
		private IFactoryModule factoryModule;
		private IStackModule stackModule;
		private IMaterialModule materialModule;

		public override int TaskTypeID => (int)TaskTypeIDs.CheckMaterials;
		public CheckMaterialsTaskHandler(ILogger Logger, IFactoryModule FactoryModule, IStackModule StackModule, IMaterialModule MaterialModule) : base(Logger)
		{
			this.factoryModule = FactoryModule; this.stackModule = StackModule; this.materialModule = MaterialModule;
		}

		public override void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task)
		{
			Factory factory;
			Material[] materials;

			LogEnter();

			Log(LogLevels.Information, $"Checking material availability for Factory with FactoryID {Task.FactoryID}");

			factory = factoryModule.GetFactory(Task.FactoryID);
			materials = materialModule.GetMaterials(factory.FactoryTypeID);

			foreach (Material material in materials)
			{
				if (stackModule.HasEnoughResources(Task.FactoryID, material.ResourceTypeID, material.Quantity)) continue;
				Log(LogLevels.Information, $"Factory with FactoryID {Task.FactoryID} is missing material with ResourceTypeID {material.ResourceTypeID}");
				return;
			}

			Log(LogLevels.Information, $"Factory with FactoryID {Task.FactoryID} has enough material to build, consuming resources");
			foreach (Material material in materials)
			{
				Try(() => stackModule.Consume(Task.FactoryID, material.ResourceTypeID, material.Quantity)).OrAlert($"Failed to consume material with ResourceTypeID {material.MaterialID}");
			}
			
			Try(()=>TaskSchedulerModule.EnqueueTask(Task.FactoryID,(int)TaskTypeIDs.Build,10)).OrAlert("Failed to enqueue new task");
		}


	}
}
