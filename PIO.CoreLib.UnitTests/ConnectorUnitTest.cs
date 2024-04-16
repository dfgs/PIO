using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class ConnectorUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Connector Connector;

			Connector = new InputConnector() { ID = new ConnectorID(0),FactoryID= new FactoryID(12),  ResourceType = "Type1" };
			Assert.AreEqual("Type1", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);
			Assert.AreEqual(new ConnectorID(0), Connector.ID);
			Assert.AreEqual(new FactoryID(12), Connector.FactoryID);

			Connector = new OutputConnector(new ConnectorID(1), new FactoryID(13), "Type2") ;
			Assert.AreEqual("Type2", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);
			Assert.AreEqual(new ConnectorID(1), Connector.ID);
			Assert.AreEqual(new FactoryID(13), Connector.FactoryID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Connector() { ResourceType=null });
			Assert.ThrowsException<PIOInvalidParameterException>(() => new InputConnector(ConnectorID.Empty, FactoryID.Empty, null));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new OutputConnector(ConnectorID.Empty, FactoryID.Empty, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}



	}

}