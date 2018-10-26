using LogLib;
using ModuleLib;
using PIOClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOViewModelLib
{
	public abstract class ViewModel<T> : Module
	{

		public dynamic Model
		{
			get;
			private set;
		}

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

		protected virtual T OnLoad()
		{
			return Model;
		}

		public void Load()
		{
			T model;

			try
			{
				model=OnLoad();
				HasError = false;
			}
			catch (Exception ex)
			{
				Log(ex);
				HasError = true;
				return;
			}
			Load(model);
		}

		protected virtual void OnLoaded(T Model)
		{

		}


		public void Load(T Model)
		{
			try
			{
				this.Model = Model;
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
