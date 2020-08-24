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
	public class FactoryTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetFactoryType()
		{
			MockedDatabase<FactoryType> database;
			FactoryTypeModule module;
			FactoryType result;

			database = new MockedDatabase<FactoryType>(false, 1, (t) => new FactoryType() { FactoryTypeID = t });
			module = new FactoryTypeModule(NullLogger.Instance, database);
			result = module.GetFactoryType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.FactoryTypeID);
		}
		[TestMethod]
		public void ShouldGetFactoryTypes()
		{
			MockedDatabase<FactoryType> database;
			FactoryTypeModule module;
			FactoryType[] results;

			database = new MockedDatabase<FactoryType>(false, 3, (t) => new FactoryType() { FactoryTypeID = t });
			module = new FactoryTypeModule(NullLogger.Instance, database);
			results = module.GetFactoryTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].FactoryTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypeAndLogError()
		{
			MockedDatabase<FactoryType> database;
			FactoryTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<FactoryType>(true,1, (t) => new FactoryType() { FactoryTypeID = t });
			module = new FactoryTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFactoryType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypesAndLogError()
		{
			MockedDatabase<FactoryType> database;
			FactoryTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<FactoryType>(true, 3, (t) => new FactoryType() { FactoryTypeID = t });
			module = new FactoryTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFactoryTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
