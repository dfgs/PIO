using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public interface IIngredient:IPIOData<IngredientID>
	{
		RecipeID RecipeID
		{
			get;
		}

		string ResourceType
		{
			get;
		}
		
		float Rate
		{
			get;
		}

	}
}
