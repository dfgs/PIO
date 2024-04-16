using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class UniqueIDGeneratorUnitTest
	{
		[TestMethod]
		public void GenerateShouldReturnUniqueValues()
		{
			Assert.AreEqual(0, UniqueIDGenerator<int>.GenerateID());
			Assert.AreEqual(1, UniqueIDGenerator<int>.GenerateID());
			Assert.AreEqual(2, UniqueIDGenerator<int>.GenerateID());

			Assert.AreEqual(0, UniqueIDGenerator<string>.GenerateID());
			Assert.AreEqual(1, UniqueIDGenerator<string>.GenerateID());

		}





	}

}