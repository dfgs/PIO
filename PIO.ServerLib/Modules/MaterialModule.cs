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

namespace PIO.ServerLib.Modules
{
	public class MaterialModule : DatabaseModule,IMaterialModule
	{

		public MaterialModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Material GetMaterial(int MaterialID)
		{
			ISelect<MaterialTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Material table (MaterialID={MaterialID})");
			query = new Select<MaterialTable>(MaterialTable.MaterialID, MaterialTable.FactoryTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).Where(MaterialTable.MaterialID.IsEqualTo(MaterialID));
			return TrySelectFirst <MaterialTable,Material>(query).OrThrow("Failed to query");
		}

		public Material[] GetMaterials(int FactoryTypeID)
		{
			ISelect<MaterialTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Material table (FactoryTypeID={FactoryTypeID})");
			query = new Select<MaterialTable>(MaterialTable.MaterialID, MaterialTable.FactoryTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).Where(MaterialTable.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return TrySelectMany<MaterialTable,Material>(query).OrThrow("Failed to query");
		}

	}
}
