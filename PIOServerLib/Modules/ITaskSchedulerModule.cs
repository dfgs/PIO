using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	interface ITaskSchedulerModule:IModule
	{
		void SetTask(int FactoryID, int TaskID);

	}
}
