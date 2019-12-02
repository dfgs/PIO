using NetORMLib.Databases;
using NetORMLib.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public delegate T RowFactoryDelegate<T>(int Index);

	public class MockedDatabase<T> : IDatabase
	{
		private bool throwException;
		private int results;
		private RowFactoryDelegate<T> rowFactoryDelegate ;

		public MockedDatabase(bool ThrowException,int Results, RowFactoryDelegate<T> RowFactoryDelegate )
		{
			this.throwException = ThrowException;
			this.results = Results;
			this.rowFactoryDelegate = RowFactoryDelegate;
		}

		public IEnumerable<TRow> Execute<TRow>(ISelect Query)
			where TRow:new()
		{
			if (throwException) throw new NotImplementedException();
			for (int t = 0; t < results; t++)
			{
				yield return (TRow)(object)rowFactoryDelegate(t) ;
			}
		}

		public object Execute(ISelectIdentity Query)
		{
			throw new NotImplementedException();
		}

		public void Execute(IQuery Query)
		{
		}

		public void Execute(params IQuery[] Queries)
		{
		}

		public void Execute(IEnumerable<IQuery> Queries)
		{
		}

		public IEnumerable<string> GetTables()
		{
			throw new NotImplementedException();
		}
	}
}
