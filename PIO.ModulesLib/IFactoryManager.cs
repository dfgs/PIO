﻿using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IFactoryManager:IPIOModule
	{

		IFactory? GetFactory(FactoryID FactoryID);
		IFactory[]? GetFactories();

	}
}
