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
	public class FSMModule : Module,IFSMModule
	{
		private IFactoryModule factoryModule;
		private IStateModule stateModule;
		private ITaskSchedulerModule taskSchedulerModule;

		public FSMModule(ILogger Logger,IFactoryModule FactoryModule,IStateModule StateModule,ITaskSchedulerModule TaskSchedulerModule) : base(Logger)
		{
			this.factoryModule = FactoryModule;
			this.stateModule = StateModule;
			this.taskSchedulerModule = TaskSchedulerModule;
		}

		/*public Row GetState(int StateID)
		{
			ISelect query;
			LogEnter();

			query = new Select<State>(State.StateID, State.Name).Where(State.StateID.IsEqualTo(StateID));
			return Try(query).OrThrow("Failed to query").FirstOrDefault();
		}*/

		public void Initialize(int FactoryID, int StateID)
		{
			dynamic state;
			LogEnter();

			state=Try(()=>stateModule.GetState(StateID)).OrThrow("Failed to get new state information");
			Try(()=>factoryModule.SetState(FactoryID, StateID)).OrThrow("Failed to initialize factory new state");
			Try( ()=> { taskSchedulerModule.StartTask(FactoryID, state.TaskID, DateTime.Now.AddSeconds(state.Duration)); } ).OrThrow("Failed to schedule new state"); 
		}


	}
}
