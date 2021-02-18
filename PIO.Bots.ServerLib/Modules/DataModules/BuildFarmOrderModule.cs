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
	public class BuildFarmOrderModule : DatabaseModule, IBuildFarmOrderModule
	{

		public BuildFarmOrderModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public BuildFarmOrder GetBuildFarmOrder(int BuildFarmOrderID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying BuildFarmOrder table (BuildFarmOrderID={BuildFarmOrderID})");
			query=new Select(OrderTable.OrderID,  OrderTable.BotID, BuildFarmOrderTable.BuildFarmOrderID, BuildFarmOrderTable.OrderID, BuildFarmOrderTable.BuildingTypeID, BuildFarmOrderTable.PlanetID, BuildFarmOrderTable.X, BuildFarmOrderTable.Y)
				.From(BotsDB.BuildFarmOrderTable.Join(BotsDB.OrderTable.On(BuildFarmOrderTable.OrderID,OrderTable.OrderID)))
				.Where(BuildFarmOrderTable.BuildFarmOrderID.IsEqualTo(BuildFarmOrderID));
			return TrySelectFirst<BuildFarmOrderTable, BuildFarmOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public BuildFarmOrder[] GetBuildFarmOrders()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFarmOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildFarmOrderTable.BuildFarmOrderID, BuildFarmOrderTable.OrderID, BuildFarmOrderTable.BuildingTypeID, BuildFarmOrderTable.PlanetID, BuildFarmOrderTable.X, BuildFarmOrderTable.Y)
								.From(BotsDB.BuildFarmOrderTable.Join(BotsDB.OrderTable.On(BuildFarmOrderTable.OrderID, OrderTable.OrderID)));
			return TrySelectMany<BuildFarmOrderTable, BuildFarmOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		public BuildFarmOrder[] GetBuildFarmOrders(int PlanetID, int X, int Y)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFarmOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildFarmOrderTable.BuildFarmOrderID, BuildFarmOrderTable.OrderID, BuildFarmOrderTable.BuildingTypeID, BuildFarmOrderTable.PlanetID, BuildFarmOrderTable.X, BuildFarmOrderTable.Y)
								.From(BotsDB.BuildFarmOrderTable.Join(BotsDB.OrderTable.On(BuildFarmOrderTable.OrderID, OrderTable.OrderID)))
								.Where(BuildFarmOrderTable.PlanetID.IsEqualTo(PlanetID).And(BuildFarmOrderTable.X.IsEqualTo(X)).And(BuildFarmOrderTable.Y.IsEqualTo(Y)));
			return TrySelectMany<BuildFarmOrderTable, BuildFarmOrder>(query).OrThrow<PIODataException>("Failed to query");
		}


		public BuildFarmOrder[] GetWaitingBuildFarmOrders(int PlanetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying BuildFarmOrder table");
			query = new Select(OrderTable.OrderID, OrderTable.BotID, BuildFarmOrderTable.BuildFarmOrderID, BuildFarmOrderTable.OrderID, BuildFarmOrderTable.BuildingTypeID, BuildFarmOrderTable.PlanetID, BuildFarmOrderTable.X, BuildFarmOrderTable.Y)
				.From(BotsDB.BuildFarmOrderTable.Join(BotsDB.OrderTable.On(BuildFarmOrderTable.OrderID, OrderTable.OrderID)))
				.Where(BuildFarmOrderTable.PlanetID.IsEqualTo(PlanetID).And( OrderTable.BotID.IsNull()));
			return TrySelectMany<BuildFarmOrderTable, BuildFarmOrder>(query).OrThrow<PIODataException>("Failed to query");
		}
		

		public BuildFarmOrder CreateBuildFarmOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X, int Y)
		{
			IInsert query;
			BuildFarmOrder item;
			object result;

			LogEnter();

			item = new BuildFarmOrder() { PlanetID = PlanetID, BuildingTypeID = BuildingTypeID, X=X,Y=Y };

			Log(LogLevels.Information, $"Inserting into Order table ()");
			query = new Insert().Into(BotsDB.OrderTable)
				.Set(OrderTable.BotID, item.BotID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.OrderID = Convert.ToInt32(result);

			Log(LogLevels.Information, $"Inserting into BuildFarmOrder table (OrderID={item.OrderID}, BuildingTypeID={BuildingTypeID},PlanetID={item.PlanetID}, X={X}, Y={Y})");
			query = new Insert().Into(BotsDB.BuildFarmOrderTable)
					.Set(BuildFarmOrderTable.OrderID, item.OrderID)
					.Set(BuildFarmOrderTable.PlanetID, item.PlanetID)
					.Set(BuildFarmOrderTable.X, item.X)
					.Set(BuildFarmOrderTable.Y, item.Y)
					.Set(BuildFarmOrderTable.BuildingTypeID, item.BuildingTypeID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");

			item.BuildFarmOrderID = Convert.ToInt32(result);
			return item;
		}



	}
}
