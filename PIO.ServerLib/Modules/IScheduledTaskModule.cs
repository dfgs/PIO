using ModuleLib;
using NetORMLib;
using PIO.Models;
using PIO.WebServerLib.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.ServerLib.Modules
{
	public interface IScheduledTaskModule:IDatabaseModule
	{
		Task GetScheduledTask(int ScheduledTaskID);
		IEnumerable<Models.Task> GetScheduledTasks(int FactoryID);
		int CreateScheduledTask(int FactoryID, int TaskID, System.DateTime ETA);
		void DeleteScheduledTask(int ScheduledTaskID);
		//void SetTask(int FactoryID, int TaskID);
	}
}
