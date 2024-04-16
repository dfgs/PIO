using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIO.CoreLib
{
	public static class UniqueIDGenerator<T>
	{
		private static int id=0;

		public static int GenerateID()
		{
			return id++;
		}

	}
}
