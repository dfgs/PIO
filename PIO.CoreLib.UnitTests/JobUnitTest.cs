using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class JobUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Job job;

			job = new Job() { ID = new JobID(0),FactoryID= new FactoryID(12) };
			Assert.AreEqual(new JobID(0), job.ID);
			Assert.AreEqual(new FactoryID(12), job.FactoryID);

			job = new Job(new JobID(1), new FactoryID(13)) ;
			Assert.AreEqual(new JobID(1), job.ID);
			Assert.AreEqual(new FactoryID(13), job.FactoryID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Job(JobID.Empty, FactoryID.Empty, null));
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new Job(JobID.Empty, FactoryID.Empty, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}



	}

}