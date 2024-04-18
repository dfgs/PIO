using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Ingredient:PIOData<IngredientID>, IIngredient
	{
		
		public required RecipeID RecipeID
		{
			get;
			set;
		}
		public required string ResourceType
		{
			get;
			set;
		}

		public float Rate
		{
			get;
			set;
		}

		public Ingredient() 
		{
		}

		[SetsRequiredMembers]
		public Ingredient(IngredientID ID, RecipeID RecipeID,string ResourceType,float Rate)
		{
			if (ResourceType == null) throw new PIOInvalidParameterException(nameof(ResourceType));
			this.ID= ID;
			this.RecipeID = RecipeID;
			this.ResourceType = ResourceType;
			this.Rate= Rate;
		}


	}
}
