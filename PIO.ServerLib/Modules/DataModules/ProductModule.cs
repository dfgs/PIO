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
			query=new Select(ProductTable.ProductID, ProductTable.BuildingTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration).From(PIODB.ProductTable).Where(ProductTable.ProductID.IsEqualTo(ProductID));
			return TrySelectFirst <ProductTable,Product>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Product[] GetProducts(BuildingTypeIDs BuildingTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Product table (BuildingTypeID={BuildingTypeID})");
			query=new Select(ProductTable.ProductID, ProductTable.BuildingTypeID, ProductTable.ResourceTypeID, ProductTable.Quantity, ProductTable.Duration).From(PIODB.ProductTable).Where(ProductTable.BuildingTypeID.IsEqualTo(BuildingTypeID));
			return TrySelectMany<ProductTable,Product>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Product CreateProduct(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity,int Duration)
		{
			IInsert query;
			Product item;
			object result;

			LogEnter();

			item = new Product() { BuildingTypeID = BuildingTypeID, ResourceTypeID = ResourceTypeID, Quantity = Quantity,Duration=Duration };

			Log(LogLevels.Information, $"Inserting into Product table (BuildingTypeID={BuildingTypeID},ResourceTypeID={ResourceTypeID}, Quantity={Quantity}, Duration={Duration})");
			query = new Insert().Into(PIODB.ProductTable).Set(ProductTable.BuildingTypeID, item.BuildingTypeID).Set(ProductTable.ResourceTypeID, item.ResourceTypeID).Set(ProductTable.Quantity, item.Quantity).Set(ProductTable.Duration, item.Duration);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.ProductID = Convert.ToInt32(result);

			return item;
		}

	}
}
