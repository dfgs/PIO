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
		

	}

}