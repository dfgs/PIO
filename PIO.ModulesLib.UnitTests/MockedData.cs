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
		public static IDataSource DataSource1;
		private static ITopologySorter mainSorter;

		static MockedData()
		{
			mainSorter = new TopologySorter();

			DataSource1=InitDataSource1();	
		}

		private static IDataSource InitDataSource1()
		{
			DataSource dataSource;
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;


			// F1 -out1-in2-> F2 -out0.5-in2-> F3 -out0.25->
	
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

			dataSource = new DataSource();

			dataSource.AddFactory(factory1);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory3);

			dataSource.AddRecipe(recipe1);
			dataSource.AddRecipe(recipe2);
			dataSource.AddRecipe(recipe3);

			dataSource.AddIngredient(ingredient1);
			dataSource.AddIngredient(ingredient2);
			dataSource.AddIngredient(ingredient3);

			dataSource.AddProduct(product1);
			dataSource.AddProduct(product2);
			dataSource.AddProduct(product3);

			dataSource.AddInputConnector(inputConnector1);
			dataSource.AddInputConnector(inputConnector2);
			dataSource.AddInputConnector(inputConnector3);

			dataSource.AddOutputConnector(outputConnector1);
			dataSource.AddOutputConnector(outputConnector2);
			dataSource.AddOutputConnector(outputConnector3);

			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);

			return dataSource;
		}

		public static IDataSource GetMockedDataSource(IDataSource Model)
		{
			IDataSource dataSource;

			dataSource = Mock.Of<IDataSource>();

			Mock.Get(dataSource).Setup(m => m.GetFactories()).Returns(Model.GetFactories());
			Mock.Get(dataSource).Setup(m => m.GetFactory(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetFactory(id));
			Mock.Get(dataSource).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => Model.GetRecipe(id));
			Mock.Get(dataSource).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetIngredients(id));
			Mock.Get(dataSource).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetProducts(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetInputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetOutputConnectors(id));
			Mock.Get(dataSource).Setup(m => m.GetConnection(It.IsAny<ConnectionID>())).Returns<ConnectionID>(id => Model.GetConnection(id));
			Mock.Get(dataSource).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetConnections(id));
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetInputConnector(id));
			Mock.Get(dataSource).Setup(m => m.GetOutputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetOutputConnector(id));

			return dataSource;
		}
		public static IRecipeManager GetMockedRecipeManager(IDataSource Model)
		{
			IRecipeManager recipeManager;

			recipeManager = Mock.Of<IRecipeManager>();
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetIngredients(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Returns<RecipeID>(id => Model.GetProducts(id).ToArray());
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Returns<FactoryTypeID>(id => Model.GetRecipe(id));

			return recipeManager;
		}

		public static IConnectionManager GetMockedConnectionManager(IDataSource Model)
		{
			IConnectionManager connectionManager;

			connectionManager = Mock.Of<IConnectionManager>();
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetInputConnector(id));
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetInputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Returns<FactoryID>(id => Model.GetOutputConnectors(id).ToArray());
			Mock.Get(connectionManager).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Returns<ConnectorID>(id => Model.GetConnections(id).ToArray());

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
