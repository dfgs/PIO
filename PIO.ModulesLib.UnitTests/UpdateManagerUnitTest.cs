using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class UpdateManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new UpdateManager(null, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>(),Mock.Of<IBufferManager>() ));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, null, Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>(), Mock.Of<IBufferManager>())); ;
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), null, Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>(), Mock.Of<IBufferManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), null, Mock.Of<IConnectionManager>(), Mock.Of<IBufferManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), null, Mock.Of<IBufferManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<ITopologySorter>(), Mock.Of<IRecipeManager>(), Mock.Of<IConnectionManager>(), null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = Mock.Of<IConnectionManager>();
			bufferManager = Mock.Of<IBufferManager>();

			updateManager = new UpdateManager(logger,dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result=updateManager.GetEfficiency(new FactoryID(2), null, Enumerable.Empty<IConnector>());
			Assert.AreEqual(0f,result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Ingredients"));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result = updateManager.GetEfficiency(new FactoryID(2), Enumerable.Empty<IIngredient>(), null);
			Assert.AreEqual(0f, result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = Mock.Of<IConnectionManager>();
			bufferManager = Mock.Of<IBufferManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "TypeError", Rate = 1 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = Mock.Of<IConnectionManager>();
			bufferManager = Mock.Of<IBufferManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 2 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 2 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = Mock.Of<IConnectionManager>();
			bufferManager = Mock.Of<IBufferManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 2 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 1 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 1 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = Mock.Of<IConnectionManager>();
			bufferManager = Mock.Of<IBufferManager>();

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			float result;

			logger = new DebugLogger();

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 0 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 0 };

			connector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1", Rate = 0 };
			connector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2", Rate = 0 };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result = updateManager.UpdateConnectors(new FactoryID(2), 1f,null, Enumerable.Empty<IConnector>());
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Products"));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result = updateManager.UpdateConnectors(new FactoryID(2), 1f, Enumerable.Empty<IProduct>(), null);
			Assert.IsFalse(result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "[Factory ID 2]", "Parameter", "null", "Connectors"));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type4" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;


			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			logger = new DebugLogger();
			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
			result = updateManager.UpdateConnectors(new FactoryID(2), 2, [product1, product2], [connector1, connector2]);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 2]", "Efficiency"));


			logger = new DebugLogger();
			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 2 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 4 };

			connector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			connector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			topologySorter = MockedData.GetMockedTopologySorter();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();
			
			topologySorter=Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m=>m.Sort(It.IsAny<IDataSource>())).Throws<InvalidOperationException>();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
			result=updateManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "sort","factories"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetBuffers()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();

			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);
			Mock.Get(bufferManager).Setup(m => m.GetBuffers()).Returns((IBuffer[]?)null);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error,"Failed", "get", "buffers"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfBufferIsInInvalidState()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();

			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);
			Mock.Get(bufferManager).Setup(m => m.IsBufferValid(bufferManager.GetBuffer(new BufferID(2))!)).Returns(false);
			
			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 2]","Buffer", "invalid", "state"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfBufferCannotBeUpdated()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();

			topologySorter = Mock.Of<ITopologySorter>();
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);
			Mock.Get(bufferManager).Setup(m => m.UpdateBuffer(bufferManager.GetBuffer(new BufferID(2))!,0f)).Returns(false);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
			result = updateManager.Update(0f);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 2]", "Failed", "update"));
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			Mock.Get(recipeManager).Setup(m => m.GetRecipe(new FactoryTypeID("Type2"))).Returns((IRecipe?)null);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();


			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			Mock.Get(recipeManager).Setup(m => m.GetIngredients(new RecipeID(2))).Returns((IIngredient[]?)null);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			Mock.Get(recipeManager).Setup(m => m.GetProducts(new RecipeID(2))).Returns((IProduct[]?)null);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
	
			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnectors(new FactoryID(2))).Returns((IInputConnector[]?)null);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
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
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			Mock.Get(connectionManager).Setup(m => m.GetOutputConnectors(new FactoryID(2))).Returns((IOutputConnector[]?)null);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed", "get", "output", "connectors", "[Factory ID 2]"));
		}


		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetConnections()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			Mock.Get(connectionManager).Setup(m => m.GetConnections(new ConnectorID(5))).Returns((IConnection[]?)null);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed", "get",  "connections", "[Connector ID 5]"));
		}

		[TestMethod]
		public void UpdateShouldLogWarningIfCannotGetDestinationConnector()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			Mock.Get(connectionManager).Setup(m => m.GetInputConnector(new ConnectorID(2))).Returns((IInputConnector?)null);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager,connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "Failed","get", "destination", "connector", "[Connection ID 1]"));
		}


		[TestMethod]
		public void UpdateShouldSetValidRatesInConnectors1()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			topologySorter = MockedData.GetMockedTopologySorter();
			
			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource1);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource1);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(0, logger.WarningCount);
			Assert.AreEqual(0, logger.ErrorCount);
			Assert.AreEqual(0, logger.FatalCount);
			Assert.AreEqual(1f, dataSource.GetOutputConnector(new ConnectorID(4))!.Rate);
			Assert.AreEqual(0.5f, dataSource.GetOutputConnector(new ConnectorID(5))!.Rate);
			Assert.AreEqual(0.25f, dataSource.GetOutputConnector(new ConnectorID(6))!.Rate);

		}

		[TestMethod]
		public void UpdateShouldSetValidRatesInConnectors2()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IRecipeManager recipeManager;
			IConnectionManager connectionManager;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource2);

			topologySorter = MockedData.GetMockedTopologySorter();

			recipeManager = MockedData.GetMockedRecipeManager(MockedData.DataSource2);
			connectionManager = MockedData.GetMockedConnectionManager(MockedData.DataSource2);
			bufferManager = MockedData.GetMockedBufferManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, topologySorter, recipeManager, connectionManager, bufferManager);
			result = updateManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(0, logger.WarningCount);
			Assert.AreEqual(0, logger.ErrorCount);
			Assert.AreEqual(0, logger.FatalCount);
			Assert.AreEqual(1f, dataSource.GetOutputConnector(new ConnectorID(5))!.Rate);
			Assert.AreEqual(2f, dataSource.GetOutputConnector(new ConnectorID(6))!.Rate);
			Assert.AreEqual(0.5f, dataSource.GetOutputConnector(new ConnectorID(7))!.Rate);

		}



	}

}