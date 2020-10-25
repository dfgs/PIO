using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using PIO.Models.Exceptions;


namespace PIO.ServerLib.Modules
{
	public class MoverModule : TaskGeneratorModule, IMoverModule
	{

		private IFactoryModule factoryModule;



		public MoverModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule, IFactoryModule FactoryModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.factoryModule = FactoryModule; 
		}

		public Task BeginMoveTo(int WorkerID, int TargetFactoryID)
		{
			Factory targetFactory;
			Worker worker;
			Task task;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginMoveTo");
			}

			Log(LogLevels.Information, $"Get target factory (FactoryID={TargetFactoryID})");
			targetFactory = Try(() => factoryModule.GetFactory(TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to get target factory");
			if (targetFactory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={TargetFactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={TargetFactoryID})", null, ID, ModuleName, "BeginMoveTo");
			}

			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.CreateTask(TaskTypeIDs.MoveTo, WorkerID, null, TargetFactoryID, null, null, GetLastETA(WorkerID).AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndMoveTo(int WorkerID, int TargetFactoryID)
		{
			Worker worker;
			Factory targetFactory;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "EndMoveTo");
			}

			Log(LogLevels.Information, $"Get target factory (FactoryID={TargetFactoryID})");
			targetFactory = Try(() => factoryModule.GetFactory(TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to get target factory");
			if (targetFactory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exist (FactoryID={TargetFactoryID})");
				throw new PIONotFoundException($"Factory doesn't exist (FactoryID={TargetFactoryID})", null, ID, ModuleName, "EndMoveTo");
			}


			Log(LogLevels.Information, $"Updating worker (WorkerID={WorkerID})");
			Try(() => workerModule.UpdateWorker(WorkerID,TargetFactoryID)).OrThrow<PIOInternalErrorException>("Failed to update worker");
		}





	}
}
