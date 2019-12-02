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
		public CheckMaterialsTaskHandler(ILogger Logger, IFactoryModule FactoryModule,IStackModule StackModule, IMaterialModule MaterialModule ) : base(Logger)
		{
			this.factoryModule = FactoryModule;this.stackModule = StackModule;this.materialModule = MaterialModule;
		}

		public override void Execute(ITaskSchedulerModule TaskSchedulerModule, Task Task)
		{
			Factory factory;
			Material[] materials;
			Stack[] stacks;
			Stack stack;

			LogEnter();

			Log(LogLevels.Information, $"Checking material availability for Factory with FactoryID {Task.FactoryID}");

			factory = factoryModule.GetFactory(Task.FactoryID);
			materials = materialModule.GetMaterials(factory.FactoryTypeID);
			stacks = stackModule.GetStacks(Task.FactoryID);

			foreach(Material material in materials)
			{
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == material.ResourceTypeID);
				if ((stack==null) || (stack.Quantity<material.Quantity))
				{
					Log(LogLevels.Information, $"Factory with FactoryID {Task.FactoryID} is missing material with ResourceTypeID {material.ResourceTypeID}");
					return;
				}
			}

			Log(LogLevels.Information, $"Factory with FactoryID {Task.FactoryID} has enough material to build");

		}

	}
}
