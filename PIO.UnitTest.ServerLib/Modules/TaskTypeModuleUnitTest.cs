using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class TaskTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetTaskType()
		{
			IDatabase database;
			TaskTypeModule module;
			TaskType result;

			database = new MockedDatabase(false, 1);
			module = new TaskTypeModule(NullLogger.Instance, database);
			result = module.GetTaskType(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetTaskTypes()
		{
			IDatabase database;
			TaskTypeModule module;
			TaskType[] results;

			database = new MockedDatabase(false, 3);
			module = new TaskTypeModule(NullLogger.Instance, database);
			results = module.GetTaskTypes().ToArray();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetTaskTypeAndLogError()
		{
			IDatabase database;
			TaskTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new TaskTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetTaskType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTaskTypesAndLogError()
		{
			IDatabase database;
			TaskTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new TaskTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetTaskTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
