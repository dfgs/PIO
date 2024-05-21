using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class SubTaskManager : PIOModule,ISubTaskManager
	{
		
		public SubTaskManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
		}

		public ISubTask[]? GetSubTasks(JobID JobID)
		{
			ISubTask[]? subTasks = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Job ID {JobID}] Trying to get sub tasks");
			if (!Try(() => DataSource.GetSubTasks(JobID)).Then(result => subTasks = result.ToArray()).OrAlert($"[Job ID {JobID}] Failed to get sub tasks")) return null;
			return subTasks;
		}
		public ISubTask[]? GetSubTasks()
		{
			ISubTask[]? subTasks = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Job ID NA] Trying to get sub tasks");
			if (!Try(() => DataSource.GetSubTasks()).Then(result => subTasks = result.ToArray()).OrAlert($"[Job ID NA] Failed to get sub tasks")) return null;
			return subTasks;
		}


		public ISubTask? GetSubTask(SubTaskID SubTaskID)
		{
			ISubTask? subTask = null;

			LogEnter();
			Log(LogLevels.Debug, $"[SubTask ID {SubTaskID}] Trying to get sub task");
			if (!Try(() => DataSource.GetSubTask(SubTaskID)).Then(result => subTask = result).OrAlert($"[SubTask ID {SubTaskID}] Failed to get sub task")) return null;
			return subTask;

		}

		


	}
}
