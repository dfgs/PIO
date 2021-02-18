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
		public void ShouldReturnTrueWhenBuildingHasEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			

			buildingModule = new MockedBuildingModule(false, new Building() {BuildingID=1, BuildingTypeID=BuildingTypeIDs.Sawmill }   );
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
			module = new ResourceCheckerModule(NullLogger.Instance,buildingModule, stackModule,ingredientModule,null);

			Assert.IsTrue(module.HasEnoughResourcesToProduce(1));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenBuildingHasNotEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
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
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, ingredientModule,null);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
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
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, ingredientModule, null);

			Assert.IsFalse(module.HasEnoughResourcesToProduce(1));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenBuildingDoesntExistsInBuildingHasEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException< PIONotFoundException>(()=>module.HasEnoughResourcesToProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level ==LogLevels.Warning) && (item.ComponentName==module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInBuildingHasEnoughResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));




			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}







		[TestMethod]
		public void ShouldReturnEmptyArrayWhenBuildingHasEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ResourceTypeIDs[] missingResources;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
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
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(0, missingResources.Length);
		}

		[TestMethod]
		public void ShouldReturnResourcesTypesWhenBuildingHasNotEnoughResourcesToProduce()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;
			ResourceTypeIDs[] missingResources;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

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
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Stone, missingResources[0]);
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
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, ingredientModule, null);

			missingResources = module.GetMissingResourcesToProduce(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Coal, missingResources[0]);
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenBuildingDoesntExistsInGetMissingResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIONotFoundException>(() => module.GetMissingResourcesToProduce(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInGetMissingResourcesToProduce()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IIngredientModule ingredientModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(true);
			ingredientModule = new MockedIngredientModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			ingredientModule = new MockedIngredientModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, ingredientModule, null);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToProduce(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}














		[TestMethod]
		public void ShouldReturnTrueWhenBuildingHasEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID = BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule,null, materialModule);

			Assert.IsTrue(module.HasEnoughResourcesToBuild(1));
		}

		[TestMethod]
		public void ShouldReturnFalseWhenBuildingHasNotEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, null, materialModule);

			Assert.IsFalse(module.HasEnoughResourcesToBuild(1));
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, null, materialModule);

			Assert.IsFalse(module.HasEnoughResourcesToBuild(1));
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenBuildingDoesntExistsInBuildingHasEnoughResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.HasEnoughResourcesToBuild(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInBuildingHasEnoughResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(true);
			materialModule = new MockedMaterialModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.HasEnoughResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}







		[TestMethod]
		public void ShouldReturnEmptyArrayWhenBuildingHasEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;
			ResourceTypeIDs[] missingResources;

			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(0, missingResources.Length);
		}

		[TestMethod]
		public void ShouldReturnResourcesTypesWhenBuildingHasNotEnoughResourcesToBuild()
		{
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;
			ResourceTypeIDs[] missingResources;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			#region when all stacks exist
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 1 },
				new Stack() { StackID = 3, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Stone, missingResources[0]);
			#endregion

			#region when a stack is missing
			stackModule = new MockedStackModule(false,
				new Stack() { StackID = 0, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Plank, Quantity = 10 },
				new Stack() { StackID = 1, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 },
				new Stack() { StackID = 2, BuildingID = 1, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 }
				);
			materialModule = new MockedMaterialModule(false,
				new Material() { MaterialID = 0, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 5 },
				new Material() { MaterialID = 1, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Stone, Quantity = 10 },
				new Material() { MaterialID = 3, BuildingTypeID=BuildingTypeIDs.Sawmill, ResourceTypeID = ResourceTypeIDs.Coal, Quantity = 6 }
				);
			module = new ResourceCheckerModule(NullLogger.Instance, buildingModule, stackModule, null, materialModule);

			missingResources = module.GetMissingResourcesToBuild(1);
			Assert.IsNotNull(missingResources);
			Assert.AreEqual(1, missingResources.Length);
			Assert.AreEqual(ResourceTypeIDs.Coal, missingResources[0]);
			#endregion

		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenBuildingDoesntExistsInGetMissingResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(false);

			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIONotFoundException>(() => module.GetMissingResourcesToBuild(2));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldThrowExceptionAndLogErrorWhenSubModuleFailsInGetMissingResourcesToBuild()
		{
			MemoryLogger logger;
			ResourceCheckerModule module;
			IBuildingModule buildingModule;
			
			IStackModule stackModule;
			IMaterialModule materialModule;


			buildingModule = new MockedBuildingModule(false, new Building() { BuildingID = 1, BuildingTypeID = BuildingTypeIDs.Sawmill });

			stackModule = new MockedStackModule(true);
			materialModule = new MockedMaterialModule(false);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));




			stackModule = new MockedStackModule(false);
			materialModule = new MockedMaterialModule(true);
			logger = new MemoryLogger();
			module = new ResourceCheckerModule(logger, buildingModule, stackModule, null, materialModule);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetMissingResourcesToBuild(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}




	}
}
