using Moq;
using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib.UnitTests
{
	internal static class MockedData
	{
		private static DataSource mainDataSource;
		private static ITopologySorter mainSorter;

		static MockedData()
		{
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;

			mainSorter = new TopologySorter();

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 2 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(3), ResourceType = "Type3", Rate = 2 };

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(3), ResourceType = "Type4", Rate = 1 };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type3" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(5), FactoryID = new FactoryID(2), ResourceType = "Type3" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(6), FactoryID = new FactoryID(3), ResourceType = "Type4" };

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(4), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(5), DestinationID = new ConnectorID(3) };

			mainDataSource = new DataSource();

			mainDataSource.AddFactory(factory1);
			mainDataSource.AddFactory(factory2);
			mainDataSource.AddFactory(factory3);

			mainDataSource.AddRecipe(recipe1);
			mainDataSource.AddRecipe(recipe2);
			mainDataSource.AddRecipe(recipe3);

			mainDataSource.AddIngredient(ingredient1);
			mainDataSource.AddIngredient(ingredient2);
			mainDataSource.AddIngredient(ingredient3);

			mainDataSource.AddProduct(product1);
			mainDataSource.AddProduct(product2);
			mainDataSource.AddProduct(product3);

			mainDataSource.AddInputConnector(inputConnector1);
			mainDataSource.AddInputConnector(inputConnector2);
			mainDataSource.AddInputConnector(inputConnector3);

			mainDataSource.AddOutputConnector(outputConnector1);
			mainDataSource.AddOutputConnector(outputConnector2);
			mainDataSource.AddOutputConnector(outputConnector3);

			mainDataSource.AddConnection(connection1);
			mainDataSource.AddConnection(connection2);
		}

		// create Mocked DataSource for test purpose
		public static IDataSource GetMockedDataSource()
		{
			IDataSource dataSource;

			dataSource = Mock.Of<IDataSource>();

			Mock.Get(dataSource).Setup(m => m.GetFactories()).Returns(mainDataSource.GetFactories());
			Mock.Get(dataSource).Setup(m => m.GetFactory(It.IsAny<FactoryID>())).Returns<FactoryID>(id => mainDataSource.GetFactory(id));
			Mock.Get(dataSource).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => mainDataSource.GetRecipe(id));
			Mock.Get(dataSource).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => mainDataSource.GetIngredients(id));
			Mock.Get(dataSource).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => mainDataSource.GetProducts(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => mainDataSource.GetInputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => mainDataSource.GetOutputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetConnection(It.IsAny<ConnectionID>())).Returns<ConnectionID>(id => mainDataSource.GetConnection(id));
			Mock.Get(dataSource).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => mainDataSource.GetConnections(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => mainDataSource.GetInputConnector(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => mainDataSource.GetOutputConnector(id));

			return dataSource;
		}
		public static IRecipeManager GetMockedRecipeManager()
		{
			IRecipeManager recipeManager;

			recipeManager = Mock.Of<IRecipeManager>();
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => mainDataSource.GetIngredients(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => mainDataSource.GetProducts(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => mainDataSource.GetRecipe(id));

			return recipeManager;
		}

		public static IConnectionManager GetMockedConnectionManager()
		{
			IConnectionManager connectionManager;

			connectionManager = Mock.Of<IConnectionManager>();
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => mainDataSource.GetInputConnector(id));
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => mainDataSource.GetInputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => mainDataSource.GetOutputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => mainDataSource.GetConnections(id).ToArray());

			return connectionManager;
		}

		public static ITopologySorter GetMockedTopologySorter()
		{
			ITopologySorter mock;

			mock = Mock.Of<ITopologySorter>();
			Mock.Get(mock).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns<IDataSource>(dataSource=>mainSorter.Sort(dataSource));

			return mock;
		}
	}

}
