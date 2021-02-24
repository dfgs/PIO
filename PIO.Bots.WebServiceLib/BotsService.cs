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
		private IHarvestOrderModule harvestOrderModule;
		private IBuildOrderModule buildFactoryOrderModule;

		private IBotSchedulerModule botSchedulerModule;

		public BotsService(ILogger Logger,
			IBotModule BotModule, IOrderModule OrderModule, 
			IProduceOrderModule ProduceOrderModule,IHarvestOrderModule HarvestOrderModule,
			IBuildOrderModule BuildFactoryOrderModule,
			IBotSchedulerModule BotSchedulerModule, IOrderManagerModule OrderManagerModule
		) : base(Logger)
		{
			LogEnter();
			this.botModule = BotModule; this.orderModule = OrderModule;
			this.produceOrderModule = ProduceOrderModule;this.harvestOrderModule = HarvestOrderModule;
			this.buildFactoryOrderModule = BuildFactoryOrderModule;
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

		
		public ProduceOrder GetProduceOrder(int ProduceOrderID)
		{
			LogEnter();
			return Try(() => produceOrderModule.GetProduceOrder(ProduceOrderID)).OrThrow(GenerateFaultException);
		}
		public ProduceOrder[] GetProduceOrders(int PlanetID)
		{
			LogEnter();
			return Try(() => produceOrderModule.GetProduceOrders(PlanetID)).OrThrow(GenerateFaultException);
		}
		
		public HarvestOrder GetHarvestOrder(int HarvestOrderID)
		{
			LogEnter();
			return Try(() => harvestOrderModule.GetHarvestOrder(HarvestOrderID)).OrThrow(GenerateFaultException);
		}
		public HarvestOrder[] GetHarvestOrders(int PlanetID)
		{
			LogEnter();
			return Try(() => harvestOrderModule.GetHarvestOrders(PlanetID)).OrThrow(GenerateFaultException);
		}
		


		public BuildOrder GetBuildOrder(int BuildFactoryOrderID)
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildOrder(BuildFactoryOrderID)).OrThrow(GenerateFaultException);
		}
		public BuildOrder[] GetBuildOrders(int PlanetID)
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildOrders(PlanetID)).OrThrow(GenerateFaultException);
		}
		public BuildOrder[] GetBuildOrdersAtPosition(int PlanetID,int X,int Y)
		{
			LogEnter();
			return Try(() => buildFactoryOrderModule.GetBuildOrders(PlanetID,X,Y)).OrThrow(GenerateFaultException);
		}



		#endregion

		#region functional
		public ProduceOrder CreateProduceOrder(int PlanetID, int BuildingID)
		{
			LogEnter();
			return Try(() => orderManagerModule.CreateProduceOrder(PlanetID, BuildingID)).OrThrow(GenerateFaultException);
		}
		public HarvestOrder CreateHarvestOrder(int PlanetID, int BuildingID)
		{
			LogEnter();
			return Try(() => orderManagerModule.CreateHarvestOrder(PlanetID, BuildingID)).OrThrow(GenerateFaultException);
		}
		public BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X, int Y)
		{
			LogEnter();
			return Try(() => orderManagerModule.CreateBuildOrder(PlanetID, BuildingTypeID, X, Y)).OrThrow(GenerateFaultException);
		}
		

		public Bot CreateBot(int WorkerID)
		{
			LogEnter();
			return Try(() => botSchedulerModule.CreateBot(WorkerID)).OrThrow(GenerateFaultException);
		}
		public void DeleteBot(int BotID)
		{
			LogEnter();
			Try(() => botSchedulerModule.DeleteBot(BotID)).OrThrow(GenerateFaultException);
		}


		#endregion

	}
}
