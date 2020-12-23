using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PIO.Models
{
	[ServiceContract]
	public interface IPIOService
	{
		#region data
		[OperationContract]
		Planet GetPlanet(int PlanetID);
		[OperationContract]
		Planet[] GetPlanets();
		[OperationContract]
		Building GetBuilding(int BuildingID);
		[OperationContract]
		Building GetBuildingAtPos(int PlanetID, int X, int Y);
		[OperationContract]
		Building[] GetBuildings(int PlanetID);
		[OperationContract]
		Factory GetFactory(int FactoryID);
		[OperationContract]
		Factory GetFactoryAtPos(int PlanetID, int X,int Y);
		[OperationContract]
		Factory[] GetFactories(int PlanetID);
		[OperationContract]
		Worker GetWorker(int WorkerID);
		[OperationContract]
		Worker[] GetWorkers(int FactoryID);
		[OperationContract]
		Stack GetStack(int StackID);
		[OperationContract]
		Stack[] GetStacks(int FactoryID);
		[OperationContract]
		int GetStackQuantity(int FactoryID,ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		ResourceType GetResourceType(ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		ResourceType[] GetResourceTypes();
		[OperationContract]
		BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		BuildingType[] GetBuildingTypes();
		[OperationContract]
		FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		FactoryType[] GetFactoryTypes();
		[OperationContract]
		TaskType GetTaskType(TaskTypeIDs TaskTypeID);
		[OperationContract]
		TaskType[] GetTaskTypes();
		[OperationContract]
		Material GetMaterial(int MaterialID);
		[OperationContract]
		Material[] GetMaterials(FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		Ingredient GetIngredient(int IngredientID);
		[OperationContract]
		Ingredient[] GetIngredients(FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		Product GetProduct(int ProductID);
		[OperationContract]
		Product[] GetProducts(FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		Task GetTask(int TaskID);
		[OperationContract]
		Task[] GetTasks(int WorkerID);
		[OperationContract]
		Task GetLastTask(int WorkerID);
		#endregion

		#region functional
		[OperationContract]
		bool HasEnoughResourcesToProduce(int FactoryID);
		[OperationContract]
		ResourceTypeIDs[] GetMissingResourcesToProduce(int FactoryID);
		[OperationContract]
		bool WorkerIsInFactory(int WorkerID, int FactoryID);
		[OperationContract]
		bool WorkerIsInBuilding(int WorkerID, int BuildingID);
		[OperationContract]
		Task Idle(int WorkerID,int Duration);
		[OperationContract]
		Task Produce(int WorkerID);
		[OperationContract]
		Task MoveTo(int WorkerID, int X, int Y);
		[OperationContract]
		Task MoveToFactory(int WorkerID, int FactoryID);
		[OperationContract]
		Task CarryTo(int WorkerID, int TargetFactoryID, ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		Task CreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		Task BuildFactory(int WorkerID);
		#endregion

	}


}
