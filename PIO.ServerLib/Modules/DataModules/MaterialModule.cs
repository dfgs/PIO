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
			query=new Select(MaterialTable.MaterialID, MaterialTable.BuildingTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).From(PIODB.MaterialTable).Where(MaterialTable.MaterialID.IsEqualTo(MaterialID));
			return TrySelectFirst <MaterialTable,Material>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Material[] GetMaterials(BuildingTypeIDs BuildingTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Material table (BuildingTypeID={BuildingTypeID})");
			query=new Select(MaterialTable.MaterialID, MaterialTable.BuildingTypeID, MaterialTable.ResourceTypeID, MaterialTable.Quantity).From(PIODB.MaterialTable).Where(MaterialTable.BuildingTypeID.IsEqualTo(BuildingTypeID));
			return TrySelectMany<MaterialTable,Material>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Material CreateMaterial(BuildingTypeIDs BuildingTypeID, ResourceTypeIDs ResourceTypeID, int Quantity)
		{
			IInsert query;
			Material item;
			object result;

			LogEnter();

			item = new Material() { BuildingTypeID=BuildingTypeID, ResourceTypeID = ResourceTypeID, Quantity = Quantity, };

			Log(LogLevels.Information, $"Inserting into Material table (BuildingTypeID={BuildingTypeID},ResourceTypeID={ResourceTypeID}, Quantity={Quantity})");
			query = new Insert().Into(PIODB.MaterialTable).Set(MaterialTable.BuildingTypeID,item.BuildingTypeID).Set(MaterialTable.ResourceTypeID, item.ResourceTypeID).Set(MaterialTable.Quantity, item.Quantity);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			item.MaterialID = Convert.ToInt32(result);

			return item;
		}

	}
}
