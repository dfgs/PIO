using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class StackModule : DatabaseModule,IStackModule
	{

		public StackModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Row<Stack> GetStack(int StackID)
		{
			ISelect<Stack> query;
			LogEnter();

			query = new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where(Stack.StackID.IsEqualTo(StackID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<Row<Stack>> GetStacks(int FactoryID)
		{
			ISelect<Stack> query;
			LogEnter();

			query = new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where(Stack.FactoryID.IsEqualTo(FactoryID));
			return Try(query).OrThrow("Failed to query");
		}

	}
}
