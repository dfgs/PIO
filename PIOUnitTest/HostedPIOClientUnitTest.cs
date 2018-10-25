using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetORMLib;
using PIOClientLib;
using PIOServerLib.Tables;
using PIOUnitTest.Mocks;

namespace PIOUnitTest
{
	[TestClass]
	public class HostedPIOClientUnitTest
	{
		[TestMethod]
		public void ShouldThrowError()
		{
			IPIOClient client;

			client = new HostedPIOClient(new MockedPlanetModule(true));
			Assert.ThrowsException<PIOClientException>(client.GetPlanets);
		}

		[TestMethod]
		public void ShouldGetPlanets()
		{
			IPIOClient client;
			Row[] items;

			client = new HostedPIOClient(new MockedPlanetModule(false));
			items = client.GetPlanets().ToArray();
			Assert.AreEqual(1, items.Length);
			foreach (dynamic item in items)
			{
				Assert.AreEqual(0, item.PlanetID);
				Assert.AreEqual(null, item.Name);
			}
		}
	}
}
