using LogLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.BotsLib
{
	public class BotException:TryException
	{
		
		public BotException(string Message,Exception InnerException,int ComponentID,string ComponentName,string MethodName):base(Message,InnerException,ComponentID,ComponentName,MethodName)
		{

		}

	}
}
