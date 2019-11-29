using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IFactoryModule : IDatabaseModule
	{
		Factory GetFactory(int FactoryID);
		IEnumerable<Factory> GetFactories(int PlanetID);
		/*int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);*/
	}
}
