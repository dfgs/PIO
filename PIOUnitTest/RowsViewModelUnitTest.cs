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
	public class RowsViewModelUnitTest
	{
		
		[TestMethod]
		public void MayNotLoad()
		{
			IPIOClient client;
			PlanetsViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true, true));
			client.Connect();
			vm = new PlanetsViewModel(NullLogger.Instance,client);
			vm.Load(() => client.GetPlanets());
			Assert.AreEqual(true, vm.HasError);
		}

		[TestMethod]
		public void ShouldLoadImplicit()
		{
			IPIOClient client;
			PlanetsViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPIOServer(true,false));
			client.Connect();
			vm = new PlanetsViewModel(NullLogger.Instance,client);
			vm.Load(() => client.GetPlanets());
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(1, vm.Count);
			Assert.AreEqual(0, vm[0].PlanetID);
			Assert.AreEqual("New planet", vm[0].Name);
		}

		
	}
}
