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
		void RemoveTask(int TaskID);
		Task[] GetTasks(int FactoryID);
		Task[] GetTasks();
		Task CreateTask(int FactoryID, int TaskTypeID, DateTime ETA);

	}
}
