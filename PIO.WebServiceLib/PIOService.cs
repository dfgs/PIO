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
		private IPlanetModule planetModule;
		private IBuildingModule buildingModule;
		private IFactoryModule factoryModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;
		private IResourceTypeModule resourceTypeModule;
		private IBuildingTypeModule buildingTypeModule;
		private IFactoryTypeModule factoryTypeModule;
		private ITaskTypeModule taskTypeModule;
		private IMaterialModule materialModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;
		private ITaskModule taskModule;

		private IIdlerModule idlerModule;
		private IResourceCheckerModule resourceCheckerModule;
		private IProducerModule producerModule;
		private IMoverModule moverModule;
		private ICarrierModule carrierModule;
		private IFactoryBuilderModule factoryBuilderModule;

		public PIOService(ILogger Logger,
			IPlanetModule PlanetModule,IBuildingModule BuildingModule,  IFactoryModule FactoryModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
			IBuildingTypeModule BuildingTypeModule,
			IFactoryTypeModule FactoryTypeModule,ITaskTypeModule TaskTypeModule,
			IMaterialModule MaterialModule,
			IIngredientModule IngredientModule, IProductModule ProductModule, 
			ITaskModule TaskModule,

			IResourceCheckerModule ResourceCheckerModule, IIdlerModule IdlerModule, IProducerModule ProducerModule,
			IMoverModule MoverModule,ICarrierModule CarrierModule,IFactoryBuilderModule FactoryBuilderModule
		) :base(Logger)
		{
			LogEnter();
			this.planetModule = PlanetModule;
			this.buildingModule = BuildingModule;
			this.factoryModule = FactoryModule;
			this.workerModule = WorkerModule;
			this.stackModule = StackModule;
			this.resourceTypeModule = ResourceTypeModule;
			this.taskTypeModule = TaskTypeModule;
			this.buildingTypeModule = BuildingTypeModule;
			this.factoryTypeModule = FactoryTypeModule;
			this.taskTypeModule = TaskTypeModule;
			this.materialModule = MaterialModule;
			this.ingredientModule = IngredientModule;
			this.productModule = ProductModule;
			this.taskModule = TaskModule;

			this.resourceCheckerModule = ResourceCheckerModule;
			this.idlerModule = IdlerModule;
			this.producerModule = ProducerModule;
			this.moverModule = MoverModule;
			this.carrierModule = CarrierModule;
			this.factoryBuilderModule = FactoryBuilderModule;
		}

		private FaultException GenerateFaultException(Exception InnerException, int ComponentID, string ComponentName, string MethodName)
		{
			return new FaultException(InnerException.Message, new FaultCode(((PIOException)InnerException).FaultCode));
		}

		#region data
		public Planet GetPlanet(int PlanetID)
		{
			LogEnter();
			return Try( ()=>planetModule.GetPlanet(PlanetID)).OrThrow(GenerateFaultException);
		}
		public Planet[] GetPlanets()
		{
			LogEnter();
			return Try(() => planetModule.GetPlanets()).OrThrow(GenerateFaultException);
		}

		public Worker GetWorker(int WorkerID)
		{
			LogEnter();
			return Try(() => workerModule.GetWorker(WorkerID)).OrThrow(GenerateFaultException);
		}
		public Worker[] GetWorkers(int FactoryID)
		{
			LogEnter();
			return Try(() => workerModule.GetWorkers(FactoryID)).OrThrow(GenerateFaultException);
		}
		public Building GetBuilding(int BuildingID)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuilding(BuildingID)).OrThrow(GenerateFaultException);
		}
		public Building GetBuildingAtPos(int PlanetID, int X,int Y)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuilding(PlanetID, X,Y)).OrThrow(GenerateFaultException);
		}
		public Building[] GetBuildings(int PlanetID)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuildings(PlanetID)).OrThrow(GenerateFaultException);
		}
		public Factory GetFactory(int FactoryID)
		{
			LogEnter();
			return Try(() => factoryModule.GetFactory(FactoryID)).OrThrow(GenerateFaultException);
		}
		public Factory GetFactoryAtPos(int PlanetID, int X, int Y)
		{
			LogEnter();
			return Try(() => factoryModule.GetFactory(PlanetID, X,Y)).OrThrow(GenerateFaultException);
		}
		public Factory[] GetFactories(int PlanetID)
		{
			LogEnter();
			return Try(() => factoryModule.GetFactories(PlanetID)).OrThrow(GenerateFaultException);
		}

		public Stack GetStack(int StackID)
		{
			LogEnter();
			return Try(() => stackModule.GetStack(StackID)).OrThrow(GenerateFaultException);
		}

		public Stack[] GetStacks(int FactoryID)
		{
			LogEnter();
			return Try(() => stackModule.GetStacks(FactoryID)).OrThrow(GenerateFaultException);
		}
		public int GetStackQuantity(int FactoryID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();
			return Try(() => stackModule.GetStackQuantity(FactoryID,ResourceTypeID)).OrThrow(GenerateFaultException);
		}

		public ResourceType GetResourceType(ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();
			return Try(() => resourceTypeModule.GetResourceType(ResourceTypeID)).OrThrow(GenerateFaultException);
		}
		public ResourceType[] GetResourceTypes()
		{
			LogEnter();
			return Try(() => resourceTypeModule.GetResourceTypes()).OrThrow(GenerateFaultException);
		}

		public BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID)
		{
			LogEnter();
			return Try(() => buildingTypeModule.GetBuildingType(BuildingTypeID)).OrThrow(GenerateFaultException);
		}
		public BuildingType[] GetBuildingTypes()
		{
			LogEnter();
			return Try(() => buildingTypeModule.GetBuildingTypes()).OrThrow(GenerateFaultException);
		}
		public FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID)
		{
			LogEnter();
			return Try(() => factoryTypeModule.GetFactoryType(FactoryTypeID)).OrThrow(GenerateFaultException);
		}
		public FactoryType[] GetFactoryTypes()
		{
			LogEnter();
			return Try(() => factoryTypeModule.GetFactoryTypes()).OrThrow(GenerateFaultException);
		}
		public TaskType GetTaskType(TaskTypeIDs TaskTypeID)
		{
			LogEnter();
			return Try(() => taskTypeModule.GetTaskType(TaskTypeID)).OrThrow(GenerateFaultException);
		}
		public TaskType[] GetTaskTypes()
		{
			LogEnter();
			return Try(() => taskTypeModule.GetTaskTypes()).OrThrow(GenerateFaultException);
		}

		public Material GetMaterial(int MaterialID)
		{
			LogEnter();
			return Try(() => materialModule.GetMaterial(MaterialID)).OrThrow(GenerateFaultException);
		}

		public Material[] GetMaterials(FactoryTypeIDs FactoryTypeID)
		{
			LogEnter();
			return Try(() => materialModule.GetMaterials(FactoryTypeID)).OrThrow(GenerateFaultException);
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			LogEnter();
			return Try(() => ingredientModule.GetIngredient(IngredientID)).OrThrow(GenerateFaultException);
		}

		public Ingredient[] GetIngredients(FactoryTypeIDs FactoryTypeID)
		{
			LogEnter();
			return Try(() => ingredientModule.GetIngredients(FactoryTypeID)).OrThrow(GenerateFaultException);
		}
		public Product GetProduct(int ProductID)
		{
			LogEnter();
			return Try(() => productModule.GetProduct(ProductID)).OrThrow(GenerateFaultException);
		}

		public Product[] GetProducts(FactoryTypeIDs FactoryTypeID)
		{
			LogEnter();
			return Try(() => productModule.GetProducts(FactoryTypeID)).OrThrow(GenerateFaultException);
		}

		public Task GetTask(int TaskID)
		{
			LogEnter();
			return Try(() => taskModule.GetTask(TaskID)).OrThrow(GenerateFaultException);
		}

		public Task[] GetTasks(int WorkerID)
		{
			LogEnter();
			return Try(() => taskModule.GetTasks(WorkerID)).OrThrow(GenerateFaultException);
		}
		public Task GetLastTask(int WorkerID)
		{
			LogEnter();
			return Try(() => taskModule.GetLastTask(WorkerID)).OrThrow(GenerateFaultException);
		}
		#endregion

		#region functional

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.HasEnoughResourcesToProduce(FactoryID)).OrThrow(GenerateFaultException);
		}

		public Task Idle(int WorkerID,int Duration)
		{
			LogEnter();

			return Try(() => idlerModule.BeginIdle(WorkerID,Duration)).OrThrow(GenerateFaultException);
		}
		public Task Produce(int WorkerID)
		{
			LogEnter();

			return Try(() => producerModule.BeginProduce(WorkerID)).OrThrow(GenerateFaultException);
		}

		public Task MoveTo(int WorkerID, int X, int Y)
		{
			LogEnter();

			return Try(() => moverModule.BeginMoveTo(WorkerID, X,Y)).OrThrow(GenerateFaultException);
		}

		public Task CarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();

			return Try(() => carrierModule.BeginCarryTo(WorkerID, TargetFactoryID, ResourceTypeID)).OrThrow(GenerateFaultException);
		}
		public Task CreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			LogEnter();

			return Try(() => factoryBuilderModule.BeginCreateBuilding(WorkerID, FactoryTypeID)).OrThrow(GenerateFaultException);
		}

		public Task BuildFactory(int WorkerID)
		{
			LogEnter();

			return Try(() => factoryBuilderModule.BeginBuild(WorkerID)).OrThrow(GenerateFaultException);
		}

		#endregion


	}
}
