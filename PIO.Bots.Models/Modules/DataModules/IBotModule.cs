using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Bots.Models.Modules
{
	public interface IBotModule:IDatabaseModule
	{
		Bot GetBot(int BotID);
		Bot GetBotForWorker(int WorkerID);
		Bot[] GetBots();

		Bot CreateBot(int WorkerID);
		void DeleteBot(int BotID);

	}
}
