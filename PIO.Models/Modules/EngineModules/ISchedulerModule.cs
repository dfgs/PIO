using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Modules.EngineModules;

namespace PIO.Models.Modules
{
	public interface ISchedulerModule: IEngineModule
	{
		//void Add(Task Task); 

		/*bool RegisterCallBack(ITaskCallBack Callback);
		bool UnregisterCallBack(ITaskCallBack Callback);*/

		event TaskEventHandler TaskStarted;
		event TaskEventHandler TaskEnded;

	}
}
