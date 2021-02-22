using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class PlanetGeneratorModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldReturnFalseWhenPlanetIsNotGenerated()
		{
			PlanetGeneratorModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();

			module = new PlanetGeneratorModule(logger,null,null,null,null,null,null,null,null,null,null );

			Assert.IsFalse(module.Generate());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}






	}
}
