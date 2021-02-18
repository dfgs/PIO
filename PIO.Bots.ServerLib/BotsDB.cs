using PIO.Bots.ServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.ServerLib
{
	public static class BotsDB
	{
		public static BotTable BotTable = new BotTable();
		public static OrderTable OrderTable = new OrderTable();
		public static ProduceOrderTable ProduceOrderTable = new ProduceOrderTable();
		public static BuildOrderTable BuildOrderTable = new BuildOrderTable();


	}
}
