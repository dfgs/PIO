using System;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.Models;
using PIO.UnitTest.WebServiceLib.Mocks;
using PIO.WebServiceLib;

namespace PIO.UnitTest.WebServiceLib
{
	[TestClass]
	public class PIOServiceUnitTest
	{

		[TestMethod]
		public void ShouldGetPlanet()
		{
			PIOService service;
			Planet result;

			service = new PIOService(NullLogger.Instance, new MockedPlanetModule(3, false), null, null, null, null, null, null, null, null);
			result = service.GetPlanet(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
		}
		[TestMethod]
		public void ShouldGetPlanets()
		{
			PIOService service;
			Planet[] result;

			service = new PIOService(NullLogger.Instance, new MockedPlanetModule(3, false), null, null, null, null, null, null, null, null);
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

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger,new MockedPlanetModule(3, true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetPlanet(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPlanetsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, new MockedPlanetModule(3, true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetPlanets());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetFactory()
		{
			PIOService service;
			Factory result;

			service = new PIOService(NullLogger.Instance, null, new MockedFactoryModule(3, false), null, null, null, null, null, null, null);
			result = service.GetFactory(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);
		}
		[TestMethod]
		public void ShouldGetFactories()
		{
			PIOService service;
			Factory[] result;

			service = new PIOService(NullLogger.Instance, null, new MockedFactoryModule(3, false), null, null, null, null, null, null, null);
			result = service.GetFactories(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetFactoryAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger,null, new MockedFactoryModule(3, true),  null, null, null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetFactory(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoriesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null,new MockedFactoryModule(3, true), null, null, null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetFactories(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetWorker()
		{
			PIOService service;
			Worker result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedWorkerModule(3, false), null, null, null, null, null,  null);
			result = service.GetWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
		}
		[TestMethod]
		public void ShouldGetWorkers()
		{
			PIOService service;
			Worker[] result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedWorkerModule(3, false), null, null, null, null, null,  null);
			result = service.GetWorkers(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetWorkerAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, new MockedWorkerModule(3, true), null, null, null, null,  null, null);

			Assert.ThrowsException<Exception>(() => service.GetWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWorkersAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, new MockedWorkerModule(3, true), null, null, null, null,  null, null);

			Assert.ThrowsException<Exception>(() => service.GetWorkers(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetStack()
		{
			PIOService service;
			Stack result;

			service = new PIOService(NullLogger.Instance, null, null, null, new MockedStackModule(3, false), null, null, null, null, null);
			result = service.GetStack(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.StackID);
		}
		[TestMethod]
		public void ShouldGetStacks()
		{
			PIOService service;
			Stack[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  new MockedStackModule(3, false),  null, null, null, null,null);
			result = service.GetStacks(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetStackAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, new MockedStackModule(3, true),  null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetStack(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStacksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, new MockedStackModule(3, true),  null, null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetStacks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		/*
		
		Stack GetStack(int StackID);
		[TestMethod]
		Stack[] GetStacks(int FactoryID);
		[TestMethod]
		ResourceType GetResourceType(int ResourceTypeID);
		[TestMethod]
		ResourceType[] GetResourceTypes();
		[TestMethod]
		FactoryType GetFactoryType(int FactoryTypeID);
		[TestMethod]
		FactoryType[] GetFactoryTypes();
		[TestMethod]
		Material GetMaterial(int MaterialID);
		[TestMethod]
		Material[] GetMaterials(int FactoryTypeID);
		[TestMethod]
		Ingredient GetIngredient(int IngredientID);
		[TestMethod]
		Ingredient[] GetIngredients(int FactoryTypeID);
		[TestMethod]
		Task GetTask(int TaskID);
		[TestMethod]
		Task[] GetTasks(int FactoryID);

		[TestMethod]
		public void TestMethod1()
		{
		}*/


	}
}
