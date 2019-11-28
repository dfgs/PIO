using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PIO.ServerLib.Modules
{
	public interface ITaskModule:IDatabaseModule
	{
		Task GetTask(int TaskID);
		//IEnumerable<Row> GetTasks(int FactoryID);
		//void SetTask(int FactoryID, int TaskID);
	}
}
