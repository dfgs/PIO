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
		private IBuildingModule buildingModule;
		private IWorkerModule workerModule;
		private IStackModule stackModule;
		private IResourceTypeModule resourceTypeModule;
		private IBuildingTypeModule buildingTypeModule;
		private ITaskTypeModule taskTypeModule;
		private IMaterialModule materialModule;
		private IIngredientModule ingredientModule;
		private IProductModule productModule;
		private ITaskModule taskModule;

		private IIdlerModule idlerModule;
		private IResourceCheckerModule resourceCheckerModule;
		private ILocationCheckerModule locationCheckerModule;
		private IProducerModule producerModule;
		private IHarvesterModule harvesterModule;
		private IMoverModule moverModule;
		private ITakerModule takerModule;
		private IStorerModule storerModule;

		private IBuilderModule builderModule;



		public PIOService(ILogger Logger,
			IPlanetModule PlanetModule, ICellModule CellModule,
			IBuildingModule BuildingModule,
			IWorkerModule WorkerModule,
			IStackModule StackModule,IResourceTypeModule ResourceTypeModule,
			IBuildingTypeModule BuildingTypeModule,
			ITaskTypeModule TaskTypeModule,
			IMaterialModule MaterialModule,
			IIngredientModule IngredientModule, IProductModule ProductModule, 
			ITaskModule TaskModule,

			ISchedulerModule SchedulerModule,

			IResourceCheckerModule ResourceCheckerModule,ILocationCheckerModule LocationCheckerModule,
			IIdlerModule IdlerModule, IProducerModule ProducerModule,IHarvesterModule HarvesterModule,
			IMoverModule MoverModule,ITakerModule TakerModule,IStorerModule StorerModule,
			IBuilderModule BuilderModule
		) :base(Logger)
		{
			LogEnter();
			this.planetModule = PlanetModule;
			this.cellModule = CellModule;
			
			this.buildingModule = BuildingModule;

			this.workerModule = WorkerModule;
			this.stackModule = StackModule;
			this.locationCheckerModule = LocationCheckerModule;
			this.resourceTypeModule = ResourceTypeModule;
			this.taskTypeModule = TaskTypeModule;
			
			this.buildingTypeModule = BuildingTypeModule;

			this.taskTypeModule = TaskTypeModule;
			this.materialModule = MaterialModule;
			this.ingredientModule = IngredientModule;
			this.productModule = ProductModule;
			this.taskModule = TaskModule;

	
			this.resourceCheckerModule = ResourceCheckerModule;
			this.idlerModule = IdlerModule;
			this.producerModule = ProducerModule;
			this.harvesterModule = HarvesterModule;
			this.moverModule = MoverModule;
			this.takerModule = TakerModule;
			this.storerModule = StorerModule;

			this.builderModule = BuilderModule;
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

		

		
		public Building GetBuilding(int BuildingID)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuilding(BuildingID)).OrThrow(GenerateFaultException);
		}
		public Building GetBuildingAtPos(int PlanetID, int X, int Y)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuilding(PlanetID, X, Y)).OrThrow(GenerateFaultException);
		}
		public Building[] GetBuildings(int PlanetID)
		{
			LogEnter();
			return Try(() => buildingModule.GetBuildings(PlanetID)).OrThrow(GenerateFaultException);
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

		public Material[] GetMaterials(BuildingTypeIDs BuildingTypeID)
		{
			LogEnter();
			return Try(() => materialModule.GetMaterials(BuildingTypeID)).OrThrow(GenerateFaultException);
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			LogEnter();
			return Try(() => ingredientModule.GetIngredient(IngredientID)).OrThrow(GenerateFaultException);
		}

		public Ingredient[] GetIngredients(BuildingTypeIDs BuildingTypeID)
		{
			LogEnter();
			return Try(() => ingredientModule.GetIngredients(BuildingTypeID)).OrThrow(GenerateFaultException);
		}
		public Product GetProduct(int ProductID)
		{
			LogEnter();
			return Try(() => productModule.GetProduct(ProductID)).OrThrow(GenerateFaultException);
		}

		public Product[] GetProducts(BuildingTypeIDs BuildingTypeID)
		{
			LogEnter();
			return Try(() => productModule.GetProducts(BuildingTypeID)).OrThrow(GenerateFaultException);
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

		public bool HasEnoughResourcesToProduce(int BuildingID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.HasEnoughResourcesToProduce(BuildingID)).OrThrow(GenerateFaultException);
		}
		public ResourceTypeIDs[] GetMissingResourcesToProduce(int BuildingID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.GetMissingResourcesToProduce(BuildingID)).OrThrow(GenerateFaultException);
		}

		public bool HasEnoughResourcesToBuild(int BuildingID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.HasEnoughResourcesToBuild(BuildingID)).OrThrow(GenerateFaultException);
		}
		public ResourceTypeIDs[] GetMissingResourcesToBuild(int BuildingID)
		{
			LogEnter();

			return Try(() => resourceCheckerModule.GetMissingResourcesToBuild(BuildingID)).OrThrow(GenerateFaultException);
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
		public Task Harvest(int WorkerID)
		{
			LogEnter();

			return Try(() => harvesterModule.BeginHarvest(WorkerID)).OrThrow(GenerateFaultException);
		}

		public Task MoveTo(int WorkerID, int X, int Y)
		{
			LogEnter();

			return Try(() => moverModule.BeginMoveTo(WorkerID, X, Y).FirstOrDefault()).OrThrow(GenerateFaultException);
		}
		public Task MoveToBuilding(int WorkerID, int BuildingID)
		{
			LogEnter();

			return Try(() => moverModule.BeginMoveTo(WorkerID, BuildingID).FirstOrDefault()).OrThrow(GenerateFaultException);
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
		public Task CreateBuilding(int WorkerID, BuildingTypeIDs BuildingTypeID)
		{
			LogEnter();

			return Try(() => builderModule.BeginCreateBuilding(WorkerID, BuildingTypeID)).OrThrow(GenerateFaultException);
		}
		

		public Task Build(int WorkerID)
		{
			LogEnter();

			return Try(() => builderModule.BeginBuild(WorkerID)).OrThrow(GenerateFaultException);
		}



		#endregion




	}
}
