using PIO.Models;
using PIO.Models.Modules;
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
			if (ThrowException) throw new Exception("Mocked exception");
			return new Ingredient() { IngredientID = IngredientID };
		}

		public Ingredient[] GetIngredients(int FactoryTypeID)
		{
			if (ThrowException) throw new Exception("Mocked exception");
			return Generate((t) => new Ingredient() { IngredientID = t,FactoryTypeID=FactoryTypeID });
		}

		
	}
}
