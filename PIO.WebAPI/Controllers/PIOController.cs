using LogLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PIO.DataProvider;
using PIO.Models;
using System.Runtime.CompilerServices;

namespace PIO.WebAPI.Controllers
{
	public abstract class PIOController: ControllerBase
	{
		private readonly LogLib.ILogger logger;

		private IDataProvider dataProvider;
		protected IDataProvider DataProvider
		{
			get => dataProvider;
		}

		private static int idCounter = 0;
		public int ID
		{
			get;
			private set;
		}

		public virtual string ModuleName
		{
			get { return GetType().Name; }
		}

		public PIOController(LogLib.ILogger Logger, IDataProvider DataProvider)
		{
			idCounter++;
			this.ID = idCounter;
			this.logger = Logger;
			this.dataProvider = DataProvider;
		}
		
		protected void LogEnter([CallerMemberName] string? MethodName = null)
		{
			logger.LogEnter(ID, ModuleName, MethodName);
		}

		protected void Log(Message Message, [CallerMemberName] string? MethodName = null)
		{
			logger.Log(ID, ModuleName, MethodName, Message);
		}
		protected void Log(Exception ex, [CallerMemberName] string? MethodName = null)
		{
			logger.Log(ID, ModuleName, MethodName, Message.Error($"An unexpected exception occured in {ModuleName}:{MethodName} ({ExceptionFormatter.Format(ex)})"));
		}
		



	}
}
