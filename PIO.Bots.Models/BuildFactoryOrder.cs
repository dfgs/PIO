using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models
{
	[DataContract]
	public class BuildFactoryOrder:Order
	{
		[DataMember]
		public int BuildFactoryOrderID { get; set; }
		[DataMember]
		public FactoryTypeIDs FactoryTypeID { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }
	}

}
