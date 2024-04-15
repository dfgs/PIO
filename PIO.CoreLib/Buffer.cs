using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib
{
	public class Buffer: IBuffer
	{
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

		public float Usage
		{
			get;
			set;
		}

		public float SpaceLeft
		{
			get => Capacity - Usage;
		}

		public bool IsValid
		{
			get => (InRate >= 0) && (OutRate >= 0)  && (Capacity >= 0) && (Usage >= 0) && (Usage <= Capacity);
		}

		public Buffer()
		{

		}

		public float GetETA()
		{
			if (!IsValid) throw new PIOInvalidBufferStateException();
			if (InternalRate == 0) return float.MaxValue;

			
			if (InternalRate<0)
			{
				if (Usage == 0) return 0;
				return Usage / -InternalRate;
			}
			else
			{
				if (Usage == Capacity) return 0;
				return SpaceLeft / InternalRate;
			}

		}

		public float GetCapacityAt(float Cycle)
		{
			if (!IsValid) throw new PIOInvalidBufferStateException();
			if (Cycle<0) throw new PIOInvalidParameterException(nameof(Cycle));
			if (Cycle > GetETA()) throw new PIOInvalidParameterException(nameof(Cycle));

			return Usage +InternalRate*Cycle;

		}



	}
}
