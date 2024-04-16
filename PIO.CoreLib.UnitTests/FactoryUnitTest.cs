using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class FactoryUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Factory factory;

			factory = new Factory() { ID = FactoryID.New(), FactoryType = "Type1" };
			Assert.AreEqual("Type1", factory.FactoryType);
			Assert.IsNotNull(factory.Inputs);
			Assert.IsNotNull(factory.Outputs);
			Assert.IsNotNull(factory.IOs);
			Assert.AreEqual(new FactoryID(0), factory.ID);

			factory = new Factory(FactoryID.New(), "Type2") ;
			Assert.AreEqual("Type2", factory.FactoryType);
			Assert.IsNotNull(factory.Inputs);
			Assert.IsNotNull(factory.Outputs);
			Assert.IsNotNull(factory.IOs);
			Assert.AreEqual(new FactoryID(1), factory.ID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Factory(FactoryID.New(), null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		[TestMethod]
		public void IOsPropertyShouldReturnInputsAndOutputs()
		{
			Factory factory;
			IInputConnector input1, input2, input3;
			IOutputConnector output1,output2,output3;
			IConnector[] results;

			input1 = Mock.Of<IInputConnector>();
			input2 = Mock.Of<IInputConnector>();
			input3 = Mock.Of<IInputConnector>();

			output1 = Mock.Of<IOutputConnector>();
			output2 = Mock.Of<IOutputConnector>();
			output3 = Mock.Of<IOutputConnector>();

			factory = new Factory() { ID = FactoryID.New(), FactoryType = "Type1" };
			factory.Inputs.Add(input1);
			factory.Inputs.Add(input2);
			factory.Inputs.Add(input3);
			factory.Outputs.Add(output1);
			factory.Outputs.Add(output2);
			factory.Outputs.Add(output3);

			results=factory.IOs.ToArray();

			Assert.AreEqual(6, results.Length);
			Assert.IsTrue(results.Contains(input1));
			Assert.IsTrue(results.Contains(input2));
			Assert.IsTrue(results.Contains(input3));
			Assert.IsTrue(results.Contains(output1));
			Assert.IsTrue(results.Contains(output2));
			Assert.IsTrue(results.Contains(output3));
		}



	}

}