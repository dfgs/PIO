using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PIO.WebServiceLib
{
	[ServiceContract]
	public interface IPIOService
	{

		[OperationContract]
		Planet GetPlanet(int PlanetID);
		[OperationContract]
		Planet[] GetPlanets();
		[OperationContract]
		Factory GetFactory(int FactoryID);
		[OperationContract]
		Factory[] GetFactories(int PlanetID);
		[OperationContract]
		Stack GetStack(int StackID);
		[OperationContract]
		Stack[] GetStacks(int FactoryID);
		[OperationContract]
		ResourceType GetResourceType(int ResourceTypeID);
		[OperationContract]
		ResourceType[] GetResourceTypes();
		[OperationContract]
		FactoryType GetFactoryType(int FactoryTypeID);
		[OperationContract]
		FactoryType[] GetFactoryTypes();
		[OperationContract]
		Material GetMaterial(int MaterialID);
		[OperationContract]
		Material[] GetMaterials(int FactoryTypeID);

	}


}
