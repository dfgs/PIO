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

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, produceOrderModule,null, 10);
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

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingProduceOrders(1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}




		[TestMethod]
		public void ShouldGetWaitingBuildFactoryOrders()
		{
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			BuildFactoryOrder[] result;

			orderModule = Substitute.For<IOrderModule>();
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.GetWaitingBuildFactoryOrders(Arg.Any<int>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest }, new BuildFactoryOrder() { OrderID = 2, BuildFactoryOrderID = 2, FactoryTypeID = FactoryTypeIDs.Forest } });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, buildFactoryOrderModule, 10);
			result = module.GetWaitingBuildFactoryOrders(1);
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Length);

		}

		[TestMethod]
		public void ShouldNotGetWaitingBuildFactoryOrders()
		{
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;


			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.GetWaitingBuildFactoryOrders(Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, null, buildFactoryOrderModule,10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.GetWaitingBuildFactoryOrders(1));
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
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { PlanetID = 1 });


			module = new OrderManagerModule(NullLogger.Instance,client, orderModule,produceOrderModule, null, 10);
			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(1, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.FactoryID);

			result = module.CreateProduceOrder(1,1);
			Assert.AreEqual(2, counter);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, produceOrderModule, null, 10);
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
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, 10);
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
			produceOrderModule.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>()).Returns(new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
			produceOrderModule.When((del) => del.CreateProduceOrder(Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			produceOrderModule.GetProduceOrders(Arg.Any<int>()).Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateProduceOrder(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}





		[TestMethod]
		public void ShouldCreateBuildFactoryOrder()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			BuildFactoryOrder result;
			int counter = 0;

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID=1});
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);


			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Factory)null);


			module = new OrderManagerModule(NullLogger.Instance, client, orderModule,null, buildFactoryOrderModule, 10);
			result = module.CreateBuildFactoryOrder(1,FactoryTypeIDs.Forest, 1,1);
			Assert.AreEqual(1, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(FactoryTypeIDs.Forest, result.FactoryTypeID);

			result = module.CreateBuildFactoryOrder(1, FactoryTypeIDs.Forest, 1, 1);
			Assert.AreEqual(2, counter);
			Assert.IsNotNull(result);
			Assert.AreEqual(FactoryTypeIDs.Forest, result.FactoryTypeID);

		}



		[TestMethod]
		public void ShouldNotCreateBuildFactoryOrder()
		{
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns((x) => { throw new PIODataException("UnitTestException", null, 1, "UnitTest", "UnitTest"); });

			module = new OrderManagerModule(logger, null, orderModule, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateBuildFactoryOrder(1, FactoryTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));
		}

		[TestMethod]
		public void ShouldNotCreateBuildFactoryOrderIfPositionIsNotFree()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, null, buildFactoryOrderModule,  10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateBuildFactoryOrder(1, FactoryTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}

		[TestMethod]
		public void ShouldNotCreateBuildFactoryOrderIfAlreadyExists()
		{
			ClientLib.PIOServiceReference.IPIOService client;
			IOrderModule orderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			OrderManagerModule module;
			MemoryLogger logger;
			int counter = 0;

			logger = new MemoryLogger();

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest });
			buildFactoryOrderModule.When((del) => del.CreateBuildFactoryOrder(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>(), Arg.Any<int>(), Arg.Any<int>())).Do((del) => counter++);
			buildFactoryOrderModule.GetBuildFactoryOrders(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest } });
			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 2 });

			module = new OrderManagerModule(logger, client, orderModule, null, buildFactoryOrderModule, 10);
			Assert.ThrowsException<PIOInvalidOperationException>(() => module.CreateBuildFactoryOrder(1, FactoryTypeIDs.Forest, 1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Warning) && (item.ComponentName == module.ModuleName)));
			Assert.AreEqual(0, counter);
		}





		[TestMethod]
		public void ShouldReturnIdleTaskIfNoOrderArePresent()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetWorker(Arg.Any<int>()).Returns(new Worker() { PlanetID = 1 });
			client.Idle(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() {TaskTypeID=TaskTypeIDs.Idle ,WorkerID=1});
			orderModule = Substitute.For<IOrderModule>();
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { });

			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.GetBuildFactoryOrders().Returns(new BuildFactoryOrder[] {  });

			module = new OrderManagerModule(NullLogger.Instance,client, orderModule, produceOrderModule, buildFactoryOrderModule, 10);
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

			module = new OrderManagerModule(logger, client, orderModule, produceOrderModule, null, 10);
			Assert.ThrowsException<PIOInternalErrorException>(() => module.CreateTask(1, 1));
			Assert.IsNotNull(logger.Logs.FirstOrDefault(item => (item.Level == LogLevels.Error) && (item.ComponentName == module.ModuleName)));

		}
		[TestMethod]
		public void ShouldNotReturnIdleTaskIfAllOrderFailToCreateTask()
		{
			IOrderModule orderModule;
			IProduceOrderModule produceOrderModule;
			IBuildFactoryOrderModule buildFactoryOrderModule;
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			buildFactoryOrderModule = Substitute.For<IBuildFactoryOrderModule>();
			buildFactoryOrderModule.GetBuildFactoryOrders().Returns(new BuildFactoryOrder[] { new BuildFactoryOrder() { OrderID = 2, BuildFactoryOrderID = 1 } } );

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, buildFactoryOrderModule, 10);
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

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, null, 10);
			module.UnassignAll(1);

		}


		[TestMethod]
		public void ShouldAssignOrder()
		{
			IOrderModule orderModule;
			OrderManagerModule module;

			orderModule = Substitute.For<IOrderModule>();
			orderModule.When(x => x.Assign(Arg.Any<int>(), Arg.Any<int>())).Do(x => { return; });

			module = new OrderManagerModule(NullLogger.Instance, null, orderModule, null, null, 10);
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

			module = new OrderManagerModule(logger, null, orderModule, null, null, 10);
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

			module = new OrderManagerModule(logger, null, orderModule, null, null, 10);
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
			client.GetFactory(Arg.Any<int>()).Returns(new Factory() { FactoryID=1});
			client.Produce(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Produce, WorkerID = 1 });
			client.GetMissingResourcesToProduce(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
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
			//orderModule.CreateOrder(Arg.Any<int>()).Returns(new Order() { OrderID = 1 });
			produceOrderModule = Substitute.For<IProduceOrderModule>();
			produceOrderModule.GetProduceOrders().Returns(new ProduceOrder[] { new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 } });

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, produceOrderModule, null, 10);
			result = module.CreateTaskFromProduceOrder(new Worker() { PlanetID = 1 ,ResourceTypeID=ResourceTypeIDs.Wood}, new ProduceOrder() { OrderID = 1, ProduceOrderID = 1, FactoryID = 1 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Store, result.TaskTypeID);
		}
		#endregion


		#region build factory order
		
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnMoveToIfFactoryDoesntExistsAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Factory)null);
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnCreateBuildingIfFactoryDoesntExistsAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns((Factory)null);
			client.CreateBuilding(Arg.Any<int>(), Arg.Any<FactoryTypeIDs>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.CreateBuilding, WorkerID = 1 });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 11, Y = 12 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.CreateBuilding, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnNullIfFactoryIsAlreadyBuilt()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 0 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNull(result);
		}

		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnMoveToIfFactoryExistsAndHasEnoughResourcesAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() {PlanetID=1,X=11,Y=12,RemainingBuildSteps=10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnBuildFactoryIfFactoryExistsAndHasEnoughResourcesAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.BuildFactory(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Build, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 11, Y = 12 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Build, result.TaskTypeID);
		}


		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnMoveToIfFactoryExistsAndDoentHaveEnoughResourcesAndWorkerIsCarryingMissingResourceAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] {ResourceTypeIDs.Wood });
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 ,ResourceTypeID=ResourceTypeIDs.Wood}, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnStoreIfFactoryExistsAndDoesntHabeEnoughResourcesAnWorkerIsCarryingMissingResourceAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.Store(Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Store, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 11, Y = 12, ResourceTypeID = ResourceTypeIDs.Wood }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Store, result.TaskTypeID);
		}


		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnNullIfFactoryExistsAndDoesntHaveEnoughResourcesAndCannotLocateMissingResource()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveTo(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns((Stack)null);
			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNull(result);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnMoveToIfFactoryExistsAndDoesntHaveEnoughResourcesAndCanLocateMissingResourceAndWorkerIsNotOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.MoveToBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.MoveTo, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.MoveTo, result.TaskTypeID);
		}
		[TestMethod]
		public void CreateTaskFromBuildFactoryOrderShouldReturnTakeIfFactoryExistsAndDoesntHaveEnoughResourcesAndCanLocateMissingResourceAndWorkerIsOnSite()
		{
			IOrderModule orderModule;
			ClientLib.PIOServiceReference.IPIOService client;
			OrderManagerModule module;
			Task result;

			client = Substitute.For<PIO.ClientLib.PIOServiceReference.IPIOService>();
			client.GetFactoryAtPos(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>()).Returns(new Factory() { PlanetID = 1, X = 11, Y = 12, RemainingBuildSteps = 10 });
			client.Take(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Task() { TaskTypeID = TaskTypeIDs.Take, WorkerID = 1 });
			client.GetMissingResourcesToBuild(Arg.Any<int>()).Returns(new ResourceTypeIDs[] { ResourceTypeIDs.Wood });
			client.FindStack(Arg.Any<int>(), Arg.Any<ResourceTypeIDs>()).Returns(new Stack() { BuildingID = 2, Quantity = 10, ResourceTypeID = ResourceTypeIDs.Wood, StackID = 1 });
			client.WorkerIsInBuilding(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

			orderModule = Substitute.For<IOrderModule>();

			module = new OrderManagerModule(NullLogger.Instance, client, orderModule, null, null, 10);
			result = module.CreateTaskFromBuildFactoryOrder(new Worker() { PlanetID = 1, X = 1, Y = 2 }, new BuildFactoryOrder() { OrderID = 1, BuildFactoryOrderID = 1, FactoryTypeID = FactoryTypeIDs.Forest, X = 11, Y = 12 });
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.WorkerID);
			Assert.AreEqual(TaskTypeIDs.Take, result.TaskTypeID);
		}
		#endregion


	}
}
