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
	public class BotModule : DatabaseModule,IBotModule
	{

		public BotModule(ILogger Logger, IDatabase Database) : base(Logger, Database)
		{
		}

		

		public Bot GetBot(int BotID)
		{
			ISelect query;
			LogEnter();
			
			Log(LogLevels.Information, $"Querying Bot table (BotID={BotID})");
			query=new Select(BotTable.BotID, BotTable.WorkerID).From(BotsDB.BotTable).Where(BotTable.BotID.IsEqualTo(BotID));
			return TrySelectFirst<BotTable, Bot>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Bot GetBotForWorker(int WorkerID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Bot table (WorkerID={WorkerID})");
			query = new Select(BotTable.BotID, BotTable.WorkerID).From(BotsDB.BotTable).Where(BotTable.WorkerID.IsEqualTo(WorkerID));
			return TrySelectFirst<BotTable, Bot>(query).OrThrow<PIODataException>("Failed to query");
		}



		public Bot[] GetBots()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Bot table");
			query=new Select(BotTable.BotID,  BotTable.WorkerID).From(BotsDB.BotTable);
			return TrySelectMany<BotTable, Bot>(query).OrThrow<PIODataException>("Failed to query");
		}


		public Bot CreateBot(int WorkerID)
		{
			IInsert query;
			Bot item;
			object result;

			LogEnter();
			Log(LogLevels.Information, $"Inserting into Bot table (WorkerID={WorkerID})");
			item = new Bot() {  WorkerID = WorkerID};
			query = new Insert().Into(BotsDB.BotTable).Set(BotTable.WorkerID, item.WorkerID);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.BotID = Convert.ToInt32(result);
			return item;
		}
			
		public void DeleteBot(int BotID)
		{
			IDelete query;

			LogEnter();
			Log(LogLevels.Information, $"Deleting from Bot table (BotID={BotID})");
			query = new Delete().From(BotsDB.BotTable).Where(BotTable.BotID.IsEqualTo(BotID));
			Try(query).OrThrow<PIODataException>("Failed to delete");
		}





	}
}
