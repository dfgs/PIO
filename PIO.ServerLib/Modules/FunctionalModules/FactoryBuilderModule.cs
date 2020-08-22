using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;

namespace PIO.ServerLib.Modules
{
	public class FactoryBuilderModule :Module, IFactoryBuilderModule
	{
		private IFactoryModule factoryModule;
		private IFactoryTypeModule factoryTypeModule;

		public FactoryBuilderModule(ILogger Logger, IFactoryModule FactoryModule, IFactoryTypeModule FactoryTypeModule) : base(Logger)
		{
			this.factoryModule = FactoryModule;this.factoryTypeModule = FactoryTypeModule;
		}

	
		public void Build(int FactoryID)
		{
			Factory factory;
			FactoryType factoryType;


			LogEnter();

			Log(LogLevels.Information, $"Building Factory (FactoryID={FactoryID})");
			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow("Failed to build");
			if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exists (FactoryID={FactoryID})");
				return;
			}
			factoryType = Try(() => factoryTypeModule.GetFactoryType(factory.FactoryTypeID)).OrThrow("Failed to build");
			if (factoryType == null)
			{
				Log(LogLevels.Warning, $"FactoryType doesn't exists (FactoryTypeID={factory.FactoryTypeID})");
				return;
			}

			if (factory.HealthPoints==factoryType.HealthPoints)
			{
				Log(LogLevels.Error, $"HealthPoints are maximum value (FactoryID={FactoryID})");
				throw new InvalidOperationException($"HealthPoints are maximum value (FactoryID={FactoryID})");
			}

			Try(() => factoryModule.SetHealthPoints(FactoryID,factory.HealthPoints+1)).OrThrow("Failed to build");
		}


	}
}
