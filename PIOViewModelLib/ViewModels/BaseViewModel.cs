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

		public ViewModel( ILogger Logger,IPIOClient Client) : base( Logger)
		{
			this.Client = Client;
		}

		protected virtual void OnLoaded(T Model)
		{

		}

		protected abstract T OnLoad();
		public void Load()
		{
			T model;

			try
			{
				model=OnLoad();
				HasError = false;
				OnLoaded(model);
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}

		public void Load(T Model)
		{
			try
			{
				HasError = false;
				OnLoaded(Model);
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
			}
		}


	}
}
