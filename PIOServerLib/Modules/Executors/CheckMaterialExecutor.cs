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

		private IMaterialModule materialModule;
		private IFactoryModule factoryModule;

		public CheckMaterialExecutor(ILogger Logger,IFactoryModule FactoryModule, IMaterialModule MaterialModule):base(Logger)
		{
			this.factoryModule = FactoryModule;
			this.materialModule = MaterialModule;
		}

		public override int Execute(int FactoryID)
		{
			dynamic factory;
			IEnumerable<dynamic> materials;

			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow($"Failed to get factory {FactoryID}");
			
			Log(LogLevels.Information, $"Getting materials for factory {FactoryID}, factory type {factory.FactoryTypeID}");
			materials = Try(() => materialModule.GetMaterials(factory.FactoryTypeID)).OrThrow($"Failed to get material for factory type {factory.FactoryTypeID}");

			
			return 1;
		}
	}
}
