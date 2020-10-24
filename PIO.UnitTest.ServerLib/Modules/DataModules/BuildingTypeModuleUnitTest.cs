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
	public class BuildingTypeModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetBuildingType()
		{
			MockedDatabase<BuildingType> database;
			BuildingTypeModule module;
			BuildingType result;

			database = new MockedDatabase<BuildingType>(false, 1, (t) => new BuildingType() { BuildingTypeID = (BuildingTypeIDs)t });
			module = new BuildingTypeModule(NullLogger.Instance, database);
			result = module.GetBuildingType(BuildingTypeIDs.Factory);
			Assert.IsNotNull(result);
			Assert.AreEqual(BuildingTypeIDs.Factory, result.BuildingTypeID);
		}
		[TestMethod]
		public void ShouldGetBuildingTypes()
		{
			MockedDatabase<BuildingType> database;
			BuildingTypeModule module;
			BuildingType[] results;

			database = new MockedDatabase<BuildingType>(false, 3, (t) => new BuildingType() { BuildingTypeID = (BuildingTypeIDs)t });
			module = new BuildingTypeModule(NullLogger.Instance, database);
			results = module.GetBuildingTypes();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual((BuildingTypeIDs)t, results[t].BuildingTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetBuildingTypeAndLogError()
		{
			MockedDatabase<BuildingType> database;
			BuildingTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<BuildingType>(true,1, (t) => new BuildingType() { BuildingTypeID = (BuildingTypeIDs)t });
			module = new BuildingTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildingType(BuildingTypeIDs.Factory));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingTypesAndLogError()
		{
			MockedDatabase<BuildingType> database;
			BuildingTypeModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<BuildingType>(true, 3, (t) => new BuildingType() { BuildingTypeID = (BuildingTypeIDs)t });
			module = new BuildingTypeModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetBuildingTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
