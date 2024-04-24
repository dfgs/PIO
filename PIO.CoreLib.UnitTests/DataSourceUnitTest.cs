using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class DataSourceUnitTest
	{
		

		[TestMethod]
		public void AddFactoryShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource=new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddFactory(null) );
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddInputConnectorShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddInputConnector(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}
		[TestMethod]
		public void AddOutputConnectorShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddOutputConnector(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddConnectionShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddConnection(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}
		[TestMethod]
		public void AddBufferShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddBuffer(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}
	
		[TestMethod]
		public void AddRecipeShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddRecipe(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}
		[TestMethod]
		public void AddIngredientShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddIngredient(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}









		[TestMethod]
		public void GetFactoryShouldReturnValidValue()
		{
			IFactory factory;
			IDataSource dataSource;

			factory = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			dataSource = new DataSource();
			dataSource.AddFactory(factory);

			Assert.AreEqual(factory,dataSource.GetFactory(new FactoryID(1)));
		}
		[TestMethod]
		public void GetFactoryShouldReturnNull()
		{
			IFactory factory;
			IDataSource dataSource;

			factory = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			dataSource = new DataSource();
			dataSource.AddFactory(factory);

			Assert.IsNull(dataSource.GetFactory(new FactoryID(0)));
		}
		[TestMethod]
		public void GetFactoriesShouldReturnValidValues()
		{
			IFactory factory1, factory2, factory3;
			IDataSource dataSource;
			IFactory[] results;

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryTypeID = new FactoryTypeID("Type1") };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryTypeID = new FactoryTypeID("Type1") };

			dataSource = new DataSource();
			dataSource.AddFactory(factory1);
			dataSource.AddFactory(factory2);
			dataSource.AddFactory(factory3);

			results = dataSource.GetFactories().ToArray();
			Assert.AreEqual(3,results.Length);
			Assert.IsTrue(results.Contains(factory1));
			Assert.IsTrue(results.Contains(factory2));
			Assert.IsTrue(results.Contains(factory3));
		}

		[TestMethod]
		public void GetInputConnectorShouldReturnValidValue()
		{
			IInputConnector connector;
			IDataSource dataSource;

			connector = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1),ResourceType="Type1" };
			dataSource = new DataSource();
			dataSource.AddInputConnector(connector);

			Assert.AreEqual(connector, dataSource.GetInputConnector(new ConnectorID(1)));
		}
		[TestMethod]
		public void GetInputConnectorShouldReturnNull()
		{
			IInputConnector connector;
			IDataSource dataSource;

			connector = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddInputConnector(connector);

			Assert.IsNull(dataSource.GetInputConnector(new ConnectorID(0)));
		}

		[TestMethod]
		public void GetInputConnectorsShouldReturnValidValues()
		{
			IInputConnector connector1, connector2, connector3;
			IDataSource dataSource;
			IInputConnector[] results;

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddInputConnector(connector1);
			dataSource.AddInputConnector(connector2);
			dataSource.AddInputConnector(connector3);

			results = dataSource.GetInputConnectors(new FactoryID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(connector1));
			Assert.IsFalse(results.Contains(connector2));
			Assert.IsTrue(results.Contains(connector3));
		}
		[TestMethod]
		public void GetInputConnectorsShouldReturnNoValues()
		{
			IInputConnector connector1, connector2, connector3;
			IDataSource dataSource;
			IInputConnector[] results;

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddInputConnector(connector1);
			dataSource.AddInputConnector(connector2);
			dataSource.AddInputConnector(connector3);

			results = dataSource.GetInputConnectors(new FactoryID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}

		[TestMethod]
		public void GetOutputConnectorShouldReturnValidValue()
		{
			IOutputConnector connector;
			IDataSource dataSource;

			connector = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddOutputConnector(connector);

			Assert.AreEqual(connector, dataSource.GetOutputConnector(new ConnectorID(1)));
		}
		[TestMethod]
		public void GetOutputConnectorShouldReturnNull()
		{
			IOutputConnector connector;
			IDataSource dataSource;

			connector = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddOutputConnector(connector);

			Assert.IsNull(dataSource.GetOutputConnector(new ConnectorID(0)));
		}

		[TestMethod]
		public void GetOutputConnectorsShouldReturnValidValues()
		{
			IOutputConnector connector1, connector2, connector3;
			IDataSource dataSource;
			IOutputConnector[] results;

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddOutputConnector(connector1);
			dataSource.AddOutputConnector(connector2);
			dataSource.AddOutputConnector(connector3);

			results = dataSource.GetOutputConnectors(new FactoryID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(connector1));
			Assert.IsFalse(results.Contains(connector2));
			Assert.IsTrue(results.Contains(connector3));
		}
		[TestMethod]
		public void GetOutputConnectorsShouldReturnNoValues()
		{
			IOutputConnector connector1, connector2, connector3;
			IDataSource dataSource;
			IOutputConnector[] results;

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddOutputConnector(connector1);
			dataSource.AddOutputConnector(connector2);
			dataSource.AddOutputConnector(connector3);

			results = dataSource.GetOutputConnectors(new FactoryID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}




		[TestMethod]
		public void GetConnectionShouldReturnValidValue()
		{
			IConnection connection;
			IDataSource dataSource;

			connection = new Connection() { ID = new ConnectionID(1), SourceID=new ConnectorID(1), DestinationID=new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddConnection(connection);

			Assert.AreEqual(connection, dataSource.GetConnection(new ConnectionID(1)));
		}
		[TestMethod]
		public void GetConnectionShouldReturnNull()
		{
			IConnection connection;
			IDataSource dataSource;

			connection = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddConnection(connection);

			Assert.IsNull(dataSource.GetConnection(new ConnectionID(0)));
		}
		[TestMethod]
		public void GetConnectionsShouldReturnValidValues()
		{
			IConnection connection1, connection2, connection3;
			IDataSource dataSource;
			IConnection[] results;

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(4) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(5) };

			dataSource = new DataSource();
			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			results = dataSource.GetConnections(new ConnectorID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(connection1));
			Assert.IsFalse(results.Contains(connection2));
			Assert.IsTrue(results.Contains(connection3));
		}
		[TestMethod]
		public void GetConnectionsShouldReturnNoValues()
		{
			IConnection connection1, connection2, connection3;
			IDataSource dataSource;
			IConnection[] results;

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(4) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(5) };

			dataSource = new DataSource();
			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			results = dataSource.GetConnections(new ConnectorID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}

		[TestMethod]
		public void GetBufferShouldReturnValidValue()
		{
			IBuffer buffer;
			IDataSource dataSource;

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID=new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddBuffer(buffer);

			Assert.AreEqual(buffer, dataSource.GetBuffer(new BufferID(1)));
			Assert.AreEqual(buffer, dataSource.GetBuffer(new ConnectorID(2)));
		}
		[TestMethod]
		public void GetBufferShouldReturnNull()
		{
			IBuffer buffer;
			IDataSource dataSource;

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddBuffer(buffer);

			Assert.IsNull(dataSource.GetBuffer(new BufferID(0)));
			Assert.IsNull(dataSource.GetBuffer(new ConnectorID(0)));
		}

		[TestMethod]
		public void GetBuffersShouldReturnValidValues()
		{
			IBuffer buffer1, buffer2, buffer3;
			IDataSource dataSource;
			IBuffer[] results;

			buffer1 = new Buffer() { ID = new BufferID(1), ConnectorID=new ConnectorID(1) };
			buffer2 = new Buffer() { ID = new BufferID(2), ConnectorID = new ConnectorID(2) };
			buffer3 = new Buffer() { ID = new BufferID(3), ConnectorID = new ConnectorID(3) };

			dataSource = new DataSource();
			dataSource.AddBuffer(buffer1);
			dataSource.AddBuffer(buffer2);
			dataSource.AddBuffer(buffer3);

			results = dataSource.GetBuffers().ToArray();
			Assert.AreEqual(3, results.Length);
			Assert.IsTrue(results.Contains(buffer1));
			Assert.IsTrue(results.Contains(buffer2));
			Assert.IsTrue(results.Contains(buffer3));
		}

		[TestMethod]
		public void GetRecipeShouldReturnValidValue()
		{
			IRecipe recipe;
			IDataSource dataSource;

			recipe = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			dataSource = new DataSource();
			dataSource.AddRecipe(recipe);

			Assert.AreEqual(recipe, dataSource.GetRecipe(new RecipeID(1)));
			Assert.AreEqual(recipe, dataSource.GetRecipe(new FactoryTypeID("Type1")));
		}
		[TestMethod]
		public void GetRecipeShouldReturnNull()
		{
			IRecipe recipe;
			IDataSource dataSource;

			recipe = new Recipe() { ID = new RecipeID(1), FactoryTypeID = new FactoryTypeID("Type1") };
			dataSource = new DataSource();
			dataSource.AddRecipe(recipe);

			Assert.IsNull(dataSource.GetRecipe(new RecipeID(0)));
			Assert.IsNull(dataSource.GetRecipe(new FactoryTypeID("Undefined")));
		}



		[TestMethod]
		public void GetIngredientShouldReturnValidValue()
		{
			IIngredient ingredient;
			IDataSource dataSource;

			ingredient = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType="Type1",Rate=1 };
			dataSource = new DataSource();
			dataSource.AddIngredient(ingredient);

			Assert.AreEqual(ingredient, dataSource.GetIngredient(new IngredientID(1)));
		}
		[TestMethod]
		public void GetIngredientShouldReturnNull()
		{
			IIngredient ingredient;
			IDataSource dataSource;

			ingredient = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			dataSource = new DataSource();
			dataSource.AddIngredient(ingredient);

			Assert.IsNull(dataSource.GetIngredient(new IngredientID(0)));
		}
		[TestMethod]
		public void GetIngredientsShouldReturnValidValues()
		{
			IIngredient ingredient1, ingredient2, ingredient3;
			IDataSource dataSource;
			IIngredient[] results;

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(1), ResourceType = "Type3", Rate = 1 };

			dataSource = new DataSource();
			dataSource.AddIngredient(ingredient1);
			dataSource.AddIngredient(ingredient2);
			dataSource.AddIngredient(ingredient3);

			results = dataSource.GetIngredients(new RecipeID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(ingredient1));
			Assert.IsFalse(results.Contains(ingredient2));
			Assert.IsTrue(results.Contains(ingredient3));
		}
		[TestMethod]
		public void GetIngredientsShouldReturnNoValues()
		{
			IIngredient ingredient1, ingredient2, ingredient3;
			IDataSource dataSource;
			IIngredient[] results;

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(1), ResourceType = "Type3", Rate = 1 };

			dataSource = new DataSource();
			dataSource.AddIngredient(ingredient1);
			dataSource.AddIngredient(ingredient2);
			dataSource.AddIngredient(ingredient3);

			results = dataSource.GetIngredients(new RecipeID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}



		[TestMethod]
		public void GetProductShouldReturnValidValue()
		{
			IProduct product;
			IDataSource dataSource;

			product = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			dataSource = new DataSource();
			dataSource.AddProduct(product);

			Assert.AreEqual(product, dataSource.GetProduct(new ProductID(1)));
		}
		[TestMethod]
		public void GetProductShouldReturnNull()
		{
			IProduct product;
			IDataSource dataSource;

			product = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			dataSource = new DataSource();
			dataSource.AddProduct(product);

			Assert.IsNull(dataSource.GetProduct(new ProductID(0)));
		}
		[TestMethod]
		public void GetProductsShouldReturnValidValues()
		{
			IProduct product1, product2, product3;
			IDataSource dataSource;
			IProduct[] results;

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(1), ResourceType = "Type3", Rate = 1 };

			dataSource = new DataSource();
			dataSource.AddProduct(product1);
			dataSource.AddProduct(product2);
			dataSource.AddProduct(product3);

			results = dataSource.GetProducts(new RecipeID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(product1));
			Assert.IsFalse(results.Contains(product2));
			Assert.IsTrue(results.Contains(product3));
		}
		[TestMethod]
		public void GetProductsShouldReturnNoValues()
		{
			IProduct product1, product2, product3;
			IDataSource dataSource;
			IProduct[] results;

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(1), ResourceType = "Type3", Rate = 1 };

			dataSource = new DataSource();
			dataSource.AddProduct(product1);
			dataSource.AddProduct(product2);
			dataSource.AddProduct(product3);

			results = dataSource.GetProducts(new RecipeID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}


	}

}