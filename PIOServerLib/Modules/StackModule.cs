using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class StackModule : Module,IStackModule
	{
		private IDatabase database;

		public StackModule(ILogger Logger,IDatabase Database) : base(Logger)
		{
			this.database = Database;
		}

		public Row GetStack(int StackID)
		{
			return this.database.Execute(new Select<Stack>(Stack.StackID, Stack.FactoryID,Stack.ResourceID,Stack.Quantity).Where(Stack.StackID.IsEqualTo(StackID))).FirstOrDefault();
		}

		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			return this.database.Execute(new Select<Stack>(Stack.StackID, Stack.FactoryID, Stack.ResourceID, Stack.Quantity).Where(Stack.FactoryID.IsEqualTo(FactoryID)));
		}

	}
}
