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

		
		Task CreateTask(int WorkerID);

		ProduceOrder CreateProduceOrder(int FactoryID);
		
	}
}
