using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PIO.Models
{
	[DataContract]
	public class Planet
	{
		[DataMember]
		public int PlanetID { get; set; }
		[DataMember]
		public string Name { get; set; }

	}
}
