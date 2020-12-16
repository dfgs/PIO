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
		

		public BotsService(ILogger Logger,
			IOrderModule OrderModule
		) : base(Logger)
		{
			LogEnter();
			this.orderModule = OrderModule;
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
		#endregion


	}
}
