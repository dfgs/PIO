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
	public class ResourceModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetResource()
		{
			IDatabase database;
			ResourceModule module;
			Resource result;

			database = new MockedDatabase(false, 1);
			module = new ResourceModule(NullLogger.Instance, database);
			result = module.GetResource(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetResources()
		{
			IDatabase database;
			ResourceModule module;
			Resource[] results;

			database = new MockedDatabase(false, 3);
			module = new ResourceModule(NullLogger.Instance, database);
			results = module.GetResources().ToArray();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetResourceAndLogError()
		{
			IDatabase database;
			ResourceModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new ResourceModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetResource(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourcesAndLogError()
		{
			IDatabase database;
			ResourceModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new ResourceModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetResources());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
