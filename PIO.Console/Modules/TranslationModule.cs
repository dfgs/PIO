using LogLib;
using ModuleLib;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Console.Modules
{
	public class TranslationModule : Module, ITranslationModule
	{
		private PIOServiceClient PIOClient;
		private Dictionary<string, string> dictionary;


		public TranslationModule(ILogger Logger, PIOServiceClient PIOClient) : base(Logger)
		{
			this.PIOClient = PIOClient;
			dictionary = new Dictionary<string, string>();
		}

		

		public async System.Threading.Tasks.Task LoadAsync(string CountryCode)
		{
			Phrase[] items;

			LogEnter();

			Log(LogLevels.Information, "Loading phrases");
			try
			{
				items = await TryAsync(PIOClient.GetPhrasesAsync(CountryCode)).OrThrow("Failed to load phrases");
			}
			catch
			{
				return;
			}

			Log(LogLevels.Information, "Creating dictionary");
			dictionary.Clear();
			foreach (Phrase phrase in items)
			{
				Try( ()=>dictionary.Add(phrase.Key, phrase.Value)).OrAlert($"Failed to add phrase to dictionary (Key={phrase.Key}, Value={phrase.Value})");
			}

			
		}
		public string Translate(string PhraseKey)
		{
			string result;
			LogEnter();

			if (!dictionary.TryGetValue(PhraseKey, out result)) return PhraseKey;
			return result;
		}

		public void Add(string PhraseKey, string Value)
		{
			LogEnter();

			if (dictionary.ContainsKey(PhraseKey))
			{
				Log(LogLevels.Warning, $"Phrase key {PhraseKey} already exists");
				return;
			}
			dictionary.Add(PhraseKey, Value);
		}


	}
}
