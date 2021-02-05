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
		private IBuildFactoryOrderModule buildFactoryOrderModule;


		


		public OrderManagerModule(ILogger Logger, PIO.ClientLib.PIOServiceReference.IPIOService Client, IOrderModule OrderModule,IProduceOrderModule ProduceOrderModule, IBuildFactoryOrderModule BuildFactoryOrderModule, int IdleDuration) : base(Logger)
		{
			this.client = Client;this.idleDuration = IdleDuration;
			this.orderModule = OrderModule;this.produceOrderModule = ProduceOrderModule;this.buildFactoryOrderModule = BuildFactoryOrderModule;

		}

		public void UnassignAll(int BotID)
		{
			Log(LogLevels.Information, $"Clearing worker assignment (BotID={BotID})");
			Try(() => orderModule.UnAssignAll(BotID)).OrThrow<PIOInternalErrorException>("Failed to clear worker assignment");

		}
		public void Assign(int OrderID, int BotID)
		{
			Log(LogLevels.Information, $"Assigning worker (OrderID={OrderID}, BotID={BotID})");
			Try(() => orderModule.Assign(OrderID,BotID)).OrThrow<PIOInternalErrorException>("Failed to assign worker");

		}
		public ProduceOrder CreateProduceOrder(int PlanetID,int FactoryID)
		{
			ProduceOrder produceOrder;
			Factory factory;

			LogEnter();

			factory=AssertExists(()=> client.GetFactory(FactoryID),$"FactoryID=({FactoryID})");
			if (factory.PlanetID!=PlanetID)
			{
				Log(LogLevels.Warning, $"Factory is not in the same planet (FactoryID={FactoryID})");
				throw new PIOInvalidOperationException($"Factory is not in the same planet (FactoryID={FactoryID})", null, ID, ModuleName, "CreateProduceOrder");
			}


			Log(LogLevels.Information, $"Creating ProduceOrder");
			produceOrder=Try(() => produceOrderModule.CreateProduceOrder(PlanetID,FactoryID)).OrThrow<PIOInternalErrorException>("Failed to create ProduceOrder");


			return produceOrder;
		}

		public BuildFactoryOrder CreateBuildFactoryOrder(int PlanetID, FactoryTypeIDs FactoryTypeID,int X,int Y)
		{
			BuildFactoryOrder buildFactoryOrder;
			Factory factory;

			LogEnter();

			Log(LogLevels.Information, $"Checking if position is free");
			factory = Try(() => client.GetFactoryAtPos(PlanetID, X, Y)).OrThrow<PIOInternalErrorException>("Failed to check if position is free");
			if (factory != null)
			{
				Log(LogLevels.Warning, $"Position is not free (PlanetID={PlanetID}, X={X}, Y={Y})");
				throw new PIOInvalidOperationException($"Position is not free (PlanetID={PlanetID}, X={X}, Y={Y})", null, ID, ModuleName, "CreateBuildFactoryOrder");
			}


			Log(LogLevels.Information, $"Creating BuildFactoryOrder");
			buildFactoryOrder = Try(() => buildFactoryOrderModule.CreateBuildFactoryOrder(PlanetID, FactoryTypeID,X,Y)).OrThrow<PIOInternalErrorException>("Failed to create BuildFactoryOrder");


			return buildFactoryOrder;
		}


		public Task CreateTaskFromProduceOrder(Worker Worker, ProduceOrder ProduceOrder)
		{
			bool result;
			ResourceTypeIDs[] missingResourceTypeID;
			Factory factory;
			Stack stack;
			Task task;

			factory = AssertExists<Factory>(() => client.GetFactory(ProduceOrder.FactoryID), $"FactoryID={ProduceOrder.FactoryID}");

			Log(LogLevels.Information, $"Checking if factory as enough resources to produce (FactoryID={ProduceOrder.FactoryID})");
			missingResourceTypeID = Try(() => client.GetMissingResourcesToProduce(ProduceOrder.FactoryID)).OrThrow<PIOInternalErrorException>("Failed to check resources");
			if ((missingResourceTypeID == null) || (missingResourceTypeID.Length == 0))
			{
				Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={Worker.WorkerID}, FactoryID={ProduceOrder.FactoryID})");
				result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, factory.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
				if (result)
				{
					Log(LogLevels.Information, $"Worker is on site, creating produce task");
					task = Try(() => client.Produce(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				else
				{
					Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
					task = Try(() => client.MoveToBuilding(Worker.WorkerID, factory.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				return task;
			}
			else
			{
				Log(LogLevels.Information, $"Checking if worker is carryingt missing resource (PlanetID={Worker.PlanetID})");
				if ((Worker.ResourceTypeID != null) && missingResourceTypeID.Contains(Worker.ResourceTypeID.Value))
				{
					Log(LogLevels.Information, $"Worker is carrying missing resource, checking if worker is on site (WorkerID={Worker.WorkerID}, FactoryID={ProduceOrder.FactoryID})");
					result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, factory.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
					if (result)
					{
						Log(LogLevels.Information, $"Worker is on site, creating store task");
						task = Try(() => client.Store(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
						return task;
					}
					else
					{
						Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
						task = Try(() => client.MoveToBuilding(Worker.WorkerID, factory.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
						return task;
					}
				}
				else
				{
					Log(LogLevels.Information, $"Trying to find missing resource (PlanetID={Worker.PlanetID}, ResourceTypeID={missingResourceTypeID[0]})");
					stack = Try(() => client.FindStack(Worker.PlanetID, missingResourceTypeID[0])).OrThrow("Failed to find missing resource");
					if (stack == null)
					{
						Log(LogLevels.Information, $"Missing resource not found, cannot create task");
						return null;
					}
					else
					{
						Log(LogLevels.Information, $"Missing resource found, checking if worker can carry to");
						result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, stack.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
						if (result)
						{
							Log(LogLevels.Information, $"Worker can access resource, creating take task");
							task = Try(() => client.Take(Worker.WorkerID, missingResourceTypeID[0])).OrThrow<PIOInternalErrorException>("Failed to create task");
							return task;
						}
						else
						{
							Log(LogLevels.Information, $"Missing resource found, creating moveto task");
							task = Try(() => client.MoveToBuilding(Worker.WorkerID, stack.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
							return task;
						}
					}
				}

			}


		}


		public ProduceOrder[] GetWaitingProduceOrders(int PlanetID)
		{
			ProduceOrder[] produceOrders;

			LogEnter();

			Log(LogLevels.Information, $"Getting Orders");
			produceOrders = Try(() => produceOrderModule.GetWaitingProduceOrders(PlanetID)).OrThrow<PIOInternalErrorException>("Failed to get Orders");
			
			return produceOrders;
		}


		public Task CreateTask(int BotID,int WorkerID)
		{
			ProduceOrder[] produceOrders;
			Task task;
			Worker worker;

			LogEnter();

			worker = AssertExists<Worker>(() => client.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			produceOrders = GetWaitingProduceOrders(worker.PlanetID);

			if ((produceOrders==null) || (produceOrders.Length==0))
			{
				Log(LogLevels.Information, $"No order defined, creating idle task");
				task= Try(()=>client.Idle(WorkerID, idleDuration)).OrThrow<PIOInternalErrorException>("Failed to create task");
				return task;
			}

			foreach (ProduceOrder order in produceOrders)
			{
				task = CreateTaskFromProduceOrder(worker, order);
				if (task != null)
				{
					Log(LogLevels.Information, $"Assigning bot to order (OrderID={order.OrderID}, Bot={BotID})");
					Try(() => Assign(order.OrderID, BotID)).OrThrow<PIOInternalErrorException>("Failed to assign worker to order");
					return task;
				}
			}

			Log(LogLevels.Information, $"Cannot create any task from orders, returning idle task");
			task = Try(() => client.Idle(WorkerID, idleDuration)).OrThrow<PIOInternalErrorException>("Failed to create task");
			return task;


		}

		
	}
}
