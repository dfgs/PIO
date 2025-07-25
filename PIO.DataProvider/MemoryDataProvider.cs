using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintLib.Attributes;

namespace PIO.DataProvider
{
	[Blueprint("MemoryDataProvider")]
	public partial class MemoryDataProvider : IDataProvider
	{
		private PIODatabase database;

		public MemoryDataProvider(PIODatabase Database)
		{
			this.database = Database ?? throw new ArgumentNullException(nameof(Database));
		}	

		
		
	}
}
