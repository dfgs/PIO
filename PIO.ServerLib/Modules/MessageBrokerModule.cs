using ModuleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using PIO.Models;

namespace PIO.ServerLib.Modules
{
	public class MessageBrokerModule : Module, IMessageBrokerModule
	{
		private readonly List<IMessageListenerModule> listeners;
		private readonly IEventModule eventModule;

		public MessageBrokerModule(ILogger Logger,IEventModule EventModule) : base(Logger)
		{
			listeners = new List<IMessageListenerModule>();
			this.eventModule = EventModule;
		}

		public void Post(Message Message)
		{
			Event ev;

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
