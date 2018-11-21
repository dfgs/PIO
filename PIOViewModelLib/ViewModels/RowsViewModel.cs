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

namespace PIOViewModelLib.ViewModels
{
	public abstract class RowsViewModel<T> : ViewModel<IEnumerable<Row>>,IEnumerable<T>,INotifyCollectionChanged
		where T: RowViewModel
	{
		private List<T> items;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private Func<ILogger,  T> createItem;

		public int Count
		{
			get => items.Count;
		}

		public T this[int Index]
		{
			get => items[Index]; 
		}

		public RowsViewModel(ILogger Logger, Func<ILogger,  T> CreateItem) : base( Logger)
		{
			this.items = new List<T>();
			this.createItem = CreateItem;
		}

		protected override sealed void OnSetModel(IPIOClient Client, IEnumerable<Row> Model)
		{
			T vm;

			items.Clear();
			foreach(Row row in Model)
			{
				vm = createItem(Logger);
				vm.Load(Client, row);
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
