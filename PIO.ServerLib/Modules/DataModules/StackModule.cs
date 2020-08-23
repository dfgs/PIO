using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetORMLib.Filters;

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

			Log(LogLevels.Information, $"Querying Stack table (StackID={StackID})");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			return TrySelectFirst <StackTable,Stack>(query).OrThrow("Failed to query");
		}

		public Stack[] GetStacks(int FactoryID)
		{
			ISelect<StackTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (FactoryID={FactoryID})");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<StackTable,Stack>(query).OrThrow("Failed to query");
		}
		/*public bool HasEnoughResources(int FactoryID, int ResourceTypeID, int Quantity)
		{
			ISelect<StackTable> query;
			Stack stack;
			LogEnter();

			if (Quantity == 0) return true;

			Log(LogLevels.Information, $"Querying Stack table (FactoryID={FactoryID}, ResourceTypeID={ResourceTypeID})");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(new AndFilter<StackTable>(StackTable.FactoryID.IsEqualTo(FactoryID), StackTable.ResourceTypeID.IsEqualTo(ResourceTypeID)) );
			stack= TrySelectFirst<StackTable, Stack>(query).OrThrow("Failed to query");
			return ((stack != null) && (stack.Quantity >= Quantity));
		}*/

		public void Consume(int FactoryID, int ResourceTypeID, int Quantity)
		{
			Stack stack;
			ISelect<StackTable> query;
			IUpdate<StackTable> update;

			LogEnter();

			if (Quantity == 0) return;

			Log(LogLevels.Information, $"Querying Stack table (FactoryID={FactoryID}, ResourceTypeID={ResourceTypeID})");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(new AndFilter<StackTable>(StackTable.FactoryID.IsEqualTo(FactoryID), StackTable.ResourceTypeID.IsEqualTo(ResourceTypeID)));
			stack = TrySelectFirst<StackTable, Stack>(query).OrThrow("Failed to query");
			if ((stack == null) || (stack.Quantity < Quantity)) throw new InvalidOperationException($"Not enough quantity in stack");

			stack.Quantity -= Quantity;
			Log(LogLevels.Information, $"Updating Stack table (StackID={stack.StackID}, Quantity={stack.Quantity})");
			update = new Update<StackTable>().Set(StackTable.Quantity, stack.Quantity).Where(StackTable.StackID.IsEqualTo(stack.StackID));
			Try(update).OrThrow("Failed to update");
		}


	}
}
