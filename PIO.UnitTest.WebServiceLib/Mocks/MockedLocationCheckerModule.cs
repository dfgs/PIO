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
	public class MockedLocationCheckerModule :MockedFunctionalModule,  ILocationCheckerModule
	{

		private bool result;

		public MockedLocationCheckerModule(bool ThrowException,bool Result) : base( ThrowException)
		{
			this.result = Result;
		}

		
		public bool WorkerIsInBuilding(int WorkerID, int BuildingID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return result;
		}

		public bool WorkerIsInFactory(int WorkerID, int FactoryID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return result;
		}
	}
}
