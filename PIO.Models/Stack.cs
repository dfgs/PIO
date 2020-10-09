using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Stack
	{
		[DataMember]
		public int StackID { get; set; }
		[DataMember]
		public int FactoryID { get; set; }
		[DataMember]
		public ResourceTypeIDs ResourceTypeID { get; set; }
		[DataMember]
		public int Quantity { get; set; }


	}
}
