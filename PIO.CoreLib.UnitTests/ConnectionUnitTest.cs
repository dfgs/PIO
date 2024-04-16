using Moq;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class ConnectionUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Connection Connection;
			IInputConnector input1, input2;
			IOutputConnector output1, output2;

			input1 = Mock.Of<IInputConnector>();
			Mock.Get(input1).Setup(m => m.ID).Returns(new ConnectorID(1));
			input2 = Mock.Of<IInputConnector>();
			Mock.Get(input2).Setup(m => m.ID).Returns(new ConnectorID(2));

			output1 = Mock.Of<IOutputConnector>();
			Mock.Get(output1).Setup(m => m.ID).Returns(new ConnectorID(3));
			output2 = Mock.Of<IOutputConnector>();
			Mock.Get(output2).Setup(m => m.ID).Returns(new ConnectorID(4));


			Connection = new Connection() { ID= new ConnectionID(0), SourceID =output1.ID, DestinationID =input1.ID };
			Assert.AreEqual(output1.ID, Connection.SourceID);
			Assert.AreEqual(input1.ID, Connection.DestinationID);
			Assert.AreEqual(new ConnectionID(0), Connection.ID);

			Connection = new Connection(new ConnectionID(1), output2.ID,input2.ID);
			Assert.AreEqual(output2.ID, Connection.SourceID);
			Assert.AreEqual(input2.ID, Connection.DestinationID);
			Assert.AreEqual(new ConnectionID(1), Connection.ID);
		}
		/*[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
			IInputConnector input1, input2;
			IOutputConnector output1, output2;

			input1 = Mock.Of<IInputConnector>();
			input2 = Mock.Of<IInputConnector>();

			output1 = Mock.Of<IOutputConnector>();
			output2 = Mock.Of<IOutputConnector>();

			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection(ConnectionID.Empty, null, input1));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection(ConnectionID.Empty, output1, null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.


		}*/

	}

}