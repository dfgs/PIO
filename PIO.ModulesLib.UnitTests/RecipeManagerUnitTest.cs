using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class RecipeManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new RecipeManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new RecipeManager(NullLogger.Instance, null)); ;
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void GetRecipeShouldLogErrorIfDataSourceThrowsException()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IRecipe? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetRecipe(It.IsAny<FactoryTypeID>())).Throws(new InvalidOperationException());

			recipeManager = new RecipeManager(logger, dataSource);
			result = recipeManager.GetRecipe(new FactoryTypeID("Type1"));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[FactoryType ID Type1]", "Failed", "recipe"));

		}
		[TestMethod]
		public void GetRecipeShouldReturnValidValue()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IRecipe? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			recipeManager = new RecipeManager(logger, dataSource);
			
			result = recipeManager.GetRecipe(new FactoryTypeID("Type1"));
			Assert.IsNotNull(result);
			Assert.AreEqual(new RecipeID(1), result.ID);

			result = recipeManager.GetRecipe(new FactoryTypeID("Type2"));
			Assert.IsNotNull(result);
			Assert.AreEqual(new RecipeID(2), result.ID);
		}


		[TestMethod]
		public void GetIngredientsShouldLogErrorIfDataSourceThrowsException()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IIngredient[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Throws(new InvalidOperationException());

			recipeManager = new RecipeManager(logger, dataSource);
			result = recipeManager.GetIngredients(new RecipeID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Recipe ID 1]", "Failed", "ingredients"));
		}
		[TestMethod]
		public void GetIngredientShouldReturnValidValues()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IIngredient[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			recipeManager = new RecipeManager(logger, dataSource);
			result = recipeManager.GetIngredients(new RecipeID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new IngredientID(1), result[0].ID);

			result = recipeManager.GetIngredients(new RecipeID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new IngredientID(2), result[0].ID);
		}

		[TestMethod]
		public void GetProductsShouldLogErrorIfDataSourceThrowsException()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IProduct[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Throws(new InvalidOperationException());

			recipeManager = new RecipeManager(logger, dataSource);
			result = recipeManager.GetProducts(new RecipeID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Recipe ID 1]", "Failed", "products"));
		}
		[TestMethod]
		public void GetProductShouldReturnValidValues()
		{
			IRecipeManager recipeManager;
			IDataSource dataSource;
			DebugLogger logger;
			IProduct[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			recipeManager = new RecipeManager(logger, dataSource);
			result = recipeManager.GetProducts(new RecipeID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ProductID(1), result[0].ID);

			result = recipeManager.GetProducts(new RecipeID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ProductID(2), result[0].ID);
		}



	}

}