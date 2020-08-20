using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PIO.Models;
using PIO.Models.Modules;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class PIOService : IPIOService
	{
		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;
		private IWorkerModule WorkerModule;
		private IStackModule StackModule;
		private IResourceTypeModule ResourceTypeModule;
		private IFactoryTypeModule FactoryTypeModule;
		private IMaterialModule MaterialModule;
		private ITaskModule TaskModule;

		public PIOService(
			IPlanetModule PlanetModule, IFactoryModule FactoryModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
			IFactoryTypeModule FactoryTypeModule,IMaterialModule MaterialModule,
			ITaskModule TaskModule
		)
		{
			this.PlanetModule = PlanetModule;
			this.FactoryModule = FactoryModule;
			this.WorkerModule = WorkerModule;
			this.StackModule = StackModule;
			this.ResourceTypeModule = ResourceTypeModule;
			this.FactoryTypeModule = FactoryTypeModule;
			this.MaterialModule = MaterialModule;
			this.TaskModule = TaskModule;
		}

		public Planet GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}
		public Planet[] GetPlanets()
		{
			return PlanetModule.GetPlanets();
		}

		public Worker GetWorker(int WorkerID)
		{
			return WorkerModule.GetWorker(WorkerID);
		}
		public Worker[] GetWorkers(int PlanetID)
		{
			return WorkerModule.GetWorkers(PlanetID);
		}
		public Factory GetFactory(int FactoryID)
		{
			return FactoryModule.GetFactory(FactoryID);
		}
		public Factory[] GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID);
		}

		public Stack GetStack(int StackID)
		{
			return StackModule.GetStack(StackID);
		}

		public Stack[] GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID);
		}

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			return ResourceTypeModule.GetResourceType(ResourceTypeID);
		}
		public ResourceType[] GetResourceTypes()
		{
			return ResourceTypeModule.GetResourceTypes();
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			return FactoryTypeModule.GetFactoryType(FactoryTypeID);
		}
		public FactoryType[] GetFactoryTypes()
		{
			return FactoryTypeModule.GetFactoryTypes();
		}

		public Material GetMaterial(int MaterialID)
		{
			return MaterialModule.GetMaterial(MaterialID);
		}

		public Material[] GetMaterials(int FactoryID)
		{
			return MaterialModule.GetMaterials(FactoryID);
		}

	
		public Task GetTask(int TaskID)
		{
			return TaskModule.GetTask(TaskID);
		}

		public Task[] GetTasks(int FactoryID)
		{
			return TaskModule.GetTasks(FactoryID);
		}


	}
}
