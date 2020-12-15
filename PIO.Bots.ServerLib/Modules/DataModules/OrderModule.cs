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
			query = new Select(OrderTable.OrderID).From(BotsDB.OrderTable).Where(OrderTable.OrderID.IsEqualTo(OrderID));
			return TrySelectFirst<OrderTable, Order>(query).OrThrow<PIODataException>("Failed to query");
		}

		
		/*public Order[] GetOrders(int WorkerID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Order table (WorkerID={WorkerID})");
			query = new Select(OrderTable.OrderID).From(BotsDB.OrderTable).Where(OrderTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectMany<OrderTable, Order>(query).OrThrow<PIODataException>("Failed to query");
		}*/

		public Order[] GetOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Order table");
			query = new Select(OrderTable.OrderID).From(BotsDB.OrderTable);
			return TrySelectMany<OrderTable, Order>(query).OrThrow<PIODataException>("Failed to query");
		}


		/*public Order CreateOrder(OrderTypeIDs OrderTypeID, int WorkerID, int? X, int? Y,int? BuildingID, int? TargetFactoryID, ResourceTypeIDs? ResourceTypeID, FactoryTypeIDs? FactoryTypeID, DateTime ETA)
		{
			IInsert query;
			Order item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Order table (OrderTypeID={OrderTypeID}, WorkerID={WorkerID}, ETA={ETA})");
			item = new Order() { OrderTypeID=OrderTypeID, WorkerID=WorkerID, X=X,Y=Y,BuildingID=BuildingID, FactoryID =TargetFactoryID, ResourceTypeID=ResourceTypeID, FactoryTypeID=FactoryTypeID, ETA=ETA };
			query = new Insert().Into(BotsDB.OrderTable).Set(OrderTable.OrderTypeID,item.OrderTypeID).Set(OrderTable.WorkerID, item.WorkerID)
				.Set(OrderTable.X,item.X).Set(OrderTable.Y,item.Y)
				.Set(OrderTable.BuildingID, item.BuildingID).Set(OrderTable.FactoryID, item.FactoryID)
				.Set(OrderTable.ResourceTypeID, item.ResourceTypeID).Set(OrderTable.FactoryTypeID,item.FactoryTypeID).Set(OrderTable.ETA,item.ETA);
			result=Try(query).OrThrow<PIODataException>("Failed to insert");
			item.OrderID = Convert.ToInt32(result);
			return item;
		}

		public void DeleteOrder(int OrderID)
		{
			IDelete query;

			LogEnter();
			Log(LogLevels.Information, $"Deleting from Order table (OrderID={OrderID})");
			query = new Delete().From(BotsDB.OrderTable).Where(OrderTable.OrderID.IsEqualTo(OrderID));
			Try(query).OrThrow<PIODataException>("Failed to delete");
		}*/

	}
}
