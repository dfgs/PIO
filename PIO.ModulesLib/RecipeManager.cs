using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class RecipeManager : PIOModule,IRecipeManager
	{

		public RecipeManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
		}


	

		public IIngredient[]? GetIngredients(RecipeID RecipeID)
		{
			IIngredient[]? connectors = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Recipe ID {RecipeID}] Trying to get ingredients");
			if (!Try(() => DataSource.GetIngredients(RecipeID)).Then(result=>connectors=result.ToArray()).OrAlert($"[Recipe ID {RecipeID}] Failed to get ingredients")) return null;
			return connectors;
		}
		public IProduct[]? GetProducts(RecipeID RecipeID)
		{
			IProduct[]? connectors = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Recipe ID {RecipeID}] Trying to get products");
			if (!Try(() => DataSource.GetProducts(RecipeID)).Then(result => connectors = result.ToArray()).OrAlert($"[Recipe ID {RecipeID}] Failed to get products")) return null;
			return connectors;

		}





	}
}
