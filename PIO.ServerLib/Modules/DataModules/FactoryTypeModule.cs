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
	public class FactoryTypeModule : DatabaseModule,IFactoryTypeModule
	{

		public FactoryTypeModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public FactoryType GetFactoryType(FactoryTypeIDs FactoryTypeID)
		{
			ISelect<FactoryTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying FactoryType table (FactoryTypeID={FactoryTypeID})");
			query = new Select<FactoryTypeTable>(FactoryTypeTable.FactoryTypeID, FactoryTypeTable.Name,FactoryTypeTable.HealthPoints).Where(FactoryTypeTable.FactoryTypeID.IsEqualTo(FactoryTypeID));
			return TrySelectFirst<FactoryTypeTable,FactoryType>(query).OrThrow<PIODataException>("Failed to query");
		}

		public FactoryType[] GetFactoryTypes()
		{
			ISelect<FactoryTypeTable> query;
			LogEnter();

			Log(LogLevels.Information, $"Querying FactoryType table");
			query = new Select<FactoryTypeTable>(FactoryTypeTable.FactoryTypeID, FactoryTypeTable.Name, FactoryTypeTable.HealthPoints);
			return TrySelectMany<FactoryTypeTable,FactoryType>(query).OrThrow<PIODataException>("Failed to query");
		}

	}
}
