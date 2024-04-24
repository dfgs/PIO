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
		public void GetIngredientsShouldLogErrorIfDataSourceThrowsException()
		{
			IRecipeManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IIngredient[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetIngredients(It.IsAny<RecipeID>())).Throws(new InvalidOperationException());

			factoryManager = new RecipeManager(logger, dataSource);
			result = factoryManager.GetIngredients(new RecipeID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Recipe ID 1]", "Failed", "ingredients"));
		}
		[TestMethod]
		public void GetIngredientShouldReturnValidValid()
		{
			IRecipeManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IIngredient[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			factoryManager = new RecipeManager(logger, dataSource);
			result = factoryManager.GetIngredients(new RecipeID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new IngredientID(1), result[0].ID);

			result = factoryManager.GetIngredients(new RecipeID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new IngredientID(2), result[0].ID);
		}

		[TestMethod]
		public void GetProductsShouldLogErrorIfDataSourceThrowsException()
		{
			IRecipeManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IProduct[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetProducts(It.IsAny<RecipeID>())).Throws(new InvalidOperationException());

			factoryManager = new RecipeManager(logger, dataSource);
			result = factoryManager.GetProducts(new RecipeID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Recipe ID 1]", "Failed", "products"));
		}
		[TestMethod]
		public void GetProductShouldReturnValidValid()
		{
			IRecipeManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IProduct[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			factoryManager = new RecipeManager(logger, dataSource);
			result = factoryManager.GetProducts(new RecipeID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ProductID(1), result[0].ID);

			result = factoryManager.GetProducts(new RecipeID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ProductID(2), result[0].ID);
		}



	}

}