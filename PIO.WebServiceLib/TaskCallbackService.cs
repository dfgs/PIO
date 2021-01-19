using LogLib;
using ModuleLib;
using PIO.Models;
using PIO.Models.Modules;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class TaskCallbackService: Module, ITaskCallbackService
	{
		private ITaskCallBack ServiceCallback = null;
		private static List<ITaskCallBack> Subscribers = new List<ITaskCallBack>();

		public TaskCallbackService(ILogger Logger, ISchedulerModule SchedulerModule) : base(Logger)
		{
			Subscribers = new List<ITaskCallBack>();
			SchedulerModule.TaskStarted += SchedulerModule_TaskStarted;
			SchedulerModule.TaskEnded += SchedulerModule_TaskEnded;
		}

		

		private FaultException GenerateFaultException(Exception InnerException, int ComponentID, string ComponentName, string MethodName)
		{
			return new FaultException(InnerException.Message, new FaultCode(((PIOException)InnerException).FaultCode));
		}

		

		public bool Subscribe()
		{
			LogEnter();

			Log(LogLevels.Information, "Subscribing callback");

			ServiceCallback = Try(()=>OperationContext.Current.GetCallbackChannel<ITaskCallBack>()).OrThrow(GenerateFaultException);
			lock (Subscribers)
			{
				if (Subscribers.Contains(ServiceCallback))
				{
					Log(LogLevels.Warning, "Callback is already subscribed");
					return false;
				}
				Subscribers.Add(ServiceCallback);
				return true;
			}
		}

		public bool Unsubscribe()
		{
			LogEnter();

			Log(LogLevels.Information, "Unsubscribing callback");

			ServiceCallback = Try(() => OperationContext.Current.GetCallbackChannel<ITaskCallBack>()).OrThrow(GenerateFaultException);
			lock (Subscribers)
			{
				if (!Subscribers.Contains(ServiceCallback))
				{
					Log(LogLevels.Warning, "Callback is not subscribed");
					return false;
				}
				Subscribers.Remove(ServiceCallback);
				return true;
			}
		}


		private void SchedulerModule_TaskEnded(object sender, TaskEventArgs e)
		{
			OnTaskStarted(e.Task);
		}

		private void SchedulerModule_TaskStarted(object sender, TaskEventArgs e)
		{
			OnTaskEnded(e.Task);
		}
		private async void OnTaskStarted(Models.Task Task)
		{
			List<ITaskCallBack> failedSubscribers;
			ITaskCallBack[] items;

			LogEnter();

			failedSubscribers = new List<ITaskCallBack>();
			Log(LogLevels.Information, "Notify subscribers");

			lock (Subscribers)
			{
				items = Subscribers.ToArray();
			}

			foreach (ITaskCallBack subscriber in items)
			{
				if (((IChannel)subscriber).State == CommunicationState.Opened)
				{
					//Try(async () => await subscriber.OnTaskStarted(Task)).OrAlert("Failed to notify subscriber");
					try
					{
						await subscriber.OnTaskStarted(Task);
					}
					catch
					{

					}
							
				}
				else failedSubscribers.Add(subscriber);
			}

			lock (Subscribers)
			{
				foreach (ITaskCallBack subscriber in failedSubscribers)
				{
					Subscribers.Remove(subscriber);
				}
			}
		}

		private async void OnTaskEnded(Models.Task Task)
		{
			List<ITaskCallBack> failedSubscribers;
			ITaskCallBack[] items;

			LogEnter();

			failedSubscribers = new List<ITaskCallBack>();
			Log(LogLevels.Information, "Notify subscribers");

			lock (Subscribers)
			{
				items = Subscribers.ToArray();
			}

			foreach (ITaskCallBack subscriber in items)
			{
				if (((IChannel)subscriber).State == CommunicationState.Opened)
				{
					//Try(async () => await subscriber.OnTaskStarted(Task)).OrAlert("Failed to notify subscriber");
					try
					{
						await subscriber.OnTaskEnded(Task);
					}
					catch
					{

					}

				}
				else failedSubscribers.Add(subscriber);
			}

			lock (Subscribers)
			{
				foreach (ITaskCallBack subscriber in failedSubscribers)
				{
					Subscribers.Remove(subscriber);
				}
			}
		}






	}
}
