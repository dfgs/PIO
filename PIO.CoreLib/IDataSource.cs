using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IDataSource
	{
		void AddFactory(Factory Factory);
		void AddConnector(Connector Connector);
		void AddConnection(Connection Connection);

		Factory? GetFactory(FactoryID FactoryID);
		Connector? GetConnector(ConnectorID ConnectorID);
		IEnumerable<Connector> GetConnectors(FactoryID FactoryID);
		Connection? GetConnection(ConnectionID ConnectionID);
		IEnumerable<Connection> GetConnections(ConnectorID SourceID);


	}
}
