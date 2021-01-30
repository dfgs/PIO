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
using PIO.UnitTest.ServerLib.Mocks.EngineModules;
using System.Net.Configuration;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class SchedulerModuleUnitTest
	{
		[TestMethod]
		public void ShouldAddTaskFromIdler()
		{
			SchedulerModule module;
			IIdlerModule taskModule;

			taskModule = new MockedIdlerModule();
			module = new SchedulerModule(NullLogger.Instance, null, taskModule, new MockedProducerModule(), new MockedMoverModule(),  new MockedTakerModule(), new MockedStorerModule(), new MockedFactoryBuilderModule());;
			taskModule.BeginIdle(1,10);
			Assert.AreEqual(1, module.Count);
		}
		[TestMethod]
		public void ShouldAddTaskFromProducer()
		{
			SchedulerModule module;
			IProducerModule taskModule;

			taskModule = new MockedProducerModule();
			module = new SchedulerModule(NullLogger.Instance,null, new MockedIdlerModule(), taskModule, new MockedMoverModule(),new MockedTakerModule(), new MockedStorerModule(), new MockedFactoryBuilderModule());
			taskModule.BeginProduce(1);
			Assert.AreEqual(1, module.Count);
		}

		[TestMethod]
		public void ShouldAddTaskFromMover()
		{
			SchedulerModule module;
			IMoverModule taskModule;

			taskModule = new MockedMoverModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(), taskModule, new MockedTakerModule(), new MockedStorerModule(), new MockedFactoryBuilderModule());
			taskModule.BeginMoveTo(1,2,5);
			Assert.AreEqual(1, module.Count);
		}

		

		[TestMethod]
		public void ShouldAddTaskFromTaker()
		{
			SchedulerModule module;
			ITakerModule takerModule;

			takerModule = new MockedTakerModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(), new MockedMoverModule(), takerModule, new MockedStorerModule(), new MockedFactoryBuilderModule());
			takerModule.BeginTake(1, ResourceTypeIDs.Wood);
			Assert.AreEqual(1, module.Count);
		}
		[TestMethod]
		public void ShouldAddTaskFromStorer()
		{
			SchedulerModule module;
			IStorerModule storerModule;

			storerModule = new MockedStorerModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(), new MockedMoverModule(), new MockedTakerModule(), storerModule, new MockedFactoryBuilderModule());
			storerModule.BeginStore(1);
			Assert.AreEqual(1, module.Count);
		}

		[TestMethod]
		public void ShouldAddTaskFromFactoryBuilder()
		{
			SchedulerModule module;
			IFactoryBuilderModule taskModule;

			taskModule = new MockedFactoryBuilderModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(), new MockedMoverModule(), new MockedTakerModule(), new MockedStorerModule(), taskModule);
			taskModule.BeginCreateBuilding(1, FactoryTypeIDs.Stockpile);
			Assert.AreEqual(1, module.Count);

			taskModule.BeginBuild(1);
			Assert.AreEqual(2, module.Count);
		}

		


	}
}
