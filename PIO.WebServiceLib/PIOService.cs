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
using PIO.WebServiceLib.Exceptions;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
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
		private IProductModule ProductModule;
		private ITaskModule TaskModule;

		private IResourceCheckerModule ResourceCheckerModule;

		public PIOService(ILogger Logger,
			IPlanetModule PlanetModule, IFactoryModule FactoryModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
			IFactoryTypeModule FactoryTypeModule,IMaterialModule MaterialModule,
			IIngredientModule IngredientModule, IProductModule ProductModule, 
			ITaskModule TaskModule,

			IResourceCheckerModule ResourceCheckerModule
		) :base(Logger)
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
			this.ProductModule = ProductModule;
			this.TaskModule = TaskModule;

			this.ResourceCheckerModule = ResourceCheckerModule;
		}

		#region data
		public Planet GetPlanet(int PlanetID)
		{
			LogEnter();
			return Try( ()=>PlanetModule.GetPlanet(PlanetID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public Planet[] GetPlanets()
		{
			LogEnter();
			return Try(() => PlanetModule.GetPlanets()).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Worker GetWorker(int WorkerID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorker(WorkerID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public Worker[] GetWorkers(int PlanetID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorkers(PlanetID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public Factory GetFactory(int FactoryID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactory(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public Factory[] GetFactories(int PlanetID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactories(PlanetID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Stack GetStack(int StackID)
		{
			LogEnter();
			return Try(() => StackModule.GetStack(StackID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Stack[] GetStacks(int FactoryID)
		{
			LogEnter();
			return Try(() => StackModule.GetStacks(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceType(ResourceTypeID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public ResourceType[] GetResourceTypes()
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceTypes()).OrThrow<PIOWebServiceException>("Internal error");
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public FactoryType[] GetFactoryTypes()
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryTypes()).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Material GetMaterial(int MaterialID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterial(MaterialID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Material[] GetMaterials(int FactoryID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterials(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredient(IngredientID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Ingredient[] GetIngredients(int FactoryID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredients(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		public Product GetProduct(int ProductID)
		{
			LogEnter();
			return Try(() => ProductModule.GetProduct(ProductID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Product[] GetProducts(int FactoryID)
		{
			LogEnter();
			return Try(() => ProductModule.GetProducts(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Task GetTask(int TaskID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTask(TaskID)).OrThrow<PIOWebServiceException>("Internal error");
		}

		public Task[] GetTasks(int FactoryID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTasks(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		#endregion

		#region functional

		public bool? HasEnoughResourcesToProduce(int FactoryID)
		{
			LogEnter();
			return Try(() => ResourceCheckerModule.HasEnoughResourcesToProduce(FactoryID)).OrThrow<PIOWebServiceException>("Internal error");
		}
		#endregion


	}
}
