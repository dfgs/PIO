using LogLib;
using ModuleLib;
using PIOClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib.ViewModels
{
	public abstract class ViewModel<T> : Module
	{

		
		public bool HasError
		{
			get;
			private set;
		}

		/*protected IPIOClient Client
		{
			get;
			private set;
		}*/

		public ViewModel( ILogger Logger) : base( Logger)
		{
			//this.Client = Client;
		}

		protected abstract void OnSetModel(IPIOClient Client, T Model);

		protected virtual void OnLoaded(IPIOClient Client)
		{

		}

		/*protected abstract T OnLoadModel();
		public void Load()
		{
			T model;

			try
			{
				model=OnLoadModel();
				OnSetModel(model);
				OnLoaded();
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}*/

		public void Load(IPIOClient Client, Func<T> Func)
		{
			T Model;
			try
			{
				Model = Func();
				OnSetModel(Client,Model);
				OnLoaded(Client);
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}

		public void Load(IPIOClient Client, T Model)
		{
			try
			{
				OnSetModel(Client,Model);
				OnLoaded(Client);
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}


	}
}
