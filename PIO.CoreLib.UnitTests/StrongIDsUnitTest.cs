using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class StrongIDsUnitTest
	{
		[TestMethod]
		public void NewShouldGenerateUniqueValue()
		{
			Assert.AreEqual(new FactoryID(0), FactoryID.New());
			Assert.AreEqual(new FactoryID(1), FactoryID.New());
			Assert.AreEqual(new FactoryID(2), FactoryID.New());

			Assert.AreEqual(new ConnectorID(0), ConnectorID.New());
			Assert.AreEqual(new ConnectorID(1), ConnectorID.New());
			Assert.AreEqual(new ConnectorID(2), ConnectorID.New());

			Assert.AreEqual(new ConnectionID(0), ConnectionID.New());
			Assert.AreEqual(new ConnectionID(1), ConnectionID.New());
			Assert.AreEqual(new ConnectionID(2), ConnectionID.New());

			Assert.AreEqual(new BufferID(0), BufferID.New());
			Assert.AreEqual(new BufferID(1), BufferID.New());
			Assert.AreEqual(new BufferID(2), BufferID.New());

			Assert.AreEqual(new RecipeID(0), RecipeID.New());
			Assert.AreEqual(new RecipeID(1), RecipeID.New());
			Assert.AreEqual(new RecipeID(2), RecipeID.New());

			Assert.AreEqual(new IngredientID(0), IngredientID.New());
			Assert.AreEqual(new IngredientID(1), IngredientID.New());
			Assert.AreEqual(new IngredientID(2), IngredientID.New());

			Assert.AreEqual(new ProductID(0), ProductID.New());
			Assert.AreEqual(new ProductID(1), ProductID.New());
			Assert.AreEqual(new ProductID(2), ProductID.New());
		}





	}

}