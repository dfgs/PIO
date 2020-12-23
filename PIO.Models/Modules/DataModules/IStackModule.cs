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
	public interface IStackModule:IDatabaseModule
	{
		Stack GetStack(int StackID);
		Stack GetStack(int FactoryID, ResourceTypeIDs ResourceTypeID);
		Stack[] GetStacks(int FactoryID);
		Stack FindStack(int PlanetID, ResourceTypeIDs ResourceTypeID);

		Stack InsertStack(int FactoryID, ResourceTypeIDs ResourceTypeID, int Quantity);
		void UpdateStack(int StackID, int Quantity);

		int GetStackQuantity(int FactoryID, ResourceTypeIDs ResourceTypeID);
	}
}
