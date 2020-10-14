using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.Models.Exceptions;
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
			module = new SchedulerModule(NullLogger.Instance, null, taskModule, new MockedProducerModule(), new MockedMoverModule(), new MockedCarrierModule());
			taskModule.BeginIdle(1,10);
			Assert.AreEqual(1, module.Count);
		}
		[TestMethod]
		public void ShouldAddTaskFromProducer()
		{
			SchedulerModule module;
			IProducerModule taskModule;

			taskModule = new MockedProducerModule();
			module = new SchedulerModule(NullLogger.Instance,null, new MockedIdlerModule(), taskModule, new MockedMoverModule(),new MockedCarrierModule());
			taskModule.BeginProduce(1);
			Assert.AreEqual(1, module.Count);
		}

		[TestMethod]
		public void ShouldAddTaskFromMover()
		{
			SchedulerModule module;
			IMoverModule taskModule;

			taskModule = new MockedMoverModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(), taskModule, new MockedCarrierModule());
			taskModule.BeginMoveTo(1,2);
			Assert.AreEqual(1, module.Count);
		}

		[TestMethod]
		public void ShouldAddTaskFromCarrier()
		{
			SchedulerModule module;
			ICarrierModule taskModule;

			taskModule = new MockedCarrierModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedIdlerModule(), new MockedProducerModule(),new MockedMoverModule(), taskModule);
			taskModule.BeginCarryTo(1, 2,ResourceTypeIDs.Wood);
			Assert.AreEqual(1, module.Count);
		}
	}
}
