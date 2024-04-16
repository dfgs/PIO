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
			#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => sorter.Sort(null));
			#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}
		[TestMethod]
		public void SortShouldReturnValidValues()
		{
			ITopologySorter sorter;
			IDataSource dataSource;
			IFactory[] results;

			dataSource=Mock.Of<IDataSource>();

			sorter = new TopologySorter();
			results = sorter.Sort(dataSource).ToArray();
			
		}



	}

}