using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PIO.Models;
using PIO.WebServerLib.Modules;

namespace PIO.WebServiceLib
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class PIOService : IPIOService
	{
		private IPlanetModule PlanetModule;
		private IFactoryModule FactoryModule;
		private IStackModule StackModule;
		private IResourceTypeModule ResourceModule;
		private IFactoryTypeModule FactoryTypeModule;
		private IMaterialModule MaterialModule;

		public PIOService(IPlanetModule PlanetModule, IFactoryModule FactoryModule,IStackModule StackModule,IResourceTypeModule ResourceModule,IFactoryTypeModule FactoryTypeModule,IMaterialModule MaterialModule)
		{
			this.PlanetModule = PlanetModule;
			this.FactoryModule = FactoryModule;
			this.StackModule = StackModule;
			this.ResourceModule = ResourceModule;
			this.FactoryTypeModule = FactoryTypeModule;
			this.MaterialModule = MaterialModule;
		}

		public Planet GetPlanet(int PlanetID)
		{
			return PlanetModule.GetPlanet(PlanetID);
		}
		public Planet[] GetPlanets()
		{
			return PlanetModule.GetPlanets().ToArray();
		}

		public Factory GetFactory(int FactoryID)
		{
			return FactoryModule.GetFactory(FactoryID);
		}
		public Factory[] GetFactories(int PlanetID)
		{
			return FactoryModule.GetFactories(PlanetID).ToArray();
		}

		public Stack GetStack(int StackID)
		{
			return StackModule.GetStack(StackID);
		}

		public Stack[] GetStacks(int FactoryID)
		{
			return StackModule.GetStacks(FactoryID).ToArray();
		}

		public ResourceType GetResourceType(int ResourceID)
		{
			return ResourceModule.GetResourceType(ResourceID);
		}
		public ResourceType[] GetResourceTypes()
		{
			return ResourceModule.GetResourceTypes().ToArray();
		}

		public FactoryType GetFactoryType(int FactoryTypeID)
		{
			return FactoryTypeModule.GetFactoryType(FactoryTypeID);
		}
		public FactoryType[] GetFactoryTypes()
		{
			return FactoryTypeModule.GetFactoryTypes().ToArray();
		}

		public Material GetMaterial(int MaterialID)
		{
			return MaterialModule.GetMaterial(MaterialID);
		}

		public Material[] GetMaterials(int FactoryID)
		{
			return MaterialModule.GetMaterials(FactoryID).ToArray();
		}





	}
}
