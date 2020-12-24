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
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class StackModule : DatabaseModule,IStackModule
	{

		public StackModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Stack GetStack(int StackID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (StackID={StackID})");
			query=new Select(StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity).From(PIODB.StackTable).Where(StackTable.StackID.IsEqualTo(StackID));
			return TrySelectFirst <StackTable,Stack>(query).OrThrow<PIODataException>("Failed to query");
		}

		

		public Stack[] GetStacks(int BuildingID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (BuildingID={BuildingID})");
			query=new Select(StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity).From(PIODB.StackTable).Where(StackTable.BuildingID.IsEqualTo(BuildingID));
			return TrySelectMany<StackTable,Stack>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Stack GetStack(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			ISelect query;

			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (BuildingID={BuildingID}, ResourceTypeID={ResourceTypeID})");
			query=new Select(StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity).From(PIODB.StackTable).Where(new AndFilter(StackTable.BuildingID.IsEqualTo(BuildingID), StackTable.ResourceTypeID.IsEqualTo(ResourceTypeID)));
			return TrySelectFirst<StackTable, Stack>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Stack FindStack(int PlanetID, ResourceTypeIDs ResourceTypeID)
		{
			ISelect query;

			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (PlanetID={PlanetID}, ResourceTypeID={ResourceTypeID}), Quantity>0");
			query=new Select(StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity)
				.From(
					PIODB.StackTable.Join(PIODB.BuildingTable.On(StackTable.BuildingID, BuildingTable.BuildingID))
				)
				.Where(new AndFilter(BuildingTable.PlanetID.IsEqualTo(PlanetID), StackTable.ResourceTypeID.IsEqualTo(ResourceTypeID), StackTable.Quantity.IsGreaterThan(0)));
			return TrySelectFirst<StackTable, Stack>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Stack InsertStack(int BuildingID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			IInsert query;
			Stack item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Stack table (BuildingID={BuildingID}, ResourceTypeID={ResourceTypeID}, Quantity={Quantity})");
			item=new Stack() { BuildingID = BuildingID, ResourceTypeID= ResourceTypeID, Quantity= Quantity};
			query = new Insert().Into(PIODB.StackTable).Set(StackTable.BuildingID, item.BuildingID).Set(StackTable.ResourceTypeID, item.ResourceTypeID).Set(StackTable.Quantity, item.Quantity);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.StackID=Convert.ToInt32(result);
			return item;
		}

		public void UpdateStack(int StackID, int Quantity)
		{
			IUpdate update;

			LogEnter();

			Log(LogLevels.Information, $"Updating Stack table (StackID={StackID}, Quantity={Quantity})");
			update=new Update(PIODB.StackTable).Set(StackTable.Quantity, Quantity).Where(StackTable.StackID.IsEqualTo(StackID));
			Try(update).OrThrow<PIODataException>("Failed to update");
		}
		public int GetStackQuantity(int BuildingID, ResourceTypeIDs ResourceTypeID)
		{
			ISelect query;
			Stack stack;

			LogEnter();

			Log(LogLevels.Information, $"Querying Stack table (BuildingID={BuildingID}, ResourceTypeID={ResourceTypeID})");
			query=new Select(StackTable.StackID, StackTable.BuildingID, StackTable.ResourceTypeID, StackTable.Quantity).From(PIODB.StackTable).Where( new AndFilter( StackTable.BuildingID.IsEqualTo(BuildingID), StackTable.ResourceTypeID.IsEqualTo(ResourceTypeID)));
			stack=TrySelectFirst<StackTable, Stack>(query).OrThrow<PIODataException>("Failed to query");
			if (stack == null) return 0;
			else return stack.Quantity;
		}

	}
}
