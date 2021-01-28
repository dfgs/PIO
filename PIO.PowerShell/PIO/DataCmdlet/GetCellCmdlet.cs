using PIO.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PIO.PowerShell
{
	[Cmdlet(VerbsCommon.Get, "Cell", DefaultParameterSetName = "FromID")]
	[OutputType(typeof(Cell))]
	public class GetCellCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true, ParameterSetName = "FromID")]
		public int CellID { get; set; }


		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int PlanetID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int X { get; set; }
		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true, ParameterSetName = "FromPosition")]
		public int Y { get; set; }


		protected override void ProcessRecord()
		{
			Cell result;

			switch (this.ParameterSetName)
			{
				case "FromID":
					result=Try(() => client.GetCell(CellID));
					break;
				case "FromPosition":
					result=Try(() => client.GetCellAtPos(PlanetID,X,Y));
					break;
				default:
					throw new ArgumentException("Invalid parameter set.");
			}

			WriteObject(result);
		}

	

	}
}
