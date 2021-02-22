using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
using PIO.ServerLib.Modules;
using PIO.UnitTest.ServerLib.Mocks;

namespace PIO.UnitTest.ServerLib.Modules
{
	[TestClass]
	public class CellModuleUnitTest
	{
		[TestMethod]
		public void ShouldGetCell()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			Cell result;

			database = new MockedDatabase<Cell>(false, 1, (t) => new Cell() { CellID = t });
			module = new CellModule(NullLogger.Instance, database);
			result = module.GetCell(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.CellID);
		}
		[TestMethod]
		public void ShouldGetCellUsingCoordinates()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			Cell result;

			database = new MockedDatabase<Cell>(false, 1, (t) => new Cell() { CellID = t, X = 3, Y = 4 });
			module = new CellModule(NullLogger.Instance, database);
			result = module.GetCell(1,3,4);
			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.CellID);
			Assert.AreEqual(3, result.X);
			Assert.AreEqual(4, result.Y);
		}
		[TestMethod]
		public void ShouldNotGetCellUsingCoordinates()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			Cell result;

			database = new MockedDatabase<Cell>(false, 0, (t) => new Cell() { CellID = t, X = 3, Y = 4 });
			module = new CellModule(NullLogger.Instance, database);
			result = module.GetCell(1, 4,3);
			Assert.IsNull(result);
		}
		[TestMethod]
		public void ShouldGetCells()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			Cell[] results;

			database = new MockedDatabase<Cell>(false, 3, (t) => new Cell() { CellID = t,X=t,Y=t });
			module = new CellModule(NullLogger.Instance, database);
			results = module.GetCells(0,0,0,1,1);
			Assert.IsNotNull(results);
			Assert.AreEqual(3, results.Length);
			for(int t=0;t<3;t++)
			{
				Assert.IsNotNull(results[t]);
				Assert.AreEqual(t, results[t].CellID);
			}
		}
		[TestMethod]
		public void ShouldNotGetCellAndLogError()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Cell>(true,1, (t) => new Cell() { CellID = t });
			module = new CellModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetCell(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetCellUsingCoordinateAndLogError()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();
			database = new MockedDatabase<Cell>(true, 1, (t) => new Cell() { CellID = t });
			module = new CellModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetCell(1, 3,4));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}
		[TestMethod]
		public void ShouldNotGetCellsAndLogError()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Cell>(true, 3, (t) => new Cell() { CellID = t });
			module = new CellModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.GetCells(1,0,0,3,3));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName==module.ModuleName)));
		}


		[TestMethod]
		public void ShouldCreateCell()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			Cell result;

			database = new MockedDatabase<Cell>(false, 1, (t) => new Cell() { CellID = t });
			module = new CellModule(NullLogger.Instance, database);
			result = module.CreateCell(1, 10, 10);
			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.X);
			Assert.AreEqual(1, database.InsertedCount);
		}
		[TestMethod]
		public void ShouldNotCreateCellAndLogError()
		{
			MockedDatabase<Cell> database;
			CellModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();
			database = new MockedDatabase<Cell>(true, 1, (t) => new Cell() { CellID = t });
			module = new CellModule(logger, database);
			Assert.ThrowsException<PIODataException>(() => module.CreateCell(1, 10, 10));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, database.InsertedCount);
		}



	}
}
