using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Exceptions;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.UnitTest.WebServiceLib.Mocks
{
	public abstract class MockedDatabaseModule:IDatabaseModule
	{
		private int count;
		public int Count
		{
			get { return count; }
		}

		private bool throwException;
		public bool ThrowException
		{
			get { return throwException; }
		}
		public MockedDatabaseModule(int Count,bool ThrowException)
		{
			this.count = Count;
			this.throwException = ThrowException;
		}
		
		protected T[] Generate<T>( Func<int,T> Generator)
		{
			T[] items;

			items = new T[count];
			for(int t=0;t<count;t++)
			{
				items[t] = Generator(t);
			}

			return items;
		}

	}

}
