using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IStackModule:IDatabaseModule
	{
		Stack GetStack(int StackID);
		Stack[] GetStacks(int FactoryID);
		Stack InsertStack(int FactoryID, ResourceTypeIDs ResourceTypeID, int Quantity);
		void UpdateStack(int StackID, int Quantity);

		int GetStackQuantity(int FactoryID, ResourceTypeIDs ResourceTypeID);
	}
}
