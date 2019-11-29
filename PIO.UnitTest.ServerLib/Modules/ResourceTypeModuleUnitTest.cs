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
	public class ResourceTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetResourceType()
		{
			IDatabase database;
			ResourceTypeModule module;
			ResourceType result;

			database = new MockedDatabase(false, 1);
			module = new ResourceTypeModule(NullLogger.Instance, database);
			result = module.GetResourceType(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetResourceTypes()
		{
			IDatabase database;
			ResourceTypeModule module;
			ResourceType[] results;

			database = new MockedDatabase(false, 3);
			module = new ResourceTypeModule(NullLogger.Instance, database);
			results = module.GetResourceTypes().ToArray();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetResourceTypeAndLogError()
		{
			IDatabase database;
			ResourceTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new ResourceTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetResourceType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourceTypesAndLogError()
		{
			IDatabase database;
			ResourceTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new ResourceTypeModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetResourceTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
