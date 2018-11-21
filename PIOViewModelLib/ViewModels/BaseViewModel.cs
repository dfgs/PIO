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

		protected IPIOClient Client
		{
			get;
			private set;
		}

		public ViewModel(ILogger Logger,IPIOClient Client) : base( Logger)
		{
			this.Client = Client;
		}

		protected abstract void OnSetModel(T Model);

		protected virtual void OnLoaded()
		{

		}

		public void Load(Func<T> Func)
		{
			T Model;
			try
			{
				Model = Func();
				OnSetModel(Model);
				OnLoaded();
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}

		/*public void Load(T Model)
		{
			try
			{
				OnSetModel(Model);
				OnLoaded();
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}*/


	}
}
