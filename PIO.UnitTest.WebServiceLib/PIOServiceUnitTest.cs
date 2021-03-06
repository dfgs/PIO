﻿using System;
using System.Linq;
using System.ServiceModel;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIO.UnitTest.WebServiceLib.Mocks;
using PIO.WebServiceLib;

namespace PIO.UnitTest.WebServiceLib
{
	[TestClass]
	public class PIOServiceUnitTest
	{
		[TestMethod]
		public void ShouldGetPhrase()
		{
			PIOService service;
			Phrase result;
			IPhraseModule subModule;

			subModule = Substitute.For<IPhraseModule>();
			subModule.GetPhrase(Arg.Any<string>(), Arg.Any<string>()).Returns(new Phrase() { PhraseID = 1 });

			service = new PIOService(NullLogger.Instance,  subModule, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetPhrase("Key","FR");
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PhraseID);
		}
		[TestMethod]
		public void ShouldGetPhrases()
		{
			PIOService service;
			Phrase[] result;
			IPhraseModule subModule;

			subModule = Substitute.For<IPhraseModule>();
			subModule.GetPhrases(Arg.Any<string>()).Returns(new Phrase[] { new Phrase() { PhraseID = 1 }, new Phrase() { PhraseID = 2 }, new Phrase() { PhraseID = 3 } });

			service = new PIOService(NullLogger.Instance,  subModule, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetPhrases("EN");
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetPhraseAndLogError()
		{
			PIOService service;
			MemoryLogger logger;
			IPhraseModule subModule;

			logger = new MemoryLogger();

			subModule = Substitute.For<IPhraseModule>();
			subModule.GetPhrase(Arg.Any<string>(),Arg.Any<string>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			service = new PIOService(logger,  subModule, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetPhrase("Key","EN"));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPhrasesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;
			IPhraseModule subModule;

			logger = new MemoryLogger();

			subModule = Substitute.For<IPhraseModule>();
			subModule.GetPhrases(Arg.Any<string>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			service = new PIOService(logger, subModule, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetPhrases("EN"));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}




		[TestMethod]
		public void ShouldGetPlanet()
		{
			PIOService service;
			Planet result;
			IPlanetModule subModule;

			subModule = Substitute.For<IPlanetModule>();
			subModule.GetPlanet(Arg.Any<int>()).Returns( new Planet() {PlanetID=1 });

			service = new PIOService(NullLogger.Instance, null, subModule, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null);
			result = service.GetPlanet(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
		}
		[TestMethod]
		public void ShouldGetPlanets()
		{
			PIOService service;
			Planet[] result;
			IPlanetModule subModule;

			subModule = Substitute.For<IPlanetModule>();
			subModule.GetPlanets().Returns(new Planet[] { new Planet() { PlanetID = 1 }, new Planet() { PlanetID = 2 }, new Planet() { PlanetID = 3 } });

			service = new PIOService(NullLogger.Instance, null, subModule,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null);
			result = service.GetPlanets();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item!=null));
		}

		[TestMethod]
		public void ShouldNotGetPlanetAndLogError()
		{
			PIOService service;
			MemoryLogger logger;
			IPlanetModule subModule;

			logger = new MemoryLogger();

			subModule = Substitute.For<IPlanetModule>();
			subModule.GetPlanet(Arg.Any<int>()).Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			service = new PIOService(logger, null, subModule, null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.GetPlanet(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPlanetsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;
			IPlanetModule subModule;

			logger = new MemoryLogger();

			subModule = Substitute.For<IPlanetModule>();
			subModule.GetPlanets().Returns((id) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			service = new PIOService(logger, null, subModule, null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.GetPlanets());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}








		[TestMethod]
		public void ShouldGetBuilding()
		{
			PIOService service;
			Building result;

			service = new PIOService(NullLogger.Instance, null, null, null, new MockedBuildingModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetBuilding(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildingID);
		}
		[TestMethod]
		public void ShouldGetBuildingUsingCoordinate()
		{
			PIOService service;
			Building result;

			service = new PIOService(NullLogger.Instance, null, null, null,  new MockedBuildingModule(3, false),    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetBuildingAtPos(1, 1, 1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.BuildingID);
		}
		[TestMethod]
		public void ShouldGetBuildings()
		{
			PIOService service;
			Building[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  new MockedBuildingModule(3, false),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetBuildings(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetBuildingAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, new MockedBuildingModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetBuilding(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingUsingCoordindateAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null,   new MockedBuildingModule(3, true),    null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetBuildingAtPos(1, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null,   new MockedBuildingModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetBuildings(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}





		[TestMethod]
		public void ShouldGetCell()
		{
			PIOService service;
			Cell result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedCellModule(3, false),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetCell(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.CellID);
		}
		[TestMethod]
		public void ShouldGetCellUsingCoordinate()
		{
			PIOService service;
			Cell result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedCellModule(3, false), null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetCellAtPos(1, 1, 1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.CellID);
		}
		[TestMethod]
		public void ShouldGetCells()
		{
			PIOService service;
			Cell[] result;

			service = new PIOService(NullLogger.Instance, null, null,  new MockedCellModule(3, false), null,   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null, null, null, null);
			result = service.GetCells(1,1,1,3,3);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetCellAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null,  new MockedCellModule(3, true), null, null, null,null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetCell(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetCellUsingCoordindateAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, new MockedCellModule(3, true), null, null, null,  null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetCellAtPos(1, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetCellsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, new MockedCellModule(3, true), null, null, null,  null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetCells(1,1,1,3,3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}




		[TestMethod]
		public void ShouldGetWorker()
		{
			PIOService service;
			Worker result;

			service = new PIOService(NullLogger.Instance, null, null, null, null,  new MockedWorkerModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
		}
		[TestMethod]
		public void ShouldGetWorkers()
		{
			PIOService service;
			Worker[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null,  new MockedWorkerModule(3, false),   null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetWorkers(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldGetAllWorkers()
		{
			PIOService service;
			Worker[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null,  new MockedWorkerModule(3, false),  null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetAllWorkers();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldNotGetWorkerAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null,  new MockedWorkerModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWorkersAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null,null, null,   new MockedWorkerModule(3, true),  null, null, null,null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetWorkers(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetAllWorkersAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null,   new MockedWorkerModule(3, true), null, null, null, null, null, null, null, null, null, null ,null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetAllWorkers());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetStack()
		{
			PIOService service;
			Stack result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,  new MockedStackModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetStack(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.StackID);
		}
		[TestMethod]
		public void ShouldFindStack()
		{
			PIOService service;
			Stack result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, new MockedStackModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.FindStack(1, ResourceTypeIDs.Coal);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.StackID);
		}
		[TestMethod]
		public void ShouldGetStacks()
		{
			PIOService service;
			Stack[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,  new MockedStackModule(3, false), null, null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetStacks(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldGetStackQuantity()
		{
			PIOService service;
			int result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,   new MockedStackModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetStackQuantity(1,ResourceTypeIDs.Coal);
			Assert.AreEqual(3, result);
		}

		[TestMethod]
		public void ShouldNotGetStackAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,   new MockedStackModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetStack(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotFindStackAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  new MockedStackModule(3, true),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.FindStack(1,ResourceTypeIDs.Coal));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStacksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,   new MockedStackModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetStacks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStackQuantityAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, new MockedStackModule(3, true),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetStackQuantity(1,ResourceTypeIDs.Coal));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetResourceType()
		{
			PIOService service;
			ResourceType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, new MockedResourceTypeModule(3, false), null, null, null, null, null, null, null, null, null, null, null,  null, null, null, null, null);
			result = service.GetResourceType(ResourceTypeIDs.Wood);
			Assert.IsNotNull(result);
			Assert.AreEqual(ResourceTypeIDs.Wood, result.ResourceTypeID);
		}
		[TestMethod]
		public void ShouldGetResourceTypes()
		{
			PIOService service;
			ResourceType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,   new MockedResourceTypeModule(3, false),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetResourceTypes();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetResourceTypeAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null,   new MockedResourceTypeModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetResourceType(ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourceTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null,   new MockedResourceTypeModule(3, true), null, null, null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetResourceTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}




	
		[TestMethod]
		public void ShouldGetBuildingType()
		{
			PIOService service;
			BuildingType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null,  new MockedBuildingTypeModule(3, false),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetBuildingType(BuildingTypeIDs.Forest);
			Assert.IsNotNull(result);
			Assert.AreEqual(BuildingTypeIDs.Forest, result.BuildingTypeID);
		}
		[TestMethod]
		public void ShouldGetBuildingTypes()
		{
			PIOService service;
			BuildingType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null, new MockedBuildingTypeModule(3, false),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetBuildingTypes();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetBuildingTypeAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null,   new MockedBuildingTypeModule(3, true),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetBuildingType(BuildingTypeIDs.Forest));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetBuildingTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, new MockedBuildingTypeModule(3, true),  null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetBuildingTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetTaskType()
		{
			PIOService service;
			TaskType result;

			service = new PIOService(NullLogger.Instance, null,null, null, null, null, null, null, null, new MockedTaskTypeModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetTaskType(TaskTypeIDs.MoveTo);
			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void ShouldGetTaskTypes()
		{
			PIOService service;
			TaskType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null, null, null, new MockedTaskTypeModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetTaskTypes();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetTaskTypeAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null, new MockedTaskTypeModule(3, true),null, null, null, null, null, null, null, null, null, null, null, null, null,null);

			Assert.ThrowsException<FaultException>(() => service.GetTaskType(TaskTypeIDs.MoveTo));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTaskTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null,  null, new MockedTaskTypeModule(3, true), null, null, null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTaskTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetMaterial()
		{
			PIOService service;
			Material result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null,   null, null, null, new MockedMaterialModule(3, false),   null, null, null, null, null, null, null, null, null,  null, null, null, null);
			result = service.GetMaterial(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.MaterialID);
		}
		[TestMethod]
		public void ShouldGetMaterials()
		{
			PIOService service;
			Material[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null,   null, null, null, new MockedMaterialModule(3, false), null, null, null, null, null, null, null, null, null,null, null, null, null);
			result = service.GetMaterials(BuildingTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetMaterialAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, new MockedMaterialModule(3, true),   null, null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMaterial(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetMaterialsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null, new MockedMaterialModule(3, true),  null,  null, null, null, null, null, null, null, null, null, null,null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMaterials(BuildingTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetIngredient()
		{
			PIOService service;
			Ingredient result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null, null, null, null, null, new MockedIngredientModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetIngredient(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.IngredientID);
		}
		[TestMethod]
		public void ShouldGetIngredients()
		{
			PIOService service;
			Ingredient[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,   null, null, null, null, new MockedIngredientModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetIngredients(BuildingTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetIngredientAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null,  null, null, new MockedIngredientModule(3, true), null, null, null, null, null, null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.GetIngredient(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetIngredientsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null,  null, null, null, new MockedIngredientModule(3, true),  null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetIngredients(BuildingTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetProduct()
		{
			PIOService service;
			Product result;

			service = new PIOService(NullLogger.Instance, null, null, null,null, null, null,   null, null, null, null, null, new MockedProductModule(3, false),  null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetProduct(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProductID);
		}
		[TestMethod]
		public void ShouldGetProducts()
		{
			PIOService service;
			Product[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null,  null, null, null, null, null, new MockedProductModule(3, false),   null, null, null, null, null, null, null, null,  null, null, null);
			result = service.GetProducts(BuildingTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetProductAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null, null, null, null, new MockedProductModule(3, true),null, null, null, null, null, null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.GetProduct(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetProductsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null,  new MockedProductModule(3, true), null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetProducts(BuildingTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetTask()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null, null, null, null, null, null, null, new MockedTaskModule(3, false), null, null, null, null, null, null, null, null, null, null);
			result = service.GetTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TaskID);
		}
		[TestMethod]
		public void ShouldGetTasks()
		{
			PIOService service;
			Task[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null,  null, null, null, null, null, null, new MockedTaskModule(3, false), null, null, null, null, null, null, null,  null, null, null);
			result = service.GetTasks(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}
		[TestMethod]
		public void ShouldGetLastTask()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null,  null, null, null,   null, null, null, null, null, null, new MockedTaskModule(3, false), null, null, null, null, null, null, null,  null, null, null);
			result = service.GetLastTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.TaskID);
		}
		[TestMethod]
		public void ShouldNotGetTaskAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null,  null, null, null, null, new MockedTaskModule(3, true), null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null,  null, null, null, null, new MockedTaskModule(3, true),  null, null, null, null, null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotGetLastTaskAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null,  null, null, null, null, null, null, null, new MockedTaskModule(3, true),  null, null, null, null, null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.GetLastTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}





		[TestMethod]
		public void ShouldGetHasEnoughResourcesToProduce()
		{
			PIOService service;
			bool result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null,  null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, true), null, null, null,  null, null, null, null, null);
			result = service.HasEnoughResourcesToProduce(1);
			Assert.IsTrue(result);

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, false), null, null, null, null, null, null, null, null);
			result = service.HasEnoughResourcesToProduce(1);
			Assert.IsFalse(result);

		}


		[TestMethod]
		public void ShouldNotGetHasEnoughResourcesToProduceAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null,   null, null, null, null, null, new MockedResourceCheckerModule(true,true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetMissingResourcesToProduce()
		{
			PIOService service;
			ResourceTypeIDs[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null,  null, null, null,  null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, true), null, null, null, null, null, null, null, null);
			result = service.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);

			service = new PIOService(NullLogger.Instance, null, null, null, null,  null, null, null, null,  null, null, null, null, null, null, new MockedResourceCheckerModule(false, false), null, null, null, null,  null, null, null, null);
			result = service.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Length);

		}


		[TestMethod]
		public void ShouldNotGetMissingResourcesToProduceAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(true, true), null, null, null, null, null, null,null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMissingResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}





		[TestMethod]
		public void ShouldGetHasEnoughResourcesToBuild()
		{
			PIOService service;
			bool result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null,null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, true), null, null, null, null, null, null, null, null);
			result = service.HasEnoughResourcesToBuild(1);
			Assert.IsTrue(result);

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null,  null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, false), null, null, null, null, null, null, null, null);
			result = service.HasEnoughResourcesToBuild(1);
			Assert.IsFalse(result);

		}


		[TestMethod]
		public void ShouldNotGetHasEnoughResourcesToBuildAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null,null, null, null, null, null, new MockedResourceCheckerModule(true, true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.HasEnoughResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetMissingResourcesToBuild()
		{
			PIOService service;
			ResourceTypeIDs[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, true), null, null, null, null, null, null, null, null);
			result = service.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null,  null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, false), null, null, null, null, null, null, null, null);
			result = service.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Length);

		}


		[TestMethod]
		public void ShouldNotGetMissingResourcesToBuildAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null, null, null, null, null, null, new MockedResourceCheckerModule(true, true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMissingResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}





		[TestMethod]
		public void ShouldGetWorkerIsInBuilding()
		{
			PIOService service;
			bool result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,  null,  null, null, null, null, null, null, null, null, null, new MockedLocationCheckerModule(false, true), null, null, null,  null, null, null, null);
			result = service.WorkerIsInBuilding(1, 2);
			Assert.IsTrue(result);

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,null,  null, null, null, null, null, null, null, null, null, new MockedLocationCheckerModule(false, false), null, null, null, null, null, null, null);
			result = service.WorkerIsInBuilding(1, 2);
			Assert.IsFalse(result);



		}


		[TestMethod]
		public void ShouldNotGetWorkerIsInBuildingAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, new MockedLocationCheckerModule(true, true), null, null, null, null, null,  null, null);

			Assert.ThrowsException<FaultException>(() => service.WorkerIsInBuilding(1, 2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


		[TestMethod]
		public void ShouldIdle()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, new MockedIdlerModule(false), null, null, null, null, null,  null);
			result = service.Idle(5,10);
			Assert.IsNotNull(result);
			Assert.AreEqual(5, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotIdleAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null,  null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, new MockedIdlerModule(true),  null, null, null, null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.Idle(1,10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}

		[TestMethod]
		public void ShouldProduce()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, null, new MockedProducerModule(false), null, null, null, null, null);
			result = service.Produce(10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotProduceAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null,  null, null, null, null, null, null, null, null, new MockedProducerModule(true), null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.Produce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}
		[TestMethod]
		public void ShouldHarvest()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null, new MockedHarvesterModule(false), null, null, null, null);
			result = service.Harvest(10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotHarvestAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedHarvesterModule(true), null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.Harvest(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldMove()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null,  null, null, null, null, null, null, null, null, null, null, null, null, new MockedMoverModule(false),  null, null, null);
			result = service.MoveTo(10,1,1);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotMoveAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, new MockedMoverModule(true), null, null, null);

			Assert.ThrowsException<FaultException>(() => service.MoveTo(10,1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}

		[TestMethod]
		public void ShouldMoveToBuilding()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, null, null, new MockedMoverModule(false), null,  null, null);
			result = service.MoveToBuilding(10, 1);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotMoveAndLogErrorToFactory()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, new MockedMoverModule(true), null, null,  null);

			Assert.ThrowsException<FaultException>(() => service.MoveToBuilding(10, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}


	
		[TestMethod]
		public void ShouldTake()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, null, null, null,  new MockedTakerModule(false), null, null);
			result = service.Take(10,  ResourceTypeIDs.Wood);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotTakeAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null,  null, null, null, null, null, null, null, null, null, null, new MockedTakerModule(true), null, null);

			Assert.ThrowsException<FaultException>(() => service.Take(10,  ResourceTypeIDs.Wood));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldStore()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null,  null, null, null, null, null, null, null, null, null, null, null, null, null,null,new MockedStorerModule(false), null);
			result = service.Store(10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotStoreAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedStorerModule(true),  null);

			Assert.ThrowsException<FaultException>(() => service.Store(10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == service.ModuleName)));
		}
		[TestMethod]
		public void ShouldCreateBuilding()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedBuilderModule(false));
			result = service.CreateBuilding(10, BuildingTypeIDs.Sawmill);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
			Assert.AreEqual(BuildingTypeIDs.Sawmill, result.BuildingTypeID);
		}


		[TestMethod]
		public void ShouldNotCreateBuildingAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,null,  null, null,null, new MockedBuilderModule(true));

			Assert.ThrowsException<FaultException>(() => service.CreateBuilding(10,  BuildingTypeIDs.Sawmill));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}

	


		[TestMethod]
		public void ShouldBuild()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null,  null, null, null,  null, null, null, null, null, null, null, null, null, null, null, null, null, new MockedBuilderModule(false));
			result = service.Build(10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotBuildAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger();
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null,  null,  null, null, null, null, null, null, null, null, null, null, null, null, new MockedBuilderModule(true));

			Assert.ThrowsException<FaultException>(() => service.Build(10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Error) && (item.ComponentName==service.ModuleName)));
		}

	





	}
}
