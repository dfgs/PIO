using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface ITaskTypeModule:IDatabaseModule
	{
		TaskType GetTaskType(TaskTypeIDs TaskTypeID);
		TaskType[] GetTaskTypes();

		TaskType CreateTaskType(TaskTypeIDs TaskTypeID, string PhraseKey);


	}
}
