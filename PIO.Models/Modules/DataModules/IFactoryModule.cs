using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IFactoryModule : IDatabaseModule
	{
		Factory GetFactory(int FactoryID);
		Factory GetFactory(int PlanetID, int X, int Y);
		Factory[] GetFactories(int PlanetID);

		//void SetHealthPoints(int FactoryID,int HealthPoints);
		Factory CreateFactory(int PlanetID, int X, int Y, int RemainingBuildSteps, FactoryTypeIDs FactoryTypeID);

	}
}
