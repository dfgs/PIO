using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogLib;
using NetORMLib;
using PIOClientLib;

namespace PIOViewModelLib
{
	public abstract class RowsViewModel<T> : ViewModel<IEnumerable<T>>,IEnumerable<T>,INotifyCollectionChanged
		where T: RowViewModel
	{
		private List<T> items;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private Func<ILogger, IPIOClient, T> createItem;

		public RowsViewModel(ILogger Logger, IPIOClient Client, Func<ILogger, IPIOClient, T> CreateItem) : base( Logger, Client)
		{
			items = new List<T>();
			this.createItem = CreateItem;
		}

		protected abstract IEnumerable<Row> GetRows();

		protected override sealed IEnumerable<T> OnLoad()
		{
			List<T> rows;
			T vm;

			rows = new List<T>();
			foreach(Row row in GetRows())
			{
				vm = createItem(Logger,Client);
				vm.Load(row);
				rows.Add(vm);
			}
			return rows;
		}

		protected override sealed void OnLoaded(IEnumerable<T> Model)
		{
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (CollectionChanged != null) CollectionChanged(this, e);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}



	}
}
