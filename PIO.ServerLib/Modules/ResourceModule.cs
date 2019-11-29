using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.Models;
using PIO.ServerLib.Tables;
using PIO.WebServerLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.ServerLib.Modules
{
	public class ResourceModule : DatabaseModule,IResourceModule
	{

		public ResourceModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Resource GetResource(int ResourceID)
		{
			ISelect<ResourceTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying resource with ResourceID {ResourceID}");
			query = new Select<ResourceTable>(ResourceTable.ResourceID, ResourceTable.Name).Where(ResourceTable.ResourceID.IsEqualTo(ResourceID));
			return TrySelectFirst<ResourceTable,Resource>(query).OrThrow("Failed to query");
		}

		public IEnumerable<Resource> GetResources()
		{
			ISelect<ResourceTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying resources");
			query = new Select<ResourceTable>(ResourceTable.ResourceID, ResourceTable.Name);
			return TrySelectMany<ResourceTable,Resource>(query).OrThrow("Failed to query");
		}

	}
}
