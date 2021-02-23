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
	public class PhraseModule : DatabaseModule,IPhraseModule
	{

		public PhraseModule(ILogger Logger,IDatabase Database) : base(Logger,Database)
		{
		}

		public Phrase GetPhrase(string Key,string CountryCode)
		{
			ISelect query;
			LogEnter(); 

			Log(LogLevels.Information, $"Querying Phrase table (Key={Key}, CountryCode={CountryCode})");
			query=new Select(PhraseTable.PhraseID, PhraseTable.Key, PhraseTable.CountryCode,PhraseTable.Value).From(PIODB.PhraseTable).Where(PhraseTable.PhraseID.IsEqualTo(Phrase.GenerateID(Key,CountryCode)));
			return TrySelectFirst<PhraseTable,Phrase>(query).OrThrow<PIODataException>("Failed to query");
		}

		public Phrase[] GetPhrases(string CountryCode)
		{
			ISelect query;
			LogEnter();

			Log(LogLevels.Information, $"Querying Phrase table");
			query=new Select(PhraseTable.PhraseID, PhraseTable.Key, PhraseTable.CountryCode, PhraseTable.Value).From(PIODB.PhraseTable).Where(PhraseTable.CountryCode.IsEqualTo(CountryCode));
			return TrySelectMany<PhraseTable,Phrase>(query).OrThrow<PIODataException>("Failed to query");
		}
		public Phrase CreatePhrase(string Key, string CountryCode,string Value)
		{
			IInsert query;
			Phrase item;
			object result;

			LogEnter();

			item = new Phrase() { PhraseID=Phrase.GenerateID(Key,CountryCode), Key = Key, CountryCode= CountryCode,Value=Value, };

			Log(LogLevels.Information, $"Inserting into Phrase table (Key={Key}, CountryCode={CountryCode}, Value={Value})");
			query = new Insert().Into(PIODB.PhraseTable).Set(PhraseTable.PhraseID, item.PhraseID).Set(PhraseTable.Key, item.Key).Set(PhraseTable.CountryCode, item.CountryCode).Set(PhraseTable.Value, item.Value);
			result = Try(query).OrThrow<PIODataException>("Failed to insert");
			//item.PhraseID = Convert.ToInt32(result);

			return item;
		}



	}
}
