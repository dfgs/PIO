using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Event
	{
		[DataMember]
		public int EventID { get; set; }
		[DataMember]
		public string Name { get; set; }

	}
}
