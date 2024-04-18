using Moq;
using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class IngredientUnitTest
	{
		[TestMethod]
		public void ConstructorShouldSetInternalProperties()
		{
			Ingredient ingredient;

			ingredient = new Ingredient() { ID = new IngredientID(0), RecipeID=new RecipeID(1), ResourceType = "Type1",Rate=3 };
			Assert.AreEqual("Type1", ingredient.ResourceType);
			Assert.AreEqual(new IngredientID(0), ingredient.ID);
			Assert.AreEqual(new RecipeID(1), ingredient.RecipeID);
			Assert.AreEqual(3, ingredient.Rate);

			ingredient = new Ingredient(new IngredientID(0), new RecipeID(1), "Type1", 3 );
			Assert.AreEqual("Type1", ingredient.ResourceType);
			Assert.AreEqual(new IngredientID(0), ingredient.ID);
			Assert.AreEqual(new RecipeID(1), ingredient.RecipeID);
			Assert.AreEqual(3, ingredient.Rate);
		}

		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Assert.ThrowsException<PIOInvalidParameterException>(() => new Ingredient(IngredientID.Empty,RecipeID.Empty, null,2));
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
		}

		



	}

}