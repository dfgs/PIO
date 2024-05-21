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

			worker = new Worker() { ID = new WorkerID(0) };
			Assert.AreEqual(new WorkerID(0), worker.ID);

			worker = new Worker(new WorkerID(1));
			Assert.AreEqual(new WorkerID(1), worker.ID);

		}







	}

}