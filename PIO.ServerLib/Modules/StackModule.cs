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

			Log(LogLevels.Information, $"Querying stack with StackID {StackID}");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			return TrySelectFirst <StackTable,Stack>(query).OrThrow("Failed to query");
		}

		public Stack[] GetStacks(int FactoryID)
		{
			ISelect<StackTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying stacks with FactoryID {FactoryID}");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<StackTable,Stack>(query).OrThrow("Failed to query");
		}

		public void Consume(int StackID,  int Quantity)
		{
			Stack stack;
			ISelect<StackTable> query;
			IUpdate<StackTable> update;

			LogEnter();

			Log(LogLevels.Information, $"Consuming {Quantity} resource(s) from stack with StackID {StackID}");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			stack=TrySelectFirst<StackTable, Stack>(query).OrThrow("Failed to query");
			if (stack == null) throw new InvalidOperationException($"Invalid stack with StackID {StackID}");
			if (stack.Quantity<Quantity) throw new InvalidOperationException($"Not enough quantity in stack with StackID {StackID}");
			stack.Quantity -= Quantity;
			update = new Update<StackTable>().Set(StackTable.Quantity, stack.Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			Try(update).OrThrow("Failed to update");

		}


	}
}
