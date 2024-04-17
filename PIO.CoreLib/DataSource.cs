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
		private List<IFactory> factories;
		private List<IInputConnector> inputConnectors;
		private List<IOutputConnector> outputConnectors;
		private List<IConnection> connections;
		private List<IBuffer> buffers;

		public DataSource() 
		{ 
			this.factories = new List<IFactory>();
			this.inputConnectors = new List<IInputConnector>();
			this.outputConnectors = new List<IOutputConnector>();
			this.connections = new List<IConnection>();
			buffers = new List<IBuffer>();
		}

		public void AddFactory(IFactory Factory)
		{ 
			if (Factory==null) throw new PIOInvalidParameterException(nameof(Factory));
			this.factories.Add(Factory); 
		}

		public void AddInputConnector(IInputConnector InputConnector)
		{
			if (InputConnector == null) throw new PIOInvalidParameterException(nameof(InputConnector));
			this.inputConnectors.Add(InputConnector);
		}
		public void AddOutputConnector(IOutputConnector OutputConnector)
		{
			if (OutputConnector == null) throw new PIOInvalidParameterException(nameof(OutputConnector));
			this.outputConnectors.Add(OutputConnector);
		}
		public void AddConnection(IConnection Connection)
		{
			if (Connection == null) throw new PIOInvalidParameterException(nameof(Connection));
			this.connections.Add(Connection); 
		}

		public void AddBuffer(IBuffer Buffer)
		{
			if (Buffer == null) throw new PIOInvalidParameterException(nameof(Buffer));
			this.buffers.Add(Buffer); 
		}

		public IFactory? GetFactory(FactoryID FactoryID)
		{
			return factories.FirstOrDefault(item => item.ID == FactoryID);
		}
		public IEnumerable<IFactory> GetFactories()
		{
			return factories;
		}

		public IInputConnector? GetInputConnector(ConnectorID ConnectorID)
		{
			return inputConnectors.FirstOrDefault(item => item.ID == ConnectorID);
		}

		public IEnumerable<IInputConnector> GetInputConnectors(FactoryID FactoryID)
		{
			return inputConnectors.Where(item => item.FactoryID == FactoryID);
		}

		public IOutputConnector? GetOutputConnector(ConnectorID ConnectorID)
		{
			return outputConnectors.FirstOrDefault(item => item.ID == ConnectorID);
		}

		public IEnumerable<IOutputConnector> GetOutputConnectors(FactoryID FactoryID)
		{
			return outputConnectors.Where(item => item.FactoryID == FactoryID);
		}


		public IConnection? GetConnection(ConnectionID ConnectionID)
		{
			return connections.FirstOrDefault(item=>item.ID == ConnectionID);
		}

		public IEnumerable<IConnection> GetConnections(ConnectorID SourceID)
		{
			return connections.Where(item => item.SourceID == SourceID);
		}

		public IBuffer? GetBuffer(BufferID BufferID)
		{
			return buffers.FirstOrDefault(item=>item.ID==BufferID);
		}
		public IBuffer? GetBuffer(ConnectorID ConnectorID)
		{
			return buffers.FirstOrDefault(item=>item.ConnectorID==ConnectorID);
		}


}
}
