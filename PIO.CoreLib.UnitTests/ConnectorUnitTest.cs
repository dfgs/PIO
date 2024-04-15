using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class ConnectorUnitTest
	{
		[TestMethod]
		public void ConstructorShoudSetInternalProperties()
		{
			Connector Connector;

			Connector = new Connector() { ResourceType = "Type1" };
			Assert.AreEqual("Type1", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);

			Connector = new Connector("Type2") ;
			Assert.AreEqual("Type2", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);
		}





	}

}