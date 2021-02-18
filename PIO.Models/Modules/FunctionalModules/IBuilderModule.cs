using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IBuilderModule : ITaskGeneratorModule
	{

		Task BeginCreateBuilding(int WorkerID,  BuildingTypeIDs BuildingTypeID);
		void EndCreateBuilding(int WorkerID, BuildingTypeIDs BuildingTypeID);
		Task BeginBuild(int WorkerID);
		void EndBuild(int WorkerID);
	}
}
