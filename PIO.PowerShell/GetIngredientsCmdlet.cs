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
	[Cmdlet(VerbsCommon.Get, "Ingredients")]
	[OutputType(typeof(Ingredient[]))]
	public class GetIngredientsCmdlet : PIOCmdLet
	{

		[Parameter(Position = 0, ValueFromPipeline = true, Mandatory = true)]
		public int FactoryTypeID { get; set; }

		protected override void ProcessRecord()
		{
			Ingredient[] result;

			result = client.GetIngredients(FactoryTypeID);

			WriteObject(result);
		}

	

	}
}
