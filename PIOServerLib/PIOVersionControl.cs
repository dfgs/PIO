using NetORMLib.Databases;
using NetORMLib.Queries;
using NetORMLib.VersionControl;
using PIOServerLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOServerLib
{
	public class PIOVersionControl : VersionControl
	{
		public PIOVersionControl(IDatabase Database) : base(Database)
		{
		}

		public override int GetTargetRevision()
		{
			return 1;
		}

		protected override IEnumerable<IQuery> OnUpgradeTo(int Version)
		{
			switch(Version)
			{
				case 1:
					yield return new CreateTable<Planet>(Planet.PlanetID, Planet.Name);
					yield return new Insert<Planet>().Set(Planet.Name , "Default");
					break;

			}
		}


	}
}
