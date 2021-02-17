using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.Models;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IBuildFarmOrderModule:IDatabaseModule
	{
		BuildFarmOrder GetBuildFarmOrder(int BuildFarmOrderID);
		BuildFarmOrder[] GetBuildFarmOrders();
		BuildFarmOrder[] GetBuildFarmOrders(int PlanetID,  int X, int Y);
		BuildFarmOrder[] GetWaitingBuildFarmOrders(int PlanetID);
		BuildFarmOrder CreateBuildFarmOrder(int PlanetID, FarmTypeIDs FarmTypeID,int X,int Y);

	}
}
