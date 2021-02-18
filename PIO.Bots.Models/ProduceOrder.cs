using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models
{
	[DataContract]
	public class ProduceOrder:Order
	{
		[DataMember]
		public int ProduceOrderID { get; set; }
		[DataMember]
		public int BuildingID { get; set; }
	}

}
