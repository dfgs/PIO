using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IScheduledTaskModule:IDatabaseModule
	{
		Row GetScheduledTask(int ScheduledTaskID);
		IEnumerable<Row> GetScheduledTasks(int FactoryID);
		int CreateScheduledTask(int FactoryID, int TaskID,DateTime ETA);
		//void SetTask(int FactoryID, int TaskID);
	}
}
