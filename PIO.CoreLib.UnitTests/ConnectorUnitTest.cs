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

			Connector = new Connector() { ResourceType = "Type1" };
			Assert.AreEqual("Type1", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);

			Connector = new Connector("Type2") ;
			Assert.AreEqual("Type2", Connector.ResourceType);
			Assert.IsNotNull(Connector.Buffer);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Connector() { ResourceType=null });
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Connector(null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}



	}

}