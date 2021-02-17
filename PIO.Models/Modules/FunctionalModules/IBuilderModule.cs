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

		Task BeginCreateBuilding(int WorkerID,  FactoryTypeIDs? FactoryTypeID,FarmTypeIDs? FarmTypeID);
		void EndCreateBuilding(int WorkerID, FactoryTypeIDs? FactoryTypeID, FarmTypeIDs? FarmTypeID);
		Task BeginBuild(int WorkerID);
		void EndBuild(int WorkerID);
	}
}
