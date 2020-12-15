using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.BaseModulesLib.Modules.DataModules;
using PIO.ModulesLib.Exceptions;


namespace PIO.ServerLib.Modules
{
	public class ProductModule : DatabaseModule,IProductModule
	{

		public ProductModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Product GetProduct(int ProductID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Product table (ProductID={ProductID})");
			query = new Select(ProductTable.ProductID, ProductTable.FactoryTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration).From(PIODB.ProductTable).Where(ProductTable.ProductID.IsEqualTo(ProductID));
			return TrySelectFirst <ProductTable,Product>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Product[] GetProducts(FactoryTypeIDs FactoryTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Product table (FactoryTypeID={FactoryTypeID})");
			query = new Select(ProductTable.ProductID, ProductTable.FactoryTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration).From(PIODB.ProductTable).Where(ProductTable.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return TrySelectMany<ProductTable,Product>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
