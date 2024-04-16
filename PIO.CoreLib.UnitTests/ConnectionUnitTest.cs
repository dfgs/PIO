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
			input2 = Mock.Of<IInputConnector>();

			output1 = Mock.Of<IOutputConnector>();
			output2 = Mock.Of<IOutputConnector>();


			Connection = new Connection() { Source=output1,Destination=input1};
			Assert.AreEqual(output1, Connection.Source);
			Assert.AreEqual(input1, Connection.Destination);

			Connection = new Connection(output2,input2);
			Assert.AreEqual(output2, Connection.Source);
			Assert.AreEqual(input2, Connection.Destination);
		}
		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
			IInputConnector input1, input2;
			IOutputConnector output1, output2;

			input1 = Mock.Of<IInputConnector>();
			input2 = Mock.Of<IInputConnector>();

			output1 = Mock.Of<IOutputConnector>();
			output2 = Mock.Of<IOutputConnector>();

			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection() { Source = null, Destination = input1 });
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection() { Source = output1, Destination = null });
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection(null, input1));
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Connection(output1, null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.


		}

	}

}