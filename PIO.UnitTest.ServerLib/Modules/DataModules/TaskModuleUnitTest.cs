using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Exceptions;
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
			MockedDatabase<Task> database;
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
			MockedDatabase<Task> database;
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
		public void ShouldNotGetTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true,1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}

		[TestMethod]
		public void ShouldInsertTask()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			Task result;
			DateTime eta;

			database = new MockedDatabase<Task>(false, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			eta = DateTime.Now;
			result = module.InsertTask(1,eta);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(eta, result.ETA);
			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotInsertTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Task>(true, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.InsertTask(1,DateTime.Now));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}

	}
}
