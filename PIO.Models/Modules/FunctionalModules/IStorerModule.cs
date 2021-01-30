using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IStorerModule : ITaskGeneratorModule
	{

		Task BeginStore(int WorkerID);
		void EndStore(int WorkerID,ResourceTypeIDs ResourceTypeID);
	}
}
