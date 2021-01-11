using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IProduceOrderModule:IDatabaseModule
	{
		ProduceOrder GetProduceOrder(int ProduceOrderID);
		ProduceOrder[] GetProduceOrders();
		ProduceOrder[] GetProduceOrders(int PlanetID);
		ProduceOrder[] GetWaitingProduceOrders(int PlanetID);
		ProduceOrder CreateProduceOrder(int PlanetID,int FactoryID);

	}
}
