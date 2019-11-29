using ModuleLib;
using NetORMLib;
using PIO.Models;
using PIO.WebServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public interface IScheduledTaskModule:IDatabaseModule
	{
		ScheduledTask GetScheduledTask(int ScheduledTaskID);
		IEnumerable<ScheduledTask> GetScheduledTasks(int FactoryID);
		int CreateScheduledTask(int FactoryID, int TaskID, DateTime ETA);
		void DeleteScheduledTask(int ScheduledTaskID);
		//void SetTask(int FactoryID, int TaskID);
	}
}
