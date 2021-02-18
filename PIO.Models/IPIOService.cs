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
	// netsh http add urlacl url=http://+:8733/PIO/service user=dfgs8
	// netsh http add urlacl url=http://+:8735/PIO/TaskCallback/service user=dfgs8
	// netsh http add urlacl url=http://+:8734/PIO/Bots/service user=dfgs8
	[ServiceContract]
	public interface IPIOService
	{
		#region data
		[OperationContract]
		Planet GetPlanet(int PlanetID);
		[OperationContract]
		Planet[] GetPlanets();
		[OperationContract]
		Cell GetCell(int CellID);
		[OperationContract]
		Cell GetCellAtPos(int PlanetID, int X, int Y);
		[OperationContract]
		Cell[] GetCells(int PlanetID, int X, int Y,int Width,int Height);

		[OperationContract]
		Factory GetFactory(int FactoryID);
		[OperationContract]
		Factory GetFactoryAtPos(int PlanetID, int X, int Y);
		[OperationContract]
		Factory[] GetFactories(int PlanetID);
		
		[OperationContract]
		Building GetBuilding(int BuildingID);
		[OperationContract]
		Building GetBuildingAtPos(int PlanetID, int X, int Y);
		[OperationContract]
		Building[] GetBuildings(int PlanetID);



		[OperationContract]
		Worker GetWorker(int WorkerID);
		[OperationContract]
		Worker[] GetWorkers(int PlanetID);
		[OperationContract]
		Worker[] GetAllWorkers();
		[OperationContract]
		Stack GetStack(int StackID);
		[OperationContract]
		Stack FindStack(int PlanetID, ResourceTypeIDs ResourceTypeID);

		[OperationContract]
		Stack[] GetStacks(int BuildingID);
		[OperationContract]
		int GetStackQuantity(int BuildingID, ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		ResourceType GetResourceType(ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		ResourceType[] GetResourceTypes();

		[OperationContract]
		FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID);
		[OperationContract]
		FactoryType[] GetFactoryTypes();
		
		[OperationContract]
		BuildingType GetBuildingType(BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		BuildingType[] GetBuildingTypes();
		[OperationContract]
		TaskType GetTaskType(TaskTypeIDs TaskTypeID);
		[OperationContract]
		TaskType[] GetTaskTypes();
		[OperationContract]
		Material GetMaterial(int MaterialID);
		[OperationContract]
		Material[] GetMaterials(BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		Ingredient GetIngredient(int IngredientID);
		[OperationContract]
		Ingredient[] GetIngredients(BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		Product GetProduct(int ProductID);
		[OperationContract]
		Product[] GetProducts(BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		Task GetTask(int TaskID);
		[OperationContract]
		Task[] GetTasks(int WorkerID);
		[OperationContract]
		Task GetLastTask(int WorkerID);
		#endregion

		#region functional
		[OperationContract]
		bool HasEnoughResourcesToProduce(int BuildingID);
		[OperationContract]
		ResourceTypeIDs[] GetMissingResourcesToProduce(int BuildingID);
		
		[OperationContract]
		bool HasEnoughResourcesToBuild(int BuildingID);
		[OperationContract]
		ResourceTypeIDs[] GetMissingResourcesToBuild(int BuildingID);

		[OperationContract]
		bool WorkerIsInBuilding(int WorkerID, int BuildingID);
		[OperationContract]
		Task Idle(int WorkerID,int Duration);
		[OperationContract]
		Task Produce(int WorkerID);
		[OperationContract]
		Task MoveTo(int WorkerID, int X, int Y);
		[OperationContract]
		Task MoveToBuilding(int WorkerID, int BuildingID);
		[OperationContract]
		Task Take(int WorkerID, ResourceTypeIDs ResourceTypeID);
		[OperationContract]
		Task Store(int WorkerID);

		[OperationContract]
		Task CreateBuilding(int WorkerID, BuildingTypeIDs BuildingTypeID);
		[OperationContract]
		Task Build(int WorkerID);
		



		#endregion

	}


}
