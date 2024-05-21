using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class WorkerManager : PIOModule, IWorkerManager
	{
		public WorkerManager(ILogger Logger, IDataSource DataSource) : base(Logger, DataSource)
		{
		}


		public IWorker[]? GetWorkers()
		{
			IWorker[]? buffers = null;

			LogEnter();
			Log(LogLevels.Debug, $"Trying to get workers");
			if (!Try(() => DataSource.GetWorkers()).Then(result => buffers = result.ToArray()).OrAlert($"Failed to get workers")) return null;
			return buffers;
		}
		public IWorker? GetWorker(WorkerID WorkerID)
		{
			IWorker? buffer = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Worker ID {WorkerID}] Trying to get worker");
			if (!Try(() => DataSource.GetWorker(WorkerID)).Then(result => buffer = result).OrAlert($"[Worker ID {WorkerID}] Failed to get workers")) return null;
			return buffer;

		}
	}
}
