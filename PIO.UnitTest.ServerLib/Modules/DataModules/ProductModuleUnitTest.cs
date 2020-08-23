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
	public class ProductModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetProduct()
		{
			MockedDatabase<Product> database;
			ProductModule module;
			Product result;

			database = new MockedDatabase<Product>(false, 1, (t) => new Product() { ProductID = t });
			module = new ProductModule(NullLogger.Instance, database);
			result = module.GetProduct(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.ProductID);
		}
		[TestMethod]
		public void ShouldGetProducts()
		{
			MockedDatabase<Product> database;
			ProductModule module;
			Product[] results;

			database = new MockedDatabase<Product>(false, 3, (t) => new Product() { ProductID = t });
			module = new ProductModule(NullLogger.Instance, database);
			results = module.GetProducts(1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].ProductID);
			}
		}
		[TestMethod]
		public void ShouldNotGetProductAndLogError()
		{
			MockedDatabase<Product> database;
			ProductModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Product>(true,1, (t) => new Product() { ProductID = t });
			module = new ProductModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetProduct(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetProductsAndLogError()
		{
			MockedDatabase<Product> database;
			ProductModule module;
			MemoryLogger logger;


			logger = new MemoryLogger(new DefaultLogFormatter());
			database = new MockedDatabase<Product>(true, 3, (t) => new Product() { ProductID = t });
			module = new ProductModule(logger, database);
			Assert.ThrowsException<Exception>(() => module.GetProducts(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => item.Contains("Error") && item.Contains(module.ModuleName)));
		}


	}
}
