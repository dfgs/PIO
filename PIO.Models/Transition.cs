using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Transition
	{
		[DataMember]
		public int TransitionID { get; set; }
		[DataMember]
		public int StatID { get; set; }
		[DataMember]
		public int NextStateID { get; set; }
		[DataMember]
		public int EventID { get; set; }

	}
}
