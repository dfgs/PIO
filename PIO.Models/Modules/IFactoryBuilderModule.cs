using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IFactoryBuilderModule : IPIOModule
	{

		void Build(int FactoryID);
	}
}
