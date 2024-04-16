using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class DataSource : IDataSource
	{
		private List<Factory> factories;
		private List<Connector> connectors;
		private List<Connection> connections;

		public DataSource() 
		{ 
			this.factories = new List<Factory>();
			this.connectors = new List<Connector>();
			this.connections = new List<Connection>();
		}

		public void AddFactory(Factory Factory)
		{ 
			if (Factory==null) throw new PIOInvalidParameterException(nameof(Factory));
			this.factories.Add(Factory); 
		}

		public void AddConnector(Connector Connector)
		{
			if (Connector == null) throw new PIOInvalidParameterException(nameof(Connector));
			this.connectors.Add(Connector); 
		}
		public void AddConnection(Connection Connection)
		{
			if (Connection == null) throw new PIOInvalidParameterException(nameof(Connection));
			this.connections.Add(Connection); 
		}


		public Factory? GetFactory(FactoryID FactoryID)
		{
			return factories.FirstOrDefault(item=>item.ID == FactoryID);
		}

		public Connector? GetConnector(ConnectorID ConnectorID)
		{
			return connectors.FirstOrDefault(item=>item.ID == ConnectorID);
		}

		public IEnumerable<Connector> GetConnectors(FactoryID FactoryID)
		{
			return connectors.Where(item=>item.FactoryID == FactoryID);
		}


		public Connection? GetConnection(ConnectionID ConnectionID)
		{
			return connections.FirstOrDefault(item=>item.ID == ConnectionID);
		}

		public IEnumerable<Connection> GetConnections(ConnectorID SourceID)
		{
			return connections.Where(item => item.SourceID == SourceID);
		}

		


	}
}
