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

			factory = new Factory() { ID = new FactoryID(0), FactoryTypeID = new FactoryTypeID("Type1") };
			Assert.AreEqual(new FactoryTypeID("Type1"), factory.FactoryTypeID);
			Assert.AreEqual(new FactoryID(0), factory.ID);

			factory = new Factory(new FactoryID(1), new FactoryTypeID("Type2")) ;
			Assert.AreEqual(new FactoryTypeID("Type2"), factory.FactoryTypeID);
			Assert.AreEqual(new FactoryID(1), factory.ID);
		}

		

		



	}

}