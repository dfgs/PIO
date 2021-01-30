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
		void UnAssignAll(int BotID);
		void Assign(int OrderID,int BotID);

		
	}
}
