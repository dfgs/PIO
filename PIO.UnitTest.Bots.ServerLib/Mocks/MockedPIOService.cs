using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.Bots.ServiceLib.Mocks
{
	public class MockedPIOService : PIO.ClientLib.PIOServiceReference.IPIOService
	{
		private bool throwException;
		private Task lastTask;
		private bool hasEnoughResources;
		private bool inFactory;

		public MockedPIOService(bool ThrowException,Task LastTask,bool HasEnoughResources,bool InFactory)
		{
			this.throwException = ThrowException;this.lastTask = LastTask;
			this.hasEnoughResources = HasEnoughResources;
			this.inFactory = InFactory;
		}

		public Models.Task GetLastTask(int WorkerID)
		{
			if (throwException) throw new Exception("GetCurrentTask ERROR");
			return lastTask;
		}

		public Models.Task Idle(int WorkerID, int Duration)
		{
			if (throwException) throw new Exception("Idle ERROR");
			return new Task() { WorkerID = WorkerID, TaskTypeID = TaskTypeIDs.Idle };
		}
		public Models.Task Produce(int WorkerID)
		{
			if (throwException) throw new Exception("Produce ERROR");
			return new Task() { WorkerID = WorkerID, TaskTypeID = TaskTypeIDs.Produce };
		}

		public bool HasEnoughResourcesToProduce(int FactoryID)
		{
			if (throwException) throw new Exception("HasEnoughResources ERROR");
			return hasEnoughResources;
		}

		public bool WorkerIsInFactory(int WorkerID, int FactoryID)
		{
			if (throwException) throw new Exception("WorkerLocation ERROR");
			return inFactory;
		}

		public bool WorkerIsInBuilding(int WorkerID, int BuildingID)
		{
			throw new NotImplementedException();
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

		
		

		public Models.Task MoveTo(int WorkerID, int X, int Y)
		{
			throw new NotImplementedException();
		}

		

		
	}
}
