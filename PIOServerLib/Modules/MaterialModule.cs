using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib.Modules
{
	public class MaterialModule : DatabaseModule,IMaterialModule
	{

		public MaterialModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

	

		public IEnumerable<Row> GetMaterials(int FactoryTypeID)
		{
			ISelect query;
			LogEnter();

			query = new Select<Material>(Material.MaterialID, Material.FactoryTypeID, Material.ResourceID, Material.Quantity).Where(Material.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return Try(query).OrThrow("Failed to query");
		}

	}
}
