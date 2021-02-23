using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIO.ModulesLib.Modules.DataModules;

namespace PIO.Models.Modules
{
	public interface IPhraseModule:IDatabaseModule
	{
		Phrase GetPhrase(string Key,string CountryCode);
		Phrase[] GetPhrases(string CountryCode);

		Phrase CreatePhrase( string Key,string CountryCode,string Value);
	}
}
