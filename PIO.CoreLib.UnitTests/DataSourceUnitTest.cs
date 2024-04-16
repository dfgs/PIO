using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class DataSourceUnitTest
	{
		

		[TestMethod]
		public void AddFactoryShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource=new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddFactory(null) );
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddConnectorShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddConnector(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void AddConnectionShouldThrowExceptionIfParameterIsNull()
		{
			IDataSource dataSource;

			dataSource = new DataSource();

#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => dataSource.AddConnection(null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}


		[TestMethod]
		public void GetFactoryShouldReturnValidValue()
		{
			Factory factory;
			IDataSource dataSource;

			factory = new Factory() { ID = new FactoryID(1), FactoryType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddFactory(factory);

			Assert.AreEqual(factory,dataSource.GetFactory(new FactoryID(1)));
		}
		[TestMethod]
		public void GetFactoryShouldReturnNull()
		{
			Factory factory;
			IDataSource dataSource;

			factory = new Factory() { ID = new FactoryID(1), FactoryType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddFactory(factory);

			Assert.IsNull(dataSource.GetFactory(new FactoryID(0)));
		}


		[TestMethod]
		public void GetConnectorShouldReturnValidValue()
		{
			Connector connector;
			IDataSource dataSource;

			connector = new Connector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1),ResourceType="Type1" };
			dataSource = new DataSource();
			dataSource.AddConnector(connector);

			Assert.AreEqual(connector, dataSource.GetConnector(new ConnectorID(1)));
		}
		[TestMethod]
		public void GetConnectorShouldReturnNull()
		{
			Connector connector;
			IDataSource dataSource;

			connector = new Connector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			dataSource = new DataSource();
			dataSource.AddConnector(connector);

			Assert.IsNull(dataSource.GetConnector(new ConnectorID(0)));
		}

		[TestMethod]
		public void GetConnectorsShouldReturnValidValues()
		{
			Connector connector1, connector2, connector3;
			IDataSource dataSource;
			Connector[] results;

			connector1 = new Connector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new Connector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new Connector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddConnector(connector1);
			dataSource.AddConnector(connector2);
			dataSource.AddConnector(connector3);

			results = dataSource.GetConnectors(new FactoryID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(connector1));
			Assert.IsFalse(results.Contains(connector2));
			Assert.IsTrue(results.Contains(connector3));
		}
		[TestMethod]
		public void GetConnectorsShouldReturnNoValues()
		{
			Connector connector1, connector2, connector3;
			IDataSource dataSource;
			Connector[] results;

			connector1 = new Connector() { ID = new ConnectorID(1), FactoryID = new FactoryID(1), ResourceType = "Type1" };
			connector2 = new Connector() { ID = new ConnectorID(2), FactoryID = new FactoryID(2), ResourceType = "Type1" };
			connector3 = new Connector() { ID = new ConnectorID(3), FactoryID = new FactoryID(1), ResourceType = "Type1" };

			dataSource = new DataSource();
			dataSource.AddConnector(connector1);
			dataSource.AddConnector(connector2);
			dataSource.AddConnector(connector3);

			results = dataSource.GetConnectors(new FactoryID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}

		[TestMethod]
		public void GetConnectionShouldReturnValidValue()
		{
			Connection connection;
			IDataSource dataSource;

			connection = new Connection() { ID = new ConnectionID(1), SourceID=new ConnectorID(1), DestinationID=new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddConnection(connection);

			Assert.AreEqual(connection, dataSource.GetConnection(new ConnectionID(1)));
		}
		[TestMethod]
		public void GetConnectionShouldReturnNull()
		{
			Connection connection;
			IDataSource dataSource;

			connection = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(2) };
			dataSource = new DataSource();
			dataSource.AddConnection(connection);

			Assert.IsNull(dataSource.GetConnection(new ConnectionID(0)));
		}
		[TestMethod]
		public void GetConnectionsShouldReturnValidValues()
		{
			Connection connection1, connection2, connection3;
			IDataSource dataSource;
			Connection[] results;

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(4) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(5) };

			dataSource = new DataSource();
			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			results = dataSource.GetConnections(new ConnectorID(1)).ToArray();
			Assert.AreEqual(2, results.Length);
			Assert.IsTrue(results.Contains(connection1));
			Assert.IsFalse(results.Contains(connection2));
			Assert.IsTrue(results.Contains(connection3));
		}
		[TestMethod]
		public void GetConnectionsShouldReturnNoValues()
		{
			Connection connection1, connection2, connection3;
			IDataSource dataSource;
			Connection[] results;

			connection1 = new Connection() { ID = new ConnectionID(1), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(3) };
			connection2 = new Connection() { ID = new ConnectionID(2), SourceID = new ConnectorID(2), DestinationID = new ConnectorID(4) };
			connection3 = new Connection() { ID = new ConnectionID(3), SourceID = new ConnectorID(1), DestinationID = new ConnectorID(5) };

			dataSource = new DataSource();
			dataSource.AddConnection(connection1);
			dataSource.AddConnection(connection2);
			dataSource.AddConnection(connection3);

			results = dataSource.GetConnections(new ConnectorID(3)).ToArray();
			Assert.AreEqual(0, results.Length);
		}


	}

}