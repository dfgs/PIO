using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LogLib;
using ModuleLib;
using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;

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

		private FaultException GenerateFaultException(Exception InnerException, int ComponentID, string ComponentName, string MethodName)
		{
			return new FaultException(InnerException.Message, new FaultCode(((PIOException)InnerException).FaultCode));
		}

		#region data
		public Planet GetPlanet(int PlanetID)
		{
			LogEnter();
			return Try( ()=>PlanetModule.GetPlanet(PlanetID)).OrThrow(GenerateFaultException);
		}
		public Planet[] GetPlanets()
		{
			LogEnter();
			return Try(() => PlanetModule.GetPlanets()).OrThrow(GenerateFaultException);
		}

		public Worker GetWorker(int WorkerID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorker(WorkerID)).OrThrow(GenerateFaultException);
		}
		public Worker[] GetWorkers(int PlanetID)
		{
			LogEnter();
			return Try(() => WorkerModule.GetWorkers(PlanetID)).OrThrow(GenerateFaultException);
		}
		public Factory GetFactory(int FactoryID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactory(FactoryID)).OrThrow(GenerateFaultException);
		}
		public Factory[] GetFactories(int PlanetID)
		{
			LogEnter();
			return Try(() => FactoryModule.GetFactories(PlanetID)).OrThrow(GenerateFaultException);
		}

		public Stack GetStack(int StackID)
		{
			LogEnter();
			return Try(() => StackModule.GetStack(StackID)).OrThrow(GenerateFaultException);
		}

		public Stack[] GetStacks(int FactoryID)
		{
			LogEnter();
			return Try(() => StackModule.GetStacks(FactoryID)).OrThrow(GenerateFaultException);
		}

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceType(ResourceTypeID)).OrThrow(GenerateFaultException);
		}
		public ResourceType[] GetResourceTypes()
		{
			LogEnter();
			return Try(() => ResourceTypeModule.GetResourceTypes()).OrThrow(GenerateFaultException);
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow(GenerateFaultException);
		}
		public FactoryType[] GetFactoryTypes()
		{
			LogEnter();
			return Try(() => FactoryTypeModule.GetFactoryTypes()).OrThrow(GenerateFaultException);
		}

		public Material GetMaterial(int MaterialID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterial(MaterialID)).OrThrow(GenerateFaultException);
		}

		public Material[] GetMaterials(int FactoryID)
		{
			LogEnter();
			return Try(() => MaterialModule.GetMaterials(FactoryID)).OrThrow(GenerateFaultException);
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredient(IngredientID)).OrThrow(GenerateFaultException);
		}

		public Ingredient[] GetIngredients(int FactoryID)
		{
			LogEnter();
			return Try(() => IngredientModule.GetIngredients(FactoryID)).OrThrow(GenerateFaultException);
		}
		public Product GetProduct(int ProductID)
		{
			LogEnter();
			return Try(() => ProductModule.GetProduct(ProductID)).OrThrow(GenerateFaultException);
		}

		public Product[] GetProducts(int FactoryID)
		{
			LogEnter();
			return Try(() => ProductModule.GetProducts(FactoryID)).OrThrow(GenerateFaultException);
		}

		public Task GetTask(int TaskID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTask(TaskID)).OrThrow(GenerateFaultException);
		}

		public Task[] GetTasks(int FactoryID)
		{
			LogEnter();
			return Try(() => TaskModule.GetTasks(FactoryID)).OrThrow(GenerateFaultException);
		}
		#endregion

		#region functional

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			LogEnter();
			//throw new FaultException("Factory not found",FaultCodes.NotFound);

			return Try(() => ResourceCheckerModule.HasEnoughResourcesToProduce(FactoryID)).OrThrow(GenerateFaultException);
		}
		#endregion


	}
}
