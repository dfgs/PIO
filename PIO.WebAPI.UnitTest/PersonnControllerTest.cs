using BlueprintLib.Attributes;
using LogLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using PIO.DataProvider;
using PIO.Models;
using PIO.WebAPI.Controllers;


namespace PIO.WebAPI.UnitTest
{
	[DTO("Personn"), Blueprint("ControllerUnitTest")]
	public partial class PersonnControllerTest
	{
		public void test()
		{
			DebugLogger logger;
			IDataProvider dataProvider;
			PersonnController controller;
			IStatusCodeActionResult? result;
			

			// Arrange
			logger = new DebugLogger();

			dataProvider = Mock.Of<IDataProvider>();
			Mock.Get(dataProvider).Setup(mock => mock.GetPersonn(It.IsAny<byte>())).Throws<InvalidOperationException>();
#pragma warning disable CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.
			Mock.Get(dataProvider).Setup(mock => mock.GetPersonn(It.IsAny<byte>())).Returns<Personn?>(null);
#pragma warning restore CS8625 // Impossible de convertir un littéral ayant une valeur null en type référence non-nullable.

			controller = new PersonnController(logger, dataProvider);

			// Act
			result = controller.Get() as IStatusCodeActionResult;
			
			// Assert
			Assert.IsNotNull(result);
			

			Assert.AreEqual(0, logger.WarningCount);
			Assert.AreEqual(0, logger.ErrorCount);
			Assert.AreEqual(0, logger.FatalCount);
		}

	}
}
