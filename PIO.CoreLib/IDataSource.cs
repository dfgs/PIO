using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IDataSource
	{
		void AddFactory(IFactory Factory);
		void AddInputConnector(IInputConnector Connector);
		void AddOutputConnector(IOutputConnector Connector);
		void AddConnection(IConnection Connection);

		IFactory? GetFactory(FactoryID FactoryID);
		IEnumerable<IFactory> GetFactories();
		
		IInputConnector? GetInputConnector(ConnectorID ConnectorID);
		IEnumerable<IInputConnector> GetInputConnectors(FactoryID FactoryID);

		IOutputConnector? GetOutputConnector(ConnectorID ConnectorID);
		IEnumerable<IOutputConnector> GetOutputConnectors(FactoryID FactoryID);



		IConnection? GetConnection(ConnectionID ConnectionID);
		IEnumerable<IConnection> GetConnections(ConnectorID SourceID);


	}
}
