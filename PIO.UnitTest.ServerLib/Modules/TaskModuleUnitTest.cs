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
	public class TaskModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetTask()
		{
			IDatabase database;
			TaskModule module;
			Task result;

			database = new MockedDatabase<Task>(false, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			result = module.GetTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.TaskID);
		}
		[TestMethod]
		public void ShouldGetTasks()
		{
			IDatabase database;
			TaskModule module;
			Task[] results;

			database = new MockedDatabase<Task>(false, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			results = module.GetTasks(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].TaskID);
			}
		}
		[TestMethod]
		public void ShouldGetAllTasks()
		{
			IDatabase database;
			TaskModule module;
			Task[] results;

			database = new MockedDatabase<Task>(false, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			results = module.GetTasks();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].TaskID);
			}
		}
		[TestMethod]
		public void ShouldNotGetTaskAndLogError()
		{
			IDatabase database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true,1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			IDatabase database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetAllTasksAndLogError()
		{
			IDatabase database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetTasks());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
