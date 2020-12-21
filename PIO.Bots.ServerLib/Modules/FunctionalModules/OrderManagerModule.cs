using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;
using PIO.Bots.ServerLib.Tables;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIOBaseModulesLib.Modules.FunctionalModules;

namespace PIO.Bots.ServerLib.Modules
{
	public class OrderManagerModule : FunctionalModule, IOrderManagerModule
	{
		private IOrderModule orderModule;
		private IProduceOrderModule produceOrderModule;

		private List<ProduceOrder> produceOrders;
		public int ProduceOrderCount
		{
			get { return produceOrders.Count;}
		}


		public OrderManagerModule(ILogger Logger, IOrderModule OrderModule,IProduceOrderModule ProduceOrderModule) : base(Logger)
		{
			this.orderModule = OrderModule;this.produceOrderModule = ProduceOrderModule;

			produceOrders = new List<ProduceOrder>();
		}

		public ProduceOrder CreateProduceOrder(int FactoryID)
		{
			Order order;
			ProduceOrder produceOrder;

			LogEnter();

			Log(LogLevels.Information, $"Creating Order");
			order = Try(() => orderModule.CreateOrder()).OrThrow<PIOInternalErrorException>("Failed to create Order");
			Log(LogLevels.Information, $"Creating ProduceOrder");
			produceOrder = Try(() => produceOrderModule.CreateProduceOrder(order.OrderID,FactoryID)).OrThrow<PIOInternalErrorException>("Failed to create ProduceOrder");

			produceOrders.Add(produceOrder);

			return produceOrder;
		}

		public Task CreateTask(int WorkerID)
		{
			throw new NotImplementedException();
		}


	}
}
