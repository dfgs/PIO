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
	public class LocationCheckerModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldReturnTrueWhenWorkerIsInBuilding()
		{
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;

			workerModule = new MockedWorkerModule(false, new Worker() {  WorkerID=1, PlanetID = 5, X =10,Y=10});
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });
			
			module = new LocationCheckerModule(NullLogger.Instance,workerModule,buildingModule);

			Assert.IsTrue(module.WorkerIsInBuilding(1,3));
		}
		[TestMethod]
		public void ShouldReturnFalseWhenWorkerIsNotInBuilding()
		{
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;


			// different planet
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 6, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });
			module = new LocationCheckerModule(NullLogger.Instance, workerModule, buildingModule );
			Assert.IsFalse(module.WorkerIsInBuilding(1, 3));
			
			// different X
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 5, X = 9, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });
			module = new LocationCheckerModule(NullLogger.Instance, workerModule, buildingModule);
			Assert.IsFalse(module.WorkerIsInBuilding(1, 3));
			
			// different Y
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 5, X = 10, Y = 9 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });
			module = new LocationCheckerModule(NullLogger.Instance, workerModule, buildingModule);
			Assert.IsFalse(module.WorkerIsInBuilding(1, 3));
		}

		[TestMethod]
		public void WorkerIsInBuildingShouldThrowExceptionAndLogErrorWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 10, PlanetID = 5, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });

			logger = new MemoryLogger();
			module = new LocationCheckerModule(logger, workerModule, buildingModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.WorkerIsInBuilding(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}
		[TestMethod]
		public void WorkerIsInBuildingShouldThrowExceptionAndLogErrorWhenFactoryDoesntExists()
		{
			MemoryLogger logger;
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 5, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });

			logger = new MemoryLogger();
			module = new LocationCheckerModule(logger, workerModule, buildingModule);
			Assert.ThrowsException< PIONotFoundException>(()=>module.WorkerIsInBuilding(1,2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void WorkerIsInBuildingShouldThrowExceptionAndLogErrorWhenBuildingDoesntExists()
		{
			MemoryLogger logger;
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 5, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 30, PlanetID = 5, X = 10, Y = 10 });

			logger = new MemoryLogger();
			module = new LocationCheckerModule(logger, workerModule, buildingModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.WorkerIsInBuilding(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}


		[TestMethod]
		public void WorkerIsInBuildingShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			LocationCheckerModule module;
			IWorkerModule workerModule;
			IBuildingModule buildingModule;

			//
			workerModule = new MockedWorkerModule(true, new Worker() { WorkerID = 1, PlanetID = 5, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });

			logger = new MemoryLogger();
			module = new LocationCheckerModule(logger, workerModule, buildingModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.WorkerIsInBuilding(1, 3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

			//
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 5, X = 10, Y = 10 });
			buildingModule = new MockedBuildingModule(true, new Building() { BuildingID = 3, PlanetID = 5, X = 10, Y = 10 });

			logger = new MemoryLogger();
			module = new LocationCheckerModule(logger, workerModule, buildingModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.WorkerIsInBuilding(1, 3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));


			

		}












	}
}
