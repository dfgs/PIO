using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.Models.Exceptions;
using PIO.Models.Modules;
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
		
		public Stack[] GetStacks(int FactoryID)
		{
			if (throwException) throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest");
			return items.Where(item => item.FactoryID == FactoryID).ToArray();
		}

		public Stack InsertStack(int FactoryID, int ResourceTypeID, int Quantity)
		{
			Stack item;
			item = new Stack() {StackID=items.Count, FactoryID = FactoryID, ResourceTypeID = ResourceTypeID, Quantity = Quantity };
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
