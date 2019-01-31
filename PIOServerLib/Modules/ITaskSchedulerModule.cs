using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface ITaskSchedulerModule:IThreadModule
	{
		void StartTask(int FactoryID, int TaskID,DateTime ETA);

	}
}
