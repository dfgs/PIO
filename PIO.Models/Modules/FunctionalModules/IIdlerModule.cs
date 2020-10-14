using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IIdlerModule : ITaskGeneratorModule
	{

		Task BeginIdle(int WorkerID,int Duration);
		void EndIdle(int WorkerID);
	}
}
