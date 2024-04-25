using PIO.CoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ModulesLib
{
	public interface IConnectionManager:IPIOModule
	{
		IInputConnector[]? GetInputConnectors(FactoryID FactoryID);
		IOutputConnector[]? GetOutputConnectors(FactoryID FactoryID);

	}
}
