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
	public class StackModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetStack()
		{
			MockedDatabase<Stack> database;
			StackModule module;
			Stack result;

			database = new MockedDatabase<Stack>(false, 1, (t) => new Stack() { StackID = t });
			module = new StackModule(NullLogger.Instance, database);
			result = module.GetStack(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.StackID);
		}
		[TestMethod]
		public void ShouldGetStacks()
		{
			MockedDatabase<Stack> database;
			StackModule module;
			Stack[] results;

			database = new MockedDatabase<Stack>(false, 3, (t) => new Stack() { StackID = t });
			module = new StackModule(NullLogger.Instance, database);
			results = module.GetStacks(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].StackID);
			}
		}
		[TestMethod]
		public void ShouldNotGetStackAndLogError()
		{
			MockedDatabase<Stack> database;
			StackModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Stack>(true,1, (t) => new Stack() { StackID = t });
			module = new StackModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetStack(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStacksAndLogError()
		{
			MockedDatabase<Stack> database;
			StackModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Stack>(true, 3, (t) => new Stack() { StackID = t });
			module = new StackModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetStacks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}




		[TestMethod]
		public void ShouldConsume()
		{
			MockedDatabase<Stack> database;
			StackModule module;

			database = new MockedDatabase<Stack>(false, 1, (t) => new Stack() { StackID = t,FactoryID=0,ResourceTypeID=0, Quantity=5 });
			module = new StackModule(NullLogger.Instance, database);
			module.Consume(0,0,1);
			Assert.AreEqual(1, database.UpdatedCount);
		}
		[TestMethod]
		public void ShouldNotConsume()
		{
			MockedDatabase<Stack> database;
			StackModule module;


			database = new MockedDatabase<Stack>(false, 1, (t) => new Stack() { StackID = t, FactoryID = 0, ResourceTypeID = 0, Quantity = 5 });
			module = new StackModule(NullLogger.Instance, database);
			Assert.ThrowsException<InvalidOperationException>(() => module.Consume(0,0,10));
		}

		

	}
}
