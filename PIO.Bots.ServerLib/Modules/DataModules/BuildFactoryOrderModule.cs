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
	public class BuildFactoryOrderModule : DatabaseModule, IBuildFactoryOrderModule
	{

		public BuildFactoryOrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public BuildFactoryOrder GetBuildFactoryOrder(int BuildFactoryOrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying BuildFactoryOrder table (BuildFactoryOrderID={BuildFactoryOrderID})");
			query=new Select(OrderTable.OrderID, OrderTable.BotID, BuildFactoryOrderTable.BuildFactoryOrderID, BuildFactoryOrderTable.OrderID, BuildFactoryOrderTable.FactoryTypeID, BuildFactoryOrderTable.X, BuildFactoryOrderTable.Y)
				.From(BotsDB.BuildFactoryOrderTable.Join(BotsDB.OrderTable.On(BuildFactoryOrderTable.OrderID,OrderTable.OrderID)))
				.Where(BuildFactoryOrderTable.BuildFactoryOrderID.IsEqualTo(BuildFactoryOrderID));
			return TrySelectFirst<BuildFactoryOrderTable, BuildFactoryOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public BuildFactoryOrder[] GetBuildFactoryOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFactoryOrder table");
			query = new Select(OrderTable.OrderID,OrderTable.BotID, BuildFactoryOrderTable.BuildFactoryOrderID, BuildFactoryOrderTable.OrderID, BuildFactoryOrderTable.FactoryTypeID, BuildFactoryOrderTable.X, BuildFactoryOrderTable.Y)
								.From(BotsDB.BuildFactoryOrderTable.Join(BotsDB.OrderTable.On(BuildFactoryOrderTable.OrderID, OrderTable.OrderID)));
			return TrySelectMany<BuildFactoryOrderTable, BuildFactoryOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public BuildFactoryOrder[] GetWaitingBuildFactoryOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFactoryOrder table");
			query = new Select(OrderTable.OrderID,  OrderTable.BotID, BuildFactoryOrderTable.BuildFactoryOrderID, BuildFactoryOrderTable.OrderID, BuildFactoryOrderTable.FactoryTypeID, BuildFactoryOrderTable.X, BuildFactoryOrderTable.Y)
				.From(BotsDB.BuildFactoryOrderTable.Join(BotsDB.OrderTable.On(BuildFactoryOrderTable.OrderID, OrderTable.OrderID)))
				.Where(OrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<BuildFactoryOrderTable, BuildFactoryOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID, int X, int Y)
		{
			IInsert query;
			BuildFactoryOrder item;
			object result;

			LogEnter();

			item = new BuildFactoryOrder() { PlanetID = PlanetID, FactoryTypeID = FactoryTypeID,X=X,Y=Y };

			Log(LogLevels.Information, $"Inserting into Order table");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.PlanetID, item.PlanetID)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into BuildFactoryOrder table (OrderID={item.OrderID}, FactoryTypeID={FactoryTypeID}, X={X}, Y={Y})");
			query = new Insert().Into(BotsDB.BuildFactoryOrderTable)
					.Set(BuildFactoryOrderTable.OrderID, item.OrderID)
					.Set(BuildFactoryOrderTable.X, item.X)
					.Set(BuildFactoryOrderTable.Y, item.Y)
					.Set(BuildFactoryOrderTable.FactoryTypeID, item.FactoryTypeID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.BuildFactoryOrderID = Convert.ToInt32(result);
			return item;
		}



	}
}
