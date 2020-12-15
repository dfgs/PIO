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
		private IFactoryModule factoryModule;
		private IStackModule stackModule;
		private IIngredientModule ingredientModule;

		public ResourceCheckerModule(ILogger Logger, IFactoryModule FactoryModule, IStackModule StackModule,IIngredientModule IngredientModule) : base(Logger)
		{
			this.factoryModule = FactoryModule;this.stackModule = StackModule;this.ingredientModule = IngredientModule;
		}

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			Factory factory;
			Ingredient[] ingredients;
			Stack[] stacks;
			Stack stack;

			LogEnter();

			Log(LogLevels.Information, $"Get factory (FactoryID={FactoryID})");
			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get factory");

			if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={FactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={FactoryID})", null, ID, ModuleName, "HasEnoughResourcesToProduce");
			}

			Log(LogLevels.Information, $"Get ingredients (FactoryTypeID={factory.FactoryTypeID})");
			ingredients= Try(() => ingredientModule.GetIngredients(factory.FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get ingredients");

			Log(LogLevels.Information, $"Get stacks (FactoryID={factory.FactoryID})");
			stacks = Try(() => stackModule.GetStacks(factory.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to get stacks");


			foreach (Ingredient ingredient in ingredients)
			{
				Log(LogLevels.Information, $"Check stack quantity (ResourceTypeID={ingredient.ResourceTypeID}, Quantity={ingredient.Quantity})");
				stack = stacks.FirstOrDefault(item => item.ResourceTypeID == ingredient.ResourceTypeID);
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



	}
}
