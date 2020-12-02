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
	public class PlanetModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetPlanet()
		{
			MockedDatabase<Planet> database;
			PlanetModule module;
			Planet result;

			database = new MockedDatabase<Planet>(false, 1, (t) => new Planet() { PlanetID = t });
			module = new PlanetModule(NullLogger.Instance, database);
			result = module.GetPlanet(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.PlanetID);
		}
		[TestMethod]
		public void ShouldGetPlanets()
		{
			MockedDatabase<Planet> database;
			PlanetModule module;
			Planet[] results;

			database = new MockedDatabase<Planet>(false, 3, (t) => new Planet() { PlanetID = t });
			module = new PlanetModule(NullLogger.Instance, database);
			results = module.GetPlanets();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].PlanetID);
			}
		}
		[TestMethod]
		public void ShouldNotGetPlanetAndLogError()
		{
			MockedDatabase<Planet> database;
			PlanetModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Planet>(true,1, (t) => new Planet() { PlanetID = t });
			module = new PlanetModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetPlanet(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPlanetsAndLogError()
		{
			MockedDatabase<Planet> database;
			PlanetModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Planet>(true, 3, (t) => new Planet() { PlanetID = t });
			module = new PlanetModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetPlanets());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


	}
}
