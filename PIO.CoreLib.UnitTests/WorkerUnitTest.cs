using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class WorkerUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Worker worker;

			worker = new Worker(new WorkerID(1));
			Assert.AreEqual(new WorkerID(1), worker.ID);

		}







	}

}