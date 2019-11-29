using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServerLib.Modules
{
	public interface IResourceModule:IDatabaseModule
	{
		Resource GetResource(int ResourceID);
		IEnumerable<Resource> GetResources();
	}
}
