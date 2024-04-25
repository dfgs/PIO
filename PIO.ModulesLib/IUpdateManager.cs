﻿using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IUpdateManager:IPIOUpdateModule
	{
		float GetEfficiency(FactoryID FactoryID, IEnumerable<IIngredient> Ingredients, IEnumerable<IConnector> Connectors);
		bool UpdateConnectors(FactoryID FactoryID, float Efficiency, IEnumerable<IProduct> Products, IEnumerable<IConnector> Connectors);



	}
}
