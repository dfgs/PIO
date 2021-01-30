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
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;

namespace PIO.WebServiceLib
{

	
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class PIOService : Module, IPIOService
	{
		private IPlanetModule planetModule;
		private ICellModule cellModule;
		private IFactoryModule factoryModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;
		private IResourceTypeModule resourceTypeModule;
		private IFactoryTypeModule factoryTypeModule;
		private ITaskTypeModule taskTypeModule;
		private IMaterialModule materialModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;
		private ITaskModule taskModule;

		private IIdlerModule idlerModule;
		private IResourceCheckerModule resourceCheckerModule;
		private ILocationCheckerModule locationCheckerModule;
		private IProducerModule producerModule;
		private IMoverModule moverModule;
		private ICarrierModule carrierModule;
		private ITakerModule takerModule;
		private IStorerModule storerModule;

		private IFactoryBuilderModule factoryBuilderModule;



		public PIOService(ILogger Logger,
			IPlanetModule PlanetModule, ICellModule CellModule, IFactoryModule FactoryModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
						IFactoryTypeModule FactoryTypeModule,ITaskTypeModule TaskTypeModule,
			IMaterialModule MaterialModule,
			IIngredientModule IngredientModule, IProductModule ProductModule, 
			ITaskModule TaskModule,

			ISchedulerModule SchedulerModule,

			IResourceCheckerModule ResourceCheckerModule,ILocationCheckerModule LocationCheckerModule,
			IIdlerModule IdlerModule, IProducerModule ProducerModule,
			IMoverModule MoverModule,ICarrierModule CarrierModule,ITakerModule TakerModule,IStorerModule StorerModule,
			IFactoryBuilderModule FactoryBuilderModule
		) :base(Logger)
		{
			LogEnter();
			this.planetModule = PlanetModule;
			this.cellModule = CellModule;
			this.factoryModule = FactoryModule;
			this.workerModule = WorkerModule;
			this.stackModule = StackModule;
			this.locationCheckerModule = LocationCheckerModule;
			this.resourceTypeModule = ResourceTypeModule;
			this.taskTypeModule = TaskTypeModule;
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
			this.takerModule = TakerModule;
			this.storerModule = StorerModule;

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
		public Worker[] GetWorkers(int PlanetID)
		{
			LogEnter();
			return Try(() => workerModule.GetWorkers(PlanetID)).OrThrow(GenerateFaultException);
		}
		public Worker[] GetAllWorkers()
		{
			LogEnter();
			return Try(() => workerModule.GetWorkers()).OrThrow(GenerateFaultException);
		}
		public Cell GetCell(int CellID)
		{
			LogEnter();
			return Try(() => cellModule.GetCell(CellID)).OrThrow(GenerateFaultException);
		}
		public Cell GetCellAtPos(int PlanetID, int X, int Y)
		{
			LogEnter();
			return Try(() => cellModule.GetCell(PlanetID, X, Y)).OrThrow(GenerateFaultException);
		}
		public Cell[] GetCells(int PlanetID, int X, int Y,int Width,int Height)
		{
			LogEnter();
			return Try(() => cellModule.GetCells(PlanetID, X, Y,Width,Height)).OrThrow(GenerateFaultException);
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
		public Stack FindStack(int PlanetID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();
			return Try(() => stackModule.FindStack(PlanetID,ResourceTypeID)).OrThrow(GenerateFaultException);
		}

		public Stack[] GetStacks(int BuildingID)
		{
			LogEnter();
			return Try(() => stackModule.GetStacks(BuildingID)).OrThrow(GenerateFaultException);
		}
		public int GetStackQuantity(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();
			return Try(() => stackModule.GetStackQuantity(BuildingID, ResourceTypeID)).OrThrow(GenerateFaultException);
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
		public ResourceTypeIDs[] GetMissingResourcesToProduce(int FactoryID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.GetMissingResourcesToProduce(FactoryID)).OrThrow(GenerateFaultException);
		}

	

		public bool WorkerIsInBuilding(int WorkerID, int BuildingID)
		{
			LogEnter();

			return Try(() => locationCheckerModule.WorkerIsInBuilding(WorkerID, BuildingID)).OrThrow(GenerateFaultException);
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

			return Try(() => moverModule.BeginMoveTo(WorkerID, X, Y)).OrThrow(GenerateFaultException);
		}
		public Task MoveToBuilding(int WorkerID, int BuildingID)
		{
			LogEnter();

			return Try(() => moverModule.BeginMoveTo(WorkerID, BuildingID)).OrThrow(GenerateFaultException);
		}

		public Task CarryTo(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();

			return Try(() => carrierModule.BeginCarryTo(WorkerID, TargetBuildingID, ResourceTypeID)).OrThrow(GenerateFaultException);
		}
		public Task Take(int WorkerID, ResourceTypeIDs ResourceTypeID)
		{
			LogEnter();

			return Try(() => takerModule.BeginTake(WorkerID, ResourceTypeID)).OrThrow(GenerateFaultException);
		}
		public Task Store(int WorkerID)
		{
			LogEnter();

			return Try(() => storerModule.BeginStore(WorkerID)).OrThrow(GenerateFaultException);
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
