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
		public void ShouldAddTask()
		{
			SchedulerModule module;
			IProducerModule producerModule;

			producerModule = new MockedProducerModule();
			module = new SchedulerModule(NullLogger.Instance,null,producerModule);
			producerModule.BeginProduce(1);
			Assert.AreEqual(1, module.Count);
		}


	
	}
}
