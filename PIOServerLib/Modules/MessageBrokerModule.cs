using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;

namespace PIOServerLib.Modules
{
	public class MessageBrokerModule : Module, IMessageBrokerModule
	{
		private List<IMessageListenerModule> listeners;
		private IEventModule eventModule;

		public MessageBrokerModule(ILogger Logger,IEventModule EventModule) : base(Logger)
		{
			listeners = new List<IMessageListenerModule>();
			this.eventModule = EventModule;
		}

		public void Post(Message Message)
		{
			dynamic ev;

			LogEnter();
			ev = Try(() => eventModule.GetEvent(Message.EventID)).OrThrow($"Failed to get event {Message.EventID} information");
			Log(LogLevels.Information, $"Received event {ev.Name} for factory {Message.FactoryID}");
			foreach (IMessageListenerModule listener in listeners)
			{
				listener.Send(Message);
			}
		}

		public void Subscribe(IMessageListenerModule MessageListener)
		{
			listeners.Add(MessageListener);

		}




	}
}
