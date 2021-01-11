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
		int ProduceOrderCount
		{
			get;
		}

		void UnassignAll(int WorkerID);
		void Assign(int OrderID,int WorkerID);

		ProduceOrder[] GetWaitingProduceOrders(int PlanetID);
		
		Task CreateTask(int WorkerID);

		Task CreateTaskFromProduceOrder(Worker Worker, ProduceOrder ProduceOrder);



		ProduceOrder CreateProduceOrder(int PlanetID,int FactoryID);
		
	}
}
