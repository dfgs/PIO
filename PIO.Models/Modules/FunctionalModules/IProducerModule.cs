using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IProducerModule : ITaskGeneratorModule
	{

		Task BeginProduce(int WorkerID, int FactoryID);
		void EndProduce(int WorkerID, int FactoryID);
	}
}
