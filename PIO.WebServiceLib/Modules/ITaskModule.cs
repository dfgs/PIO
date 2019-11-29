using NetORMLib;
using PIO.Models;
using PIO.WebServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIO.WebServerLib.Modules
{
	public interface ITaskModule:IDatabaseModule
	{
		Task GetTask(int TaskID);
		IEnumerable<Task> GetTasks(int FactoryID);
	}
}
