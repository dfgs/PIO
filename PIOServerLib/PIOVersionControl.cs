using NetORMLib.Databases;
using NetORMLib.Queries;
using NetORMLib.VersionControl;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIOServerLib
{
	public class PIOVersionControl : VersionControl
	{
		public PIOVersionControl(IDatabase Database) : base(Database)
		{
		}

		public override int GetTargetRevision()
		{
			return 3;
		}

		protected override IEnumerable<IQuery> OnUpgradeTo(int Version)
		{
			switch(Version)
			{
				case 1:
					yield return new CreateTable<Planet>(Planet.PlanetID, Planet.Name);
					yield return new CreateTable<Status>(Status.StatusID, Status.Name);
					yield return new CreateTable<Task>(Task.TaskID, Task.FactoryID, Task.Name);
					yield return new CreateTable<Factory>(Factory.FactoryID, Factory.PlanetID, Factory.Name, Factory.StatusID);
					break;
				case 2:
					yield return new CreateRelation<Planet, Factory, int>(Planet.PlanetID, Factory.PlanetID);
					yield return new CreateRelation<Status, Factory, int>(Status.StatusID, Factory.StatusID);
					yield return new CreateRelation<Factory, Task, int>(Factory.FactoryID, Task.FactoryID);
					break;
				case 3:
					yield return new Insert<Planet>().Set(Planet.Name, "Default");
					yield return new Insert<Status>().Set(Status.Name, "Under construction");
					yield return new Insert<Status>().Set(Status.Name, "Operational");
					yield return new Insert<Factory>().Set(Factory.PlanetID, 0).Set(Factory.Name,"Stockpile").Set(Factory.StatusID,1);
					break;

			}
		}


	}
}
