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
		public void ShouldProduce()
		{
			ProducerModule module;
			IFactoryModule factoryModule;
			IBuildingModule buildingModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID=3 });
			factoryModule = new MockedFactoryModule(false, new Factory() {FactoryID=1, FactoryTypeID=FactoryTypeIDs.Sawmill, BuildingID = 3 }   );
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false, 
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID=1, FactoryTypeID= FactoryTypeIDs.Sawmill, Duration=4, Quantity=2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule,buildingModule, factoryModule,stackModule,ingredientModule,productModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			result = module.BeginProduce(1);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Produce, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}

		[TestMethod]
		public void ShouldEnqueueTaskWithCorrectETA()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result,result2;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 20 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 20 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 20 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 20 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginProduce(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Produce, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);

			result2 = module.BeginProduce(1);
			Assert.AreEqual(TaskTypeIDs.Produce, result2.TaskTypeID);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(2, schedulerModule.Count);

			Assert.IsTrue((result2.ETA - result.ETA).TotalSeconds >= 4);


		}

		[TestMethod]
		public void ShouldReturnNullWhenFactoryHasNoProduct()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3});
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false);
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginProduce(1);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void ShouldNotProduceWhenFactoryHasNotEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
			new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
			new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
			);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule,productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

		}

		
		[TestMethod]
		public void ShouldNotProduceWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldNotProduceWhenBuildingIsNotFinished()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 , RemainingBuildSteps=1});
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false,new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 0 }); 
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule,stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);


			stackModule = new MockedStackModule(false , new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			ingredientModule = new MockedIngredientModule(false, new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 0 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule,  workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}



		[TestMethod]
		public void ShouldEndTaskWhenStackExists()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
			);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, ResourceTypeID=ResourceTypeIDs.Stone, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndProduce(1);
			stack = stackModule.GetStack(2);
			Assert.AreEqual(12, stack.Quantity);

		}
		[TestMethod]
		public void ShouldEndTaskWhenStackDoesntExists()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, ResourceTypeID = ResourceTypeIDs.Tree, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndProduce(1);
			stack = stackModule.GetStack(4);
			Assert.AreEqual(2, stack.Quantity);

		}

		[TestMethod]
		public void ShouldEndTaskWhenFactoryHasNoProduct()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, FactoryID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false);
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndProduce(1);

		}


		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Warning") && item.Contains(module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IFactoryModule factoryModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 3 });
			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(true, new Product() { ProductID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger(new DefaultLogFormatter());
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, factoryModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));



		}




	}
}
