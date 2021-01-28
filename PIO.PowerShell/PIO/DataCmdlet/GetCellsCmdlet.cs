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
	[Cmdlet(VerbsCommon.Get, "Cells")]
	[OutputType(typeof(Cell[]))]
	public class GetCellsCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int PlanetID { get; set; }
		[Parameter(Position = 1, ValueFromPipeline = true, Mandatory = true)]
		public int X { get; set; }
		[Parameter(Position = 2, ValueFromPipeline = true, Mandatory = true)]
		public int Y { get; set; }
		[Parameter(Position = 3, ValueFromPipeline = true, Mandatory = true)]
		public int Width { get; set; }
		[Parameter(Position = 4, ValueFromPipeline = true, Mandatory = true)]
		public int Height { get; set; }


		protected override void ProcessRecord()
		{
			Cell[] result;

			result = Try(() => client.GetCells(PlanetID,X,Y,Width,Height));

			WriteObject(result);
		}

	

	}
}
