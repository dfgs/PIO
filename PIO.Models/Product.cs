using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Product
	{
		[DataMember]
		public int ProductID { get; set; }
		[DataMember]
		public int FactoryTypeID { get; set; }
		[DataMember]
		public int ResourceTypeID { get; set; }
		[DataMember]
		public int Quantity { get; set; }
		[DataMember]
		public int Duration { get; set; }


	}
}
