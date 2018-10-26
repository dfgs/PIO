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
	public abstract class RowsViewModel<T> : ViewModel<IEnumerable<Row>>,IEnumerable<T>,INotifyCollectionChanged
		where T: RowViewModel
	{
		private List<T> items;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private Func<ILogger, IPIOClient, T> createItem;

		public int Count
		{
			get => items.Count;
		}

		public T this[int Index]
		{
			get => items[Index]; 
		}

		public RowsViewModel(ILogger Logger, IPIOClient Client, Func<ILogger, IPIOClient, T> CreateItem) : base( Logger, Client)
		{
			this.items = new List<T>();
			this.createItem = CreateItem;
		}

		protected override sealed void OnLoaded(IEnumerable<Row> Model)
		{
			T vm;

			items.Clear();
			foreach(Row row in Model)
			{
				vm = createItem(Logger, Client);
				vm.Load(row);
				items.Add(vm);
			}
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
