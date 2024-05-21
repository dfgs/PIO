using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class UpdateManager : PIOModule,IUpdateManager
	{
		private IFactoryManager factoryManager;
		private ISubTaskManager subTaskManager;
		

		public UpdateManager(ILogger Logger,IDataSource DataSource, IFactoryManager FactoryManager,ISubTaskManager SubTaskManager) : base(Logger,DataSource)
		{
			if (FactoryManager == null) throw new PIOInvalidParameterException(nameof(FactoryManager));
			if (SubTaskManager == null) throw new PIOInvalidParameterException(nameof(SubTaskManager));
			this.factoryManager = FactoryManager;
			this.subTaskManager= SubTaskManager;
		}

	


		public bool Update(float Cycle)
		{
			IFactory[]? factories ;
			ISubTask[]? subTasks ;

			LogEnter();

			
			Log(LogLevels.Information, "[Factory ID NA] Get factories");
			factories = factoryManager.GetFactories();
			if (factories == null)
			{
				Log(LogLevels.Error, $"[Factory ID NA] Fail to get factories");
				return false;
			}

			Log(LogLevels.Information, "[SubTask ID NA] Get sub tasks");
			subTasks = subTaskManager.GetSubTasks();
			if (subTasks == null)
			{
				Log(LogLevels.Error, $"[SubTask ID NA] Fail to get sub tasks");
				return false;
			}

			return true;
		}



	}
}
