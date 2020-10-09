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
using PIO.Models.Exceptions;

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
			return TrySelectFirst <StackTable,Stack>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Stack[] GetStacks(int FactoryID)
		{
			ISelect<StackTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (FactoryID={FactoryID})");
			query = new Select<StackTable>(StackTable.StackID, StackTable.FactoryID, StackTable.ResourceTypeID, StackTable.Quantity).Where(StackTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<StackTable,Stack>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Stack InsertStack(int FactoryID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			IInsert<StackTable> query;
			Stack item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Stack table (FactoryID={FactoryID}, ResourceTypeID={ResourceTypeID}, Quantity={Quantity})");
			item = new Stack() { FactoryID = FactoryID, ResourceTypeID= ResourceTypeID, Quantity= Quantity};
			query = new Insert<StackTable>().Set(StackTable.FactoryID, item.FactoryID).Set(StackTable.ResourceTypeID, item.ResourceTypeID).Set(StackTable.Quantity, item.Quantity);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.StackID = Convert.ToInt32(result);
			return item;
		}

		public void UpdateStack(int StackID, int Quantity)
		{
			IUpdate<StackTable> update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Stack table (StackID={StackID}, Quantity={Quantity})");
			update = new Update<StackTable>().Set(StackTable.Quantity, Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}


	}
}
