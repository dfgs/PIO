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
using PIO.Models.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class IngredientModule : DatabaseModule,IIngredientModule
	{

		public IngredientModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Ingredient GetIngredient(int IngredientID)
		{
			ISelect<IngredientTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Ingredient table (IngredientID={IngredientID})");
			query = new Select<IngredientTable>(IngredientTable.IngredientID, IngredientTable.FactoryTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).Where(IngredientTable.IngredientID.IsEqualTo(IngredientID));
			return TrySelectFirst <IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Ingredient[] GetIngredients(int FactoryTypeID)
		{
			ISelect<IngredientTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Ingredient table (FactoryTypeID={FactoryTypeID})");
			query = new Select<IngredientTable>(IngredientTable.IngredientID, IngredientTable.FactoryTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).Where(IngredientTable.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return TrySelectMany<IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
