using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Task
	{
		[DataMember]
		public int TaskID { get; set; }
		[DataMember]
		public TaskTypeIDs TaskTypeID { get; set; }
		[DataMember]
		public int WorkerID { get; set; }
		[DataMember]
		public DateTime ETA { get; set; }
		[DataMember]
		public int? X { get; set; }
		[DataMember]
		public int? Y { get; set; }
		[DataMember]
		public int? TargetFactoryID { get; set; }
		[DataMember]
		public ResourceTypeIDs? ResourceTypeID { get; set; }
		[DataMember]
		public FactoryTypeIDs? FactoryTypeID { get; set; }


	}
}
