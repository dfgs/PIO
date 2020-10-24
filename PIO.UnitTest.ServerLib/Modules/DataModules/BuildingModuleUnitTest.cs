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


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Building>(true,1, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuilding(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingsAndLogError()
		{
			MockedDatabase<Building> database;
			BuildingModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Building>(true, 3, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildings(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
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

			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Building>(true, 3, (t) => new Building() { BuildingID = t });
			module = new BuildingModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.SetHealthPoints(0,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}*/
	}
}
