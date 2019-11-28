using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Factory
	{
		[DataMember]
		public int FactoryID { get; set; }
		[DataMember]
		public int PlanetID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int StateID { get; set; }

	}
}
