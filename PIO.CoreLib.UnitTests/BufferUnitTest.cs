using PIO.CoreLib.Exceptions;

namespace PIO.CoreLib.UnitTests
{
	[TestClass]
	public class BufferUnitTest
	{
		[TestMethod]
		public void IsValidShouldReturnTrue()
		{
			Buffer buffer;

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2) };
			Assert.IsTrue(buffer.IsValid);

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);
		}

		[TestMethod]
		public void IsValidShouldReturnFalse()
		{
			Buffer buffer;

			// negative InRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = -1, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsFalse(buffer.IsValid);

			// negative OutRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = -1, Capacity = 100, Usage = 100 };
			Assert.IsFalse(buffer.IsValid);

			// negative Capacity
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = -100, Usage = 100 };
			Assert.IsFalse(buffer.IsValid);

			// negative Usage
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = -100 };
			Assert.IsFalse(buffer.IsValid);

			// negative Space left
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = 200 };
			Assert.IsFalse(buffer.IsValid);


		}
		[TestMethod]
		public void InternalRateShouldReturnCorrectValue()
		{
			Buffer buffer;

			// Negative InternalRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 2, Capacity = 100, Usage = 0 };
			Assert.AreEqual(-1f, buffer.InternalRate);

			// Positive InternalRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 2, OutRate = 1, Capacity = 100, Usage = 0 };
			Assert.AreEqual(1f, buffer.InternalRate);

			// Positive InternalRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.AreEqual(1f, buffer.InternalRate);

			// Null InternalRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = 0 };
			Assert.AreEqual(0f, buffer.InternalRate);
		}

		[TestMethod]
		public void SpaceLeftShouldReturnCorrectValue()
		{
			Buffer buffer;


			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.AreEqual(100f, buffer.SpaceLeft);

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 0, Capacity = 100, Usage = 50 };
			Assert.AreEqual(50, buffer.SpaceLeft);

			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 0, Capacity = 100, Usage = 100 };
			Assert.AreEqual(0f, buffer.SpaceLeft);

		}
		[TestMethod]
		public void GetETAShouldThrowExceptionIfBufferIsInvalid()
		{
			Buffer buffer;


			// Negative InRate
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = -1, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsFalse(buffer.IsValid);
			Assert.ThrowsException<PIOInvalidBufferStateException>(() => buffer.GetETA());
		}

		[TestMethod]
		public void GetETAShouldReturnInfinity()
		{
			Buffer buffer;

			// internal rate is zero
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(float.MaxValue, buffer.GetETA());
		}

		[TestMethod]
		public void GetETAShouldReturnZero()
		{
			Buffer buffer;

			// cannot consume more
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 0 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(0f, buffer.GetETA());


			// cannot store more
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(0f, buffer.GetETA());

		}

		[TestMethod]
		public void GetETAShouldReturnValidValue()
		{
			Buffer buffer;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(100f, buffer.GetETA());

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(50f, buffer.GetETA());

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(50f, buffer.GetETA());

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(100f, buffer.GetETA());

		}

		[TestMethod]
		public void GetCapacityAtShouldThrownAnExceptionIsCycleIsNegative()
		{
			Buffer buffer;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.IsTrue(buffer.IsValid);
			Assert.ThrowsException<PIOInvalidParameterException>(()=> buffer.GetCapacityAt(-1));
		}

		[TestMethod]
		public void GetCapacityAtShouldThrownAnExceptionIsCycleIsGreaterThanETA()
		{
			Buffer buffer;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.IsTrue(buffer.IsValid);
			Assert.ThrowsException<PIOInvalidParameterException>(() => buffer.GetCapacityAt(110));
		}

		[TestMethod]
		public void GetCapacityAtShouldReturnValidValue()
		{
			Buffer buffer;

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 0 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(10f, buffer.GetCapacityAt(10));

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(60f, buffer.GetCapacityAt(10));

			// buffer is filling
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 1, OutRate = 0, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(100f, buffer.GetCapacityAt(50));

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 50 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(40f, buffer.GetCapacityAt(10));

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(90f, buffer.GetCapacityAt(10));

			// buffer is emptying
			buffer = new Buffer() { ID = new BufferID(1), ConnectorID = new ConnectorID(2), InRate = 0, OutRate = 1, Capacity = 100, Usage = 100 };
			Assert.IsTrue(buffer.IsValid);
			Assert.AreEqual(0f, buffer.GetCapacityAt(100));

		}



	}

}