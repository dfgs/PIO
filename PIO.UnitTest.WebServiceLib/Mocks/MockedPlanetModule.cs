using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedPlanetModule : MockedDatabaseModule, IPlanetModule
	{
		public MockedPlanetModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Planet GetPlanet(int PlanetID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return new Planet() { PlanetID = PlanetID };
		}

		public Planet[] GetPlanets()
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Planet() { PlanetID = t });
		}
	}
}
