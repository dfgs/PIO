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

			factory = new Factory() { ID = new FactoryID(0), FactoryType = "Type1" };
			Assert.AreEqual("Type1", factory.FactoryType);
			Assert.AreEqual(new FactoryID(0), factory.ID);

			factory = new Factory(new FactoryID(1), "Type2") ;
			Assert.AreEqual("Type2", factory.FactoryType);
			Assert.AreEqual(new FactoryID(1), factory.ID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Factory(FactoryID.Empty, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		



	}

}