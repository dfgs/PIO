using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IMoverModule : ITaskGeneratorModule
	{

		Task BeginMoveTo(int WorkerID,int TargetFactoryID);
		void EndMoveTo(int WorkerID, int TargetFactoryID);
	}
}
