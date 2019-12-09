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
	public class SearchMaterialsTaskHandler : TaskHandler
	{
		private IFactoryModule factoryModule;
		private IStackModule stackModule;
		private IMaterialModule materialModule;

		public override int TaskTypeID => (int)TaskTypeIDs.SearchMaterial;
		public SearchMaterialsTaskHandler(ILogger Logger, IFactoryModule FactoryModule, IStackModule StackModule, IMaterialModule MaterialModule) : base(Logger)
		{
			this.factoryModule = FactoryModule; this.stackModule = StackModule; this.materialModule = MaterialModule;
		}

		public override void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task)
		{
			Factory factory;
			Material[] materials;
			bool result;

			LogEnter();

			Log(LogLevels.Information, $"Checking material availability (FactoryID={Task.FactoryID})");

			factory = Try(() => factoryModule.GetFactory(Task.FactoryID)).OrThrow("Failed to get factory");
			materials = Try(() => materialModule.GetMaterials(factory.FactoryTypeID)).OrThrow("Failed to get materials");

			foreach (Material material in materials)
			{
				result = Try(() => stackModule.HasEnoughResources(Task.FactoryID, material.ResourceTypeID, material.Quantity)).OrThrow("Failed to check resource");
				if (result) continue;
				Log(LogLevels.Information, $"Factory is missing material with (FactoryID={Task.FactoryID}, ResourceTypeID={material.ResourceTypeID})");

				Try(() => TaskSchedulerModule.EnqueueTask(Task.FactoryID, (int)TaskTypeIDs.SearchMaterial,null, 1)).OrAlert("Failed to enqueue new task");
				return;
			}

			Log(LogLevels.Information, $"Factory has enough material to build, consuming resources (FactoryID={Task.FactoryID})");
			foreach (Material material in materials)
			{
				Try(() => stackModule.Consume(Task.FactoryID, material.ResourceTypeID, material.Quantity)).OrAlert($"Failed to consume material  (FactoryID={Task.FactoryID}, ResourceTypeID={material.MaterialID})");
			}
			
			Try(()=>TaskSchedulerModule.EnqueueTask(Task.FactoryID,(int)TaskTypeIDs.Build, null, 10)).OrAlert("Failed to enqueue new task");
		}


	}
}
