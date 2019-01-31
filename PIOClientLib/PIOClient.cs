using LogLib;
using ModuleLib;
using NetORMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOClientLib
{
	public abstract class PIOClient : Module, IPIOClient
	{
		public bool IsConnected
		{
			get;
			private set;
		}

		public PIOClient(ILogger Logger) : base( Logger)
		{
		}

		protected abstract void OnConnect();
		public void Connect()
		{
			LogEnter();

			if (IsConnected) return;
			Try(OnConnect).OrThrow((ex) => new PIOClientException("Failed to connect", ex));
			IsConnected = true;
		}

		protected abstract void OnDisconnect();
		public void Disconnect()
		{
			LogEnter();
			if (!IsConnected) return;
			Try(OnDisconnect).OrThrow((ex) => new PIOClientException("Failed to disconnect", ex));
			IsConnected = false;
		}


		protected abstract Row OnGetPlanet(int PlanetID);
		public Row GetPlanet(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanet(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get planet", ex));
		}

		protected abstract IEnumerable<Row> OnGetPlanets();
		public IEnumerable<Row> GetPlanets()
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanets()).OrThrow((ex) => new PIOClientException("Failed to get planets", ex));
		}

		protected abstract IEnumerable<Row> OnGetFactories(int PlanetID);
		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetFactories(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get factories", ex));
		}

		protected abstract Row OnBuildFactory(int PlanetID, int FactoryTypeID);
		public Row BuildFactory(int PlanetID,int FactoryTypeID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnBuildFactory(PlanetID,FactoryTypeID)).OrThrow((ex) => new PIOClientException("Failed to build factory", ex));
		}

		protected abstract IEnumerable<Row> OnGetStacks(int FactoryID);
		public IEnumerable<Row> GetStacks(int FactoryID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetStacks(FactoryID)).OrThrow((ex) => new PIOClientException("Failed to get stacks", ex));
		}

		protected abstract IEnumerable<Row> OnGetTasks(int FactoryID);
		public IEnumerable<Row> GetTasks(int FactoryID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetTasks(FactoryID)).OrThrow((ex) => new PIOClientException("Failed to get task", ex));
		}

		protected abstract Row OnGetState(int StateID);
		public Row GetState(int StateID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetState(StateID)).OrThrow((ex) => new PIOClientException("Failed to get state", ex));
		}



	}
}
