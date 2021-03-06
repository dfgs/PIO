﻿using PIO.Models;

using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public class MockedStackModule : MockedDatabaseModule, IStackModule
	{
		public MockedStackModule(int Count,bool ThrowException) : base(Count, ThrowException)
		{
		}

		

		public Stack GetStack(int StackID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Stack() { StackID = StackID };
		}
		public Stack FindStack(int PlanetID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Stack() { StackID = 1,ResourceTypeID=ResourceTypeID};
		}
		public int GetStackQuantity(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return 3;
		}
		public Stack GetStack(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return new Stack() { StackID = 1,ResourceTypeID=ResourceTypeID };
		}
		public Stack[] GetStacks(int BuildingID)
		{
			if (ThrowException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return Generate((t) => new Stack() { StackID = t, BuildingID = BuildingID });
		}

		public Stack InsertStack(int BuildingID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			throw new NotImplementedException();
		}

		public void UpdateStack(int StackID, int Quantity)
		{
			throw new NotImplementedException();
		}
	}
}
