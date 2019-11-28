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
		private bool throwException;

		public int ID => 0;

		public string ModuleName => GetType().Name;

		public MockedModule(bool ThrowException)
		{
			this.throwException = ThrowException;
		}

		protected Row<T>[] GenerateRows(int Count,Action<dynamic> Initializer=null)
		{
			Row<T>[] items;
			dynamic item;

			// don't use yield operator here, in order to trigger exception

			if (throwException) throw new Exception();
			items = new Row<T>[Count];
			for (int t = 0; t < Count; t++)
			{
				item = new Row<T>();
				if (Initializer != null) Initializer(item);
				items[t] = item;
			}
			return items;
		}
		

	}
}
