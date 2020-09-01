using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Worker
	{
		[DataMember]
		public int WorkerID { get; set; }
		[DataMember]
		public int FactoryID { get; set; }




	}
}
