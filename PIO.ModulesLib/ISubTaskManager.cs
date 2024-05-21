using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface ISubTaskManager:IPIOModule
	{
		ISubTask[]? GetSubTasks(JobID JobID);
		ISubTask[]? GetSubTasks();
		ISubTask? GetSubTask(SubTaskID SubTaskID);


	}
}
