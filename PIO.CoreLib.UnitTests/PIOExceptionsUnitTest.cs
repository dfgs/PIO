using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class PIOExceptionsUnitTest
	{
		[TestMethod]
		public void PIOInvalidBufferStateExceptionShouldThrow()
		{
			Assert.ThrowsException<PIOInvalidBufferStateException>(() => throw new PIOInvalidBufferStateException());
		}
		[TestMethod]
		public void PIOInvalidParameterExceptionShouldThrow()
		{
			Assert.ThrowsException<PIOInvalidParameterException>(() => throw new PIOInvalidParameterException("Param1"));
		}
		[TestMethod]
		public void PIOInvalidParameterExceptionShouldHaveValidParameterName()
		{
			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => throw new PIOInvalidParameterException(null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}





	}

}