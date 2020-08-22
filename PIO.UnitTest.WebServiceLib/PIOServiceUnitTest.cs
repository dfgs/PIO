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


		[TestMethod]
		public void ShouldGetResourceType()
		{
			PIOService service;
			ResourceType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, new MockedResourceTypeModule(3, false), null, null, null, null);
			result = service.GetResourceType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ResourceTypeID);
		}
		[TestMethod]
		public void ShouldGetResourceTypes()
		{
			PIOService service;
			ResourceType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, new MockedResourceTypeModule(3, false),  null, null, null, null);
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

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, new MockedResourceTypeModule(3, true),  null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetResourceType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourceTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, new MockedResourceTypeModule(3, true), null, null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetResourceTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetFactoryType()
		{
			PIOService service;
			FactoryType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, new MockedFactoryTypeModule(3, false), null, null, null);
			result = service.GetFactoryType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryTypeID);
		}
		[TestMethod]
		public void ShouldGetFactoryTypes()
		{
			PIOService service;
			FactoryType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, new MockedFactoryTypeModule(3, false), null, null, null);
			result = service.GetFactoryTypes();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetFactoryTypeAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, new MockedFactoryTypeModule(3, true),  null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetFactoryType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, new MockedFactoryTypeModule(3, true),  null, null, null);

			Assert.ThrowsException<Exception>(() => service.GetFactoryTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}




		[TestMethod]
		public void ShouldGetMaterial()
		{
			PIOService service;
			Material result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, new MockedMaterialModule(3, false),   null, null);
			result = service.GetMaterial(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.MaterialID);
		}
		[TestMethod]
		public void ShouldGetMaterials()
		{
			PIOService service;
			Material[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, new MockedMaterialModule(3, false), null, null);
			result = service.GetMaterials(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetMaterialAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, new MockedMaterialModule(3, true),   null, null);

			Assert.ThrowsException<Exception>(() => service.GetMaterial(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetMaterialsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, new MockedMaterialModule(3, true),  null, null);

			Assert.ThrowsException<Exception>(() => service.GetMaterials(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetIngredient()
		{
			PIOService service;
			Ingredient result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, new MockedIngredientModule(3, false), null);
			result = service.GetIngredient(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.IngredientID);
		}
		[TestMethod]
		public void ShouldGetIngredients()
		{
			PIOService service;
			Ingredient[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, new MockedIngredientModule(3, false), null);
			result = service.GetIngredients(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetIngredientAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, new MockedIngredientModule(3, true), null);

			Assert.ThrowsException<Exception>(() => service.GetIngredient(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetIngredientsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, new MockedIngredientModule(3, true),  null);

			Assert.ThrowsException<Exception>(() => service.GetIngredients(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}




		[TestMethod]
		public void ShouldGetTask()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, new MockedTaskModule(3, false));
			result = service.GetTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TaskID);
		}
		[TestMethod]
		public void ShouldGetTasks()
		{
			PIOService service;
			Task[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, new MockedTaskModule(3, false));
			result = service.GetTasks(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetTaskAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, new MockedTaskModule(3, true));

			Assert.ThrowsException<Exception>(() => service.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, new MockedTaskModule(3, true));

			Assert.ThrowsException<Exception>(() => service.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



	}
}
