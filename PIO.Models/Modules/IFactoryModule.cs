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
		Factory[] GetFactories(int PlanetID);

		void Build(int FactoryID);
		/*int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);*/
	}
}
