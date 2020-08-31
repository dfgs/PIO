using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.Models.Modules;
using PIO.Models.Exceptions;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;
using PIO.UnitTest.ServerLib.Mocks.EngineModules;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class ProducerModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldReturnTaskWhenFactoryHasEnoughResourcesToProduce()
		{
			ProducerModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ITaskModule taskModule;
			ISchedulerModule schedulerModule;
			Task result;

			schedulerModule = new MockedSchedulerModule(false);
			factoryModule = new MockedFactoryModule( new Factory() {FactoryID=1, FactoryTypeID=2, PlanetID=3,HealthPoints=100 }   );
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = 5, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = 1, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = 2, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = 3, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false, 
				new Ingredient() { IngredientID = 0, FactoryTypeID = 2, ResourceTypeID = 1, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = 2, ResourceTypeID = 2, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = 2, ResourceTypeID = 3, Quantity = 6 }
				);
			taskModule = new MockedTaskModule(false, 1);
			module = new ProducerModule(NullLogger.Instance,schedulerModule,factoryModule,workerModule,stackModule,ingredientModule,taskModule);
			result = module.Produce(1, 1);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryHasNotEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ProducerModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ITaskModule taskModule;
			ISchedulerModule schedulerModule;

			schedulerModule = new MockedSchedulerModule(false);
			factoryModule = new MockedFactoryModule(new Factory() { FactoryID = 1, FactoryTypeID = 2, PlanetID = 3, HealthPoints = 100 });
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = 5, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = 1, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = 2, Quantity = 1 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = 3, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = 2, ResourceTypeID = 1, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = 2, ResourceTypeID = 2, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = 2, ResourceTypeID = 3, Quantity = 6 }
				);
			taskModule = new MockedTaskModule(false, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger,schedulerModule, factoryModule,workerModule, stackModule, ingredientModule,taskModule);

			Assert.ThrowsException<PIONoResourcesException>(() => module.Produce(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = 5, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = 1, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = 3, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = 2, ResourceTypeID = 1, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = 2, ResourceTypeID = 2, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = 2, ResourceTypeID = 3, Quantity = 6 }
				);
			taskModule = new MockedTaskModule(false, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger,schedulerModule, factoryModule, workerModule, stackModule, ingredientModule, taskModule);

			Assert.ThrowsException<PIONoResourcesException>(() => module.Produce(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ITaskModule taskModule;
			ISchedulerModule schedulerModule;

			schedulerModule = new MockedSchedulerModule(false);
			factoryModule = new MockedFactoryModule(new Factory() { FactoryID = 1, FactoryTypeID = 2, PlanetID = 3, HealthPoints = 100 });
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			taskModule = new MockedTaskModule(false, 1);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger,schedulerModule, factoryModule,workerModule, stackModule, ingredientModule,taskModule);
			Assert.ThrowsException< PIONotFoundException>(()=>module.Produce(1,2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ITaskModule taskModule;
			ISchedulerModule schedulerModule;

			schedulerModule = new MockedSchedulerModule(false);
			factoryModule = new MockedFactoryModule(new Factory() { FactoryID = 1, FactoryTypeID = 2, PlanetID = 3, HealthPoints = 100 });
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			taskModule = new MockedTaskModule(false, 1);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger,schedulerModule, factoryModule,workerModule, stackModule, ingredientModule, taskModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.Produce(2, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			ProducerModule module;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ITaskModule taskModule;
			ISchedulerModule schedulerModule;


			schedulerModule = new MockedSchedulerModule(true);
			factoryModule = new MockedFactoryModule(new Factory() { FactoryID = 1, FactoryTypeID = 2, PlanetID = 3, HealthPoints = 100 });
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			taskModule = new MockedTaskModule(false, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, schedulerModule, factoryModule, workerModule, stackModule, ingredientModule, taskModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.Produce(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));



			schedulerModule = new MockedSchedulerModule(false);
			factoryModule = new MockedFactoryModule(new Factory() { FactoryID = 1, FactoryTypeID = 2, PlanetID = 3, HealthPoints = 100 });
			workerModule = new MockedWorkerModule(new Worker() { WorkerID = 1, PlanetID = 3 });
			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			taskModule = new MockedTaskModule(false, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger,schedulerModule, factoryModule, workerModule,stackModule, ingredientModule,taskModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.Produce(1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			schedulerModule = new MockedSchedulerModule(false);
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			taskModule = new MockedTaskModule(false, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, schedulerModule, factoryModule, workerModule, stackModule, ingredientModule, taskModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.Produce(1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));


			schedulerModule = new MockedSchedulerModule(false);
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			taskModule = new MockedTaskModule(true, 1);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, schedulerModule, factoryModule, workerModule, stackModule, ingredientModule, taskModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.Produce(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

		}

	}
}
