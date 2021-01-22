using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
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
		public void ShouldGetAllTasks()
		{
			MockedDatabase<Task> database;
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
		public void ShouldGetLastTask()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			Task result;

			database = new MockedDatabase<Task>(false, 5, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			result = module.GetLastTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(4, result.TaskID);
		}
		[TestMethod]
		public void ShouldNotGetTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true,1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetAllTasksAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true, 3, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTasks());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetLastTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetLastTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldCreateTask()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			Task result;
			DateTime eta;

			database = new MockedDatabase<Task>(false, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			eta = DateTime.Now;
			result = module.CreateTask(TaskTypeIDs.Idle,1, 3,4,5, ResourceTypeIDs.Coal, null, eta);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(5, result.BuildingID);
			Assert.AreEqual(3, result.X);
			Assert.AreEqual(4, result.Y);
			Assert.AreEqual(ResourceTypeIDs.Coal, result.ResourceTypeID);

			Assert.AreEqual(eta, result.ETA);
			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateTask(TaskTypeIDs.Idle, 1, 3, 4,5,  ResourceTypeIDs.Coal, null, DateTime.Now));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}



		[TestMethod]
		public void ShouldDeleteTask()
		{
			MockedDatabase<Task> database;
			TaskModule module;

			database = new MockedDatabase<Task>(false, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(NullLogger.Instance, database);
			module.DeleteTask(1);
			Assert.AreEqual(1, database.DeletedCount);
		}

		[TestMethod]
		public void ShouldNotDeleteTaskAndLogError()
		{
			MockedDatabase<Task> database;
			TaskModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Task>(true, 1, (t) => new Task() { TaskID = t });
			module = new TaskModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.DeleteTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}





	}
}
