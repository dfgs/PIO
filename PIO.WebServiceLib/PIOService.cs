using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LogLib;
using ModuleLib;
using PIO.Models;
using PIO.Models.Modules;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class PIOService : Module, IPIOService
	{
		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;
		private IWorkerModule WorkerModule;
		private IStackModule StackModule;
		private IResourceTypeModule ResourceTypeModule;
		private IFactoryTypeModule FactoryTypeModule;
		private IMaterialModule MaterialModule;
		private IIngredientModule IngredientModule;
		private ITaskModule TaskModule;

		public PIOService(ILogger Logger,
			IPlanetModule PlanetModule, IFactoryModule FactoryModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
			IFactoryTypeModule FactoryTypeModule,IMaterialModule MaterialModule,
			IIngredientModule IngredientModule, ITaskModule TaskModule
		):base(Logger)
		{
			LogEnter();
			this.PlanetModule = PlanetModule;
			this.FactoryModule = FactoryModule;
			this.WorkerModule = WorkerModule;
			this.StackModule = StackModule;
			this.ResourceTypeModule = ResourceTypeModule;
			this.FactoryTypeModule = FactoryTypeModule;
			this.MaterialModule = MaterialModule;
			this.IngredientModule = IngredientModule;
			this.TaskModule = TaskModule;
		}

		public Planet GetPlanet(int PlanetID)
		{
			LogEnter();
			return Try( ()=>PlanetModule.GetPlanet(PlanetID)).OrThrow("Internal error");
		}
		public Planet[] GetPlanets()
		{
			LogEnter();
			return Try(() => PlanetModule.GetPlanets()).OrThrow("Internal error");
		}

		public Worker GetWorker(int WorkerID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorker(WorkerID)).OrThrow("Internal error");
		}
		public Worker[] GetWorkers(int PlanetID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorkers(PlanetID)).OrThrow("Internal error");
		}
		public Factory GetFactory(int FactoryID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactory(FactoryID)).OrThrow("Internal error");
		}
		public Factory[] GetFactories(int PlanetID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactories(PlanetID)).OrThrow("Internal error");
		}

		public Stack GetStack(int StackID)
		{
			LogEnter();
			return Try(() => StackModule.GetStack(StackID)).OrThrow("Internal error");
		}

		public Stack[] GetStacks(int FactoryID)
		{
			LogEnter();
			return Try(() => StackModule.GetStacks(FactoryID)).OrThrow("Internal error");
		}

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceType(ResourceTypeID)).OrThrow("Internal error");
		}
		public ResourceType[] GetResourceTypes()
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceTypes()).OrThrow("Internal error");
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow("Internal error");
		}
		public FactoryType[] GetFactoryTypes()
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryTypes()).OrThrow("Internal error");
		}

		public Material GetMaterial(int MaterialID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterial(MaterialID)).OrThrow("Internal error");
		}

		public Material[] GetMaterials(int FactoryID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterials(FactoryID)).OrThrow("Internal error");
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredient(IngredientID)).OrThrow("Internal error");
		}

		public Ingredient[] GetIngredients(int FactoryID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredients(FactoryID)).OrThrow("Internal error");
		}

		public Task GetTask(int TaskID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTask(TaskID)).OrThrow("Internal error");
		}

		public Task[] GetTasks(int FactoryID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTasks(FactoryID)).OrThrow("Internal error");
		}


	}
}
