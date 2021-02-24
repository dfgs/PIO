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
	public class ProduceOrderModule : DatabaseModule, IProduceOrderModule
	{

		public ProduceOrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public ProduceOrder GetProduceOrder(int ProduceOrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying ProduceOrder table (ProduceOrderID={ProduceOrderID})");
			query=new Select(OrderTable.OrderID,  OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.PlanetID, ProduceOrderTable.BuildingID)
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID,OrderTable.OrderID)))
				.Where(ProduceOrderTable.ProduceOrderID.IsEqualTo(ProduceOrderID));
			return TrySelectFirst<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		
		public ProduceOrder[] GetProduceOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table (PlanetID={PlanetID})");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.PlanetID, ProduceOrderTable.BuildingID)
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID, OrderTable.OrderID)))
				.Where(ProduceOrderTable.PlanetID.IsEqualTo(PlanetID));
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public ProduceOrder[] GetWaitingProduceOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table (PlanetID={PlanetID})");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.PlanetID, ProduceOrderTable.BuildingID)
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID, OrderTable.OrderID)))
				.Where(ProduceOrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public ProduceOrder CreateProduceOrder(int PlanetID,int BuildingID)
		{
			IInsert query;
			ProduceOrder item;
			object result;

			LogEnter();

			item = new ProduceOrder() { PlanetID=PlanetID, BuildingID = BuildingID };

			Log(LogLevels.Information, $"Inserting into Order table ()");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into ProduceOrder table (OrderID={item.OrderID},PlanetID={item.PlanetID}, BuildingID={BuildingID})");
			query = new Insert().Into(BotsDB.ProduceOrderTable)
					.Set(ProduceOrderTable.OrderID, item.OrderID)
					.Set(ProduceOrderTable.PlanetID, item.PlanetID)
					.Set(ProduceOrderTable.BuildingID, item.BuildingID) ;
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.ProduceOrderID=Convert.ToInt32(result);
			return item;
		}

	}
}
