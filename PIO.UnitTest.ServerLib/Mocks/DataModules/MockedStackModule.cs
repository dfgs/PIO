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
	public class MockedStackModule :IStackModule
	{
		private bool throwException;
		private List<Stack> items;

		public MockedStackModule(bool ThrowException,params Stack[]Items )
		{
			this.throwException = ThrowException;
			this.items = new List<Stack>(Items);
		}

		public Stack GetStack(int StackID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.StackID == StackID);
		}
		public Stack FindStack(int PlanetID,ResourceTypeIDs ResourceTypeID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => item.ResourceTypeID == ResourceTypeID);
		}

		public int GetStackQuantity(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => (item.BuildingID == BuildingID) && (item.ResourceTypeID==ResourceTypeID))?.Quantity??0;
		}

		public Stack[] GetStacks(int BuildingID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.BuildingID == BuildingID).ToArray();
		}

		public Stack GetStack(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.FirstOrDefault(item => (item.BuildingID == BuildingID) && (item.ResourceTypeID==ResourceTypeID));
		}

		public Stack InsertStack(int BuildingID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			Stack item;
			item = new Stack() {StackID=items.Count, BuildingID = BuildingID, ResourceTypeID = ResourceTypeID, Quantity = Quantity };
			items.Add(item);
			return item;
		}

		public void UpdateStack(int StackID, int Quantity)
		{
			Stack item;
			item = items.First(t => t.StackID == StackID);
			item.Quantity = Quantity;
		}
	}
}
