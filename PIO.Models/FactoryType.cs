using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class FactoryType
	{
		[DataMember]
		public FactoryTypeIDs FactoryTypeID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int MaterialSetID { get; set; }
		[DataMember]
		public int HealthPoints { get; set; }
		[DataMember]
		public int BuildSteps { get; set; }

	}
}
