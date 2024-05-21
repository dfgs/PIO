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

			recipe = new Recipe() { ID = new RecipeID(0), FactoryTypeID = new FactoryTypeID("Type1") };
			Assert.AreEqual(new FactoryTypeID("Type1"), recipe.FactoryTypeID);
			Assert.AreEqual(new RecipeID(0), recipe.ID);

			recipe = new Recipe(new RecipeID(1), new FactoryTypeID("Type2")) ;
			Assert.AreEqual(new FactoryTypeID("Type2"), recipe.FactoryTypeID);
			Assert.AreEqual(new RecipeID(1), recipe.ID);

		}

	
		



	}

}