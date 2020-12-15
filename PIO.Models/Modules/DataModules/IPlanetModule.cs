using NetORMLib;
using PIO.Models;
using PIO.ModulesLib.Modules.DataModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IPlanetModule : IDatabaseModule
	{
		Planet GetPlanet(int PlanetID);
		Planet[] GetPlanets();
	}
}
