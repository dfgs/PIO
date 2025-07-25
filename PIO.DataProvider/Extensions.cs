using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.DataProvider
{
	public static class Extensions
	{
		public static int FirstIndex<T>(this IEnumerable<T> Items, Func<T, bool> Predicate)
		{
			int index = 0;
			foreach(T item in Items)
			{
				if (Predicate(item)) return index;
				index++;
			}
			return -1;
		}

	}
}
