using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedPlanetModule :MockedDatabaseModule<Planet>, IPlanetModule
	{

		public MockedPlanetModule(bool ThrowException, params Planet[] Items):base(ThrowException,Items)
		{
		}

		public Planet[] GetPlanets()
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.ToArray();
		}

		public Planet GetPlanet(int PlanetID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.PlanetID == PlanetID);
		}

		public Planet CreatePlanet(string Name, int Width, int Height)
		{
			throw new NotImplementedException();
		}
	}
}
