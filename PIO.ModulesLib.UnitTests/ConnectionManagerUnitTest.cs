using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class ConnectionManagerUnitTest
	{

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new ConnectionManager(null, Mock.Of<IDataSource>()));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new ConnectionManager(NullLogger.Instance, null)); ;
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

		}

	



		[TestMethod]
		public void GetInputConnectorsShouldLogErrorIfDataSourceThrowsException()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IInputConnector[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetInputConnectors(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetInputConnectors(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "input", "connectors"));
		}
		[TestMethod]
		public void GetInputConnectorShouldReturnValidValid()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IInputConnector[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetInputConnectors(new FactoryID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(1), result[0].ID);

			result = connectionManager.GetInputConnectors(new FactoryID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(2), result[0].ID);
		}

		[TestMethod]
		public void GetOutputConnectorsShouldLogErrorIfDataSourceThrowsException()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IOutputConnector[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetOutputConnectors(It.IsAny<FactoryID>())).Throws(new InvalidOperationException());

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetOutputConnectors(new FactoryID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Factory ID 1]", "Failed", "output", "connectors"));
		}
		[TestMethod]
		public void GetOutputConnectorShouldReturnValidValid()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IOutputConnector[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetOutputConnectors(new FactoryID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(4), result[0].ID);

			result = connectionManager.GetOutputConnectors(new FactoryID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectorID(5), result[0].ID);
		}



		[TestMethod]
		public void GetInputConnectorShouldLogErrorIfDataSourceThrowsException()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IInputConnector? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetInputConnector(It.IsAny<ConnectorID>())).Throws(new InvalidOperationException());

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetInputConnector(new ConnectorID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Connector ID 1]", "Failed", "input", "connector"));
		}
		[TestMethod]
		public void GetInputConnectoShouldReturnValidValid()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IInputConnector? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetInputConnector(new ConnectorID(1));
			Assert.IsNotNull(result);
			Assert.AreEqual(new ConnectorID(1), result.ID);

			result = connectionManager.GetInputConnector(new ConnectorID(2));
			Assert.IsNotNull(result);
			Assert.AreEqual(new ConnectorID(2), result.ID);
		}



		[TestMethod]
		public void GetConnectionsShouldLogErrorIfDataSourceThrowsException()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IConnection[]? result;

			logger = new DebugLogger();

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetConnections(It.IsAny<ConnectorID>())).Throws(new InvalidOperationException());

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetConnections(new ConnectorID(1));
			Assert.IsNull(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "[Connector ID 1]", "Failed", "connections"));
		}
		[TestMethod]
		public void GetConnectionShouldReturnValidValid()
		{
			IConnectionManager connectionManager;
			IDataSource dataSource;
			DebugLogger logger;
			IConnection[]? result;

			logger = new DebugLogger();

			dataSource = MockedData.GetMockedDataSource();

			connectionManager = new ConnectionManager(logger, dataSource);
			result = connectionManager.GetConnections(new ConnectorID(4));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectionID(1), result[0].ID);

			result = connectionManager.GetConnections(new ConnectorID(5));
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(new ConnectionID(2), result[0].ID);
		}



	}

}