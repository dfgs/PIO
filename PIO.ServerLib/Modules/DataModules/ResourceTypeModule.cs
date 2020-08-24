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
using PIO.Models.Exceptions;

namespace PIO.ServerLib.Modules
{
	public class ResourceTypeModule : DatabaseModule,IResourceTypeModule
	{

		public ResourceTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public ResourceType GetResourceType(int ResourceTypeID)
		{
			ISelect<ResourceTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ResourceType table (ResourceTypeID={ResourceTypeID})");
			query = new Select<ResourceTypeTable>(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name).Where(ResourceTypeTable.ResourceTypeID.IsEqualTo(ResourceTypeID));
			return TrySelectFirst<ResourceTypeTable,ResourceType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public ResourceType[] GetResourceTypes()
		{
			ISelect<ResourceTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying ResourceType table");
			query = new Select<ResourceTypeTable>(ResourceTypeTable.ResourceTypeID, ResourceTypeTable.Name);
			return TrySelectMany<ResourceTypeTable,ResourceType>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
