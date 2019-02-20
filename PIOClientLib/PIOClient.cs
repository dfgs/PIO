using LogLib;
using ModuleLib;
using NetORMLib;
using PIOServerLib.Rows;
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


		protected abstract PlanetRow OnGetPlanet(int PlanetID);
		public PlanetRow GetPlanet(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanet(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get planet", ex));
		}

		protected abstract IEnumerable<PlanetRow> OnGetPlanets();
		public IEnumerable<PlanetRow> GetPlanets()
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanets()).OrThrow((ex) => new PIOClientException("Failed to get planets", ex));
		}

		protected abstract IEnumerable<FactoryRow> OnGetFactories(int PlanetID);
		public IEnumerable<FactoryRow> GetFactories(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetFactories(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get factories", ex));
		}

		protected abstract FactoryRow OnBuildFactory(int PlanetID, int FactoryTypeID);
		public FactoryRow BuildFactory(int PlanetID,int FactoryTypeID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnBuildFactory(PlanetID,FactoryTypeID)).OrThrow((ex) => new PIOClientException("Failed to build factory", ex));
		}

		protected abstract IEnumerable<StackRow> OnGetStacks(int FactoryID);
		public IEnumerable<StackRow> GetStacks(int FactoryID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetStacks(FactoryID)).OrThrow((ex) => new PIOClientException("Failed to get stacks", ex));
		}

		protected abstract TaskRow OnGetTask(int TaskID);
		public TaskRow GetTask(int TaskID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetTask(TaskID)).OrThrow((ex) => new PIOClientException("Failed to get task", ex));
		}

		protected abstract StateRow OnGetState(int StateID);
		public StateRow GetState(int StateID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetState(StateID)).OrThrow((ex) => new PIOClientException("Failed to get state", ex));
		}



	}
}
