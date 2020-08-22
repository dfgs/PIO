using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class IngredientModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetIngredient()
		{
			MockedDatabase<Ingredient> database;
			IngredientModule module;
			Ingredient result;

			database = new MockedDatabase<Ingredient>(false, 1, (t) => new Ingredient() { IngredientID = t });
			module = new IngredientModule(NullLogger.Instance, database);
			result = module.GetIngredient(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.IngredientID);
		}
		[TestMethod]
		public void ShouldGetIngredients()
		{
			MockedDatabase<Ingredient> database;
			IngredientModule module;
			Ingredient[] results;

			database = new MockedDatabase<Ingredient>(false, 3, (t) => new Ingredient() { IngredientID = t });
			module = new IngredientModule(NullLogger.Instance, database);
			results = module.GetIngredients(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].IngredientID);
			}
		}
		[TestMethod]
		public void ShouldNotGetIngredientAndLogError()
		{
			MockedDatabase<Ingredient> database;
			IngredientModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Ingredient>(true,1, (t) => new Ingredient() { IngredientID = t });
			module = new IngredientModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetIngredient(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetIngredientsAndLogError()
		{
			MockedDatabase<Ingredient> database;
			IngredientModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Ingredient>(true, 3, (t) => new Ingredient() { IngredientID = t });
			module = new IngredientModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetIngredients(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
