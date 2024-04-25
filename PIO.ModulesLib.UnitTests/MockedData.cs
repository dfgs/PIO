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
		// create Mocked DataSource for test purpose
		public static IDataSource GetMockedDataSource()
		{
			IDataSource dataSource;
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = Mock.Of<IFactory>();
			Mock.Get(factory2).Setup(m => m.ID).Returns(new FactoryID(2));
			Mock.Get(factory2).Setup(m => m.FactoryTypeID).Returns(new FactoryTypeID("Type2"));
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

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(1))).Returns(factory1);
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(2))).Returns(factory2);
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(3))).Returns(factory3);

			Mock.Get(dataSource).Setup(m => m.GetRecipe(new FactoryTypeID("Type1"))).Returns(recipe1);
			Mock.Get(dataSource).Setup(m => m.GetRecipe(new FactoryTypeID("Type2"))).Returns(recipe2);
			Mock.Get(dataSource).Setup(m => m.GetRecipe(new FactoryTypeID("Type3"))).Returns(recipe3);

			Mock.Get(dataSource).Setup(m => m.GetIngredients(new RecipeID(1))).Returns([ingredient1]);
			Mock.Get(dataSource).Setup(m => m.GetIngredients(new RecipeID(2))).Returns([ingredient2]);
			Mock.Get(dataSource).Setup(m => m.GetIngredients(new RecipeID(3))).Returns([ingredient3]);

			Mock.Get(dataSource).Setup(m => m.GetProducts(new RecipeID(1))).Returns([product1]);
			Mock.Get(dataSource).Setup(m => m.GetProducts(new RecipeID(2))).Returns([product2]);
			Mock.Get(dataSource).Setup(m => m.GetProducts(new RecipeID(3))).Returns([product3]);

			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(new FactoryID(1))).Returns([inputConnector1]);
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(new FactoryID(2))).Returns([inputConnector2]);
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(new FactoryID(3))).Returns([inputConnector3]);

			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(new FactoryID(1))).Returns([outputConnector1]);
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(new FactoryID(2))).Returns([outputConnector2]);
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(new FactoryID(3))).Returns([outputConnector3]);

			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(1))).Returns(inputConnector1);
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(2))).Returns(inputConnector2);
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(3))).Returns(inputConnector3);

			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(new ConnectorID(4))).Returns(outputConnector1);
			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(new ConnectorID(5))).Returns(outputConnector2);
			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(new ConnectorID(6))).Returns(outputConnector3);


			Mock.Get(dataSource).Setup(m => m.GetConnection(new ConnectionID(1))).Returns(connection1);
			Mock.Get(dataSource).Setup(m => m.GetConnection(new ConnectionID(2))).Returns(connection2);

			Mock.Get(dataSource).Setup(m => m.GetConnections(new ConnectorID(4))).Returns([connection1]);
			Mock.Get(dataSource).Setup(m => m.GetConnections(new ConnectorID(5))).Returns([connection2]);


			return dataSource;
		}
		public static IRecipeManager GetMockedRecipeManager()
		{
			IRecipeManager recipeManager;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryTypeID = new FactoryTypeID("Type2") };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryTypeID = new FactoryTypeID("Type3") };

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 2 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(3), ResourceType = "Type3", Rate = 2 };

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(3), ResourceType = "Type4", Rate = 1 };


			recipeManager = Mock.Of<IRecipeManager>();
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(new RecipeID(1))).Returns([ingredient1]);
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(new RecipeID(2))).Returns([ingredient2]);
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(new RecipeID(3))).Returns([ingredient3]);

			Mock.Get(recipeManager).Setup(m => m.GetProducts(new RecipeID(1))).Returns([product1]);
			Mock.Get(recipeManager).Setup(m => m.GetProducts(new RecipeID(2))).Returns([product2]);
			Mock.Get(recipeManager).Setup(m => m.GetProducts(new RecipeID(3))).Returns([product3]);

			Mock.Get(recipeManager).Setup(m => m.GetRecipe(new FactoryTypeID("Type1"))).Returns(recipe1);
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(new FactoryTypeID("Type2"))).Returns(recipe2);
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(new FactoryTypeID("Type3"))).Returns(recipe3);

			return recipeManager;
		}

		public static IConnectionManager GetConnectionManager()
		{
			IConnectionManager connectionManager;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type3" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(4), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(5), FactoryID = new FactoryID(2), ResourceType = "Type3" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(6), FactoryID = new FactoryID(3), ResourceType = "Type4" };

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(4), DestinationID = new ConnectorID(2) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(5), DestinationID = new ConnectorID(3) };


			connectionManager = Mock.Of<IConnectionManager>();
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(new FactoryID(1))).Returns([inputConnector1]);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(new FactoryID(2))).Returns([inputConnector2]);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(new FactoryID(3))).Returns([inputConnector3]);

			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(new FactoryID(1))).Returns([outputConnector1]);
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(new FactoryID(2))).Returns([outputConnector2]);
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(new FactoryID(3))).Returns([outputConnector3]);

			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(new ConnectorID(1))).Returns(inputConnector1);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(new ConnectorID(2))).Returns(inputConnector2);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(new ConnectorID(3))).Returns(inputConnector3);

			Mock.Get(connectionManager).Setup(m => m.GetConnections(new ConnectorID(4))).Returns([connection1]);
			Mock.Get(connectionManager).Setup(m => m.GetConnections(new ConnectorID(5))).Returns([connection2]);

			return connectionManager;
		}



	}
}
