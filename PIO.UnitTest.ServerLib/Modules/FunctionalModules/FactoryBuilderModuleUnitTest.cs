using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class FactoryBuilderModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldBuild()
		{
			FactoryBuilderModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			
			factoryModule = new MockedFactoryModule( new Factory()   );
			factoryTypeModule = new MockedFactoryTypeModule( new FactoryType() { HealthPoints=10 } );
			module = new FactoryBuilderModule(NullLogger.Instance,factoryModule,factoryTypeModule);

			Assert.AreEqual(0, factoryModule.GetFactory(0).HealthPoints);
			module.Build(0);
			Assert.AreEqual(1, factoryModule.GetFactory(0).HealthPoints);
		}

		[TestMethod]
		public void ShouldNotBuildAndLogError()
		{
			FactoryBuilderModule module;
			MemoryLogger logger;
			MockedFactoryModule factoryModule;
			MockedFactoryTypeModule factoryTypeModule;

			factoryModule = new MockedFactoryModule(new Factory() );
			factoryTypeModule = new MockedFactoryTypeModule(new FactoryType() { HealthPoints = 10 });

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, factoryModule, factoryTypeModule);

			factoryModule.ThrowException = true;
			factoryTypeModule.ThrowException = false;
			Assert.ThrowsException<Exception>(() => module.Build(0));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			factoryModule.ThrowException = false;
			factoryTypeModule.ThrowException = true;
			Assert.ThrowsException<Exception>(() => module.Build(0));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotBuildWhenHPAreMaximumAndLogError()
		{
			FactoryBuilderModule module;
			MemoryLogger logger;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;

			factoryModule = new MockedFactoryModule( new Factory() { HealthPoints=10 });
			factoryTypeModule = new MockedFactoryTypeModule( new FactoryType() { HealthPoints = 10 });

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new FactoryBuilderModule(logger, factoryModule, factoryTypeModule);
			
			Assert.ThrowsException<InvalidOperationException>(() => module.Build(0));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
	}
}
