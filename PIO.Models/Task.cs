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
		public int TaskTypeID { get; set; }
		[DataMember]
		public int WorkerID { get; set; }
		[DataMember]
		public int FactoryID { get; set; }
		[DataMember]
		public DateTime ETA { get; set; }
		

	}
}
