using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServerLib.Modules
{
	public interface IPlanetModule : IDatabaseModule
	{
		Planet GetPlanet(int PlanetID);
		IEnumerable<Planet> GetPlanets();
	}
}
