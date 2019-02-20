using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Filters;
using NetORMLib.Queries;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class StackModule : DatabaseModule,IStackModule
	{

		public StackModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

	

		public StackRow GetStack(int StackID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where(Stack.StackID.IsEqualTo(StackID));
			return Try<StackRow>(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<StackRow> GetStacks(int FactoryID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where(Stack.FactoryID.IsEqualTo(FactoryID));
			return Try<StackRow>(query).OrThrow("Failed to query");
		}

		public StackRow GetStack(int FactoryID, int ResourceID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where( new AndFilter<Stack>( Stack.FactoryID.IsEqualTo(FactoryID), Stack.ResourceID.IsEqualTo(ResourceID)  ) );
			return Try<StackRow>(query).OrThrow("Failed to query").FirstOrDefault();
		}


	}
}
