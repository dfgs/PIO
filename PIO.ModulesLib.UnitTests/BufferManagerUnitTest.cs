using Castle.Core.Logging;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Data.Common;
using NullLogger = LogLib.NullLogger;
using Buffer = PIO.CoreLib.Buffer;

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


		[TestMethod]
		public void IsBufferValidValidShouldReturnTrue()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2) };
			Assert.IsTrue(bufferManager.IsBufferValid(buffer));

			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, InitialUsage = 50 };
			Assert.IsTrue(bufferManager.IsBufferValid(buffer));

			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, InitialUsage = 100 };
			Assert.IsTrue(bufferManager.IsBufferValid(buffer));

			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 100 };
			Assert.IsTrue(bufferManager.IsBufferValid(buffer));
		}

		[TestMethod]
		public void IsBufferValidShouldReturnFalse()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			// negative InRate
			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = -1, OutRate = 1, Capacity = 100, InitialUsage = 50 };
			Assert.IsFalse(bufferManager.IsBufferValid(buffer));

			// negative OutRate
			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = -1, Capacity = 100, InitialUsage = 100 };
			Assert.IsFalse(bufferManager.IsBufferValid(buffer));

			// negative Capacity
			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = -100, InitialUsage = 100 };
			Assert.IsFalse(bufferManager.IsBufferValid(buffer));

			// negative Usage
			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, InitialUsage = -100 };
			Assert.IsFalse(bufferManager.IsBufferValid(buffer));

			// negative Space left
			buffer = new CoreLib.Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, InitialUsage = 200 };
			Assert.IsFalse(bufferManager.IsBufferValid(buffer));
		}

		[TestMethod]
		public void IsBufferValidShouldLogErrorIfParameterIsNull()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			bufferManager = new BufferManager(logger, dataSource);

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result = bufferManager.IsBufferValid(null);
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.AreEqual(false, result);
			Assert.AreEqual(1, logger.FatalCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal,  "Parameter", "null", "Buffer"));


		}


		[TestMethod]
		public void UpdateBufferShouldLogErrorIfParameterIsNull()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			bool result;

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			result = bufferManager.UpdateBuffer(null, 0);
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.AreEqual(false, result);
			Assert.AreEqual(1, logger.FatalCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Fatal, "Parameter", "null", "Buffer"));

		}

		[TestMethod]
		public void UpdateBufferShouldLogErrorIfCycleIsNegative()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;
			bool result;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 0 };

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			result = bufferManager.UpdateBuffer(buffer, -1);
			Assert.AreEqual(false, result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 1]","Cycle", "invalid"));

		}

		[TestMethod]
		public void UpdateBufferShouldLogErrorIfCycleIsGreaterThanETA()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;
			bool result;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 0 };

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			result = bufferManager.UpdateBuffer(buffer, 110);
			Assert.AreEqual(false, result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 1]", "Cycle", "invalid"));
		}

		[TestMethod]
		public void UpdateBufferShouldLogErrorIfBufferCannotBeUpdated()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;
			bool result;

			// buffer is filling
			buffer = Mock.Of<IBuffer>();
			Mock.Get(buffer).Setup(m => m.ID).Returns(new BufferID(1));
			Mock.Get(buffer).Setup(m => m.IsValid).Returns(true);
			Mock.Get(buffer).Setup(m => m.Update(It.IsAny<float>())).Throws<InvalidOperationException>() ;


			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			result = bufferManager.UpdateBuffer(buffer, 0);
			Assert.AreEqual(false, result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Buffer ID 1]", "Failed", "update"));
		}


		[TestMethod]
		public void UpdateBufferShouldReturnValidValue()
		{
			IDataSource dataSource;
			DebugLogger logger;
			IBufferManager bufferManager;
			IBuffer buffer;
			bool result;

			logger = new DebugLogger();
			dataSource = MockedData.GetMockedDataSource(MockedData.DataSource1);
			bufferManager = new BufferManager(logger, dataSource);

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 0 };
			result = bufferManager.UpdateBuffer(buffer, 10);
			Assert.IsTrue(result);
			Assert.AreEqual(10f, buffer.InitialUsage);

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 50 };
			result = bufferManager.UpdateBuffer(buffer, 10);
			Assert.IsTrue(result);
			Assert.AreEqual(60f, buffer.InitialUsage);

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, InitialUsage = 50 };
			result = bufferManager.UpdateBuffer(buffer, 50);
			Assert.IsTrue(result);
			Assert.AreEqual(100f, buffer.InitialUsage);

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, InitialUsage = 50 };
			result = bufferManager.UpdateBuffer(buffer, 10);
			Assert.IsTrue(result);
			Assert.AreEqual(40f, buffer.InitialUsage);

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, InitialUsage = 100 };
			result = bufferManager.UpdateBuffer(buffer, 10);
			Assert.IsTrue(result);
			Assert.AreEqual(90f, buffer.InitialUsage);

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, InitialUsage = 100 };
			result = bufferManager.UpdateBuffer(buffer, 100);
			Assert.IsTrue(result);
			Assert.AreEqual(0f, buffer.InitialUsage);

		}

	}

}