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
			Assert.ThrowsException<ArgumentNullException>(() => new UpdateManager(null, Mock.Of<IDataSource>(), Mock.Of<IFactoryManager>(), Mock.Of<ISubTaskManager>() ));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, null, Mock.Of<IFactoryManager>(), Mock.Of<ISubTaskManager>() ));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), null, Mock.Of<ISubTaskManager>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new UpdateManager(NullLogger.Instance, Mock.Of<IDataSource>(), Mock.Of<IFactoryManager>(), null ));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}



		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetFactories()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			IFactoryManager factoryManager;
			ISubTaskManager subTaskManager;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();

			factoryManager = Mock.Of<IFactoryManager>();
			Mock.Get(factoryManager).Setup(m=>m.GetFactories()).Returns((IFactory[]?)null);

			subTaskManager = MockedData.GetMockedSubTaskManager(MockedData.DataSource1);

			updateManager = new UpdateManager(logger, dataSource, factoryManager,subTaskManager);
			result=updateManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "Fail" , "get","factories"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetSubTasks()
		{
			IUpdateManager updateManager;
			IDataSource dataSource;
			IFactoryManager factoryManager;
			ISubTaskManager subTaskManager;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();

			factoryManager = MockedData.GetMockedFactoryManager(MockedData.DataSource1);

			subTaskManager = Mock.Of<ISubTaskManager>();
			Mock.Get(subTaskManager).Setup(m => m.GetSubTasks()).Returns((ISubTask[]?)null);

			updateManager = new UpdateManager(logger, dataSource, factoryManager, subTaskManager);
			result = updateManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "Fail", "get", "sub tasks"));
		}

	}

}