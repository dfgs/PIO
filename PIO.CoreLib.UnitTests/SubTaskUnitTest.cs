using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class SubTaskUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			SubTask subTask;

			subTask = new SubTask() { ID = new SubTaskID(0),JobID= new JobID(12) };
			Assert.AreEqual(new SubTaskID(0), subTask.ID);
			Assert.AreEqual(new JobID(12), subTask.JobID);

			subTask = new SubTask(new SubTaskID(1), new JobID(13)) ;
			Assert.AreEqual(new SubTaskID(1), subTask.ID);
			Assert.AreEqual(new JobID(13), subTask.JobID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new SubTask(SubTaskID.Empty, JobID.Empty, null));
			//Assert.ThrowsException<PIOInvalidParameterException>(() => new SubTask(SubTaskID.Empty, JobID.Empty, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}



	}

}