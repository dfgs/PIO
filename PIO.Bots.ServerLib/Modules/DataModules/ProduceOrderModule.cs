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
			query = new Select(ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID).From(BotsDB.ProduceOrderTable).Where(ProduceOrderTable.ProduceOrderID.IsEqualTo(ProduceOrderID));
			return TrySelectFirst<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}

		
		public ProduceOrder[] GetProduceOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ProduceOrder table");
			query = new Select(ProduceOrderTable.ProduceOrderID, ProduceOrderTable.OrderID, ProduceOrderTable.FactoryID).From(BotsDB.ProduceOrderTable);
			return TrySelectMany<ProduceOrderTable, ProduceOrder>(query).OrThrow<PIODataException>("Failed to query");
		}

		public ProduceOrder CreateProduceOrder(int OrderID,int FactoryID)
		{
			IInsert query;
			ProduceOrder item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into ProduceOrder table (OrderID={OrderID}, FactoryID={FactoryID})");
			item = new ProduceOrder() {OrderID=OrderID,FactoryID=FactoryID };
			query = new Insert().Into(BotsDB.ProduceOrderTable)
					.Set(ProduceOrderTable.OrderID, item.OrderID)
					.Set(ProduceOrderTable.FactoryID, item.FactoryID) ;
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.ProduceOrderID = Convert.ToInt32(result);
			return item;
		}

	}
}
