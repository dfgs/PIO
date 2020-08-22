using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IMaterialModule:IDatabaseModule
	{
		Material GetMaterial(int MaterialID);
		Material[] GetMaterials(int FactoryTypeID);
	}
}
