using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.Models;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IBuildFactoryOrderModule:IDatabaseModule
	{
		BuildFactoryOrder GetBuildFactoryOrder(int BuildFactoryOrderID);
		BuildFactoryOrder[] GetBuildFactoryOrders();
		BuildFactoryOrder[] GetBuildFactoryOrders(int PlanetID,  int X, int Y);
		BuildFactoryOrder[] GetWaitingBuildFactoryOrders(int PlanetID);
		BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID,int X,int Y);

	}
}
