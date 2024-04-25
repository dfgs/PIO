using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class BufferManagerUnitTest
	{
		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new BufferManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new BufferManager(NullLogger.Instance, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetBuffers()
		{
			BufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Throws<InvalidOperationException>();

			bufferManager = new BufferManager(logger,dataSource);
			result=bufferManager.Update(0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "buffers"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfInvalidBufferIsDetected()
		{
			BufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1=Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(12));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(false);

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1]);


			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "invalid state", "[Buffer ID 12]"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotUpdateBuffer()
		{
			BufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1 = Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(12));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(true);
			Mock.Get(buffer1).Setup(m => m.Update(It.IsAny<float>())).Throws<InvalidOperationException>();


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1]);

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.Update(0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "update", "[Buffer ID 12]"));
		}

		[TestMethod]
		public void UpdateShouldUpdateBuffers()
		{
			BufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1,buffer2,buffer3;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1 = Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(1));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(true);

			buffer2 = Mock.Of<IBuffer>();
			Mock.Get(buffer2).Setup(m => m.ID).Returns(new BufferID(2));
			Mock.Get(buffer2).Setup(m => m.IsValid).Returns(true);

			buffer3 = Mock.Of<IBuffer>();
			Mock.Get(buffer3).Setup(m => m.ID).Returns(new BufferID(3));
			Mock.Get(buffer3).Setup(m => m.IsValid).Returns(true);


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1,buffer2,buffer3]);

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.Update( 0);
			Assert.IsTrue(result);
			Assert.AreEqual(0, logger.ErrorCount);
			Mock.Get(buffer1).Verify(m => m.Update(0), Times.Once);
			Mock.Get(buffer2).Verify(m => m.Update(0), Times.Once);
			Mock.Get(buffer3).Verify(m => m.Update(0), Times.Once);
		}

		[TestMethod]
		public void GetBuffersShouldLogErrorIfDataSourceThrowsException()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Throws(new InvalidOperationException());

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffers();
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error,  "Failed", "get", "buffers"));
		}
		[TestMethod]
		public void GetBuffersShouldReturnValidValue()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffers();
			Assert.IsNotNull(result);
			Assert.AreEqual(6, result.Length);
			Assert.AreEqual(new BufferID(1), result[0].ID);
			Assert.AreEqual(new BufferID(2), result[1].ID);
		}


		[TestMethod]
		public void GetBufferFromIDShouldLogErrorIfDataSourceThrowsException()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffer(It.IsAny<BufferID>())).Throws(new InvalidOperationException());

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffer(new BufferID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 1]", "Failed", "get", "buffer"));
		}
		[TestMethod]
		public void GetBufferFromIDShouldReturnValidValue()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffer(new BufferID(4));
			Assert.IsNotNull(result);
			Assert.AreEqual(new BufferID(4), result.ID);
		}

		[TestMethod]
		public void GetBufferFromConnectorIDShouldLogErrorIfDataSourceThrowsException()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffer(It.IsAny<ConnectorID>())).Throws(new InvalidOperationException());

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffer(new ConnectorID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Connector ID 1]", "Failed", "get", "buffer"));
		}
		[TestMethod]
		public void GetBufferFromConnectorIDShouldReturnValidValue()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			IBuffer? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);

			bufferManager = new BufferManager(logger, dataSource);
			result = bufferManager.GetBuffer(new ConnectorID(4));
			Assert.IsNotNull(result);
			Assert.AreEqual(new BufferID(4), result.ID);
		}


	}

}