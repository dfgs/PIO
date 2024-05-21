using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class SubTaskManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new SubTaskManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new SubTaskManager(NullLogger.Instance, null)); ;
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

	



		[TestMethod]
		public void GetSubTasksUsingJobIDShouldLogErrorIfDataSourceThrowsException()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetSubTasks(It.IsAny<JobID>())).Throws(new InvalidOperationException());

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTasks(new JobID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Job ID 1]", "Failed", "sub tasks"));
		}
		[TestMethod]
		public void GetSubTasksUsingJobIDShouldReturnValidValue()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTasks(new JobID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new SubTaskID(1), result[0].ID);

			result = subTaskManager.GetSubTasks(new JobID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new SubTaskID(2), result[0].ID);
		}

		[TestMethod]
		public void GetSubTasksShouldLogErrorIfDataSourceThrowsException()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetSubTasks()).Throws(new InvalidOperationException());

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTasks();
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Job ID NA]", "Failed", "sub tasks"));
		}
		[TestMethod]
		public void GetSubTasksShouldReturnValidValue()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTasks();
			Assert.IsNotNull(result);
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual(new SubTaskID(1), result[0].ID);
			Assert.AreEqual(new SubTaskID(2), result[1].ID);
			Assert.AreEqual(new SubTaskID(3), result[2].ID);


		}


		[TestMethod]
		public void GetSubTaskShouldLogErrorIfDataSourceThrowsException()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetSubTask(It.IsAny<SubTaskID>())).Throws(new InvalidOperationException());

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTask(new SubTaskID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[SubTask ID 1]", "Failed", "sub task"));
		}
		[TestMethod]
		public void GetSubTaskShouldReturnValidValue()
		{
			ISubTaskManager subTaskManager;
			IDataSource dataSource;
			DebugLogger logger;
			ISubTask? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			subTaskManager = new SubTaskManager(logger, dataSource);
			result = subTaskManager.GetSubTask(new SubTaskID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(new SubTaskID(1), result.ID);

			result = subTaskManager.GetSubTask(new SubTaskID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(new SubTaskID(2), result.ID);
		}



		



	}

}