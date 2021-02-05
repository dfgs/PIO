using LogLib;
using ModuleLib;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.Models;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class BotsService : Module, IBotsService
	{
		private IBotModule botModule;
		private IOrderModule orderModule;
		private IOrderManagerModule orderManagerModule;
		private IProduceOrderModule produceOrderModule;
		private IBuildFactoryOrderModule buildFactoryOrderModule;

		private IBotSchedulerModule botSchedulerModule;

		public BotsService(ILogger Logger,
			IBotModule BotModule, IOrderModule OrderModule, IProduceOrderModule ProduceOrderModule, IBuildFactoryOrderModule BuildFactoryOrderModule, IBotSchedulerModule BotSchedulerModule, IOrderManagerModule OrderManagerModule
		) : base(Logger)
		{
			LogEnter();
			this.botModule = BotModule; this.orderModule = OrderModule;this.produceOrderModule = ProduceOrderModule;this.buildFactoryOrderModule = BuildFactoryOrderModule;
			this.botSchedulerModule = BotSchedulerModule;
			this.orderManagerModule = OrderManagerModule;
		}

		private FaultException GenerateFaultException(Exception InnerException, int ComponentID, string ComponentName, string MethodName)
		{
			return new FaultException(InnerException.Message, new FaultCode(((PIOException)InnerException).FaultCode));
		}

		#region data
		public Bot GetBot(int BotID)
		{
			LogEnter();
			return Try(() => botModule.GetBot(BotID)).OrThrow(GenerateFaultException);
		}
		public Bot GetBotForWorker(int WorkerID)
		{
			LogEnter();
			return Try(() => botModule.GetBotForWorker(WorkerID)).OrThrow(GenerateFaultException);
		}
		public Bot[] GetBots()
		{
			LogEnter();
			return Try(() => botModule.GetBots()).OrThrow(GenerateFaultException);
		}

		public Order GetOrder(int OrderID)
		{
			LogEnter();
			return Try(() => orderModule.GetOrder(OrderID)).OrThrow(GenerateFaultException);
		}
		public Order[] GetOrders()
		{
			LogEnter();
			return Try(() => orderModule.GetOrders()).OrThrow(GenerateFaultException);
		}
		public ProduceOrder GetProduceOrder(int ProduceOrderID)
		{
			LogEnter();
			return Try(() => produceOrderModule.GetProduceOrder(ProduceOrderID)).OrThrow(GenerateFaultException);
		}
		public ProduceOrder[] GetProduceOrders()
		{
			LogEnter();
			return Try(() => produceOrderModule.GetProduceOrders()).OrThrow(GenerateFaultException);
		}
		public ProduceOrder[] GetProduceOrdersForFactory(int FactoryID)
		{
			LogEnter();
			return Try(() => produceOrderModule.GetProduceOrders(FactoryID)).OrThrow(GenerateFaultException);
		}


		public BuildFactoryOrder GetBuildFactoryOrder(int BuildFactoryOrderID)
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildFactoryOrder(BuildFactoryOrderID)).OrThrow(GenerateFaultException);
		}
		public BuildFactoryOrder[] GetBuildFactoryOrders()
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildFactoryOrders()).OrThrow(GenerateFaultException);
		}
		public BuildFactoryOrder[] GetBuildFactoryOrdersAtPosition(int PlanetID,int X,int Y)
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildFactoryOrders(PlanetID,X,Y)).OrThrow(GenerateFaultException);
		}

		#endregion

		#region functional
		public ProduceOrder CreateProduceOrder(int PlanetID, int FactoryID)
		{
			LogEnter();
			return Try(() => orderManagerModule.CreateProduceOrder(PlanetID,FactoryID)).OrThrow(GenerateFaultException);
		}
		public BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID, int X, int Y)
		{
			LogEnter();
			return Try(() => orderManagerModule.CreateBuildFactoryOrder(PlanetID, FactoryTypeID,X,Y)).OrThrow(GenerateFaultException);
		}

		public Bot CreateBot(int WorkerID)
		{
			LogEnter();
			return Try(() => botSchedulerModule.CreateBot(WorkerID)).OrThrow(GenerateFaultException);
		}


		#endregion

	}
}
