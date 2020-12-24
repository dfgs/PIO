using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Factory:Building
	{
		[DataMember]
		public int FactoryID { get; set; }
		
		[DataMember]
		public FactoryTypeIDs FactoryTypeID { get; set; }
		
	

	}
}
