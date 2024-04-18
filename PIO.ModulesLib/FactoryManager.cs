using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class FactoryManager : Module,IFactoryManager
	{
		private ITopologySorter topologySorter;

		public FactoryManager(ILogger Logger,ITopologySorter TopologySorter) : base(Logger)
		{
			if (TopologySorter==null) throw new PIOInvalidParameterException(nameof(TopologySorter));
			this.topologySorter = TopologySorter;
		}

		public bool Update(IDataSource DataSource, float Cycle)
		{
			IFactory[] sortedFactories = [];
			IRecipe? recipe=null;
			IIngredient[] ingredients = [];
			IProduct[] products= [];
			IInputConnector[] inputConnectors = [];
			IOutputConnector[] outputConnectors = [];

			LogEnter();

			Log(LogLevels.Information, "Sorting factories by dependency");
			if (!Try(() => topologySorter.Sort(DataSource)).Then(items => sortedFactories = items.ToArray()).OrAlert("Failed to sort factories")) return false;

			Log(LogLevels.Information, "Updating all factories");
			foreach(IFactory factory in sortedFactories)
			{
				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Processing factory");
				if (!Try(() => DataSource.GetRecipe(factory.FactoryType)).Then(result => recipe = result).OrAlert($"[Factory ID {factory.ID}] Failed to get recipe")) continue;
				if (recipe == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] No recipe found");
					continue;
				}

				Log(LogLevels.Debug, $"[Recipe ID {recipe.ID}] Get ingredients and products");
				if (!Try(() => DataSource.GetIngredients(recipe.ID)).Then(result => ingredients = result.ToArray()).OrAlert($"[Recipe ID {recipe.ID}] Failed to get ingredients")) continue;
				if (!Try(() => DataSource.GetProducts(recipe.ID)).Then(result => products = result.ToArray()).OrAlert($"[Recipe ID {recipe.ID}] Failed to get products")) continue;

				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Get input and output connectors");
				if (!Try(() => DataSource.GetInputConnectors(factory.ID)).Then(result => inputConnectors = result.ToArray()).OrAlert($"[Factory ID {factory.ID}] Failed to get input connectors")) continue;
				if (!Try(() => DataSource.GetOutputConnectors(factory.ID)).Then(result => outputConnectors = result.ToArray()).OrAlert($"[Factory ID {factory.ID}] Failed to get output connectors")) continue;


			}

			return true;
		}



	}
}
