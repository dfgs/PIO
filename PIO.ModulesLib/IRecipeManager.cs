using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IRecipeManager:IPIOModule
	{


		IIngredient[]? GetIngredients(RecipeID RecipeID);
		IProduct[]? GetProducts(RecipeID RecipeID);


	}
}
