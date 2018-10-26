using System;
using System.Linq;
using LogLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Tables;
using PIOUnitTest.Mocks;
using PIOUnitTest.ViewModels;

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

			client = new HostedPIOClient(NullLogger.Instance, new MockedPlanetModule(true));
			vm = new PlanetsViewModel(NullLogger.Instance, client);
			vm.Load();
			Assert.AreEqual(true, vm.HasError);
		}

		[TestMethod]
		public void ShouldLoadImplicit()
		{
			IPIOClient client;
			PlanetsViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPlanetModule(false));
			vm = new PlanetsViewModel(NullLogger.Instance, client);
			vm.Load();
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(1, vm.Count);
			Assert.AreEqual(0, vm[0].PlanetID);
			Assert.AreEqual("New planet", vm[0].Name);
		}

		
	}
}
