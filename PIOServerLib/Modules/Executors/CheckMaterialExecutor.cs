using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules.Executors
{
	public class CheckMaterialExecutor : Executor
	{
		public override int TaskID => 0;

		private readonly IMaterialModule materialModule;
		private readonly IFactoryModule factoryModule;
		private readonly IStackModule stackModule;
		
		public CheckMaterialExecutor(ILogger Logger,IFactoryModule FactoryModule, IMaterialModule MaterialModule,IStackModule StackModule):base(Logger)
		{
			this.factoryModule = FactoryModule;
			this.materialModule = MaterialModule;
			this.stackModule = StackModule;
		}

		public override int Execute(int FactoryID)
		{
			dynamic factory,stack;
			IEnumerable<dynamic> materials;

			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow($"Failed to get factory {FactoryID}");
			
			Log(LogLevels.Information, $"Getting materials for factory {FactoryID}, factory type {factory.FactoryTypeID}");
			materials = Try(() => materialModule.GetMaterials(factory.FactoryTypeID)).OrThrow($"Failed to get material for factory type {factory.FactoryTypeID}");

			foreach(dynamic material in materials)
			{
				stack = Try(() => stackModule.GetStack(FactoryID, material.ResourceID)).OrThrow($"Failed to check material quantity in factory {FactoryID}");
				if (stack.Quantity<material.Quantity)
				{
					Log(LogLevels.Information, $"Factory {FactoryID} has not enough material {material.MaterialID}");
					return 0;
				}
			}

			Log(LogLevels.Information, $"Factory {FactoryID} has enough material");
			return 1;
		}
	}
}
