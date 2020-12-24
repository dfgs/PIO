using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface ICarrierModule : ITaskGeneratorModule
	{

		Task BeginCarryTo(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID);
		void EndCarryTo(int WorkerID, int TargetBuildingID, ResourceTypeIDs ResourceTypeID);
	}
}
