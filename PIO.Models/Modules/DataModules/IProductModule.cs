using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IProductModule:IDatabaseModule
	{
		Product GetProduct(int ProductID);
		Product[] GetProducts(BuildingTypeIDs BuildingTypeID);
		Product CreateProduct(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity,int Duration);

	}
}
