using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class ConnectionManager : PIOModule,IConnectionManager
	{
		
		public ConnectionManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
		}

		public IInputConnector[]? GetInputConnectors(FactoryID FactoryID)
		{
			IInputConnector[]? connectors = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Factory ID {FactoryID}] Trying to get input connectors");
			if (!Try(() => DataSource.GetInputConnectors(FactoryID)).Then(result=>connectors=result.ToArray()).OrAlert($"[Factory ID {FactoryID}] Failed to get input connectors")) return null;
			return connectors;
		}
		public IOutputConnector[]? GetOutputConnectors(FactoryID FactoryID)
		{
			IOutputConnector[]? connectors = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Factory ID {FactoryID}] Trying to get output connectors");
			if (!Try(() => DataSource.GetOutputConnectors(FactoryID)).Then(result => connectors = result.ToArray()).OrAlert($"[Factory ID {FactoryID}] Failed to get output connectors")) return null;
			return connectors;

		}

		public IInputConnector? GetInputConnector(ConnectorID ConnectorID)
		{
			IInputConnector? connector = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Connector ID {ConnectorID}] Trying to get input connector");
			if (!Try(() => DataSource.GetInputConnector(ConnectorID)).Then(result => connector = result).OrAlert($"[Connector ID {ConnectorID}] Failed to get input connector")) return null;
			return connector;

		}




	}
}
