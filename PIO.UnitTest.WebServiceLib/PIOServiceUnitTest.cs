using System;
using System.Linq;
using System.ServiceModel;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIO.Models;
using PIO.Models.Exceptions;
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

			service = new PIOService(NullLogger.Instance, new MockedPlanetModule(3, false), null, null,null, null, null, null, null, null, null, null, null, null);
			result = service.GetPlanet(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.PlanetID);
		}
		[TestMethod]
		public void ShouldGetPlanets()
		{
			PIOService service;
			Planet[] result;

			service = new PIOService(NullLogger.Instance, new MockedPlanetModule(3, false), null, null, null, null, null, null, null, null, null, null, null, null);
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
			service = new PIOService(logger,new MockedPlanetModule(3, true), null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetPlanet(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetPlanetsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, new MockedPlanetModule(3, true), null, null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetPlanets());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetFactory()
		{
			PIOService service;
			Factory result;

			service = new PIOService(NullLogger.Instance, null, new MockedFactoryModule(3, false), null, null, null, null, null, null, null, null, null, null, null);
			result = service.GetFactory(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);
		}
		[TestMethod]
		public void ShouldGetFactories()
		{
			PIOService service;
			Factory[] result;

			service = new PIOService(NullLogger.Instance, null, new MockedFactoryModule(3, false), null, null, null, null, null, null, null, null, null, null, null);
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
			service = new PIOService(logger,null, new MockedFactoryModule(3, true),  null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetFactory(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoriesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null,new MockedFactoryModule(3, true), null, null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetFactories(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetWorker()
		{
			PIOService service;
			Worker result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedWorkerModule(3, false), null, null, null, null, null, null, null, null, null, null);
			result = service.GetWorker(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
		}
		[TestMethod]
		public void ShouldGetWorkers()
		{
			PIOService service;
			Worker[] result;

			service = new PIOService(NullLogger.Instance, null, null, new MockedWorkerModule(3, false), null, null, null, null, null, null, null, null, null, null);
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
			service = new PIOService(logger, null, null, new MockedWorkerModule(3, true), null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetWorker(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetWorkersAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, new MockedWorkerModule(3, true), null, null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetWorkers(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetStack()
		{
			PIOService service;
			Stack result;

			service = new PIOService(NullLogger.Instance, null, null, null, new MockedStackModule(3, false), null, null, null, null, null, null, null, null, null);
			result = service.GetStack(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.StackID);
		}
		[TestMethod]
		public void ShouldGetStacks()
		{
			PIOService service;
			Stack[] result;

			service = new PIOService(NullLogger.Instance, null, null, null,  new MockedStackModule(3, false), null, null, null, null, null, null, null, null, null);
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
			service = new PIOService(logger, null, null, null, new MockedStackModule(3, true), null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetStack(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetStacksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, new MockedStackModule(3, true), null, null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetStacks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetResourceType()
		{
			PIOService service;
			ResourceType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, new MockedResourceTypeModule(3, false), null, null, null, null, null, null, null, null);
			result = service.GetResourceType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ResourceTypeID);
		}
		[TestMethod]
		public void ShouldGetResourceTypes()
		{
			PIOService service;
			ResourceType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, new MockedResourceTypeModule(3, false), null, null, null, null, null, null, null, null);
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
			service = new PIOService(logger, null, null, null, null, new MockedResourceTypeModule(3, true),  null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetResourceType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetResourceTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, new MockedResourceTypeModule(3, true), null, null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetResourceTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetFactoryType()
		{
			PIOService service;
			FactoryType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, new MockedFactoryTypeModule(3, false), null, null, null, null, null, null, null);
			result = service.GetFactoryType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryTypeID);
		}
		[TestMethod]
		public void ShouldGetFactoryTypes()
		{
			PIOService service;
			FactoryType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, new MockedFactoryTypeModule(3, false), null, null, null, null, null, null, null);
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
			service = new PIOService(logger, null, null, null, null, null, new MockedFactoryTypeModule(3, true), null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetFactoryType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetFactoryTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, new MockedFactoryTypeModule(3, true), null, null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetFactoryTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetTaskType()
		{
			PIOService service;
			TaskType result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, new MockedTaskTypeModule(3, false),null, null, null, null, null, null);
			result = service.GetTaskType(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TaskTypeID);
		}
		[TestMethod]
		public void ShouldGetTaskTypes()
		{
			PIOService service;
			TaskType[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, new MockedTaskTypeModule(3, false), null, null, null, null, null, null);
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

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, new MockedTaskTypeModule(3, true),null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTaskType(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTaskTypesAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, new MockedTaskTypeModule(3, true),  null, null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTaskTypes());
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetMaterial()
		{
			PIOService service;
			Material result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, new MockedMaterialModule(3, false),   null, null, null, null, null);
			result = service.GetMaterial(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.MaterialID);
		}
		[TestMethod]
		public void ShouldGetMaterials()
		{
			PIOService service;
			Material[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, new MockedMaterialModule(3, false), null, null, null, null, null);
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
			service = new PIOService(logger, null, null, null, null, null, null, null, new MockedMaterialModule(3, true),   null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMaterial(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetMaterialsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, new MockedMaterialModule(3, true),  null, null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetMaterials(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


		[TestMethod]
		public void ShouldGetIngredient()
		{
			PIOService service;
			Ingredient result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, new MockedIngredientModule(3, false), null, null, null, null);
			result = service.GetIngredient(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.IngredientID);
		}
		[TestMethod]
		public void ShouldGetIngredients()
		{
			PIOService service;
			Ingredient[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, new MockedIngredientModule(3, false), null, null, null, null);
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
			service = new PIOService(logger, null, null, null, null, null, null, null, null, new MockedIngredientModule(3, true), null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetIngredient(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetIngredientsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, new MockedIngredientModule(3, true), null, null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetIngredients(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetProduct()
		{
			PIOService service;
			Product result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, new MockedProductModule(3, false),  null, null, null);
			result = service.GetProduct(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.ProductID);
		}
		[TestMethod]
		public void ShouldGetProducts()
		{
			PIOService service;
			Product[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, new MockedProductModule(3, false),  null, null, null);
			result = service.GetProducts(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.IsTrue(result.All((item) => item != null));
		}

		[TestMethod]
		public void ShouldNotGetProductAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, new MockedProductModule(3, true),null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetProduct(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetProductsAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, new MockedProductModule(3, true),  null, null, null);

			Assert.ThrowsException<FaultException>(() => service.GetProducts(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}



		[TestMethod]
		public void ShouldGetTask()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, new MockedTaskModule(3, false), null, null);
			result = service.GetTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.TaskID);
		}
		[TestMethod]
		public void ShouldGetTasks()
		{
			PIOService service;
			Task[] result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, new MockedTaskModule(3, false), null, null);
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
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, new MockedTaskModule(3, true), null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetTasksAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, new MockedTaskModule(3, true), null, null);

			Assert.ThrowsException<FaultException>(() => service.GetTasks(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}






		[TestMethod]
		public void ShouldGetHasEnoughResourcesToProduce()
		{
			PIOService service;
			bool result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, true), null);
			result = service.HasEnoughResourcesToProduce(1);
			Assert.IsTrue(result);

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(false, false), null);
			result = service.HasEnoughResourcesToProduce(1);
			Assert.IsFalse(result);

		}


		[TestMethod]
		public void ShouldNotGetHasEnoughResourcesToProduceAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null, new MockedResourceCheckerModule(true,true), null);

			Assert.ThrowsException<FaultException>(() => service.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}

		[TestMethod]
		public void ShouldProduce()
		{
			PIOService service;
			Task result;

			service = new PIOService(NullLogger.Instance, null, null, null, null, null, null, null, null, null, null, null, null,new MockedProducerModule(false));
			result = service.Produce(10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.WorkerID);
		}


		[TestMethod]
		public void ShouldNotProduceAndLogError()
		{
			PIOService service;
			MemoryLogger logger;

			logger = new MemoryLogger(new DefaultLogFormatter());
			service = new PIOService(logger, null, null, null, null, null, null, null, null, null, null, null,  null, new MockedProducerModule(true));

			Assert.ThrowsException<FaultException>(() => service.Produce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(service.ModuleName)));
		}


	}
}
