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
	public class FarmModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetFarm()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			Farm result;

			database = new MockedDatabase<Farm>(false, 1, (t) => new Farm() { FarmID = t });
			module = new FarmModule(NullLogger.Instance, database);
			result = module.GetFarm(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.FarmID);
		}
		[TestMethod]
		public void ShouldGetFarmUsingCoordinate()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			Farm result;

			database = new MockedDatabase<Farm>(false, 1, (t) => new Farm() { FarmID = t });
			module = new FarmModule(NullLogger.Instance, database);
			result = module.GetFarm(1, 3,4);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.FarmID);
		}
		[TestMethod]
		public void ShouldNotGetFarmUsingCoordinate()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			Farm result;

			database = new MockedDatabase<Farm>(false, 0, (t) => new Farm() { FarmID = t });
			module = new FarmModule(NullLogger.Instance, database);
			result = module.GetFarm(1, 3,4);
			Assert.IsNull(result);
		}
		[TestMethod]
		public void ShouldGetFarms()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			Farm[] results;

			database = new MockedDatabase<Farm>(false, 3, (t) => new Farm() { FarmID = t });
			module = new FarmModule(NullLogger.Instance, database);
			results = module.GetFarms(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].FarmID);
			}
		}
		[TestMethod]
		public void ShouldNotGetFarmAndLogError()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Farm>(true,1, (t) => new Farm() { FarmID = t });
			module = new FarmModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFarm(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFarmUsingCoordinateAndLogError()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();
			database = new MockedDatabase<Farm>(true, 1, (t) => new Farm() { FarmID = t });
			module = new FarmModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFarm(1, 3,4));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFarmsAndLogError()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Farm>(true, 3, (t) => new Farm() { FarmID = t });
			module = new FarmModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetFarms(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldCreateFarm()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			Farm result;

			database = new MockedDatabase<Farm>(false, 1, (t) => new Farm() { FarmID = t,BuildingID=1,X=10,Y=10,RemainingBuildSteps=100,FarmTypeID=FarmTypeIDs.Forest });
			module = new FarmModule(NullLogger.Instance, database);
			result = module.CreateFarm(1,10,10,100, BuildingTypeIDs.Sawmill, FarmTypeIDs.Forest);
			Assert.IsNotNull(result);
			
			
			Assert.AreEqual(2, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateFarmAndLogError()
		{
			MockedDatabase<Farm> database;
			FarmModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Farm>(true, 1, (t) => new Farm() { FarmID = t });
			module = new FarmModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateFarm(1,10,10,100, BuildingTypeIDs.Sawmill, FarmTypeIDs.Forest));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
	}
}
