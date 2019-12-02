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
	public class FactoryTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetFactoryType()
		{
			IDatabase database;
			FactoryTypeModule module;
			FactoryType result;

			database = new MockedDatabase(false, 1);
			module = new FactoryTypeModule(NullLogger.Instance, database);
			result = module.GetFactoryType(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetFactoryTypes()
		{
			IDatabase database;
			FactoryTypeModule module;
			FactoryType[] results;

			database = new MockedDatabase(false, 3);
			module = new FactoryTypeModule(NullLogger.Instance, database);
			results = module.GetFactoryTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypeAndLogError()
		{
			IDatabase database;
			FactoryTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new FactoryTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetFactoryType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypesAndLogError()
		{
			IDatabase database;
			FactoryTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new FactoryTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetFactoryTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
