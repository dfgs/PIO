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
		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new FactoryManager(null, Mock.Of<TopologySorter>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new FactoryManager(NullLogger.Instance, null ));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

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
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			bool result;

			logger = new DebugLogger();

			factory1 = new Factory() { ID = new FactoryID(1), FactoryType = "Type1" };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryType = "Type2" };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryType = "Type3" };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryType = "Type1" };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryType = "Type2" };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type1")).Returns(recipe1);
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type2")).Throws<InvalidOperationException>();
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type3")).Returns(recipe3);

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([factory1,factory2,factory3]);
			
			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "get", "recipe", "ID 2"));
		}
		[TestMethod]
		public void UpdateShouldLogErrorIfRecipeIsNotFound()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			ITopologySorter topologySorter;
			IFactory factory1, factory2, factory3;
			IRecipe recipe1, recipe2, recipe3;
			bool result;

			logger = new DebugLogger();

			factory1 = new Factory() { ID = new FactoryID(1), FactoryType = "Type1" };
			factory2 = new Factory() { ID = new FactoryID(2), FactoryType = "TypeError" };
			factory3 = new Factory() { ID = new FactoryID(3), FactoryType = "Type3" };

			recipe1 = new Recipe() { ID = new RecipeID(1), FactoryType = "Type1" };
			recipe2 = new Recipe() { ID = new RecipeID(2), FactoryType = "Type2" };
			recipe3 = new Recipe() { ID = new RecipeID(3), FactoryType = "Type3" };

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type1")).Returns(recipe1);
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type2")).Returns(recipe2);
			Mock.Get(dataSource).Setup(m => m.GetRecipe("Type3")).Returns(recipe3);

			topologySorter = Mock.Of<ITopologySorter>();
			Mock.Get(topologySorter).Setup(m => m.Sort(It.IsAny<IDataSource>())).Returns([factory1, factory2, factory3]);

			factoryManager = new FactoryManager(logger, topologySorter);
			result = factoryManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.WarningCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Warning, "found", "recipe", "ID 2"));
		}
	}

}