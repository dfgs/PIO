using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class ResourceType
	{
		[DataMember]
		public int ResourceTypeID { get; set; }
		[DataMember]
		public string Name { get; set; }
		
	}
}
