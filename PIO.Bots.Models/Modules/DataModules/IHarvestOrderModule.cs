using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IHarvestOrderModule:IDatabaseModule
	{
		HarvestOrder GetHarvestOrder(int HarvestOrderID);
		HarvestOrder[] GetHarvestOrders();
		HarvestOrder[] GetHarvestOrders(int BuildingID);
		HarvestOrder[] GetWaitingHarvestOrders(int PlanetID);
		HarvestOrder CreateHarvestOrder(int PlanetID,int BuildingID);

	}
}
