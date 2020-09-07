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
		private RowFactoryDelegate<T> rowFactoryDelegate;

		public int InsertedCount
		{
			get;
			private set;
		}
		public int UpdatedCount
		{
			get;
			private set;
		}
		public int DeletedCount
		{
			get;
			private set;
		}


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
			// simulate select top 
			if (Query.Limit >= 0)
			{
				yield return (TRow)(object)rowFactoryDelegate(results - 1);
				yield break;
			}
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
			if (throwException) throw new NotImplementedException();

			switch (Query)
			{
				case IUpdate update:
					UpdatedCount++;
					break;
				case IDelete delete:
					DeletedCount++;
					break;
				case IInsert insert:
					InsertedCount++;
					break;
			}
		}

		public object Execute(params IQuery[] Queries)
		{
			foreach (IQuery query in Queries) Execute(query);
			return null;
		}

		public object Execute(IEnumerable<IQuery> Queries)
		{
			foreach (IQuery query in Queries) Execute(query);
			return null;
		}

		public IEnumerable<string> GetTables()
		{
			throw new NotImplementedException();
		}
	}
}
