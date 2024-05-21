using PIO.CoreLib.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public class Recipe:PIOData<RecipeID>, IRecipe
	{
		
		public required FactoryTypeID FactoryTypeID
		{
			get;
			set;
		}


		public Recipe() 
		{
		}

		[SetsRequiredMembers]
		public Recipe(RecipeID ID, FactoryTypeID FactoryTypeID)
		{
			this.ID= ID;
			this.FactoryTypeID = FactoryTypeID;
		}


	}
}
