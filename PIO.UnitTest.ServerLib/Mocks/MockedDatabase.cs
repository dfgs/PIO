using NetORMLib.Databases;
using NetORMLib.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.UnitTest.ServerLib.Mocks
{
	public class MockedDatabase : IDatabase
	{
		private bool throwException;
		private int results;

		public MockedDatabase(bool ThrowException,int Results )
		{
			this.throwException = ThrowException;
			this.results = Results;
		}

		public IEnumerable<TRow> Execute<TRow>(ISelect Query) where TRow : new()
		{
			if (throwException) throw new NotImplementedException();
			for (int t = 0; t < results; t++)
			{
				yield return new TRow();
			}
		}

		public object Execute(ISelectIdentity Query)
		{
			throw new NotImplementedException();
		}

		public void Execute(IQuery Query)
		{
			throw new NotImplementedException();
		}

		public void Execute(params IQuery[] Queries)
		{
			throw new NotImplementedException();
		}

		public void Execute(IEnumerable<IQuery> Queries)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetTables()
		{
			throw new NotImplementedException();
		}
	}
}
