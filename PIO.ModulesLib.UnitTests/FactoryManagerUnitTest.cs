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
			Assert.ThrowsException<ArgumentNullException>(() => new FactoryManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new FactoryManager(NullLogger.Instance, null));

#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}


		[TestMethod]
		public void GetFactoriesShouldLogErrorIfDataSourceThrowsException()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IFactory[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetFactories()).Throws(new InvalidOperationException());

			factoryManager = new FactoryManager(logger, dataSource);
			result = factoryManager.GetFactories();
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID NA]", "Failed", "get", "factories"));
		}
		[TestMethod]
		public void GetFactoriesShouldReturnValidValue()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IFactory[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			factoryManager = new FactoryManager(logger, dataSource);
			result = factoryManager.GetFactories();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual(new FactoryID(1), result[0].ID);
			Assert.AreEqual(new FactoryID(2), result[1].ID);
		}


		[TestMethod]
		public void GetFactoryShouldLogErrorIfDataSourceThrowsException()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IFactory? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetFactory(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());

			factoryManager = new FactoryManager(logger, dataSource);
			result = factoryManager.GetFactory(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "get", "factory"));
		}
		[TestMethod]
		public void GetFactoryShouldReturnValidValue()
		{
			IFactoryManager factoryManager;
			IDataSource dataSource;
			DebugLogger logger;
			IFactory? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			factoryManager = new FactoryManager(logger, dataSource);
			result = factoryManager.GetFactory(new FactoryID(3));
			Assert.IsNotNull(result);
			Assert.AreEqual(new FactoryID(3), result.ID);
		}
	}

}