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
			query=new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID )
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID,OrderTable.OrderID)))
				.Where(ProduceOrderTable.ProduceOrderID.IsEqualTo(ProduceOrderID));
			return TrySelectFirst<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public ProduceOrder[] GetProduceOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID)
								.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID, OrderTable.OrderID)));
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public ProduceOrder[] GetProduceOrders(int FactoryID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID)
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID, OrderTable.OrderID)))
				.Where(ProduceOrderTable.FactoryID.IsEqualTo(FactoryID));
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public ProduceOrder[] GetWaitingProduceOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.PlanetID, OrderTable.BotID, ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID)
				.From(BotsDB.ProduceOrderTable.Join(BotsDB.OrderTable.On(ProduceOrderTable.OrderID, OrderTable.OrderID)))
				.Where(OrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public ProduceOrder CreateProduceOrder(int PlanetID,int FactoryID)
		{
			IInsert query;
			ProduceOrder item;
			object result;

			LogEnter();

			item = new ProduceOrder() { PlanetID=PlanetID, FactoryID = FactoryID };

			Log(LogLevels.Information, $"Inserting into Order table (PlanetID={item.PlanetID})");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.PlanetID,item.PlanetID)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into ProduceOrder table (OrderID={item.OrderID}, FactoryID={FactoryID})");
			query = new Insert().Into(BotsDB.ProduceOrderTable)
					.Set(ProduceOrderTable.OrderID, item.OrderID)
					.Set(ProduceOrderTable.FactoryID, item.FactoryID) ;
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			
			item.ProduceOrderID=Convert.ToInt32(result);
			return item;
		}

	}
}
