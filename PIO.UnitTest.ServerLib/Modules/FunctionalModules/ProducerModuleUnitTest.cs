﻿using System;
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
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Plank, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule,buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginProduce(1);

			Assert.IsNotNull(result);
			Assert.AreEqual(TaskTypeIDs.Produce, result.TaskTypeID);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(1, schedulerModule.Count);
		}
		[TestMethod]
		public void ShouldNotProduceWhenBuildingIsNotFactory()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			MemoryLogger logger;


			logger = new MemoryLogger();

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill,IsFactory=false });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Plank, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
		}

		[TestMethod]
		public void ShouldNotProduceWhenWorkerIsAlreadyWorking()
		{
			MemoryLogger logger;
			ProducerModule module;
			IWorkerModule workerModule;
			ITaskModule taskModule;

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			taskModule = new MockedTaskModule(false, new Task() { WorkerID = 1 });

			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,null,null,null,null,null);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldReturnNullWhenBuildingHasNoProduct()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Task result;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill});
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false);
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule,  buildingModule,buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			result = module.BeginProduce(1);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void ShouldNotProduceWhenBuildingHasNotEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
			new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
			new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
			);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule,buildingTypeModule, stackModule, ingredientModule,productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONoResourcesException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);
			#endregion

		}

		
		[TestMethod]
		public void ShouldNotProduceWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false,module);

			Assert.ThrowsException<PIONotFoundException>(() => module.BeginProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldNotProduceWhenBuildingIsNotFinished()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill,RemainingBuildSteps=10 });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIOInvalidOperationException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}
		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false,new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 0 }); 
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);


			stackModule = new MockedStackModule(false , new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			ingredientModule = new MockedIngredientModule(false, new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 0 });
			taskModule = new MockedTaskModule(true);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule,  workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.BeginProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
			Assert.AreEqual(0, schedulerModule.Count);

		}



		[TestMethod]
		public void ShouldEndTaskWhenStackExists()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
			);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, ResourceTypeID=ResourceTypeIDs.Stone, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
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
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;
			Stack stack;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 2, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, ResourceTypeID = ResourceTypeIDs.Plank, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndProduce(1);
			stack = stackModule.GetStack(3);
			Assert.AreEqual(2, stack.Quantity);

		}

		[TestMethod]
		public void ShouldEndTaskWhenBuildingHasNoProduct()
		{
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });

			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			productModule = new MockedProductModule(false);
			taskModule = new MockedTaskModule(false);
			module = new ProducerModule(NullLogger.Instance, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			module.EndProduce(1);

		}


		[TestMethod]
		public void ShouldNotEndTaskWhenWorkerDoesntExists()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);

			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);

			Assert.ThrowsException<PIONotFoundException>(() => module.EndProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotEndTaskWhenSubModuleFails()
		{
			MemoryLogger logger;
			ProducerModule module;
			IBuildingModule buildingModule;
			IBuildingTypeModule buildingTypeModule;
			IWorkerModule workerModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			IProductModule productModule;
			ITaskModule taskModule;
			MockedSchedulerModule schedulerModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			buildingTypeModule = new MockedBuildingTypeModule(false, new BuildingType() { BuildingTypeID = BuildingTypeIDs.Sawmill, IsFactory = true });
			workerModule = new MockedWorkerModule(false, new Worker() { WorkerID = 1, PlanetID = 1 });
			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(false, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule,  buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);
			productModule = new MockedProductModule(true, new Product() { ProductID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill, Duration = 4, Quantity = 2 });
			taskModule = new MockedTaskModule(false);
			logger = new MemoryLogger();
			module = new ProducerModule(logger, taskModule, workerModule, buildingModule, buildingTypeModule, stackModule, ingredientModule, productModule);
			schedulerModule = new MockedSchedulerModule(false, module);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.EndProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));



		}




	}
}
