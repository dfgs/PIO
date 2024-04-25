using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class UpdateManager : PIOModule,IUpdateManager
	{
		private ITopologySorter topologySorter;
		private IRecipeManager recipeManager;
		private IConnectionManager connectionManager;

		public UpdateManager(ILogger Logger,IDataSource DataSource, ITopologySorter TopologySorter,IRecipeManager RecipeManager,IConnectionManager ConnectionManager) : base(Logger,DataSource)
		{
			if (TopologySorter == null) throw new PIOInvalidParameterException(nameof(TopologySorter));
			if (RecipeManager == null) throw new PIOInvalidParameterException(nameof(RecipeManager));
			if (ConnectionManager == null) throw new PIOInvalidParameterException(nameof(ConnectionManager));
			this.topologySorter = TopologySorter;
			this.recipeManager = RecipeManager;
			this.connectionManager = ConnectionManager;
		}

		public float GetEfficiency(FactoryID FactoryID,IEnumerable<IIngredient> Ingredients,IEnumerable<IConnector> Connectors)
		{
			float efficiency;
			IConnector? connector;
			float connectorEfficiency;

			LogEnter();
			if (Ingredients == null)
			{
				Log(LogLevels.Fatal, $"[Factory ID {FactoryID}] Parameter {nameof(Ingredients)} cannot be null");
				return 0;
			}
			if (Connectors == null)
			{
				Log(LogLevels.Fatal, $"[Factory ID {FactoryID}] Parameter {nameof(Connectors)} cannot be null");
				return 0;
			}

			efficiency = 1;
			foreach(IIngredient ingredient in Ingredients)
			{
				// we don't need this ingredient
				if (ingredient.Rate == 0) continue;
				
				// if we don't have input for this ingredient, so efficiency is 0
				connector=Connectors.FirstOrDefault(item=>item.ResourceType== ingredient.ResourceType);
				if (connector==null)
				{
					Log(LogLevels.Warning, $"[Factory ID {FactoryID}] No connector found for ingredient ID {ingredient.ID}");
					efficiency = 0;
					break;
				}

				connectorEfficiency = connector.Rate / ingredient.Rate;
				if (connectorEfficiency < efficiency) efficiency = connectorEfficiency;

			}

			return efficiency;
		}
		public bool UpdateConnectors(FactoryID FactoryID, float Efficiency, IEnumerable<IProduct> Products, IEnumerable<IConnector> Connectors)
		{
			IConnector? connector;

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

		

	


		public bool Update(float Cycle)
		{
			IFactory[] sortedFactories = [];
			IRecipe? recipe=null;
			IIngredient[]? ingredients;
			IProduct[]? products;
			IInputConnector[]? inputConnectors = null;
			IOutputConnector[]? outputConnectors = null;
			IConnection[] connections = [];
			IInputConnector? destinationConnector=null;

			LogEnter();

			Log(LogLevels.Information, "[Factory ID {factory.ID}] Sorting factories by dependency");
			if (!Try(() => topologySorter.Sort(DataSource)).Then(items => sortedFactories = items.ToArray()).OrAlert("Failed to sort factories")) return false;

			Log(LogLevels.Information, "[Factory ID {factory.ID}] Updating all factories");
			foreach(IFactory factory in sortedFactories)
			{
				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Processing factory");

				recipe = recipeManager.GetRecipe(factory.FactoryTypeID);
				if (recipe == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] No recipe found");
					continue;
				}

				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Get ingredients and products");
				ingredients = recipeManager.GetIngredients(recipe.ID);
				if (ingredients == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] Failed to get ingredients");
					continue;
				}
				products = recipeManager.GetProducts(recipe.ID);
				if (products == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] Failed to get products");
					continue;
				}

				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Get input and output connectors");
				inputConnectors = connectionManager.GetInputConnectors(factory.ID);
				if (inputConnectors == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] Failed to get input connectors");
					continue;
				}
				outputConnectors = connectionManager.GetOutputConnectors(factory.ID);
				if (outputConnectors == null)
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] Failed to get output connectors");
					continue;
				}

				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Compute efficiency");
				factory.Efficiency = GetEfficiency(factory.ID, ingredients, inputConnectors);
				
				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Updating output connectors");
				if (!UpdateConnectors(factory.ID, factory.Efficiency, products, outputConnectors))
				{
					Log(LogLevels.Warning, $"[Factory ID {factory.ID}] Failed to update output connectors");
					return false;
				}

				Log(LogLevels.Debug, $"[Factory ID {factory.ID}] Updating connections");
				foreach (IOutputConnector outputConnector in outputConnectors)
				{
					Log(LogLevels.Debug, $"[Connector ID {outputConnector.ID}] Processing connector, trying to get connections");
					if (!Try(() => DataSource.GetConnections(outputConnector.ID)).Then(result => connections = result.ToArray()).OrAlert($"[Connector ID {outputConnector.ID}] Failed to get connections")) continue;
					foreach(IConnection connection in connections)
					{
						Log(LogLevels.Debug, $"[Connection ID {connection.ID}] Processing connection, trying to get destination connector");
						destinationConnector=connectionManager.GetInputConnector(connection.DestinationID);
						if (destinationConnector==null)
						{
							Log(LogLevels.Warning, $"[Connection ID {connection.ID}] Failed to get destination connector");
							continue;
						}
						destinationConnector.Rate = outputConnector.Rate;
					}
				}
			}

			return true;
		}



	}
}
