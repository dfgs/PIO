using System;
using System.Collections.Generic;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib.Databases;
using NSubstitute;
using PIO.Bots.Models;
using PIO.Bots.Models.Modules;
using PIO.Bots.ServerLib.Modules;
using PIO.Models;
using PIO.ModulesLib.Exceptions;

namespace PIO.UnitTest.Bots.ServerLib.Modules
{
	[TestClass]
	public class OrderManagerModuleUnitTest
	{

		[TestMethod]
		public void ShouldGetWaitingProduceOrders()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			ProduceOrder[] result;

			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetWaitingProduceOrders(Arg.Any<int>()).Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 }, new ProduceOrder() { OrderID = 2, ProduceOrderID = 2, BuildingID = 2 } });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, produceOrderModule, null, null, 10);
			result = module.GetWaitingProduceOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Length);

		}

		[TestMethod]
		public void ShouldNotGetWaitingProduceOrders()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetWaitingProduceOrders(Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, null, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingProduceOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}

		[TestMethod]
		public void ShouldGetWaitingHarvestOrders()
		{
			IOrderModule orderModule;
			IHarvestOrderModule harvestOrderModule;
			OrderManagerModule module;
			HarvestOrder[] result;

			orderModule = Substitute.For<IOrderModule>();
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetWaitingHarvestOrders(Arg.Any<int>()).Returns(new HarvestOrder[] { new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 }, new HarvestOrder() { OrderID = 2, HarvestOrderID = 2, BuildingID = 2 } });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, harvestOrderModule, null, 10);
			result = module.GetWaitingHarvestOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Length);

		}

		[TestMethod]
		public void ShouldNotGetWaitingHarvestOrders()
		{
			IOrderModule orderModule;
			IHarvestOrderModule harvestOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetWaitingHarvestOrders(Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, null, harvestOrderModule,null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingHarvestOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}



		[TestMethod]
		public void ShouldGetWaitingBuildOrders()
		{
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			BuildOrder[] result;

			orderModule = Substitute.For<IOrderModule>();
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.GetWaitingBuildOrders(Arg.Any<int>()).Returns(new BuildOrder[] { new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest }, new BuildOrder() { OrderID = 2, BuildOrderID = 2, BuildingTypeID = BuildingTypeIDs.Forest } });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, null, buildFactoryOrderModule, 10);
			result = module.GetWaitingBuildOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Length);

		}

		[TestMethod]
		public void ShouldNotGetWaitingBuildOrders()
		{
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.GetWaitingBuildOrders(Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, null, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingBuildOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}


		



		[TestMethod]
		public void ShouldCreateProduceOrder()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			ProduceOrder result;
			int counter=0;

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID=1});
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { PlanetID = 1 });


			module = new OrderManagerModule(NullLogger.Instance,client, orderModule,produceOrderModule, null, null, 10);
			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(1, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildingID);

			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(2, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.BuildingID);

		}



		[TestMethod]
		public void ShouldNotCreateProduceOrder()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, null, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateProduceOrder(1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotCreateProduceOrderIfFactoryIsNotInSamePlanet()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, null, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateProduceOrder(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}

		[TestMethod]
		public void ShouldNotCreateProduceOrderIfAlreadyExists()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			produceOrderModule.GetProduceOrders(Arg.Any<int>()).Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, null, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateProduceOrder(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}





		[TestMethod]
		public void ShouldCreateBuildOrder()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			BuildOrder result;
			int counter = 0;

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID=1});
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);


			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Building)null);


			module = new OrderManagerModule(NullLogger.Instance, client, orderModule,null, null, buildFactoryOrderModule, 10);
			result = module.CreateBuildOrder(1,BuildingTypeIDs.Forest, 1,1);
			Assert.AreEqual(1, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(BuildingTypeIDs.Forest, result.BuildingTypeID);

			result = module.CreateBuildOrder(1, BuildingTypeIDs.Forest, 1, 1);
			Assert.AreEqual(2, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(BuildingTypeIDs.Forest, result.BuildingTypeID);

		}



		[TestMethod]
		public void ShouldNotCreateBuildOrder()
		{
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, null, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateBuildOrder(1, BuildingTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotCreateBuildOrderIfPositionIsNotFree()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, null, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateBuildOrder(1, BuildingTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}

		[TestMethod]
		public void ShouldNotCreateBuildOrderIfAlreadyExists()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildOrder(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			buildFactoryOrderModule.GetBuildOrders(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildOrder[] { new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest } });
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, null, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateBuildOrder(1, BuildingTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}





	


		[TestMethod]
		public void ShouldReturnIdleTaskIfNoOrderArePresent()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			IHarvestOrderModule harvestOrderModule;
			IBuildOrderModule buildFactoryOrderModule;
			
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker() { PlanetID = 1 });
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() {TaskTypeID=TaskTypeIDs.Idle ,WorkerID=1});
			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { });
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetHarvestOrders().Returns(new HarvestOrder[] { });

			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.GetBuildOrders().Returns(new BuildOrder[] { });

			module = new OrderManagerModule(NullLogger.Instance,client, orderModule, produceOrderModule, harvestOrderModule,buildFactoryOrderModule,  10);
			result = module.CreateTask(1,1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);
		}

		[TestMethod]
		public void ShouldNotReturnIdleTaskIfNoOrderArePresentAndLogError()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			MemoryLogger logger;
			OrderManagerModule module;

			logger = new MemoryLogger();

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker() { PlanetID = 1 });
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateTask(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotReturnIdleTaskIfAllOrderFailToCreateTask()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			IHarvestOrderModule harvestOrderModule;
			IBuildOrderModule buildFactoryOrderModule;
			
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker());
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Idle, WorkerID = 1 });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetHarvestOrders().Returns(new HarvestOrder[] { new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 } });

			buildFactoryOrderModule = Substitute.For<IBuildOrderModule>();
			buildFactoryOrderModule.GetBuildOrders().Returns(new BuildOrder[] { new BuildOrder() { OrderID = 2, BuildOrderID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, harvestOrderModule, buildFactoryOrderModule,  10);
			result = module.CreateTask(1, 1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);

		}


		[TestMethod]
		public void ShouldUnassignAll()
		{
			IOrderModule orderModule;
			OrderManagerModule module;

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.UnAssignAll(Arg.Any<int>())).Do(x => { return; });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, null, null, 10);
			module.UnassignAll(1);

		}


		[TestMethod]
		public void ShouldAssignOrder()
		{
			IOrderModule orderModule;
			OrderManagerModule module;

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.Assign(Arg.Any<int>(), Arg.Any<int>())).Do(x => { return; });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, null, null, 10);
			module.Assign(1, 1);

		}

		[TestMethod]
		public void ShouldNotUnassignAll()
		{
			IOrderModule orderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.UnAssignAll(Arg.Any<int>())).Do(x => { throw new Exception(); });

			module = new OrderManagerModule(logger, null, orderModule, null, null, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.UnassignAll(1));

		}


		[TestMethod]
		public void ShouldNotAssignOrder()
		{
			IOrderModule orderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.Assign(Arg.Any<int>(), Arg.Any<int>())).Do(x => { throw new Exception(); });

			module = new OrderManagerModule(logger, null, orderModule, null, null, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.Assign(1, 1));

		}








		#region produce order
		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnProduceTaskIfHaveEnoughResourcesToProduce()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID=1});
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Produce, result.TaskTypeID);
		}

		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnMoveToTaskIfWorkerIsNotInFactoryAndHasEnoughResources()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.MoveToBuilding(Arg.Any<int>(),Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}

		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnNullkIfDontHaveEnoughResourcesToProduceAndMissingResourceNotFound()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Idle, WorkerID = 1 });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnMoveToTaskIfDontHaveEnoughResourcesToProduceAndMissingResourceIsCarriedByWorker()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.MoveToBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID=10,ResourceTypeID=ResourceTypeIDs.Wood,Quantity=10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1,ResourceTypeID=ResourceTypeIDs.Wood }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnMoveToTaskIfDontHaveEnoughResourcesToProduceAndMissingResourceFoundInRemoteLocation()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.MoveToBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] {ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnTakeTaskIfDontHaveEnoughResourcesToProduceAndMissingResourceIsFoundLocally()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.Take(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Take, WorkerID = 1,ResourceTypeID=ResourceTypeIDs.Wood });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Take, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnStoreTaskIfDontHaveEnoughResourcesToProduceAndMissingResourceIsCarriedByWorker()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.Store(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Store, WorkerID = 1 });
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null,null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 ,ResourceTypeID=ResourceTypeIDs.Wood}, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Store, result.TaskTypeID);
		}
		#endregion
	
		#region harvest order
		[TestMethod]
		public void CreateTaskFromHarvestOrderShouldReturnHarvestTaskIfWorkerIsOnSite()
		{
			IOrderModule orderModule;
			IHarvestOrderModule harvestOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.Harvest(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Harvest, WorkerID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetHarvestOrders().Returns(new HarvestOrder[] { new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule,null, harvestOrderModule, null, 10);
			result = module.CreateTaskFromHarvestOrder(new Worker() { PlanetID = 1 }, new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Harvest, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromHarvestOrderShouldReturnMoveToTaskIfWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			IHarvestOrderModule harvestOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuilding(Arg.Any<int>()).Returns(new Building() { BuildingID = 1 });
			client.MoveToBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			harvestOrderModule = Substitute.For<IHarvestOrderModule>();
			harvestOrderModule.GetHarvestOrders().Returns(new HarvestOrder[] { new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, harvestOrderModule, null, 10);
			result = module.CreateTaskFromHarvestOrder(new Worker() { PlanetID = 1 }, new HarvestOrder() { OrderID = 1, HarvestOrderID = 1, BuildingID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}

		#endregion


		#region build factory order

		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnMoveToIfFactoryDoesntExistsAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Building)null);
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnCreateBuildingIfFactoryDoesntExistsAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Building)null);
			client.CreateBuilding(Arg.Any<int>(), Arg.Any<BuildingTypeIDs>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.CreateBuilding, WorkerID = 1 });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 11, Y = 12 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.CreateBuilding, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnNullIfFactoryIsAlreadyBuilt()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 0 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnMoveToIfFactoryExistsAndHasEnoughResourcesAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() {PlanetID=1,X=11,Y=12,RemainingBuildSteps=10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnBuildIfFactoryExistsAndHasEnoughResourcesAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.Build(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Build, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 11, Y = 12 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Build, result.TaskTypeID);
		}


		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnMoveToIfFactoryExistsAndDoentHaveEnoughResourcesAndWorkerIsCarryingMissingResourceAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] {ResourceTypeIDs.Wood });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 ,ResourceTypeID=ResourceTypeIDs.Wood}, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnStoreIfFactoryExistsAndDoesntHabeEnoughResourcesAnWorkerIsCarryingMissingResourceAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.Store(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Store, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 11, Y = 12, ResourceTypeID = ResourceTypeIDs.Wood }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Store, result.TaskTypeID);
		}


		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnNullIfFactoryExistsAndDoesntHaveEnoughResourcesAndCannotLocateMissingResource()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNull(result);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnMoveToIfFactoryExistsAndDoesntHaveEnoughResourcesAndCanLocateMissingResourceAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveToBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildOrderShouldReturnTakeIfFactoryExistsAndDoesntHaveEnoughResourcesAndCanLocateMissingResourceAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetBuildingAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Building() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.Take(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Take, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, null, 10);
			result = module.CreateTaskFromBuildOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildOrder() { OrderID = 1, BuildOrderID = 1, BuildingTypeID = BuildingTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Take, result.TaskTypeID);
		}
		#endregion




	}
}
