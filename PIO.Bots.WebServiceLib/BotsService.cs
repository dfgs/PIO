using LogLib;
using ModuleLib;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
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
		private IOrderModule orderModule;
		private IProduceOrderModule produceOrderModule;


		public BotsService(ILogger Logger,
			IOrderModule OrderModule, IProduceOrderModule ProduceOrderModule
		) : base(Logger)
		{
			LogEnter();
			this.orderModule = OrderModule;this.produceOrderModule = ProduceOrderModule;
		}

		private FaultException GenerateFaultException(Exception InnerException, int ComponentID, string ComponentName, string MethodName)
		{
			return new FaultException(InnerException.Message, new FaultCode(((PIOException)InnerException).FaultCode));
		}

		#region data
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
		#endregion


	}
}
