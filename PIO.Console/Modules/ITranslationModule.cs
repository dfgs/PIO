using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Console.Modules
{
	public interface ITranslationModule
	{
		string Translate(string PhraseKey);

		void Add(string PhraseKey, string Value);
	}
}
