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
			if (IsConnected) return;
			TryOrThrow(OnConnect, (ex) => new PIOClientException("Failed to connect", ex));
			IsConnected = true;
		}

		protected abstract void OnDisconnect();
		public void Disconnect()
		{
			if (!IsConnected) return;
			TryOrThrow(OnDisconnect, (ex) => new PIOClientException("Failed to disconnect", ex));
			IsConnected = false;
		}

		protected abstract IEnumerable<Row> OnGetFactories(int PlanetID);
		public IEnumerable<Row> GetFactories(int PlanetID)
		{
			if (!IsConnected) throw new PIOClientException("Client is not connected",null);
			return TryGetOrThrow(()=>OnGetFactories(PlanetID), (ex) => new PIOClientException("Failed to get factories",ex));
		}

		protected abstract Row OnGetPlanet(int PlanetID);
		public Row GetPlanet(int PlanetID)
		{
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return TryGetOrThrow(() => OnGetPlanet(PlanetID), (ex) => new PIOClientException("Failed to get planet", ex));
		}

		protected abstract IEnumerable<Row> OnGetPlanets();
		public IEnumerable<Row> GetPlanets()
		{
			if (!IsConnected) throw new PIOClientException("Client is not connected", null);
			return TryGetOrThrow(() => OnGetPlanets(), (ex) => new PIOClientException("Failed to get planets", ex));
		}

		
	}
}
