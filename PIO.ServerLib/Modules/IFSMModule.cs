using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public interface IFSMModule:IMessageListenerModule
	{
		void Initialize(int FactoryID,int StateID);
	}
}
