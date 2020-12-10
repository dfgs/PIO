using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.BotsLib
{
	public class BotEvent
	{
		public IBot Bot
		{
			get;
			private set;
		}

		public BotEvent(IBot Bot)
		{
			this.Bot = Bot;

		}


	}
}
