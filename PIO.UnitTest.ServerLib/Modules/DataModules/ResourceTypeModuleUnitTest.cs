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
	public class ResourceTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetResourceType()
		{
			MockedDatabase<ResourceType> database;
			ResourceTypeModule module;
			ResourceType result;

			database = new MockedDatabase<ResourceType>(false, 1, (t) => new ResourceType() { ResourceTypeID = (ResourceTypeIDs)t });
			module = new ResourceTypeModule(NullLogger.Instance, database);
			result = module.GetResourceType(ResourceTypeIDs.Tree);
			Assert.IsNotNull(result);
			Assert.AreEqual(ResourceTypeIDs.Tree, result.ResourceTypeID);
		}
		[TestMethod]
		public void ShouldGetResourceTypes()
		{
			MockedDatabase<ResourceType> database;
			ResourceTypeModule module;
			ResourceType[] results;

			database = new MockedDatabase<ResourceType>(false, 3, (t) => new ResourceType() { ResourceTypeID = (ResourceTypeIDs)t });
			module = new ResourceTypeModule(NullLogger.Instance, database);
			results = module.GetResourceTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual((ResourceTypeIDs)t, results[t].ResourceTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetResourceTypeAndLogError()
		{
			MockedDatabase<ResourceType> database;
			ResourceTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<ResourceType>(true,1, (t) => new ResourceType() { ResourceTypeID = (ResourceTypeIDs)t });
			module = new ResourceTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetResourceType(ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourceTypesAndLogError()
		{
			MockedDatabase<ResourceType> database;
			ResourceTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<ResourceType>(true, 3, (t) => new ResourceType() { ResourceTypeID = (ResourceTypeIDs)t });
			module = new ResourceTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetResourceTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


	}
}
