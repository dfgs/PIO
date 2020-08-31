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
			Task item;
			SchedulerModule module;

			item = new Task() { TaskID=1,TaskTypeID=2, ETA=DateTime.Now,WorkerID=3 };

			module = new SchedulerModule(NullLogger.Instance,null);
			module.Add(item);
			Assert.AreEqual(1, module.Count);
		}


	
	}
}
