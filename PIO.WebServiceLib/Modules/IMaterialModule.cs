using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.WebServerLib.Modules
{
	public interface IMaterialModule:IDatabaseModule
	{
		Material GetMaterial(int MaterialID);
		IEnumerable<Material> GetMaterials(int FactoryTypeID);
	}
}
