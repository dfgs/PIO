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
		public int FactoryTypeID { get; set; }
		[DataMember]
		public int ResourceID { get; set; }
		[DataMember]
		public int Quantity { get; set; }


	}
}
