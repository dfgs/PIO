using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Material
	{
		[DataMember]
		public int MaterialID { get; set; }
		[DataMember]
		public FactoryTypeIDs FactoryTypeID { get; set; }
		[DataMember]
		public ResourceTypeIDs ResourceTypeID { get; set; }
		[DataMember]
		public int Quantity { get; set; }


	}
}
