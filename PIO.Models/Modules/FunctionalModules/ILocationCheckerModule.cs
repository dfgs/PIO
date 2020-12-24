using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.FunctionalModules;

namespace PIO.Models.Modules
{
	public interface ILocationCheckerModule : IFunctionalModule
	{

		bool WorkerIsInBuilding(int WorkerID, int BuildingID);
	}
}
