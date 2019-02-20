using System;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Rows;
using PIOServerLib.Tables;
using PIOUnitTest.Mocks;

namespace PIOUnitTest
{
	[TestClass]
	public class HostedPIOClientUnitTest
	{
		[TestMethod]
		public void MayFailToConnect()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(false, false));
			Assert.ThrowsException<PIOClientException>(() => client.Connect());
			Assert.AreEqual(false, client.IsConnected);
		}

		[TestMethod]
		public void ShouldConnect()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			client.Connect();
			Assert.AreEqual(true, client.IsConnected);
		}

		[TestMethod]
		public void ShouldDisconnect()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			client.Connect();
			Assert.AreEqual(true, client.IsConnected);
			client.Disconnect();
			Assert.AreEqual(false, client.IsConnected);
		}


		[TestMethod]
		public void MayNotGetPlanetsWhenNotConnected()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			Assert.ThrowsException<PIOClientException>(client.GetPlanets);
		}

		[TestMethod]
		public void MayNotGetPlanetsInCaseOfError()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true,true));
			client.Connect();
			Assert.ThrowsException<PIOClientException>(client.GetPlanets);
		}

		[TestMethod]
		public void ShouldGetPlanets()
		{
			IPIOClient client;
			PlanetRow[] items;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			client.Connect();

			items = client.GetPlanets().ToArray();
			Assert.AreEqual(1, items.Length);
			foreach (dynamic item in items)
			{
				Assert.AreEqual(0, item.PlanetID);
				Assert.AreEqual("New planet", item.Name);
			}
		}

		[TestMethod]
		public void MayNotGetFactorysWhenNotConnected()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			Assert.ThrowsException<PIOClientException>(()=>client.GetFactories(0));
		}

		[TestMethod]
		public void MayNotGetFactorysInCaseOfError()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, true));
			client.Connect();
			Assert.ThrowsException<PIOClientException>(() => client.GetFactories(0));
		}

		[TestMethod]
		public void ShouldGetFactories()
		{
			IPIOClient client;
			FactoryRow[] items;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			client.Connect();

			items = client.GetFactories(1).ToArray();
			Assert.AreEqual(3, items.Length);
			foreach (dynamic item in items)
			{
				Assert.AreEqual(0, item.FactoryID);
				Assert.AreEqual(1, item.PlanetID);
				Assert.AreEqual("New factory", item.Name);
			}
		}

		[TestMethod]
		public void MayNotGetStacksWhenNotConnected()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			Assert.ThrowsException<PIOClientException>(() => client.GetStacks(0));
		}

		[TestMethod]
		public void MayNotGetStacksInCaseOfError()
		{
			IPIOClient client;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, true));
			client.Connect();
			Assert.ThrowsException<PIOClientException>(() => client.GetStacks(0));
		}

		[TestMethod]
		public void ShouldGetStacks()
		{
			IPIOClient client;
			StackRow[] items;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, false));
			client.Connect();

			items = client.GetStacks(1).ToArray();
			Assert.AreEqual(3, items.Length);
			foreach (dynamic item in items)
			{
				Assert.AreEqual(0, item.StackID);
				Assert.AreEqual(1, item.FactoryID);
			}
		}



	}
}
