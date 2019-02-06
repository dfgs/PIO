using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public struct Message
	{
		public int FactoryID
		{
			get;
			private set;
		}

		public int EventID
		{
			get;
			private set;
		}

		public Message(int FactoryID,int EventID)
		{
			this.FactoryID = FactoryID;
			this.EventID = EventID;
		}


	}
}
