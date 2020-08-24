using PIO.Models;
using PIO.PowerShell.PIOWebServiceReference;
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
	[Cmdlet(VerbsCommon.Get, "Ingredient")]
	[OutputType(typeof(Ingredient))]
	public class GetIngredientCmdlet : PIOCmdLet
	{
		[Parameter(Position = 0, ValueFromPipeline =true,Mandatory =true)]
		public int IngredientID { get; set; }


		

		protected override void ProcessRecord()
		{
			Ingredient result;

			result = client.GetIngredient(IngredientID);

			WriteObject(result);
		}

	

	}
}
