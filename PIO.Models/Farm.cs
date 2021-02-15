using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Farm:Building
	{
		[DataMember]
		public int FarmID { get; set; }
		
		[DataMember]
		public FarmTypeIDs FarmTypeID { get; set; }
		
	

	}
}
