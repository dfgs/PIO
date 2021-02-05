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
		BuildFactoryOrder[] GetWaitingBuildFactoryOrders(int PlanetID);






		Task CreateTask(int BotID,int WorkerID);

		Task CreateTaskFromProduceOrder(Worker Worker, ProduceOrder ProduceOrder);
		Task CreateTaskFromBuildFactoryOrder(Worker Worker, BuildFactoryOrder BuildFactoryOrder);






		ProduceOrder CreateProduceOrder(int PlanetID, int FactoryID);
		BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID,int X,int Y);

	}
}
