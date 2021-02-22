using PIO.Models;
using PIO.ModulesLib.Modules.FunctionalModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.Bots.Models.Modules
{
	public interface IOrderManagerModule : IFunctionalModule
	{
		

		void UnassignAll(int BotID);
		void Assign(int OrderID,int BotID);




		ProduceOrder[] GetWaitingProduceOrders(int PlanetID);
		HarvestOrder[] GetWaitingHarvestOrders(int PlanetID);
		BuildOrder[] GetWaitingBuildOrders(int PlanetID);






		Task CreateTask(int BotID,int WorkerID);

		Task CreateTaskFromProduceOrder(Worker Worker, ProduceOrder ProduceOrder);
		Task CreateTaskFromHarvestOrder(Worker Worker, HarvestOrder ProduceOrder);
		Task CreateTaskFromBuildOrder(Worker Worker, BuildOrder BuildOrder);



		ProduceOrder CreateProduceOrder(int PlanetID, int BuildingID);
		HarvestOrder CreateHarvestOrder(int PlanetID, int BuildingID);
		BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X, int Y);

	}
}
