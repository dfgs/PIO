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
	public class FarmTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetFarmType()
		{
			MockedDatabase<FarmType> database;
			FarmTypeModule module;
			FarmType result;

			database = new MockedDatabase<FarmType>(false, 1, (t) => new FarmType() { FarmTypeID = (FarmTypeIDs)t });
			module = new FarmTypeModule(NullLogger.Instance, database);
			result = module.GetFarmType(FarmTypeIDs.Forest);
			Assert.IsNotNull(result);
			Assert.AreEqual(FarmTypeIDs.Forest, result.FarmTypeID);
		}
		[TestMethod]
		public void ShouldGetFarmTypes()
		{
			MockedDatabase<FarmType> database;
			FarmTypeModule module;
			FarmType[] results;

			database = new MockedDatabase<FarmType>(false, 3, (t) => new FarmType() { FarmTypeID = (FarmTypeIDs)t });
			module = new FarmTypeModule(NullLogger.Instance, database);
			results = module.GetFarmTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual((FarmTypeIDs)t, results[t].FarmTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetFarmTypeAndLogError()
		{
			MockedDatabase<FarmType> database;
			FarmTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<FarmType>(true,1, (t) => new FarmType() { FarmTypeID = (FarmTypeIDs)t });
			module = new FarmTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFarmType(FarmTypeIDs.Forest));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFarmTypesAndLogError()
		{
			MockedDatabase<FarmType> database;
			FarmTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<FarmType>(true, 3, (t) => new FarmType() { FarmTypeID = (FarmTypeIDs)t });
			module = new FarmTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFarmTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


	}
}
