using LogLib;
using ModuleLib;
using PIO.ModulesLib;
using PIO.ModulesLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PIO.BaseModulesLib
{
	public class PIOModule : Module, IPIOModule
	{
		public PIOModule(ILogger Logger) : base(Logger)
		{
		}

		protected T AssertExists<T>(Func<T> Function, string ParameterList, [CallerMemberName]string MethodName=null)
			where T:class
		{
			T item;
			string itemName;

			itemName = typeof(T).Name;

			Log(LogLevels.Information, $"Get item {itemName} ({ParameterList})");
			item=Try(() => Function()).OrThrow<PIOInternalErrorException>($"Failed to get item {itemName}");
			if (item == null)
			{
				Log(LogLevels.Warning, $"{itemName} doesn't exist ({ParameterList})");
				throw new PIONotFoundException($"{itemName} doesn't exist ({ParameterList})", null, ID, ModuleName, MethodName);
			}

			return item;
		}
		protected void Throw<TException>(LogLevels LogLevel, string Message, [CallerMemberName] string MethodName = null)
			where TException : TryException
		{
			Log(LogLevel, Message);
			throw (TException)Activator.CreateInstance(typeof(TException), Message, null, ID, ModuleName, MethodName);
		}

	}
}
