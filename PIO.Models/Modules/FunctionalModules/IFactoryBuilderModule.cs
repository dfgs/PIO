using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IFactoryBuilderModule : ITaskGeneratorModule
	{

		Task BeginCreateBuilding(int WorkerID,  FactoryTypeIDs FactoryTypeID);
		void EndCreateBuilding(int WorkerID, FactoryTypeIDs FactoryTypeID);
		void Build(int FactoryID);
	}
}
