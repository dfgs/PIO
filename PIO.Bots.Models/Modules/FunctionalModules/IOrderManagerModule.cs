using PIO.ModulesLib.Modules.FunctionalModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models.Modules
{
	public interface IOrderManagerModule : IFunctionalModule
	{
		int ProduceOrderCount
		{
			get;
		}

		ProduceOrder CreateProduceOrder(int FactoryID);
		
	}
}
