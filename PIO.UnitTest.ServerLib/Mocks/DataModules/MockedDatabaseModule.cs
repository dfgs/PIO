using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedDatabaseModule<T>
	{
		protected T[] items;
		public bool ThrowException
		{
			get;
			set;
		}
		public MockedDatabaseModule(bool ThrowException, params T[] Items)
		{
			this.ThrowException = ThrowException; this.items = Items;
		}

	}
}
