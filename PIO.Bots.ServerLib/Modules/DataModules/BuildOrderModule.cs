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
	public class BuildOrderModule : DatabaseModule, IBuildOrderModule
	{

		public BuildOrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public BuildOrder GetBuildOrder(int BuildOrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying BuildFactoryOrder table (BuildFactoryOrderID={BuildOrderID})");
			query=new Select(OrderTable.OrderID,  OrderTable.BotID, BuildOrderTable.BuildOrderID, BuildOrderTable.OrderID, BuildOrderTable.BuildingTypeID, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y)
				.From(BotsDB.BuildOrderTable.Join(BotsDB.OrderTable.On(BuildOrderTable.OrderID,OrderTable.OrderID)))
				.Where(BuildOrderTable.BuildOrderID.IsEqualTo(BuildOrderID));
			return TrySelectFirst<BuildOrderTable, BuildOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public BuildOrder[] GetBuildOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFactoryOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildOrderTable.BuildOrderID, BuildOrderTable.OrderID, BuildOrderTable.BuildingTypeID, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y)
								.From(BotsDB.BuildOrderTable.Join(BotsDB.OrderTable.On(BuildOrderTable.OrderID, OrderTable.OrderID)));
			return TrySelectMany<BuildOrderTable, BuildOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public BuildOrder[] GetBuildOrders(int PlanetID, int X, int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFactoryOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildOrderTable.BuildOrderID, BuildOrderTable.OrderID, BuildOrderTable.BuildingTypeID, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y)
								.From(BotsDB.BuildOrderTable.Join(BotsDB.OrderTable.On(BuildOrderTable.OrderID, OrderTable.OrderID)))
								.Where(BuildOrderTable.PlanetID.IsEqualTo(PlanetID).And(BuildOrderTable.X.IsEqualTo(X)).And(BuildOrderTable.Y.IsEqualTo(Y)));
			return TrySelectMany<BuildOrderTable, BuildOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public BuildOrder[] GetWaitingBuildOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFactoryOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildOrderTable.BuildOrderID, BuildOrderTable.OrderID, BuildOrderTable.BuildingTypeID, BuildOrderTable.PlanetID, BuildOrderTable.X, BuildOrderTable.Y)
				.From(BotsDB.BuildOrderTable.Join(BotsDB.OrderTable.On(BuildOrderTable.OrderID, OrderTable.OrderID)))
				.Where(BuildOrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<BuildOrderTable, BuildOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X, int Y)
		{
			IInsert query;
			BuildOrder item;
			object result;

			LogEnter();

			item = new BuildOrder() { PlanetID = PlanetID, BuildingTypeID = BuildingTypeID, X=X,Y=Y };

			Log(LogLevels.Information, $"Inserting into Order table ()");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into BuildFactoryOrder table (OrderID={item.OrderID}, BuildingTypeID={BuildingTypeID},PlanetID={item.PlanetID}, X={X}, Y={Y})");
			query = new Insert().Into(BotsDB.BuildOrderTable)
					.Set(BuildOrderTable.OrderID, item.OrderID)
					.Set(BuildOrderTable.PlanetID, item.PlanetID)
					.Set(BuildOrderTable.X, item.X)
					.Set(BuildOrderTable.Y, item.Y)
					.Set(BuildOrderTable.BuildingTypeID, item.BuildingTypeID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.BuildOrderID = Convert.ToInt32(result);
			return item;
		}



	}
}
