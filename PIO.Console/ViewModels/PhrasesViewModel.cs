using PIO.Bots.ClientLib.BotsServiceReference;
using PIO.ClientLib.PIOServiceReference;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;
using Task = System.Threading.Tasks.Task;

namespace PIO.Console.ViewModels
{
	public class PhrasesViewModel : ViewModel<string>
	{
		protected PIOServiceClient PIOClient;
		protected BotsServiceClient BotsClient;

		private Dictionary<string, string> dictionary;

		public PhrasesViewModel(PIOServiceClient PIOClient, BotsServiceClient BotsClient) 
		{
			this.PIOClient = PIOClient; this.BotsClient = BotsClient;
			dictionary = new Dictionary<string, string>();
		}

		
		public sealed override async Task RefreshAsync()
		{
			await LoadAsync();
		}

		protected override async Task OnLoadAsync(string Model)
		{
			Phrase[] items;

			items = await TryAsync(PIOClient.GetPhrasesAsync(Model));
			dictionary.Clear();
			foreach(Phrase phrase in items)
			{
				dictionary.Add(phrase.Key, phrase.Value);
			}
		}

		public async Task LoadAsync()
		{
			await TryAsync(LoadAsync(Model));
		}
		public string GetString(string Key)
		{
			string result;

			if (!dictionary.TryGetValue(Key, out result)) return Key;
			return result;

		}

	}
}
