using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class FactoryManager : PIOModule,IFactoryManager
	{

		public FactoryManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
			if (TopologySorter==null) throw new PIOInvalidParameterException(nameof(TopologySorter));
			this.topologySorter = TopologySorter;
		}

		}

		

	


			LogEnter();
			if (Products == null)
			{
				Log(LogLevels.Fatal, $"[Factory ID {FactoryID}] Parameter {nameof(Products)} cannot be null");
				return false;
			}
			if (Connectors == null)
			{
				Log(LogLevels.Fatal, $"[Factory ID {FactoryID}] Parameter {nameof(Connectors)} cannot be null");
				return false;
			}
			if ((Efficiency < 0)|| (Efficiency>1))
			{
				Log(LogLevels.Error, $"[Factory ID {FactoryID}] Parameter {nameof(Efficiency)} has invalid value {Efficiency}");
				return false;
			}
			foreach (IProduct product in Products)
			{
				connector = Connectors.FirstOrDefault(item => item.ResourceType == product.ResourceType);
				if (connector == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {FactoryID}] No ouput connector found for product ID {product.ID}");
					continue;
				}
				connector.Rate = Efficiency * product.Rate;
			}

			return true;
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

			Log(LogLevels.Information, "[Factory ID {factory.ID}] Sorting factories by dependency");
			if (!Try(() => topologySorter.Sort(DataSource)).Then(items => sortedFactories = items.ToArray()).OrAlert("Failed to sort factories")) return false;

			Log(LogLevels.Information, "[Factory ID {factory.ID}] Updating all factories");
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


				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Compute efficiency");
				factory.Efficiency = GetEfficiency(factory.ID, ingredients, inputConnectors);
				
				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Updating output connectors");
				UpdateConnectors(factory.ID, factory.Efficiency, products, outputConnectors);


			}

			return true;
		}



	}
}
