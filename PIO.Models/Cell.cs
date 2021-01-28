using NetORMLib.Columns;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIO.Models
{
	[DataContract]
	public class Cell:ILocation
	{
		[DataMember]
		public int CellID { get; set; }
		[DataMember]
		public int PlanetID { get; set; }
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public int Y { get; set; }
		/*[DataMember]
		public CellTypeIDs CellTypeID { get; set; }*/
		


	}
}
