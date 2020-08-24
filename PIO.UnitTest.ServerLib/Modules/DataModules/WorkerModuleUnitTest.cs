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

			database = new MockedDatabase<Worker>(false, 3, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(NullLogger.Instance, database);
			results = module.GetWorkers(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
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


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Worker>(true,1, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWorkersAndLogError()
		{
			MockedDatabase<Worker> database;
			WorkerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Worker>(true, 3, (t) => new Worker() { WorkerID = t });
			module = new WorkerModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetWorkers(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		

	}
}
