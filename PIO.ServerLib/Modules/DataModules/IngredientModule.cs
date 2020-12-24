using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;


namespace PIO.ServerLib.Modules
{
	public class IngredientModule : DatabaseModule,IIngredientModule
	{

		public IngredientModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Ingredient table (IngredientID={IngredientID})");
			query=new Select(IngredientTable.IngredientID, IngredientTable.FactoryTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).From(PIODB.IngredientTable).Where(IngredientTable.IngredientID.IsEqualTo(IngredientID));
			return TrySelectFirst <IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Ingredient[] GetIngredients(FactoryTypeIDs FactoryTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Ingredient table (FactoryTypeID={FactoryTypeID})");
			query=new Select(IngredientTable.IngredientID, IngredientTable.FactoryTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).From(PIODB.IngredientTable).Where(IngredientTable.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return TrySelectMany<IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
