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
	public class StackModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetStack()
		{
			IDatabase database;
			StackModule module;
			Stack result;

			database = new MockedDatabase(false, 1);
			module = new StackModule(NullLogger.Instance, database);
			result = module.GetStack(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetStacks()
		{
			IDatabase database;
			StackModule module;
			Stack[] results;

			database = new MockedDatabase(false, 3);
			module = new StackModule(NullLogger.Instance, database);
			results = module.GetStacks(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetStackAndLogError()
		{
			IDatabase database;
			StackModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new StackModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetStack(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStacksAndLogError()
		{
			IDatabase database;
			StackModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new StackModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetStacks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
