using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;
using PIO.Bots.ServerLib.Tables;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;

namespace PIO.Bots.ServerLib.Modules
{
	public class HarvestOrderModule : DatabaseModule, IHarvestOrderModule
	{

		public HarvestOrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public HarvestOrder GetHarvestOrder(int HarvestOrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying HarvestOrder table (HarvestOrderID={HarvestOrderID})");
			query=new Select(OrderTable.OrderID,  OrderTable.BotID, HarvestOrderTable.HarvestOrderID, HarvestOrderTable.OrderID, HarvestOrderTable.PlanetID, HarvestOrderTable.BuildingID)
				.From(BotsDB.HarvestOrderTable.Join(BotsDB.OrderTable.On(HarvestOrderTable.OrderID,OrderTable.OrderID)))
				.Where(HarvestOrderTable.HarvestOrderID.IsEqualTo(HarvestOrderID));
			return TrySelectFirst<HarvestOrderTable, HarvestOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		
		public HarvestOrder[] GetHarvestOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying HarvestOrder table (PlanetID={PlanetID})");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, HarvestOrderTable.HarvestOrderID, HarvestOrderTable.OrderID, HarvestOrderTable.PlanetID, HarvestOrderTable.BuildingID)
				.From(BotsDB.HarvestOrderTable.Join(BotsDB.OrderTable.On(HarvestOrderTable.OrderID, OrderTable.OrderID)))
				.Where(HarvestOrderTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<HarvestOrderTable, HarvestOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public HarvestOrder[] GetWaitingHarvestOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying HarvestOrder table (PlanetID={PlanetID})");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, HarvestOrderTable.HarvestOrderID, HarvestOrderTable.OrderID, HarvestOrderTable.PlanetID, HarvestOrderTable.BuildingID)
				.From(BotsDB.HarvestOrderTable.Join(BotsDB.OrderTable.On(HarvestOrderTable.OrderID, OrderTable.OrderID)))
				.Where(HarvestOrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<HarvestOrderTable, HarvestOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public HarvestOrder CreateHarvestOrder(int PlanetID,int BuildingID)
		{
			IInsert query;
			HarvestOrder item;
			object result;

			LogEnter();

			item = new HarvestOrder() { PlanetID=PlanetID, BuildingID = BuildingID };

			Log(LogLevels.Information, $"Inserting into Order table ()");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into HarvestOrder table (OrderID={item.OrderID},PlanetID={item.PlanetID}, BuildingID={BuildingID})");
			query = new Insert().Into(BotsDB.HarvestOrderTable)
					.Set(HarvestOrderTable.OrderID, item.OrderID)
					.Set(HarvestOrderTable.PlanetID, item.PlanetID)
					.Set(HarvestOrderTable.BuildingID, item.BuildingID) ;
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.HarvestOrderID=Convert.ToInt32(result);
			return item;
		}

	}
}
