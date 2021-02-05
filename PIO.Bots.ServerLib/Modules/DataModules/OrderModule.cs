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
	public class OrderModule : DatabaseModule,IOrderModule
	{

		public OrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public Order GetOrder(int OrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying Order table (OrderID={OrderID})");
			query=new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID).From(BotsDB.OrderTable).Where(OrderTable.OrderID.IsEqualTo(OrderID));
			return TrySelectFirst<OrderTable, Order>(query).OrThrow<PIODataException>("Failed to query");
		}

		
		

		public Order[] GetOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Order table");
			query=new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID).From(BotsDB.OrderTable);
			return TrySelectMany<OrderTable, Order>(query).OrThrow<PIODataException>("Failed to query");
		}


		

		public void Assign(int OrderID,int BotID)
		{
			IUpdate query;

			LogEnter();
			Log(LogLevels.Information, $"Updating Order Table (OrderID={OrderID}, WorkerID={BotID})");
			query = new Update(BotsDB.OrderTable).Set(OrderTable.BotID, BotID).Where(OrderTable.OrderID.IsEqualTo(OrderID));
			Try(query).OrThrow<PIODataException>("Failed to update");
		}
		public void UnAssignAll(int BotID)
		{
			IUpdate query;

			LogEnter();
			Log(LogLevels.Information, $"Updating Order Table (WorkerID={BotID})");
			query = new Update(BotsDB.OrderTable).Set(OrderTable.BotID, null).Where(OrderTable.BotID.IsEqualTo(BotID));
			Try(query).OrThrow<PIODataException>("Failed to update");
		}



	}
}
