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
	public class PlanetModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetPlanet()
		{
			IDatabase database;
			PlanetModule module;
			Planet result;

			database = new MockedDatabase(false, 1);
			module = new PlanetModule(NullLogger.Instance, database);
			result = module.GetPlanet(1);
			Assert.IsNotNull(result);
		}
		[TestMethod]
		public void ShouldGetPlanets()
		{
			IDatabase database;
			PlanetModule module;
			Planet[] results;

			database = new MockedDatabase(false, 3);
			module = new PlanetModule(NullLogger.Instance, database);
			results = module.GetPlanets().ToArray();
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
			}
		}
		[TestMethod]
		public void ShouldNotGetPlanetAndLogError()
		{
			IDatabase database;
			PlanetModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true,1);
			module = new PlanetModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetPlanet(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPlanetsAndLogError()
		{
			IDatabase database;
			PlanetModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase(true, 3);
			module = new PlanetModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetPlanets());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
