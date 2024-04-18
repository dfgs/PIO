using PIO.CoreLib.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace PIO.CoreLib
{
	public class Buffer:PIOData<BufferID>, IBuffer
	{
		
		public required ConnectorID ConnectorID
		{
			get;
			set;
		}

		public float InRate
		{
			get;
			set;
		}
		public float OutRate
		{
			get;
			set;
		}

		public float InternalRate
		{
			get => InRate - OutRate;
		}
		public float Capacity
		{
			get;
			set;
		}

		public float InitialUsage
		{
			get;
			set;
		}

		public float InitialSpaceLeft
		{
			get => Capacity - InitialUsage;
		}

		public bool IsValid
		{
			get => (InRate >= 0) && (OutRate >= 0)  && (Capacity >= 0) && (InitialUsage >= 0) && (InitialUsage <= Capacity);
		}

		public Buffer()
		{

		}

		[SetsRequiredMembers]
		public Buffer(BufferID ID, ConnectorID ConnectorID)
		{
			this.ID = ID;
			this.ConnectorID = ConnectorID;
		}

		public float GetETA()
		{
			if (!IsValid) throw new PIOInvalidBufferStateException();
			if (InternalRate == 0) return float.MaxValue;

			
			if (InternalRate<0)
			{
				if (InitialUsage == 0) return 0;
				return InitialUsage / -InternalRate;
			}
			else
			{
				if (InitialUsage == Capacity) return 0;
				return InitialSpaceLeft / InternalRate;
			}

		}

		public float GetUsageAt(float Cycle)
		{
			if (!IsValid) throw new PIOInvalidBufferStateException();
			if (Cycle<0) throw new PIOInvalidParameterException(nameof(Cycle));
			if (Cycle > GetETA()) throw new PIOInvalidParameterException(nameof(Cycle));

			return InitialUsage +InternalRate*Cycle;

		}
		public void Update(float Cycle)
		{
			if (!IsValid) throw new PIOInvalidBufferStateException();
			if (Cycle < 0) throw new PIOInvalidParameterException(nameof(Cycle));
			if (Cycle > GetETA()) throw new PIOInvalidParameterException(nameof(Cycle));

			InitialUsage += InternalRate * Cycle;
		}



	}
}
