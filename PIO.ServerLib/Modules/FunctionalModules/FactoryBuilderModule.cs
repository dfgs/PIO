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
	public class FactoryBuilderModule : TaskGeneratorModule, IFactoryBuilderModule
	{
		private IFactoryModule factoryModule;
		private IBuildingModule buildingModule;
		private IFactoryTypeModule factoryTypeModule;

		public FactoryBuilderModule(ILogger Logger, ITaskModule TaskModule, IWorkerModule WorkerModule,  IBuildingModule BuildingModule, IFactoryModule FactoryModule, IFactoryTypeModule FactoryTypeModule) : base(Logger,TaskModule,WorkerModule)
		{
			this.factoryModule = FactoryModule;this.buildingModule = BuildingModule; this.factoryTypeModule = FactoryTypeModule;

		}

		public Task BeginCreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			FactoryType factoryType;
			Worker worker;
			Task task;

			LogEnter();

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "BeginCreateBuilding");
			}
			
			factoryType = Try(() => factoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get factory type");
			if (factoryType == null)
			{
				Log(LogLevels.Warning, $"Factory type doesn't exist (FactoryTypeID={FactoryTypeID})");
				throw new PIONotFoundException($"Factory type doesn't exist (FactoryTypeID={FactoryTypeID})", null, ID, ModuleName, "BeginCreateBuilding");
			}


			Log(LogLevels.Information, $"Creating task (WorkerID={WorkerID})");
			task = Try(() => taskModule.CreateTask(TaskTypeIDs.CreateBuilding, WorkerID, null, null, null, null, FactoryTypeID, GetLastETA(WorkerID).AddSeconds(10))).OrThrow<PIOInternalErrorException>("Failed to create task");

			OnTaskCreated(task);

			return task;
		}

		public void EndCreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			FactoryType factoryType;
			Building building;
			Worker worker;

			LogEnter();

			Log(LogLevels.Information, $"End create building (FactoryTypeID={FactoryTypeID})");

			Log(LogLevels.Information, $"Get worker (WorkerID={WorkerID})");
			worker = Try(() => workerModule.GetWorker(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to get worker");
			if (worker == null)
			{
				Log(LogLevels.Warning, $"Worker doesn't exist (WorkerID={WorkerID})");
				throw new PIONotFoundException($"Worker doesn't exist (WorkerID={WorkerID})", null, ID, ModuleName, "EndCreateBuilding");
			}

			factoryType = Try(() => factoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow<PIOInternalErrorException>("Failed to get factory type");
			if (factoryType == null)
			{
				Log(LogLevels.Warning, $"Factory type doesn't exist (FactoryTypeID={FactoryTypeID})");
				throw new PIONotFoundException($"Factory type doesn't exist (FactoryTypeID={FactoryTypeID})", null, ID, ModuleName, "EndCreateBuilding");
			}

		
			Log(LogLevels.Information, $"Creating building (BuildingTypeID={BuildingTypeIDs.Factory})");
			building = Try(() => buildingModule.CreateBuilding(worker.PlanetID, worker.X,worker.Y, BuildingTypeIDs.Factory, factoryType.BuildSteps)).OrThrow<PIOInternalErrorException>("Failed to create building");

		}


		public void Build(int FactoryID)
		{
			Factory factory;
			FactoryType factoryType;


			LogEnter();

			/*Log(LogLevels.Information, $"Building Factory (FactoryID={FactoryID})");
			factory = Try(() => factoryModule.GetFactory(FactoryID)).OrThrow("Failed to build");
			if (factory == null)
			{
				Log(LogLevels.Warning, $"Factory doesn't exists (FactoryID={FactoryID})");
				return;
			}
			factoryType = Try(() => factoryTypeModule.GetFactoryType(factory.FactoryTypeID)).OrThrow("Failed to build");
			if (factoryType == null)
			{
				Log(LogLevels.Warning, $"FactoryType doesn't exists (FactoryTypeID={factory.FactoryTypeID})");
				return;
			}

			if (factory.HealthPoints==factoryType.HealthPoints)
			{
				Log(LogLevels.Error, $"HealthPoints are maximum value (FactoryID={FactoryID})");
				throw new InvalidOperationException($"HealthPoints are maximum value (FactoryID={FactoryID})");
			}

			Try(() => factoryModule.SetHealthPoints(FactoryID,factory.HealthPoints+1)).OrThrow("Failed to build");*/
		}


	}
}
