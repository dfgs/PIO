using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class RecipeUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Recipe recipe;

			recipe = new Recipe() { ID = new RecipeID(0), FactoryType = "Type1" };
			Assert.AreEqual("Type1", recipe.FactoryType);
			Assert.AreEqual(new RecipeID(0), recipe.ID);

			recipe = new Recipe(new RecipeID(1), "Type2") ;
			Assert.AreEqual("Type2", recipe.FactoryType);
			Assert.AreEqual(new RecipeID(1), recipe.ID);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Recipe(RecipeID.Empty, null));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		



	}

}