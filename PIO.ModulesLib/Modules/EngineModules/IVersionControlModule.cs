using LogLib;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PIO.ModulesLib.Modules.EngineModules
{
	public interface IVersionControlModule: IEngineModule
	{
		bool InitializeDatabase(bool DropDatabase); 


	}
}
