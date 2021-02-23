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
	public class ResourceTypeModule : DatabaseModule,IResourceTypeModule
	{

		public ResourceTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public ResourceType GetResourceType(ResourceTypeIDs ResourceTypeID)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ResourceType table (ResourceTypeID={ResourceTypeID})");
			query=new Select(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.PhraseKey).From(PIODB.ResourceTypeTable).Where(ResourceTypeTable.ResourceTypeID.IsEqualTo(ResourceTypeID));
			return TrySelectFirst<ResourceTypeTable,ResourceType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public ResourceType[] GetResourceTypes()
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ResourceType table");
			query=new Select(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.PhraseKey).From(PIODB.ResourceTypeTable);
			return TrySelectMany<ResourceTypeTable,ResourceType>(query).OrThrow<PIODataException>("Failed to query");
		}
		public ResourceType CreateResourceType(ResourceTypeIDs ResourceTypeID, string PhraseKey)
		{
			IInsert query;
			ResourceType item;
			object result;

			LogEnter();

			item = new ResourceType() { ResourceTypeID = ResourceTypeID, PhraseKey= PhraseKey, };

			Log(LogLevels.Information, $"Inserting into ResourceType table (ResourceTypeID={ResourceTypeID}, PhraseKey={PhraseKey})");
			query = new Insert().Into(PIODB.ResourceTypeTable).Set(ResourceTypeTable.ResourceTypeID, item.ResourceTypeID).Set(ResourceTypeTable.PhraseKey, item.PhraseKey);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			//item.ResourceTypeID = Convert.ToInt32(result);

			return item;
		}



	}
}
