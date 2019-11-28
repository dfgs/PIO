using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IScheduledTaskModule:IDatabaseModule
	{
		Row<ScheduledTask> GetScheduledTask(int ScheduledTaskID);
		IEnumerable<Row<ScheduledTask>> GetScheduledTasks(int FactoryID);
		int CreateScheduledTask(int FactoryID, int TaskID, DateTime ETA);
		void DeleteScheduledTask(int ScheduledTaskID);
		//void SetTask(int FactoryID, int TaskID);
	}
}
