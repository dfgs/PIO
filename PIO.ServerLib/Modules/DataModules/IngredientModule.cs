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
			query=new Select(IngredientTable.IngredientID, IngredientTable.BuildingTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).From(PIODB.IngredientTable).Where(IngredientTable.IngredientID.IsEqualTo(IngredientID));
			return TrySelectFirst <IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Ingredient[] GetIngredients(BuildingTypeIDs BuildingTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Ingredient table (BuildingTypeID={BuildingTypeID})");
			query=new Select(IngredientTable.IngredientID, IngredientTable.BuildingTypeID, IngredientTable.ResourceTypeID, IngredientTable.Quantity).From(PIODB.IngredientTable).Where(IngredientTable.BuildingTypeID.IsEqualTo(BuildingTypeID));
			return TrySelectMany<IngredientTable,Ingredient>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Ingredient CreateIngredient(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			IInsert query;
			Ingredient item;
			object result;

			LogEnter();

			item = new Ingredient() { BuildingTypeID = BuildingTypeID, ResourceTypeID = ResourceTypeID, Quantity = Quantity, };

			Log(LogLevels.Information, $"Inserting into Ingredient table (BuildingTypeID={BuildingTypeID},ResourceTypeID={ResourceTypeID}, Quantity={Quantity})");
			query = new Insert().Into(PIODB.IngredientTable).Set(IngredientTable.BuildingTypeID, item.BuildingTypeID).Set(IngredientTable.ResourceTypeID, item.ResourceTypeID).Set(IngredientTable.Quantity, item.Quantity);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.IngredientID = Convert.ToInt32(result);

			return item;
		}


	}
}
