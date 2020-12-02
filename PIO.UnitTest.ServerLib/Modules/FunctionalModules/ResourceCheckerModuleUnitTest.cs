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

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class ResourceCheckerModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldReturnTrueWhenFactoryHasEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			

			factoryModule = new MockedFactoryModule(false, new Factory() {FactoryID=1, FactoryTypeID=FactoryTypeIDs.Sawmill, BuildingID = 3 }   );
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
			module = new ResourceCheckerModule(NullLogger.Instance,factoryModule,stackModule,ingredientModule);

			Assert.IsTrue(module.HasEnoughResourcesToProduce(1));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenFactoryHasNotEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });

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
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, stackModule, ingredientModule);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
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
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, stackModule, ingredientModule);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExists()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, stackModule, ingredientModule);
			Assert.ThrowsException< PIONotFoundException>(()=>module.HasEnoughResourcesToProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFails()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });

			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, stackModule, ingredientModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));




			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, stackModule, ingredientModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}

	}
}
