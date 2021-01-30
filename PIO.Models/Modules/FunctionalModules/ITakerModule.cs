using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface ITakerModule : ITaskGeneratorModule
	{

		Task BeginTake(int WorkerID,ResourceTypeIDs ResourceTypeID);
		void EndTake(int WorkerID, ResourceTypeIDs ResourceTypeID);
	}
}
