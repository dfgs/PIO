using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedIngredientModule : MockedDatabaseModule, IIngredientModule
	{
		public MockedIngredientModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Ingredient GetIngredient(int IngredientID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Ingredient() { IngredientID = IngredientID };
		}

		public Ingredient[] GetIngredients(BuildingTypeIDs BuildingTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Ingredient() { IngredientID = t, BuildingTypeID = BuildingTypeID });
		}

		
	}
}
