using NetORMLib;
using PIOServerLib.Modules;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOUnitTest.Mocks
{
	public class MockedPlanetModule : MockedModule<Planet>, IPlanetModule
	{

		public MockedPlanetModule(bool ThrowException):base(ThrowException)
		{
		}

		public PlanetRow GetPlanet(int PlanetID)
		{
			return GenerateRows<PlanetRow>(1,(item)=>item.Name="New planet" ).FirstOrDefault() ;
		}

		public IEnumerable<PlanetRow> GetPlanets()
		{
			return GenerateRows<PlanetRow>(1, (item) => item.Name = "New planet");
		}

	}
}
