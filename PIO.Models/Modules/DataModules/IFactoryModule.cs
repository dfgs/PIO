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
		Factory GetFactory(int X, int Y);
		Factory[] GetFactories(int PlanetID);

		//void SetHealthPoints(int FactoryID,int HealthPoints);
		/*int CreateFactory(int PlanetID,int FactoryTypeID,int StateID);
		void SetState(int FactoryID, int StateID);*/
	}
}
