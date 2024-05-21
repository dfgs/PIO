using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class JobManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new JobManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new JobManager(NullLogger.Instance, null)); ;
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

	



		[TestMethod]
		public void GetJobsShouldLogErrorIfDataSourceThrowsException()
		{
			IJobManager jobManager;
			IDataSource dataSource;
			DebugLogger logger;
			IJob[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetJobs(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());

			jobManager = new JobManager(logger, dataSource);
			result = jobManager.GetJobs(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "jobs"));
		}
		[TestMethod]
		public void GetJobsShouldReturnValidValue()
		{
			IJobManager jobManager;
			IDataSource dataSource;
			DebugLogger logger;
			IJob[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			jobManager = new JobManager(logger, dataSource);
			result = jobManager.GetJobs(new FactoryID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new JobID(1), result[0].ID);

			result = jobManager.GetJobs(new FactoryID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new JobID(2), result[0].ID);
		}




		[TestMethod]
		public void GetJobShouldLogErrorIfDataSourceThrowsException()
		{
			IJobManager jobManager;
			IDataSource dataSource;
			DebugLogger logger;
			IJob? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetJob(It.IsAny<JobID>())).Throws(new InvalidOperationException());

			jobManager = new JobManager(logger, dataSource);
			result = jobManager.GetJob(new JobID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Job ID 1]", "Failed", "job"));
		}
		[TestMethod]
		public void GetJobShouldReturnValidValue()
		{
			IJobManager jobManager;
			IDataSource dataSource;
			DebugLogger logger;
			IJob? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			jobManager = new JobManager(logger, dataSource);
			result = jobManager.GetJob(new JobID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(new JobID(1), result.ID);

			result = jobManager.GetJob(new JobID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(new JobID(2), result.ID);
		}



		



	}

}