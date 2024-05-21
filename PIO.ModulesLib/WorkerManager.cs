using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System;

namespace PIO.ModulesLib
{
	public class WorkerManager : Module, IWorkerManager
	{
		public WorkerManager(ILogger Logger) : base(Logger)
		{
		}
	}
}
