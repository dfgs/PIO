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
			IConnector input1, input2, input3;
			IConnector output1,output2,output3;
			IConnector[] results;

			input1 = Mock.Of<IConnector>();
			input2 = Mock.Of<IConnector>();
			input3 = Mock.Of<IConnector>();

			output1 = Mock.Of<IConnector>();
			output2 = Mock.Of<IConnector>();
			output3 = Mock.Of<IConnector>();

			factory = new Factory() { FactoryType = "Type1" };
			factory.Inputs.Add(input1);
			factory.Inputs.Add(input2);
			factory.Inputs.Add(input3);
			factory.Inputs.Add(output1);
			factory.Inputs.Add(output2);
			factory.Inputs.Add(output3);

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