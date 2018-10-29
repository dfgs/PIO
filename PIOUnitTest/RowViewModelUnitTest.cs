using System;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Tables;
using PIOUnitTest.Mocks;
using PIOViewModelLib;

namespace PIOUnitTest
{
	[TestClass]
	public class RowViewModelUnitTest
	{
		[TestMethod]
		public void MayNotLoad()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true,true));
			client.Connect();
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load((Client) => Client.GetPlanet(0));
			Assert.AreEqual(true, vm.HasError);
		}

		[TestMethod]
		public void ShouldLoadImplicit()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true,false));
			client.Connect();
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load((Client) => Client.GetPlanet(0));
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(0, vm.PlanetID);
			Assert.AreEqual("New planet", vm.Name);
		}

		[TestMethod]
		public void ShouldLoadExplicit()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true,false));
			client.Connect();
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load(new Row(Table<Planet>.Columns));
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(0, vm.PlanetID);
			Assert.AreEqual("New planet", vm.Name);
		}
	}
}
