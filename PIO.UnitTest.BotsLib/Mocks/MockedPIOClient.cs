using PIO.ClientLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPIOService = PIO.ClientLib.PIOServiceReference.IPIOService;

namespace PIO.UnitTest.BotsLib.Mocks
{
	public class MockedPIOClient : IPIOService
	{
		private bool throwException;

		public int IdleCount
		{
			get;
			private set;
		}

		public int MoveCount
		{
			get;
			private set;
		}

		public int ProduceCount
		{
			get;
			private set;
		}


		public MockedPIOClient(bool ThrowException)
		{
			this.throwException = ThrowException;
		}
		
		public Models.Task BuildFactory(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public Models.Task CarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Models.Task CreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Building GetBuilding(int BuildingID)
		{
			return new Building() { BuildingID = BuildingID };
		}

		public Building GetBuildingAtPos(int PlanetID, int X, int Y)
		{
			return new Building() { BuildingID = 1,X=X,Y=Y };
		}

		public Building[] GetBuildings(int PlanetID)
		{
			throw new NotImplementedException();
		}

		

		public Factory[] GetFactories(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Factory GetFactory(int FactoryID)
		{
			return new Factory() { FactoryID = FactoryID,BuildingID=FactoryID };
		}

		public Factory GetFactoryAtPos(int PlanetID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		public FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public FactoryType[] GetFactoryTypes()
		{
			throw new NotImplementedException();
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			throw new NotImplementedException();
		}

		public Ingredient[] GetIngredients(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Models.Task GetLastTask(int WorkerID)
		{
			if (throwException) throw new InvalidOperationException("CLIENT ERROR");
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}

		public Material GetMaterial(int MaterialID)
		{
			throw new NotImplementedException();
		}

		public Material[] GetMaterials(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Planet GetPlanet(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Planet[] GetPlanets()
		{
			throw new NotImplementedException();
		}

		public Product GetProduct(int ProductID)
		{
			throw new NotImplementedException();
		}

		public Product[] GetProducts(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public ResourceType GetResourceType(ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public ResourceType[] GetResourceTypes()
		{
			throw new NotImplementedException();
		}

		public Stack GetStack(int StackID)
		{
			throw new NotImplementedException();
		}
		public Stack FindStack(int PlanetID,ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public int GetStackQuantity(int FactoryID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Stack[] GetStacks(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Models.Task GetTask(int TaskID)
		{
			throw new NotImplementedException();
		}

		public Models.Task[] GetTasks(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public TaskType GetTaskType(TaskTypeIDs TaskTypeID)
		{
			throw new NotImplementedException();
		}

		public TaskType[] GetTaskTypes()
		{
			throw new NotImplementedException();
		}

		public Worker GetWorker(int WorkerID)
		{
			return new Worker() { WorkerID = WorkerID };
		}

		public Worker[] GetWorkers(int FactoryID)
		{
			throw new NotImplementedException();
		}
		public Worker[] GetAllWorkers()
		{
			throw new NotImplementedException();
		}

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			return true;
		}
		public ResourceTypeIDs[] GetMissingResourcesToProduce(int FactoryID)
		{
			return new ResourceTypeIDs[] { };
		}

		public Models.Task Idle(int WorkerID, int Duration)
		{
			if (throwException) throw new InvalidOperationException("CLIENT ERROR");
			IdleCount++;
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}

		public Models.Task MoveTo(int WorkerID, int X, int Y)
		{
			if (throwException) throw new InvalidOperationException("CLIENT ERROR");
			MoveCount++;
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}
		public Models.Task MoveToBuilding(int WorkerID, int BuildingID)
		{
			if (throwException) throw new InvalidOperationException("CLIENT ERROR");
			MoveCount++;
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}
		public Models.Task Produce(int WorkerID)
		{
			if (throwException) throw new InvalidOperationException("CLIENT ERROR");
			ProduceCount++;
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}

		public bool WorkerIsInFactory(int WorkerID, int FactoryID)
		{
			throw new NotImplementedException();
		}

		public bool WorkerIsInBuilding(int WorkerID, int BuildingID)
		{
			throw new NotImplementedException();
		}

		public Task<Planet> GetPlanetAsync(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Task<Planet[]> GetPlanetsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Factory> GetFactoryAsync(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Task<Factory> GetFactoryAtPosAsync(int PlanetID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		public Task<Factory[]> GetFactoriesAsync(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Task<Worker> GetWorkerAsync(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public Task<Worker[]> GetWorkersAsync(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Task<Worker[]> GetAllWorkersAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Stack> GetStackAsync(int StackID)
		{
			throw new NotImplementedException();
		}

		public Task<Stack> FindStackAsync(int PlanetID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Stack[]> GetStacksAsync(int BuildingID)
		{
			throw new NotImplementedException();
		}

		public Task<int> GetStackQuantityAsync(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<ResourceType> GetResourceTypeAsync(ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<ResourceType[]> GetResourceTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<FactoryType> GetFactoryTypeAsync(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<FactoryType[]> GetFactoryTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<TaskType> GetTaskTypeAsync(TaskTypeIDs TaskTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<TaskType[]> GetTaskTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Material> GetMaterialAsync(int MaterialID)
		{
			throw new NotImplementedException();
		}

		public Task<Material[]> GetMaterialsAsync(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Ingredient> GetIngredientAsync(int IngredientID)
		{
			throw new NotImplementedException();
		}

		public Task<Ingredient[]> GetIngredientsAsync(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Product> GetProductAsync(int ProductID)
		{
			throw new NotImplementedException();
		}

		public Task<Product[]> GetProductsAsync(FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> GetTaskAsync(int TaskID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task[]> GetTasksAsync(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> GetLastTaskAsync(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public Task<bool> HasEnoughResourcesToProduceAsync(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Task<ResourceTypeIDs[]> GetMissingResourcesToProduceAsync(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Task<bool> WorkerIsInBuildingAsync(int WorkerID, int BuildingID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> IdleAsync(int WorkerID, int Duration)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> ProduceAsync(int WorkerID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> MoveToAsync(int WorkerID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> MoveToBuildingAsync(int WorkerID, int BuildingID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> CarryToAsync(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> CreateBuildingAsync(int WorkerID, FactoryTypeIDs FactoryTypeID)
		{
			throw new NotImplementedException();
		}

		public Task<Models.Task> BuildFactoryAsync(int WorkerID)
		{
			throw new NotImplementedException();
		}

	
	}
}
