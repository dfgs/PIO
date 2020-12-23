using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
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
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;

namespace PIO.Bots.ServerLib.Modules
{
	public class OrderManagerModule : FunctionalModule, IOrderManagerModule
	{
		private int idleDuration;

		private PIO.ClientLib.PIOServiceReference.IPIOService client;
		private IOrderModule orderModule;
		private IProduceOrderModule produceOrderModule;


		private List<ProduceOrder> produceOrders;
		public int ProduceOrderCount
		{
			get { return produceOrders.Count;}
		}


		public OrderManagerModule(ILogger Logger, PIO.ClientLib.PIOServiceReference.IPIOService Client, IOrderModule OrderModule,IProduceOrderModule ProduceOrderModule,int IdleDuration) : base(Logger)
		{
			this.client = Client;this.idleDuration = IdleDuration;
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
			ProduceOrder[] produceOrders;
			ProduceOrder prioritaryOrder;
			bool result;
			Task task;
			ResourceTypeIDs[] missingResourceTypeID;
			Worker worker;
			Stack stack;

			LogEnter();

			worker = AssertExists<Worker>(() => client.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			Log(LogLevels.Information, $"Getting Orders");
			produceOrders = Try(() => produceOrderModule.GetProduceOrders()).OrThrow<PIOInternalErrorException>("Failed to get Orders");

			if ((produceOrders==null) || (produceOrders.Length==0))
			{
				Log(LogLevels.Information, $"No order defined, creating idle task");
				task= Try(()=>client.Idle(WorkerID, idleDuration)).OrThrow<PIOInternalErrorException>("Failed to create task");
				return task;
			}
						
			prioritaryOrder = produceOrders[0];

			Log(LogLevels.Information, $"Checking if factory as enough resources to produce (FactoryID={prioritaryOrder.FactoryID})");
			missingResourceTypeID = Try(()=>client.GetMissingResourcesToProduce(prioritaryOrder.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to check resources");
			if ((missingResourceTypeID==null) || (missingResourceTypeID.Length==0))
			{
				Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={WorkerID}, FactoryID={prioritaryOrder.FactoryID})");
				result = Try(() => client.WorkerIsInFactory(WorkerID, prioritaryOrder.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
				if (result)
				{
					Log(LogLevels.Information, $"Worker is on site, creating produce task");
					task = Try(() => client.Produce(WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				else
				{
					Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
					task = Try(() => client.MoveToFactory(WorkerID,prioritaryOrder.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				return task;
			}
			else
			{
				Log(LogLevels.Information, $"Trying to find missing resource (PlanetID={worker.PlanetID}, ResourceTypeID={missingResourceTypeID[0]})");
				stack = Try(() => client.FindStack(worker.PlanetID, missingResourceTypeID[0])).OrThrow("Failed to find missing resource");
				if (stack==null)
				{
					Log(LogLevels.Information, $"Missing resource not found, creating idle task");
					task = Try(() => client.Idle(WorkerID, idleDuration)).OrThrow<PIOInternalErrorException>("Failed to create task");
					return task;
				}
				else
				{
					Log(LogLevels.Information, $"Missing resource found, checking if worker can carry to");
					result = Try(() => client.WorkerIsInFactory(WorkerID, stack.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
					if (result)
					{
						Log(LogLevels.Information, $"Worker can access resource, creating carryto task");
						task = Try(() => client.CarryTo(WorkerID, prioritaryOrder.FactoryID,missingResourceTypeID[0])).OrThrow<PIOInternalErrorException>("Failed to create task");
						return task;
					}
					else
					{
						Log(LogLevels.Information, $"Missing resource found, creating moveto task");
						task = Try(() => client.MoveToFactory(WorkerID, stack.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to create task");
						return task;
					}
				}

			}
			

		}


	}
}
