using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedIngredientModule :IIngredientModule
	{
		private bool throwException;
		private List<Ingredient> items;

		public MockedIngredientModule(bool ThrowException, params Ingredient[] Items)
		{
			this.throwException = ThrowException;
			this.items = new List<Ingredient>(Items);
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.IngredientID == IngredientID);
		}

		public Ingredient[] GetIngredients(FactoryTypeIDs FactoryTypeID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.FactoryTypeID == FactoryTypeID).ToArray();
		}



		
	}
}
