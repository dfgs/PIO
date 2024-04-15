using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class FactoryUnitTest
	{
		[TestMethod]
		public void ConstructorShoudSetInternalProperties()
		{
			Factory factory;

			factory = new Factory() { FactoryType = "Type1" };
			Assert.AreEqual("Type1", factory.FactoryType);
			Assert.IsNotNull(factory.Inputs);
			Assert.IsNotNull(factory.Outputs);
			Assert.IsNotNull(factory.IOs);

			factory = new Factory("Type2") ;
			Assert.AreEqual("Type2", factory.FactoryType);
			Assert.IsNotNull(factory.Inputs);
			Assert.IsNotNull(factory.Outputs);
			Assert.IsNotNull(factory.IOs);
		}

		[TestMethod]
		public void IOsPropertyShoudReturnInputsAndOutputs()
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

			factory = new Factory() { FactoryType = "Type1" };
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