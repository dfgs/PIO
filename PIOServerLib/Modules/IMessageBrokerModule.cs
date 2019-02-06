using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public interface IMessageBrokerModule:IModule
	{
		void Post(Message Message);
		void Subscribe(IMessageListenerModule MessageListener);
	}
}
