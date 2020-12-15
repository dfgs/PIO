using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IOrderModule:IDatabaseModule
	{
		Order GetOrder(int OrderID);
		Order[] GetOrders();
		/*
		Order CreateOrder(OrderTypeIDs OrderTypeID, int WorkerID, int? X, int? Y,int? BuildingID, int? FactoryID, ResourceTypeIDs? ResourceTypeID, FactoryTypeIDs? FactoryTypeID, DateTime ETA);
		void DeleteOrder(int OrderID);*/
	}
}
