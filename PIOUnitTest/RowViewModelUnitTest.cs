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
	public class RowViewModelUnitTest
	{
		[TestMethod]
		public void MayNotLoad()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPlanetModule(true));
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load();
			Assert.AreEqual(true, vm.HasError);
		}

		[TestMethod]
		public void ShouldLoadImplicit()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPlanetModule(false));
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load( );
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(0, vm.PlanetID);
			Assert.AreEqual("New planet", vm.Name);
		}

		[TestMethod]
		public void ShouldLoadExplicit()
		{
			IPIOClient client;
			PlanetViewModel vm;

			client = new HostedPIOClient(NullLogger.Instance, new MockedPlanetModule(false));
			vm = new PlanetViewModel(NullLogger.Instance, client);
			vm.Load(new Row(Table<Planet>.Columns));
			Assert.AreEqual(false, vm.HasError);
			Assert.AreEqual(0, vm.PlanetID);
			Assert.AreEqual("New planet", vm.Name);
		}
	}
}
