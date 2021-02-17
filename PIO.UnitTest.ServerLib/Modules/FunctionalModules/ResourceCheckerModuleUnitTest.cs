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
	public class ResourceCheckerModuleUnitTest
	{
	
		[TestMethod]
		public void ShouldReturnTrueWhenFactoryHasEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			

			factoryModule = new MockedFactoryModule(false, new Factory() {FactoryID=1, FactoryTypeID=FactoryTypeIDs.Sawmill, BuildingID = 3 }   );
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false, 
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance,factoryModule,factoryTypeModule, stackModule,ingredientModule,null);

			Assert.IsTrue(module.HasEnoughResourcesToProduce(1));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenFactoryHasNotEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } } );
			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, ingredientModule,null);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExistsInFactoryHasEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException< PIONotFoundException>(()=>module.HasEnoughResourcesToProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInFactoryHasEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));




			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}







		[TestMethod]
		public void ShouldReturnEmptyArrayWhenFactoryHasEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ResourceTypeIDs[] missingResources;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(0, missingResources.Length);
		}

		[TestMethod]
		public void ShouldReturnResourcesTypesWhenFactoryHasNotEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ResourceTypeIDs[] missingResources;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Stone, missingResources[0]);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			ingredientModule = new MockedIngredientModule(false,
				new Ingredient() { IngredientID = 0, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Ingredient() { IngredientID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Ingredient() { IngredientID = 3, FactoryTypeID = FactoryTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Coal, missingResources[0]);
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExistsInGetMissingResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIONotFoundException>(() => module.GetMissingResourcesToProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInGetMissingResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}














		[TestMethod]
		public void ShouldReturnTrueWhenFactoryHasEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID=2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule,null, materialModule);

			Assert.IsTrue(module.HasEnoughResourcesToBuild(1));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenFactoryHasNotEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, null, materialModule);

			Assert.IsFalse(module.HasEnoughResourcesToBuild(1));
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, null, materialModule);

			Assert.IsFalse(module.HasEnoughResourcesToBuild(1));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExistsInFactoryHasEnoughResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.HasEnoughResourcesToBuild(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInFactoryHasEnoughResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(true);
			materialModule = new MockedMaterialModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}







		[TestMethod]
		public void ShouldReturnEmptyArrayWhenFactoryHasEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			ResourceTypeIDs[] missingResources;

			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(0, missingResources.Length);
		}

		[TestMethod]
		public void ShouldReturnResourcesTypesWhenFactoryHasNotEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;
			ResourceTypeIDs[] missingResources;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Stone, missingResources[0]);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 3, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, MaterialSetID = 2, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, factoryModule, factoryTypeModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Coal, missingResources[0]);
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenFactoryDoesntExistsInGetMissingResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.GetMissingResourcesToBuild(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInGetMissingResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IFactoryModule factoryModule;
			IFactoryTypeModule factoryTypeModule;
			IStackModule stackModule;
			IMaterialModule materialModule;


			factoryModule = new MockedFactoryModule(false, new Factory() { FactoryID = 1, FactoryTypeID = FactoryTypeIDs.Sawmill, BuildingID = 3 });
			factoryTypeModule = new MockedFactoryTypeModule(false, new FactoryType[] { new FactoryType() { FactoryTypeID = FactoryTypeIDs.Sawmill, MaterialSetID = 2 } });

			stackModule = new MockedStackModule(true);
			materialModule = new MockedMaterialModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, factoryModule, factoryTypeModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}




	}
}
