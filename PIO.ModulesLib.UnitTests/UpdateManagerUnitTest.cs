using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class UpdateManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new UpdateManager(null, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>() ));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, null, Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>())); ;
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), null, Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), null, Mock.Of<IConnectionManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(),  Mock.Of<IRecipeManager>(),null));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}

		[TestMethod]
		public void GetEfficiencyShouldLogErrorIfParameterIsNull()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = Mock.Of<IConnectionManager>();
			
			updateManager = new UpdateManager(logger,dataSource, topologySorter, recipeManager,connectionManager);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result=updateManager.GetEfficiency(new FactoryID(2), null, Enumerable.Empty<IConnector>());
			Assert.AreEqual(0f,result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Ingredients"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = updateManager.GetEfficiency(new FactoryID(2), Enumerable.Empty<IIngredient>(), null);
			Assert.AreEqual(0f, result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenNoIngredientsAreNeeded()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = Mock.Of<IConnectionManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.GetEfficiency(new FactoryID(2),[], []);
			Assert.AreEqual(1f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturn0AndLogWarningWhenConnectorIsMissing()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = Mock.Of<IConnectionManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.GetEfficiency(new FactoryID(2), [ingredient1,ingredient2], [connector1,connector2]);
			Assert.AreEqual(0f, result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "[Factory ID 2]", "connector", "ingredient"));

		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenConnectorRateIsHigherThanRecipeRate()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = Mock.Of<IConnectionManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(1f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturnOneHalfWhenConnectorRateIsLowerThanRecipeRate()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = Mock.Of<IConnectionManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(0.5f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturn1WhenIngredientRateIs0()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(1f, result);
		}
		
		
		
		




		


		[TestMethod]
		public void UpdateConnectorsShouldLogErrorIfParameterIsNull()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = updateManager.UpdateConnectors(new FactoryID(2), 1f,null, Enumerable.Empty<IConnector>());
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Products"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = updateManager.UpdateConnectors(new FactoryID(2), 1f, Enumerable.Empty<IProduct>(), null);
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}

		[TestMethod]
		public void UpdateConnectorsShouldLogWarningIfConnectorIsMissing()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 1, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "[Factory ID 2]", "connector", "product"));

		}

		[TestMethod]
		public void UpdateConnectorsShouldLogErrorIfEfficencyIsInvalid()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IProduct product1, product2;
			IConnector connector1, connector2;
			bool result;


			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			logger = new DebugLogger();
			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 2, [product1, product2], [connector1, connector2]);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 2]", "Efficiency"));


			logger = new DebugLogger();
			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), -1, [product1, product2], [connector1, connector2]);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 2]", "Efficiency","-1"));
		}

		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIs1()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 1, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(2, connector1.Rate);
			Assert.AreEqual(4, connector2.Rate);
		}

		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIs0()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 0, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(0, connector1.Rate);
			Assert.AreEqual(0, connector2.Rate);
		}
		[TestMethod]
		public void UpdateConnectorsShouldApplyCorrectRatesWhenEfficiencyIsOneHalf()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
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
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 0.5f, [product1, product2], [connector1, connector2]);
			Assert.IsTrue(result);
			Assert.AreEqual(1, connector1.Rate);
			Assert.AreEqual(2, connector2.Rate);
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotSortFactories()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();
			
			topologySorter=Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m=>m.Sort(It.IsAny<IDataSource>())).Throws<InvalidOperationException>();
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result=updateManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "sort","factories"));
		}
		
		[TestMethod]
		public void UpdateShouldLogWarningIfRecipeIsNotFound()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(new FactoryTypeID("Type2"))).Returns<IRecipe?>(null);
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "found", "recipe", "[Factory ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetIngredients()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();


			dataSource = MockedData.GetMockedDataSource();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(new RecipeID(2))).Returns<IIngredient[]?>(null);
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed","get", "ingredients", "[Factory ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetProducts()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			Mock.Get(recipeManager).Setup(m => m.GetProducts(new RecipeID(2))).Returns<IIngredient[]?>(null);
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed", "get", "products", "[Factory ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetInputConnectors()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();
	
			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(new FactoryID(2))).Returns<IInputConnector[]?>(null);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed","get", "input","connectors", "[Factory ID 2]"));
		}
		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetOutputConnectors()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(new FactoryID(2))).Returns<IOutputConnector[]?>(null);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed", "get", "output", "connectors", "[Factory ID 2]"));
		}


		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetConnections()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetConnections(new ConnectorID(5))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "connections",  "[Connector ID 5]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetDestinationConnector()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(2))).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "destination", "connector", "[Connection ID 1]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfDestinationConnectorIsNotFound()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(new ConnectorID(2))).Returns<IInputConnector?>(null);

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);
			recipeManager = MockedData.GetMockedRecipeManager();
			connectionManager = MockedData.GetConnectionManager();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "destination", "connector", "[Connection ID 1]"));
		}


	}

}