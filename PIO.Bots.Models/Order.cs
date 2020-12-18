using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Bots.Models
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public int OrderID { get; set; }
		[DataMember]
		public DateTime CreationDate { get; set; }
	}

}
