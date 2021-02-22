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
	public class WorkerModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetWorker()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			Worker result;

			database = new MockedDatabase<Worker>(false, 1, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(NullLogger.Instance, database);
			result = module.GetWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.WorkerID);
		}
		[TestMethod]
		public void ShouldGetWorkers()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			Worker[] results;

			database = new MockedDatabase<Worker>(false, 3, (t) => new Worker() { WorkerID = t,PlanetID=2 });
			module = new WorkerModule(NullLogger.Instance, database);
			results = module.GetWorkers(2);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].WorkerID);
			}
		}
		[TestMethod]
		public void ShouldGetAllWorkers()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			Worker[] results;

			database = new MockedDatabase<Worker>(false, 3, (t) => new Worker() { WorkerID = t, PlanetID = 2 });
			module = new WorkerModule(NullLogger.Instance, database);
			results = module.GetWorkers();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for (int t = 0; t < 3; t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].WorkerID);
			}
		}
		[TestMethod]
		public void ShouldNotGetWorkerAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true,1, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWorkersAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true, 3, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWorkers(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetAllWorkersAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true, 3, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWorkers());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}


		[TestMethod]
		public void ShouldUpdateWorkerLocation()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;

			database = new MockedDatabase<Worker>(false, 1, (t) => new Worker() { WorkerID = t, PlanetID = 0 });
			module = new WorkerModule(NullLogger.Instance, database);
			module.UpdateWorker(1, 2,2);
			Assert.AreEqual(1, database.UpdatedCount);

		}
		[TestMethod]
		public void ShouldNotUpdateWorkerLocationAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true, 1, (t) => new Worker() { WorkerID = t, PlanetID = 0 });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.UpdateWorker(0, 10,10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldUpdateWorkerResourceTypeID()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;

			database = new MockedDatabase<Worker>(false, 1, (t) => new Worker() { WorkerID = t, PlanetID = 0 });
			module = new WorkerModule(NullLogger.Instance, database);
			module.UpdateWorker(1, ResourceTypeIDs.Wood);
			Assert.AreEqual(1, database.UpdatedCount);

		}
		[TestMethod]
		public void ShouldNotUpdateWorkerResourceTypeIDAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true, 1, (t) => new Worker() { WorkerID = t, PlanetID = 0 });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.UpdateWorker(0, ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateWorker()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			Worker result;

			database = new MockedDatabase<Worker>(false, 1, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(NullLogger.Instance, database);
			result = module.CreateWorker(1, 7, 9);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
			Assert.AreEqual(7, result.X);
			Assert.AreEqual(9, result.Y);

			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateWorkerAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Worker>(true, 1, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateWorker(1, 7, 9));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

			Assert.AreEqual(0, database.InsertedCount);
		}




	}
}
