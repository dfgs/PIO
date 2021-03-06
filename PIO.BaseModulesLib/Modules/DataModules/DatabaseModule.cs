﻿using LogLib;
using ModuleLib;
using NetORMLib;
using NetORMLib.Databases;
using NetORMLib.Queries;
using PIO.ModulesLib;
using PIO.ModulesLib.Modules.DataModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PIO.BaseModulesLib.Modules.DataModules
{
	public abstract class DatabaseModule : PIOModule,IDatabaseModule
	{
		private IDatabase database;

		public DatabaseModule(ILogger Logger,IDatabase Database) : base(Logger)
		{
			this.database = Database;
		}

		// ToArray must be included in lambda expression in order to be evaluated and catched by Try
		protected ITryFunction<TRow[]> TrySelectMany<TTable, TRow>(ISelect Query, [CallerMemberName]string MethodName = null)
			where TRow : new()
		{
			return Try<TRow[]>(() => this.database.Execute<TRow>(Query).ToArray(), MethodName);
		}
		// FirstOrDefault must be included in lambda expression in order to be evaluated and catched by Try
		protected ITryFunction<TRow> TrySelectFirst<TTable, TRow>(ISelect Query, [CallerMemberName]string MethodName = null)
			where TRow : new()
		{
			return Try<TRow>(() => this.database.Execute<TRow>(Query).FirstOrDefault(), MethodName);
		}
		/*protected ITryFunction<int> Try<TTable>(ISelectIdentity<TTable> Query, [CallerMemberName]string MethodName = null)
		{
			return Try<int>(() =>
			{
				object result;

				result = this.database.Execute(Query);
				return (int)result;
			}
			, MethodName) ;
		}*/

		protected ITryFunction<object> Try(IInsert Query, [CallerMemberName]string MethodName = null)
		{
			return Try(() =>
			{
				return this.database.Execute(Query, new SelectIdentity());
			}	
			, MethodName);
		}

		protected ITryAction Try(IUpdate Query, [CallerMemberName]string MethodName = null)
		{
			return Try(() => this.database.Execute(Query), MethodName);
		}
		protected ITryAction Try(IDelete Query, [CallerMemberName]string MethodName = null)
		{
			return Try(() => this.database.Execute(Query), MethodName);
		}

		/*protected ITryAction Try(IEnumerable<IQuery> Queries, [CallerMemberName]string MethodName = null)
		{
			return Try(()=>this.database.Execute(Queries), MethodName);
		}*/

	}
}
