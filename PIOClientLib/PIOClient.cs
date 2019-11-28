using LogLib;
using ModuleLib;
using NetORMLib;
using PIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


		protected abstract Row<Planet> OnGetPlanet(int PlanetID);
		public Row<Planet> GetPlanet(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanet(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get planet", ex));
		}

		protected abstract IEnumerable<Row<Planet>> OnGetPlanets();
		public IEnumerable<Row<Planet>> GetPlanets()
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetPlanets()).OrThrow((ex) => new PIOClientException("Failed to get planets", ex));
		}

		protected abstract IEnumerable<Row<Factory>> OnGetFactories(int PlanetID);
		public IEnumerable<Row<Factory>> GetFactories(int PlanetID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetFactories(PlanetID)).OrThrow((ex) => new PIOClientException("Failed to get factories", ex));
		}

		protected abstract Row<Factory> OnBuildFactory(int PlanetID, int FactoryTypeID);
		public Row<Factory> BuildFactory(int PlanetID,int FactoryTypeID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnBuildFactory(PlanetID,FactoryTypeID)).OrThrow((ex) => new PIOClientException("Failed to build factory", ex));
		}

		protected abstract IEnumerable<Row<Stack>> OnGetStacks(int FactoryID);
		public IEnumerable<Row<Stack>> GetStacks(int FactoryID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetStacks(FactoryID)).OrThrow((ex) => new PIOClientException("Failed to get stacks", ex));
		}

		protected abstract Row<Task> OnGetTask(int TaskID);
		public Row<Task> GetTask(int TaskID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetTask(TaskID)).OrThrow((ex) => new PIOClientException("Failed to get task", ex));
		}

		protected abstract Row<State> OnGetState(int StateID);
		public Row<State> GetState(int StateID)
		{
			LogEnter();
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return Try(() => OnGetState(StateID)).OrThrow((ex) => new PIOClientException("Failed to get state", ex));
		}



	}
}
