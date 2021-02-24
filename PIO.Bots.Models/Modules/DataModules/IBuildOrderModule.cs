using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.Models;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IBuildOrderModule:IDatabaseModule
	{
		BuildOrder GetBuildOrder(int BuildOrderID);
		BuildOrder[] GetBuildOrders(int PlanetID);
		BuildOrder[] GetBuildOrders(int PlanetID,int X,int Y);
		BuildOrder[] GetWaitingBuildOrders(int PlanetID);
		BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X,int Y);

	}
}
