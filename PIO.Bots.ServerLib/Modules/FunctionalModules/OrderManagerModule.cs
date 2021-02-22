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
		private IHarvestOrderModule harvestOrderModule;
		private IBuildOrderModule buildOrderModule;





		public OrderManagerModule(ILogger Logger, PIO.ClientLib.PIOServiceReference.IPIOService Client, IOrderModule OrderModule, IProduceOrderModule ProduceOrderModule, IHarvestOrderModule HarvestOrderModule, IBuildOrderModule BuildFactoryOrderModule,  int IdleDuration) : base(Logger)
		{
			this.client = Client;this.idleDuration = IdleDuration;
			this.orderModule = OrderModule;
			this.produceOrderModule = ProduceOrderModule;this.harvestOrderModule = HarvestOrderModule;
			this.buildOrderModule = BuildFactoryOrderModule;

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






		public ProduceOrder CreateProduceOrder(int PlanetID, int BuildingID)
		{
			ProduceOrder[] existingOrders;
			ProduceOrder produceOrder;
			Building building;

			LogEnter();

			Log(LogLevels.Information, $"Checking if building is on planet (PlanetID={PlanetID}, BuildingID={BuildingID})");
			building = AssertExists(()=> client.GetBuilding(BuildingID),$"BuildingID=({BuildingID})");
			if (building.PlanetID!=PlanetID)
			{
				Log(LogLevels.Warning, $"Building is not in the same planet (BuildingID={BuildingID})");
				throw new PIOInvalidOperationException($"Building is not in the same planet (BuildingID={BuildingID})", null, ID, ModuleName, "CreateProduceOrder");
			}

			Log(LogLevels.Information, $"Checking if order already exists (BuildingID={BuildingID})");
			existingOrders = Try(() => produceOrderModule.GetProduceOrders(BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get ProduceOrder");
			if ((existingOrders != null) && (existingOrders.Length>0))
			{
				Log(LogLevels.Warning, $"Produce order already exists for building (BuildingID={BuildingID})");
				throw new PIOInvalidOperationException($"Produce order already exists for building (BuildingID={BuildingID})", null, ID, ModuleName, "CreateProduceOrder");
			}

			Log(LogLevels.Information, $"Creating ProduceOrder");
			produceOrder=Try(() => produceOrderModule.CreateProduceOrder(PlanetID,BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create ProduceOrder");


			return produceOrder;
		}
		public HarvestOrder CreateHarvestOrder(int PlanetID, int BuildingID)
		{
			HarvestOrder[] existingOrders;
			HarvestOrder harvestOrder;
			Building building;

			LogEnter();

			Log(LogLevels.Information, $"Checking if building is on planet (PlanetID={PlanetID}, BuildingID={BuildingID})");
			building = AssertExists(() => client.GetBuilding(BuildingID), $"BuildingID=({BuildingID})");
			if (building.PlanetID != PlanetID)
			{
				Log(LogLevels.Warning, $"Building is not in the same planet (BuildingID={BuildingID})");
				throw new PIOInvalidOperationException($"Building is not in the same planet (BuildingID={BuildingID})", null, ID, ModuleName, "CreateHarvestOrder");
			}

			Log(LogLevels.Information, $"Checking if order already exists (BuildingID={BuildingID})");
			existingOrders = Try(() => harvestOrderModule.GetHarvestOrders(BuildingID)).OrThrow<PIOInternalErrorException>("Failed to get HarvestOrder");
			if ((existingOrders != null) && (existingOrders.Length > 0))
			{
				Log(LogLevels.Warning, $"Harvest order already exists for building (BuildingID={BuildingID})");
				throw new PIOInvalidOperationException($"Harvest order already exists for building (BuildingID={BuildingID})", null, ID, ModuleName, "CreateHarvestOrder");
			}

			Log(LogLevels.Information, $"Creating HarvestOrder");
			harvestOrder = Try(() => harvestOrderModule.CreateHarvestOrder(PlanetID, BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create HarvestOrder");


			return harvestOrder;
		}
		public BuildOrder CreateBuildOrder(int PlanetID, BuildingTypeIDs BuildingTypeID, int X,int Y)
		{
			BuildOrder[] existingOrders;
			BuildOrder buildBuildingOrder;
			Building building;

			LogEnter();

			Log(LogLevels.Information, $"Checking if position is free");
			building = Try(() => client.GetBuildingAtPos(PlanetID, X, Y)).OrThrow<PIOInternalErrorException>("Failed to check if position is free");
			if (building != null)
			{
				Log(LogLevels.Warning, $"Position is not free (PlanetID={PlanetID}, X={X}, Y={Y})");
				throw new PIOInvalidOperationException($"Position is not free (PlanetID={PlanetID}, X={X}, Y={Y})", null, ID, ModuleName, "CreateBuildFactoryOrder");
			}

			Log(LogLevels.Information, $"Checking if order already exists (PlanetID={PlanetID}, X={X}, Y={Y})");
			existingOrders = Try(() => buildOrderModule.GetBuildOrders(PlanetID,X,Y)).OrThrow<PIOInternalErrorException>("Failed to get ProduceOrder");
			if ((existingOrders != null) && (existingOrders.Length > 0))
			{
				Log(LogLevels.Warning, $"BuildBuilding order already exists (PlanetID={PlanetID}, X={X}, Y={Y})");
				throw new PIOInvalidOperationException($"BuildBuilding order already exists (PlanetID={PlanetID}, X={X}, Y={Y})", null, ID, ModuleName, "CreateBuildFactoryOrder");
			}



			Log(LogLevels.Information, $"Creating BuildFactoryOrder");
			buildBuildingOrder = Try(() => buildOrderModule.CreateBuildOrder(PlanetID, BuildingTypeID, X,Y)).OrThrow<PIOInternalErrorException>("Failed to create BuildFactoryOrder");


			return buildBuildingOrder;
		}






		public ProduceOrder[] GetWaitingProduceOrders(int PlanetID)
		{
			ProduceOrder[] produceOrders;

			LogEnter();

			Log(LogLevels.Information, $"Getting Orders");
			produceOrders = Try(() => produceOrderModule.GetWaitingProduceOrders(PlanetID)).OrThrow<PIOInternalErrorException>("Failed to get Orders");

			return produceOrders;
		}

		public HarvestOrder[] GetWaitingHarvestOrders(int PlanetID)
		{
			HarvestOrder[] produceOrders;

			LogEnter();

			Log(LogLevels.Information, $"Getting Orders");
			produceOrders = Try(() => harvestOrderModule.GetWaitingHarvestOrders(PlanetID)).OrThrow<PIOInternalErrorException>("Failed to get Orders");

			return produceOrders;
		}

		public BuildOrder[] GetWaitingBuildOrders(int PlanetID)
		{
			BuildOrder[] buildBuildingOrders;

			LogEnter();

			Log(LogLevels.Information, $"Getting Orders");
			buildBuildingOrders = Try(() => buildOrderModule.GetWaitingBuildOrders(PlanetID)).OrThrow<PIOInternalErrorException>("Failed to get Orders");

			return buildBuildingOrders;
		}







		public Task CreateTaskFromProduceOrder(Worker Worker, ProduceOrder ProduceOrder)
		{
			bool result;
			ResourceTypeIDs[] missingResourceTypeID;
			Building building;
			Stack stack;
			Task task;

			building = AssertExists<Building>(() => client.GetBuilding(ProduceOrder.BuildingID), $"BuildingID={ProduceOrder.BuildingID}");

			Log(LogLevels.Information, $"Checking if building as enough resources to produce (BuildingID={ProduceOrder.BuildingID})");
			missingResourceTypeID = Try(() => client.GetMissingResourcesToProduce(ProduceOrder.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check resources");
			if ((missingResourceTypeID == null) || (missingResourceTypeID.Length == 0))
			{
				Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={Worker.WorkerID}, BuildingID={ProduceOrder.BuildingID})");
				result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
				if (result)
				{
					Log(LogLevels.Information, $"Worker is on site, creating produce task");
					task = Try(() => client.Produce(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				else
				{
					Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
					task = Try(() => client.MoveToBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				return task;
			}
			else
			{
				Log(LogLevels.Information, $"Checking if worker is carrying missing resource (PlanetID={Worker.PlanetID})");
				if ((Worker.ResourceTypeID != null) && missingResourceTypeID.Contains(Worker.ResourceTypeID.Value))
				{
					Log(LogLevels.Information, $"Worker is carrying missing resource, checking if worker is on site (WorkerID={Worker.WorkerID}, BuildingID={ProduceOrder.BuildingID})");
					result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
					if (result)
					{
						Log(LogLevels.Information, $"Worker is on site, creating store task");
						task = Try(() => client.Store(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
						return task;
					}
					else
					{
						Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
						task = Try(() => client.MoveToBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
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

		public Task CreateTaskFromHarvestOrder(Worker Worker, HarvestOrder HarvestOrder)
		{
			bool result;
			Building building;
			Task task;

			building = AssertExists<Building>(() => client.GetBuilding(HarvestOrder.BuildingID), $"BuildingID={HarvestOrder.BuildingID}");

			
			Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={Worker.WorkerID}, BuildingID={HarvestOrder.BuildingID})");
			result = Try(() => client.WorkerIsInBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check worker location");
			if (result)
			{
				Log(LogLevels.Information, $"Worker is on site, creating harvest task");
				task = Try(() => client.Harvest(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
			}
			else
			{
				Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
				task = Try(() => client.MoveToBuilding(Worker.WorkerID, building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to create task");
			}
			return task;

		}

		public Task CreateTaskFromBuildOrder(Worker Worker, BuildOrder BuildOrder)
		{
			Building building;
			bool result;
			Task task;
			ResourceTypeIDs[] missingResourceTypeID;
			Stack stack;

			Log(LogLevels.Information, $"Checking if building exists (PlanetID={BuildOrder.PlanetID}, X={BuildOrder.X}, Y={BuildOrder.Y})");
			building = Try(() => client.GetBuildingAtPos(BuildOrder.PlanetID,BuildOrder.X,BuildOrder.Y)).OrThrow<PIOInternalErrorException>("Failed to check is building exists");
			if (building == null) 
			{
				Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={Worker.WorkerID}, X={BuildOrder.X}, Y={BuildOrder.X})");
				result = (Worker.X == BuildOrder.X) && (Worker.Y == BuildOrder.Y);
				if (result)
				{
					Log(LogLevels.Information, $"Worker is on site, creating createbuilding task");
					task = Try(() => client.CreateBuilding(Worker.WorkerID,BuildOrder.BuildingTypeID)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				else
				{
					Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
					task = Try(() => client.MoveTo(Worker.WorkerID, BuildOrder.X,BuildOrder.Y)).OrThrow<PIOInternalErrorException>("Failed to create task");
				}
				return task;
			}
			else
			{

				if (building.RemainingBuildSteps==0)
				{
					Log(LogLevels.Information, $"Building is already built (BuildingID={building.BuildingID})");
					return null;
				}

				Log(LogLevels.Information, $"Checking if building as enough resources to build (BuildingID={building.BuildingID})");
				missingResourceTypeID = Try(() => client.GetMissingResourcesToBuild(building.BuildingID)).OrThrow<PIOInternalErrorException>("Failed to check resources");
				if ((missingResourceTypeID == null) || (missingResourceTypeID.Length == 0))
				{
					Log(LogLevels.Information, $"Checking if worker is on site (WorkerID={Worker.WorkerID}, BuildingID={building.BuildingID})");
					result = (Worker.X == BuildOrder.X) && (Worker.Y == BuildOrder.Y);
					if (result)
					{
						Log(LogLevels.Information, $"Worker is on site, creating BuildBuilding task");
						task = Try(() => client.Build(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
					}
					else
					{
						Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
						task = Try(() => client.MoveTo(Worker.WorkerID, BuildOrder.X, BuildOrder.Y)).OrThrow<PIOInternalErrorException>("Failed to create task");
					}
					return task;
				}
				else
				{
					Log(LogLevels.Information, $"Checking if worker is carrying missing resource (PlanetID={Worker.PlanetID})");
					if ((Worker.ResourceTypeID != null) && missingResourceTypeID.Contains(Worker.ResourceTypeID.Value))
					{
						Log(LogLevels.Information, $"Worker is carrying missing resource, checking if worker is on site (WorkerID={Worker.WorkerID}, X={BuildOrder.X}, Y={BuildOrder.X})");
						result = (Worker.X == BuildOrder.X) && (Worker.Y == BuildOrder.Y);
						if (result)
						{
							Log(LogLevels.Information, $"Worker is on site, creating store task");
							task = Try(() => client.Store(Worker.WorkerID)).OrThrow<PIOInternalErrorException>("Failed to create task");
							return task;
						}
						else
						{
							Log(LogLevels.Information, $"Worker is not on site, creating moveto task");
							task = Try(() => client.MoveTo(Worker.WorkerID, BuildOrder.X, BuildOrder.Y)).OrThrow<PIOInternalErrorException>("Failed to create task");
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
			
		}



		public Task CreateTask(int BotID,int WorkerID)
		{
			Order[] orders;
			Task task;
			Worker worker;

			LogEnter();

			worker = AssertExists<Worker>(() => client.GetWorker(WorkerID), $"WorkerID={WorkerID}");

			orders = GetWaitingProduceOrders(worker.PlanetID).AsEnumerable<Order>().Union(GetWaitingHarvestOrders(worker.PlanetID)).Union(GetWaitingBuildOrders(worker.PlanetID)).ToArray();

			if ((orders==null) || (orders.Length==0))
			{
				Log(LogLevels.Information, $"No order defined, creating idle task");
				task= Try(()=>client.Idle(WorkerID, idleDuration)).OrThrow<PIOInternalErrorException>("Failed to create task");
				return task;
			}

			foreach (Order order in orders)
			{
				if (order is ProduceOrder produceOrder) task = CreateTaskFromProduceOrder(worker, produceOrder);
				else if (order is HarvestOrder harvestOrder) task = CreateTaskFromHarvestOrder(worker, harvestOrder);
				else if (order is BuildOrder buildBuildingOrder) task = CreateTaskFromBuildOrder(worker, buildBuildingOrder);
				else
				{
					Log(LogLevels.Warning, $"Cannot handle order of type {order.GetType().Name}");
					task = null;
				}



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
