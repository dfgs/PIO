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
		private bool throwExceptionOnGetLastTask;
		private bool throwExceptionOnGeneratingTask;
		private bool existingTask;

		public MockedPIOClient(bool ThrowExceptionOnGetLastTask,bool ExistingTask,bool ThrowExceptionOnGeneratingTask)
		{
			this.throwExceptionOnGetLastTask = ThrowExceptionOnGetLastTask;this.existingTask = ExistingTask;this.throwExceptionOnGeneratingTask = ThrowExceptionOnGeneratingTask;
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
			throw new NotImplementedException();
		}

		public Building GetBuildingAtPos(int PlanetID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		public Building[] GetBuildings(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID)
		{
			throw new NotImplementedException();
		}

		public BuildingType[] GetBuildingTypes()
		{
			throw new NotImplementedException();
		}

		public Factory[] GetFactories(int PlanetID)
		{
			throw new NotImplementedException();
		}

		public Factory GetFactory(int FactoryID)
		{
			throw new NotImplementedException();
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
			if (throwExceptionOnGetLastTask) throw new InvalidOperationException("GETLASTTASK ERROR");
			if (!existingTask) return null;
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
			throw new NotImplementedException();
		}

		public Worker[] GetWorkers(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			throw new NotImplementedException();
		}

		public Models.Task Idle(int WorkerID, int Duration)
		{
			if (throwExceptionOnGeneratingTask) throw new NotImplementedException("IDLE ERROR");
			return new Models.Task() { WorkerID = WorkerID, ETA = DateTime.Now.AddSeconds(1) };
		}

		public Models.Task MoveTo(int WorkerID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		public Models.Task Produce(int WorkerID)
		{
			throw new NotImplementedException();
		}
	}
}
