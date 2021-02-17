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
	public class MaterialModule : DatabaseModule,IMaterialModule
	{

		public MaterialModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Material GetMaterial(int MaterialID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Material table (MaterialID={MaterialID})");
			query=new Select(MaterialTable.MaterialID, MaterialTable.MaterialSetID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).From(PIODB.MaterialTable).Where(MaterialTable.MaterialID.IsEqualTo(MaterialID));
			return TrySelectFirst <MaterialTable,Material>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Material[] GetMaterials(int MaterialSetID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Material table (MaterialSetID={MaterialSetID})");
			query=new Select(MaterialTable.MaterialID, MaterialTable.MaterialSetID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).From(PIODB.MaterialTable).Where(MaterialTable.MaterialSetID.IsEqualTo(MaterialSetID));
			return TrySelectMany<MaterialTable,Material>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
