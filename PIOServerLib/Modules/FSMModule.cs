using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib.Modules
{
	public class FSMModule : Module,IFSMModule,IMessageListenerModule
	{
		private IFactoryModule factoryModule;
		private IStateModule stateModule;
		private ITransitionModule transitionModule;
		private ITaskSchedulerModule taskSchedulerModule;

		public FSMModule(ILogger Logger,IFactoryModule FactoryModule,IStateModule StateModule,ITransitionModule TransitionModule, ITaskSchedulerModule TaskSchedulerModule) : base(Logger)
		{
			this.factoryModule = FactoryModule;
			this.stateModule = StateModule;
			this.transitionModule = TransitionModule;
			this.taskSchedulerModule = TaskSchedulerModule;
		}

		


		private void UpdateState(int FactoryID,int StateID)
		{
			dynamic state;

			LogEnter();
			state = Try(() => stateModule.GetState(StateID)).OrThrow($"Failed to get state {StateID} information");
			Log(LogLevels.Information, $"Update factory {FactoryID} state to {state.Name}");
			Try(() => { factoryModule.SetState(FactoryID, StateID); }).OrThrow($"Failed to update factory {FactoryID} state");
			Try(() => { taskSchedulerModule.StartTask(FactoryID, state.TaskID, DateTime.Now.AddSeconds(state.Duration)); }).OrThrow("Failed to schedule new state");
		}

		public void Initialize(int FactoryID, int StateID)
		{
			LogEnter();
			UpdateState(FactoryID, StateID);
		}

		public void Send(Message Message)
		{
			dynamic factory;
			dynamic transition;

			factory = Try(() => factoryModule.GetFactory(Message.FactoryID)).OrThrow($"Failed to get factory {Message.FactoryID}");
			transition = Try(() => transitionModule.GetTransition(factory.StateID, Message.EventID)).OrThrow("Failed to get transition");
			if (transition==null)
			{
				Log(LogLevels.Warning, $"No transition found for current state in factory {Message.FactoryID}");
				return;
			}
			UpdateState(Message.FactoryID, transition.NextStateID);
		}


	}
}
