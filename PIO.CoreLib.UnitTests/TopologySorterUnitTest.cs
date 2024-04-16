using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class TopologySorterUnitTest
	{
		[TestMethod]
		public void SortShouldThrowExceptionIfParameterIsNull()
		{
			ITopologySorter sorter;

			sorter=new TopologySorter();
			#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => sorter.Sort(null));
			#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
		}




	}

}