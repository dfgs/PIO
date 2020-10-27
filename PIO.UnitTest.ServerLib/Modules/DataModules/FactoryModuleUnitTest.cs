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
	public class FactoryModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetFactory()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			Factory result;

			database = new MockedDatabase<Factory>(false, 1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(NullLogger.Instance, database);
			result = module.GetFactory(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.FactoryID);
		}
		[TestMethod]
		public void ShouldGetFactoryUsingCoordinate()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			Factory result;

			database = new MockedDatabase<Factory>(false, 1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(NullLogger.Instance, database);
			result = module.GetFactory(1, 3,4);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.FactoryID);
		}
		[TestMethod]
		public void ShouldNotGetFactoryUsingCoordinate()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			Factory result;

			database = new MockedDatabase<Factory>(false, 0, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(NullLogger.Instance, database);
			result = module.GetFactory(1, 3,4);
			Assert.IsNull(result);
		}
		[TestMethod]
		public void ShouldGetFactories()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			Factory[] results;

			database = new MockedDatabase<Factory>(false, 3, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(NullLogger.Instance, database);
			results = module.GetFactories(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].FactoryID);
			}
		}
		[TestMethod]
		public void ShouldNotGetFactoryAndLogError()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Factory>(true,1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFactory(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoryUsingCoordinateAndLogError()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Factory>(true, 1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFactory(1, 3,4));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoriesAndLogError()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Factory>(true, 3, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFactories(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldCreateFactory()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			Factory result;

			database = new MockedDatabase<Factory>(false, 1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(NullLogger.Instance, database);
			result = module.CreateFactory(1,  FactoryTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildingID);
			Assert.AreEqual(FactoryTypeIDs.Sawmill, result.FactoryTypeID);
			
			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateFactoryAndLogError()
		{
			MockedDatabase<Factory> database;
			FactoryModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Factory>(true, 1, (t) => new Factory() { FactoryID = t });
			module = new FactoryModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateFactory(1, FactoryTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
	}
}
