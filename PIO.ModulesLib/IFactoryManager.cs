﻿using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IFactoryManager:IPIOUpdateModule
	{
		float GetEfficiency(FactoryID FactoryID, IEnumerable<IIngredient> Ingredients, IEnumerable<IConnector> Connectors);
		bool UpdateConnectors(FactoryID FactoryID, float Efficiency, IEnumerable<IProduct> Products, IEnumerable<IConnector> Connectors);

		IRecipe? GetRecipe(FactoryTypeID FactoryTypeID);

		IInputConnector[]? GetInputConnectors(FactoryID FactoryID);
		IOutputConnector[]? GetOutputConnectors(FactoryID FactoryID);

	}
}
