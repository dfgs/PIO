using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
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

		public Stack GetStack(int StackID)
		{
			ISelect<StackTable> query;
			LogEnter();

			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceID, StackTable.Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			return TrySelectMany <StackTable,Stack>(query).OrThrow("Failed to query").FirstOrDefault();
		}

		public IEnumerable<Stack> GetStacks(int FactoryID)
		{
			ISelect<StackTable> query;
			LogEnter();

			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceID, StackTable.Quantity).Where(StackTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<StackTable,Stack>(query).OrThrow("Failed to query");
		}

	}
}
