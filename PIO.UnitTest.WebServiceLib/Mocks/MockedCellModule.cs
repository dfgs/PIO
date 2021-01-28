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
	public class MockedCellModule : MockedDatabaseModule, ICellModule
	{
		public MockedCellModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		public Cell GetCell(int CellID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Cell() { CellID = CellID };
		}
		public Cell GetCell(int PlanetID, int X,int Y)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Cell() ;
		}

		

		public Cell[] GetCells(int PlanetID, int X, int Y, int Width, int Height)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Cell() { CellID = t,PlanetID=PlanetID });
		}

		


	}
}
