using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class FactoryManagerUnitTest
	{
		// create Mocked DataSource for test purpose
		private IDataSource GetMockedDataSource()
		{
			IDataSource dataSource;
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			IIngredient ingredient1, ingredient2, ingredient3;
			IProduct product1, product2, product3;
			IInputConnector inputConnector1, inputConnector2, inputConnector3;
			IOutputConnector outputConnector1, outputConnector2, outputConnector3;
			IConnection connection1, connection2;

			factory1 = new Factory() { ID = new FactoryID(1), FactoryTypeID = new FactoryTypeID( "Type1") };
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

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1",Rate=1 };
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

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new FactoryManager(null, Mock.Of<IDataSource>(), Mock.Of<TopologySorter>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new FactoryManager(NullLogger.Instance, null, Mock.Of<TopologySorter>())); ;
			Assert.ThrowsException<PIOInvalidParameterException>(() => new FactoryManager(NullLogger.Instance, Mock.Of<IDataSource>(), null));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}

		[TestMethod]
		public void GetEfficiencyShouldLogErrorIfParameterIsNull()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger,dataSource, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result=factoryManager.GetEfficiency(new FactoryID(2), null, Enumerable.Empty<IConnector>());
			Assert.AreEqual(0f,result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Ingredients"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = factoryManager.GetEfficiency(new FactoryID(2), Enumerable.Empty<IIngredient>(), null);
			Assert.AreEqual(0f, result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenNoIngredientsAreNeeded()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2),[], []);
			Assert.AreEqual(1f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturn0AndLogWarningWhenConnectorIsMissing()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IIngredient ingredient1, ingredient2;
			IConnector connector1, connector2;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "TypeError", Rate = 1 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1,ingredient2], [connector1,connector2]);
			Assert.AreEqual(0f, result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "[Factory ID 2]", "connector", "ingredient"));

		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenConnectorRateIsHigherThanRecipeRate()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IIngredient ingredient1, ingredient2;
			IConnector connector1, connector2;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 2 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 2 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(1f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturnOneHalfWhenConnectorRateIsLowerThanRecipeRate()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IIngredient ingredient1, ingredient2;
			IConnector connector1, connector2;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 2 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 1 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(0.5f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenIngredientRateIs0()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IIngredient ingredient1, ingredient2;
			IConnector connector1, connector2;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 0 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 0 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 0 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 0 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(1f, result);
		}
		
		
		
		[TestMethod]
		public void GetRecipeShouldLogErrorIfDataSourceThrowsException()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipe? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Throws(new InvalidOperationException());
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result=factoryManager.GetRecipe(new FactoryTypeID("Type1"));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[FactoryType ID Type1]", "Failed", "recipe"));

		}
		[TestMethod]
		public void GetRecipeShouldReturnValidValid()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipe? result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetRecipe(new FactoryTypeID("Type1"));
			Assert.IsNotNull(result);
			Assert.AreEqual(new RecipeID(1), result.ID);
			
			result = factoryManager.GetRecipe(new FactoryTypeID("Type2"));
			Assert.IsNotNull(result);
			Assert.AreEqual(new RecipeID(2), result.ID);
		}




		[TestMethod]
		public void GetInputConnectorsShouldLogErrorIfDataSourceThrowsException()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IInputConnector[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetInputConnectors(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "input", "connectors"));
		}
		[TestMethod]
		public void GetInputConnectorShouldReturnValidValid()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IInputConnector[]? result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetInputConnectors(new FactoryID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(1), result[0].ID);

			result = factoryManager.GetInputConnectors(new FactoryID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(2), result[0].ID);
		}

		[TestMethod]
		public void GetOutputConnectorsShouldLogErrorIfDataSourceThrowsException()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IOutputConnector[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetOutputConnectors(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "output", "connectors"));
		}
		[TestMethod]
		public void GetOutputConnectorShouldReturnValidValid()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IOutputConnector[]? result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.GetOutputConnectors(new FactoryID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(4), result[0].ID);

			result = factoryManager.GetOutputConnectors(new FactoryID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(5), result[0].ID);
		}


		[TestMethod]
		public void UpdateConnectorsShouldLogErrorIfParameterIsNull()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = factoryManager.UpdateConnectors(new FactoryID(2), 1f,null, Enumerable.Empty<IConnector>());
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Products"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = factoryManager.UpdateConnectors(new FactoryID(2), 1f, Enumerable.Empty<IProduct>(), null);
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}

		[TestMethod]
		public void UpdateConnectorsShouldLogWarningIfConnectorIsMissing()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type4" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), 1, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "[Factory ID 2]", "connector", "product"));

		}

		[TestMethod]
		public void UpdateConnectorsShouldLogErrorIfEfficencyIsInvalid()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;


			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			logger = new DebugLogger();
			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), 2, [product1, product2], [connector1, connector2]);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 2]", "Efficiency"));


			logger = new DebugLogger();
			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), -1, [product1, product2], [connector1, connector2]);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 2]", "Efficiency","-1"));
		}

		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIs1()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), 1, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(2, connector1.Rate);
			Assert.AreEqual(4, connector2.Rate);
		}

		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIs0()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), 0, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(0, connector1.Rate);
			Assert.AreEqual(0, connector2.Rate);
		}
		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIsOneHalf()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.UpdateConnectors(new FactoryID(2), 0.5f, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(1, connector1.Rate);
			Assert.AreEqual(2, connector2.Rate);
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotSortFactories()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();
			
			topologySorter=Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m=>m.Sort(It.IsAny<IDataSource>())).Throws<InvalidOperationException>();

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result=factoryManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "sort","factories"));
		}
		
		[TestMethod]
		public void UpdateShouldLogWarningIfRecipeIsNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetRecipe(new FactoryTypeID("Type2"))).Throws(new InvalidOperationException());

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "found", "recipe", "[Factory ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetIngredients()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();


			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetIngredients(new RecipeID(2))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "ingredient", "[Recipe ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetProducts()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetProducts(new RecipeID(2))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "product", "[Recipe ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfInputConnectorsAreNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(new FactoryID(2))).Throws<InvalidOperationException>();
	
			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "found", "input","connectors", "[Factory ID 2]"));
		}
		[TestMethod]
		public void UpdateShouldLogWarningIfOutputConnectorsAreNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(new FactoryID(2))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "found", "output", "connectors", "[Factory ID 2]"));
		}


		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetConnections()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetConnections(new ConnectorID(5))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "connections",  "[Connector ID 5]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetDestinationConnector()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(2))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "destination", "connector", "[Connection ID 1]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfDestinationConnectorIsNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(2))).Returns<IInputConnector?>(null);

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, dataSource, topologySorter);
			result = factoryManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "destination", "connector", "[Connection ID 1]"));
		}


	}

}