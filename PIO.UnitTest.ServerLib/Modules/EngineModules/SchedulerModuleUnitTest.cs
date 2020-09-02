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
		public void ShouldAddTaskFromProducer()
		{
			SchedulerModule module;
			IProducerModule taskModule;

			taskModule = new MockedProducerModule();
			module = new SchedulerModule(NullLogger.Instance,null, taskModule, new MockedMoverModule());
			taskModule.BeginProduce(1);
			Assert.AreEqual(1, module.Count);
		}

		[TestMethod]
		public void ShouldAddTaskFromMover()
		{
			SchedulerModule module;
			IMoverModule taskModule;

			taskModule = new MockedMoverModule();
			module = new SchedulerModule(NullLogger.Instance, null, new MockedProducerModule(), taskModule);
			taskModule.BeginMoveTo(1,2);
			Assert.AreEqual(1, module.Count);
		}

	}
}
