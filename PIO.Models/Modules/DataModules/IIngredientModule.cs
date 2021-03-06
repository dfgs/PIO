﻿using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IIngredientModule:IDatabaseModule
	{
		Ingredient GetIngredient(int IngredientID);
		Ingredient[] GetIngredients(BuildingTypeIDs BuildingTypeID);
		Ingredient CreateIngredient(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity);

	}
}
