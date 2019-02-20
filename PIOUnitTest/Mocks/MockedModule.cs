using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOUnitTest.Mocks
{
	public abstract class MockedModule<T>:IModule
	{
		private readonly bool throwException;

		public int ID => 0;

		public string ModuleName => GetType().Name;

		public MockedModule(bool ThrowException)
		{
			this.throwException = ThrowException;
		}

		protected TRow[] GenerateRows<TRow>(int Count,Action<TRow> Initializer=null)
			where TRow:new()
		{
			TRow[] items;
			TRow item;

			// don't use yield operator here, in order to trigger exception

			if (throwException) throw new Exception();
			items = new TRow[Count];
			for (int t = 0; t < Count; t++)
			{
				item= new TRow();
				if (Initializer != null) Initializer(item);
				items[t] = item;
			}
			return items;
		}
		

	}
}
