using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using PIOBaseModulesLib.Modules.FunctionalModules;


namespace PIO.ServerLib.Modules
{
	public class ResourceCheckerModule : FunctionalModule, IResourceCheckerModule
	{
		private IBuildingModule buildingModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;
		private IMaterialModule materialModule;

		public ResourceCheckerModule(ILogger Logger, IBuildingModule BuildingModule,  IStackModule StackModule,IIngredientModule IngredientModule,IMaterialModule MaterialModule) : base(Logger)
		{
			this.buildingModule = BuildingModule; this.stackModule = StackModule;this.ingredientModule = IngredientModule;this.materialModule = MaterialModule;
		}

		public bool HasEnoughResourcesToProduce(int BuildingID)
		{
			Building building;
			Ingredient[] ingredients;
			Stack[] stacks;
			Stack stack;

			LogEnter();

			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			Log(LogLevels.Information, $"Get ingredients (BuildingTypeID={building.BuildingTypeID})");
			ingredients= Try(() => ingredientModule.GetIngredients(building.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to get ingredients");

			Log(LogLevels.Information, $"Get stacks (BuildingID={building.BuildingID})");
			stacks=Try(() => stackModule.GetStacks(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");


			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack=stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
				if (stack==null)
				{
					Log(LogLevels.Information, $"Resource not found in stacks");
					return false;
				}

				if (stack.Quantity<ingredient.Quantity)
				{
					Log(LogLevels.Information, $"Not enough quantity in stack (Quantity={stack.Quantity})");
					return false;
				}
			}

			return true;
		}
		public ResourceTypeIDs[] GetMissingResourcesToProduce(int BuildingID)
		{
			Building building;
			Ingredient[] ingredients;
			Stack[] stacks;
			Stack stack;
			List<ResourceTypeIDs> missingResources;

			LogEnter();


			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			Log(LogLevels.Information, $"Get ingredients (BuildingTypeID={building.BuildingTypeID})");
			ingredients=Try(() => ingredientModule.GetIngredients(building.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to get ingredients");

			Log(LogLevels.Information, $"Get stacks (BuildingID={building.BuildingID})");
			stacks=Try(() => stackModule.GetStacks(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			missingResources=new List<ResourceTypeIDs>();

			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack=stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
				if (stack == null)
				{
					Log(LogLevels.Information, $"Resource not found in stacks");
					missingResources.Add(ingredient.ResourceTypeID);
					continue;
				}

				if (stack.Quantity < ingredient.Quantity)
				{
					Log(LogLevels.Information, $"Not enough quantity in stack (Quantity={stack.Quantity})");
					missingResources.Add(ingredient.ResourceTypeID);
					continue;
				}
			}

			return missingResources.ToArray();
		}




		public bool HasEnoughResourcesToBuild(int BuildingID)
		{
			Building building;
			Material[] materials;
			Stack[] stacks;
			Stack stack;

			LogEnter();

			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			Log(LogLevels.Information, $"Get materials (BuildingTypeID={building.BuildingTypeID})");
			materials = Try(() => materialModule.GetMaterials(building.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to get materials");

			Log(LogLevels.Information, $"Get stacks (BuildingID={building.BuildingID})");
			stacks = Try(() => stackModule.GetStacks(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");


			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == material.ResourceTypeID);
				if (stack == null)
				{
					Log(LogLevels.Information, $"Resource not found in stacks");
					return false;
				}

				if (stack.Quantity < material.Quantity)
				{
					Log(LogLevels.Information, $"Not enough quantity in stack (Quantity={stack.Quantity})");
					return false;
				}
			}

			return true;
		}
		public ResourceTypeIDs[] GetMissingResourcesToBuild(int BuildingID)
		{
			Building building;
			BuildingType buildingType;
			Material[] materials;
			Stack[] stacks;
			Stack stack;
			List<ResourceTypeIDs> missingResources;

			LogEnter();


			building = AssertExists(() => buildingModule.GetBuilding(BuildingID), $"BuildingID={BuildingID}");

			Log(LogLevels.Information, $"Get materials (BuildingTypeID={building.BuildingTypeID})");
			materials = Try(() => materialModule.GetMaterials(building.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to get materials");

			Log(LogLevels.Information, $"Get stacks (BuildingID={building.BuildingID})");
			stacks = Try(() => stackModule.GetStacks(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");

			missingResources = new List<ResourceTypeIDs>();

			foreach (Material material in materials)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={material.ResourceTypeID}, Quantity={material.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == material.ResourceTypeID);
				if (stack == null)
				{
					Log(LogLevels.Information, $"Resource not found in stacks");
					missingResources.Add(material.ResourceTypeID);
					continue;
				}

				if (stack.Quantity < material.Quantity)
				{
					Log(LogLevels.Information, $"Not enough quantity in stack (Quantity={stack.Quantity})");
					missingResources.Add(material.ResourceTypeID);
					continue;
				}
			}

			return missingResources.ToArray();
		}

	}
}
