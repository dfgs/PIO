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

			factory1 = new Factory() { ID = new FactoryID(1), FactoryType = "Type1" };
			factory2 = Mock.Of<IFactory>();
			Mock.Get(factory2).Setup(m => m.ID).Returns(new FactoryID(2));
			Mock.Get(factory2).Setup(m => m.FactoryType).Returns("Type2");
			factory3 = new Factory() { ID = new FactoryID(3), FactoryType = "Type3" };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryType = "Type1" };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryType = "Type2" };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryType = "Type3" };

			ingredient1 = new Ingredient() { ID = new IngredientID(1), RecipeID = new RecipeID(1), ResourceType = "Type1", Rate = 1 };
			ingredient2 = new Ingredient() { ID = new IngredientID(2), RecipeID = new RecipeID(2), ResourceType = "Type2", Rate = 1 };
			ingredient3 = new Ingredient() { ID = new IngredientID(3), RecipeID = new RecipeID(3), ResourceType = "Type3", Rate = 1 };

			product1 = new Product() { ID = new ProductID(1), RecipeID = new RecipeID(1), ResourceType = "Type2", Rate = 1 };
			product2 = new Product() { ID = new ProductID(2), RecipeID = new RecipeID(2), ResourceType = "Type3", Rate = 1 };
			product3 = new Product() { ID = new ProductID(3), RecipeID = new RecipeID(3), ResourceType = "Type4", Rate = 1 };

			inputConnector1 = new InputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			inputConnector2 = new InputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type2" };
			inputConnector3 = new InputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type3" };

			outputConnector1 = new OutputConnector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type2" };
			outputConnector2 = new OutputConnector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type3" };
			outputConnector3 = new OutputConnector() { ID = new ConnectorID(3), FactoryID = new FactoryID(3), ResourceType = "Type4" };

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(1))).Returns(factory1);
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(2))).Returns(factory2);
			Mock.Get(dataSource).Setup(m => m.GetFactory(new FactoryID(3))).Returns(factory3);

			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type1")).Returns(recipe1);
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type2")).Returns(recipe2);
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type3")).Returns(recipe3);

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

			return dataSource;
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new FactoryManager(null, Mock.Of<TopologySorter>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new FactoryManager(NullLogger.Instance, null ));
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

			factoryManager = new FactoryManager(logger, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result=factoryManager.GetEfficiency(new FactoryID(2), null, Enumerable.Empty<IConnector>());
			Assert.AreEqual(0f,result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "Parameter", "null", "Ingredients"));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

			factoryManager = new FactoryManager(logger, topologySorter);
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			result = factoryManager.GetEfficiency(new FactoryID(2), Enumerable.Empty<IIngredient>(), null);
			Assert.AreEqual(0f, result);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "Parameter", "null", "Connectors"));
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2),[], []);
			Assert.AreEqual(1f, result);
		}
		[TestMethod]
		public void GetEfficiencyShouldReturn0WhenConnectorIsMissing()
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1,ingredient2], [connector1,connector2]);
			Assert.AreEqual(0f, result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "connector", "ingredient", "[Factory ID 2]"));

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

			factoryManager = new FactoryManager(logger, topologySorter);
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.GetEfficiency(new FactoryID(2), [ingredient1, ingredient2], [connector1, connector2]);
			Assert.AreEqual(0.5f, result);
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

			factoryManager = new FactoryManager(logger,topologySorter);
			result=factoryManager.Update(dataSource, 0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "sort","factories"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetRecipe()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type2")).Throws<InvalidOperationException>();

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "recipe", "[Factory ID 2]"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfRecipeIsNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IFactory factory;
			bool result;

			logger = new DebugLogger();

			dataSource = GetMockedDataSource();
			factory = dataSource.GetFactory(new FactoryID(2))!;
			Mock.Get(factory).Setup(m => m.FactoryType).Returns("TypeError");

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([dataSource.GetFactory(new FactoryID(1)), dataSource.GetFactory(new FactoryID(2)), dataSource.GetFactory(new FactoryID(3))]);

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "product", "[Recipe ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetInputConnectors()
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "input","connectors", "[Factory ID 2]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetOutputConnectors()
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

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "output", "connectors", "[Factory ID 2]"));
		}

	}

}