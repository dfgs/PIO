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
	public class TaskTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetTaskType()
		{
			MockedDatabase<TaskType> database;
			TaskTypeModule module;
			TaskType result;

			database = new MockedDatabase<TaskType>(false, 1, (t) => new TaskType() { TaskTypeID = (TaskTypeIDs)t });
			module = new TaskTypeModule(NullLogger.Instance, database);
			result = module.GetTaskType(TaskTypeIDs.Idle);
			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);
		}
		[TestMethod]
		public void ShouldGetTaskTypes()
		{
			MockedDatabase<TaskType> database;
			TaskTypeModule module;
			TaskType[] results;

			database = new MockedDatabase<TaskType>(false, 3, (t) => new TaskType() { TaskTypeID = (TaskTypeIDs)t });
			module = new TaskTypeModule(NullLogger.Instance, database);
			results = module.GetTaskTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual((TaskTypeIDs)t, results[t].TaskTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetTaskTypeAndLogError()
		{
			MockedDatabase<TaskType> database;
			TaskTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<TaskType>(true,1, (t) => new TaskType() { TaskTypeID = (TaskTypeIDs)t });
			module = new TaskTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTaskType(TaskTypeIDs.MoveTo));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTaskTypesAndLogError()
		{
			MockedDatabase<TaskType> database;
			TaskTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<TaskType>(true, 3, (t) => new TaskType() { TaskTypeID = (TaskTypeIDs)t });
			module = new TaskTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetTaskTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


	}
}
