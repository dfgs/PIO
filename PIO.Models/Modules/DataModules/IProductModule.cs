using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models.Modules
{
	public interface IProductModule:IDatabaseModule
	{
		Product GetProduct(int ProductID);
		Product[] GetProducts(int FactoryTypeID);
	}
}
