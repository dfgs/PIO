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
	public class MaterialModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetMaterial()
		{
			MockedDatabase<Material> database;
			MaterialModule module;
			Material result;

			database = new MockedDatabase<Material>(false, 1, (t) => new Material() { MaterialID = t });
			module = new MaterialModule(NullLogger.Instance, database);
			result = module.GetMaterial(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.MaterialID);
		}
		[TestMethod]
		public void ShouldGetMaterials()
		{
			MockedDatabase<Material> database;
			MaterialModule module;
			Material[] results;

			database = new MockedDatabase<Material>(false, 3, (t) => new Material() { MaterialID = t, BuildingTypeID = BuildingTypeIDs.Sawmill });
			module = new MaterialModule(NullLogger.Instance, database);
			results = module.GetMaterials(BuildingTypeIDs.Sawmill);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].MaterialID);
				Assert.AreEqual(BuildingTypeIDs.Sawmill, results[t].BuildingTypeID);
			}
		}
		[TestMethod]
		public void ShouldNotGetMaterialAndLogError()
		{
			MockedDatabase<Material> database;
			MaterialModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Material>(true,1, (t) => new Material() { MaterialID = t });
			module = new MaterialModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetMaterial(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetMaterialsAndLogError()
		{
			MockedDatabase<Material> database;
			MaterialModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Material>(true, 3, (t) => new Material() { MaterialID = t });
			module = new MaterialModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetMaterials(BuildingTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


	}
}
