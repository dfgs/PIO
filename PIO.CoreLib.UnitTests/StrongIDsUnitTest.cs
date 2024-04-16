using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class StrongIDsUnitTest
	{
		[TestMethod]
		public void NewShouldGenerateUniqueValue()
		{
			Assert.AreEqual(new FactoryID(0), FactoryID.New());
			Assert.AreEqual(new FactoryID(1), FactoryID.New());
			Assert.AreEqual(new FactoryID(2), FactoryID.New());

			Assert.AreEqual(new ConnectorID(0), ConnectorID.New());
			Assert.AreEqual(new ConnectorID(1), ConnectorID.New());
			Assert.AreEqual(new ConnectorID(2), ConnectorID.New());

			Assert.AreEqual(new ConnectionID(0), ConnectionID.New());
			Assert.AreEqual(new ConnectionID(1), ConnectionID.New());
			Assert.AreEqual(new ConnectionID(2), ConnectionID.New());
		}





	}

}