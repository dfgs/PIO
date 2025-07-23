using LogLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace PIO.WebAPI.Controllers
{
	public abstract class PIOController: ControllerBase
	{
		private readonly LogLib.ILogger logger;

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

		public PIOController(LogLib.ILogger Logger)
		{
			idCounter++;
			this.ID = idCounter;
			this.logger = Logger;
		}

		protected ResultTypeLib.IResult<T> Try<T>(Func<T> Function, [CallerMemberName] string? MethodName = null)
		{
			try
			{
				T value = Function();
				return ResultTypeLib.Result.Success(value);
			}
			catch (Exception ex)
			{
				Log(ex, MethodName);
				return ResultTypeLib.Result.Fail<T>(ex);
			}
		}

		protected ResultTypeLib.IResult<T> Try<T>(Message Message, Func<T> Function, [CallerMemberName] string? MethodName = null)
		{
			Log(Message, MethodName);
			try
			{
				T value = Function();
				return ResultTypeLib.Result.Success(value);
			}
			catch (Exception ex)
			{
				return ResultTypeLib.Result.Fail<T>(ex);
			}
		}


		protected ResultTypeLib.IResult<bool> Try(Action Action, [CallerMemberName] string? MethodName = null)
		{
			try
			{
				Action();
				return ResultTypeLib.Result.Success(true);
			}
			catch (Exception ex)
			{
				Log(ex);
				return ResultTypeLib.Result.Fail<bool>(ex);
			}

		}
		protected ResultTypeLib.IResult<bool> Try(Message Message, Action Action, [CallerMemberName] string? MethodName = null)
		{
			Log(Message, MethodName);
			try
			{
				Action();
				return ResultTypeLib.Result.Success(true);
			}
			catch (Exception ex)
			{
				Log(ex);
				return ResultTypeLib.Result.Fail<bool>(ex);
			}

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
