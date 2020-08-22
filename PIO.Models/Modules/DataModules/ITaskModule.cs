using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIO.Models.Modules
{
	public interface ITaskModule:IDatabaseModule
	{
		Task GetTask(int TaskID);
		Task[] GetTasks(int WorkerID);

	}
}
