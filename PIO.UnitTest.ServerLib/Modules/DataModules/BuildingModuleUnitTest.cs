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
	public class BuildingModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetBuilding()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			Building result;

			database = new MockedDatabase<Building>(false, 1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(NullLogger.Instance, database);
			result = module.GetBuilding(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.BuildingID);
		}
		[TestMethod]
		public void ShouldGetBuildingUsingCoordinates()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			Building result;

			database = new MockedDatabase<Building>(false, 1, (t) => new Building() { BuildingID = t, X = 3, Y = 4 });
			module = new BuildingModule(NullLogger.Instance, database);
			result = module.GetBuilding(1,3,4);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.BuildingID);
			Assert.AreEqual(3, result.X);
			Assert.AreEqual(4, result.Y);
		}
		[TestMethod]
		public void ShouldNotGetBuildingUsingCoordinates()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			Building result;

			database = new MockedDatabase<Building>(false, 0, (t) => new Building() { BuildingID = t, X = 3, Y = 4 });
			module = new BuildingModule(NullLogger.Instance, database);
			result = module.GetBuilding(1, 4,3);
			Assert.IsNull(result);
		}
		[TestMethod]
		public void ShouldGetBuildings()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			Building[] results;

			database = new MockedDatabase<Building>(false, 3, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(NullLogger.Instance, database);
			results = module.GetBuildings(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].BuildingID);
			}
		}
		[TestMethod]
		public void ShouldNotGetBuildingAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Building>(true,1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuilding(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingUsingCoordinateAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();
			database = new MockedDatabase<Building>(true, 1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuilding(1, 3,4));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingsAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Building>(true, 3, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildings(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateBuilding()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			Building result;

			database = new MockedDatabase<Building>(false, 1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(NullLogger.Instance, database);
			result = module.CreateBuilding(1,7,9, BuildingTypeIDs.Factory,5);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
			Assert.AreEqual(BuildingTypeIDs.Factory, result.BuildingTypeID);
			Assert.AreEqual(5, result.RemainingBuildSteps);
			Assert.AreEqual(7, result.X);
			Assert.AreEqual(9, result.Y);
			Assert.AreEqual(0, result.HealthPoints);

			Assert.AreEqual(1, database.InsertedCount);
		}

		[TestMethod]
		public void ShouldNotCreateBuildingAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Building>(true, 1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateBuilding(1,7,9, BuildingTypeIDs.Factory, 5));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

		/*[TestMethod]
		public void ShouldSetHealthPoints()
		{
			MockedDatabase<Building> database;
			BuildingModule module;

			database = new MockedDatabase<Building>(false, 1, (t) => new Building() { BuildingID = t});
			module = new BuildingModule(NullLogger.Instance, database);
			module.SetHealthPoints(0,10);
			Assert.AreEqual(1, database.UpdatedCount);
		}
		[TestMethod]
		public void ShouldNotSetHealthPointsAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();
			database = new MockedDatabase<Building>(true, 3, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.SetHealthPoints(0,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}*/
	}
}
