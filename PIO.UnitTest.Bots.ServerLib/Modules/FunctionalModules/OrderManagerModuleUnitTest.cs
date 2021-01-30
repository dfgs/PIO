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
			produceOrderModule.GetWaitingProduceOrders(Arg.Any<int>()).Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 }, new ProduceOrder() { OrderID = 2, ProduceOrderID = 2, FactoryID = 2 } });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, produceOrderModule, 10);
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

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingProduceOrders(1));
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

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID=1});
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(),Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1,ProduceOrderID=1,FactoryID=1 });

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { PlanetID = 1 });


			module = new OrderManagerModule(NullLogger.Instance,client, orderModule,produceOrderModule,10);
			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(1, module.ProduceOrderCount);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);

			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(2, module.ProduceOrderCount);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);

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
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateProduceOrder(1,1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, module.ProduceOrderCount);
		}

		[TestMethod]
		public void ShouldNotCreateProduceOrderIfFactoryIsNotInSamePlanet()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateProduceOrder(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, module.ProduceOrderCount);
		}


		[TestMethod]
		public void ShouldReturnIdleTaskIfNoOrderArePresent()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker() { PlanetID = 1 });
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() {TaskTypeID=TaskTypeIDs.Idle ,WorkerID=1});
			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { });

			module = new OrderManagerModule(NullLogger.Instance,client, orderModule, produceOrderModule,10);
			result = module.CreateTask(1);
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

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateTask(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotReturnIdleTaskIfAllOrderFailToCreateTask()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker());
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Idle, WorkerID = 1 });
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTask(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Idle, result.TaskTypeID);

		}






		[TestMethod]
		public void CreateTaskFromProduceOrderShouldReturnProduceTaskIfHaveEnoughResourcesToProduce()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID=1});
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.MoveToBuilding(Arg.Any<int>(),Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID=10,ResourceTypeID=ResourceTypeIDs.Wood,Quantity=10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1,ResourceTypeID=ResourceTypeIDs.Wood }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] {ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 }, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 10, ResourceTypeID = ResourceTypeIDs.Wood, Quantity = 10 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 ,ResourceTypeID=ResourceTypeIDs.Wood}, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Store, result.TaskTypeID);
		}

		[TestMethod]
		public void ShouldUnassignAll()
		{
			IOrderModule orderModule;
			OrderManagerModule module;

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x=>x.UnAssignAll(Arg.Any<int>())).Do(x=> { return; });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, 10);
			module.UnassignAll(1) ;
			
		}


		[TestMethod]
		public void ShouldAssignOrder()
		{
			IOrderModule orderModule;
			OrderManagerModule module;

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.Assign(Arg.Any<int>(), Arg.Any<int>())).Do(x => { return; });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, 10);
			module.Assign(1,1);

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

			module = new OrderManagerModule(logger, null, orderModule, null, 10);
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

			module = new OrderManagerModule(logger, null, orderModule, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(()=> module.Assign(1, 1));

		}


	}
}
