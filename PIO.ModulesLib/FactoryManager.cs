using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class FactoryManager : PIOModule,IFactoryManager
	{

		public FactoryManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
			
		}

		public IFactory[]? GetFactories()
		{
			IFactory[]? factories = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Factory ID NA] Trying to get factories");
			if (!Try(() => DataSource.GetFactories()).Then(result => factories = result.ToArray()).OrAlert($"[Factory ID NA] Failed to get factories")) return null;
			return factories;
		}

		public IFactory? GetFactory(FactoryID FactoryID)
		{
			IFactory? factory = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Factory ID {FactoryID}] Trying to get factory");
			if (!Try(() => DataSource.GetFactory(FactoryID)).Then(result => factory = result).OrAlert($"[Factory ID {FactoryID}] Failed to get factory")) return null;
			return factory;

		}
	}
}
