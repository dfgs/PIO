using LogLib;
using Moq;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;
using System.Globalization;

namespace PIO.ModulesLib.UnitTests
{
	[TestClass]
	public class BufferManagerUnitTest
	{
		[TestMethod]
		public void ConstructorShouldThrowExceptionIfParameterIsNull()
		{
#pragma warning disable CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.
			Assert.ThrowsException<ArgumentNullException>(() => new BufferManager(null));
#pragma warning restore CS8625 // Impossible de convertir un litt�ral ayant une valeur null en type r�f�rence non-nullable.

		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotGetBuffers()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Throws<InvalidOperationException>();

			bufferManager = new BufferManager(logger);
			result=bufferManager.Update(dataSource, 0);
			Assert.IsFalse(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "buffers"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfInvalidBufferIsDetected()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1=Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(12));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(false);

			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1]);


			bufferManager = new BufferManager(logger);
			result = bufferManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "invalid state", "ID 12"));
		}

		[TestMethod]
		public void UpdateShouldLogErrorIfCannotUpdateBuffer()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1 = Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(12));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(true);
			Mock.Get(buffer1).Setup(m => m.Update(It.IsAny<float>())).Throws<InvalidOperationException>();


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1]);

			bufferManager = new BufferManager(logger);
			result = bufferManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(1, logger.ErrorCount);
			Assert.IsTrue(logger.LogsContainKeyWords(LogLevels.Error, "update", "ID 12"));
		}

		[TestMethod]
		public void UpdateShouldUpdateBuffers()
		{
			IBufferManager bufferManager;
			IDataSource dataSource;
			IBuffer buffer1,buffer2,buffer3;
			DebugLogger logger;
			bool result;

			logger = new DebugLogger();

			buffer1 = Mock.Of<IBuffer>();
			Mock.Get(buffer1).Setup(m => m.ID).Returns(new BufferID(1));
			Mock.Get(buffer1).Setup(m => m.IsValid).Returns(true);

			buffer2 = Mock.Of<IBuffer>();
			Mock.Get(buffer2).Setup(m => m.ID).Returns(new BufferID(2));
			Mock.Get(buffer2).Setup(m => m.IsValid).Returns(true);

			buffer3 = Mock.Of<IBuffer>();
			Mock.Get(buffer3).Setup(m => m.ID).Returns(new BufferID(3));
			Mock.Get(buffer3).Setup(m => m.IsValid).Returns(true);


			dataSource = Mock.Of<IDataSource>();
			Mock.Get(dataSource).Setup(m => m.GetBuffers()).Returns([buffer1,buffer2,buffer3]);

			bufferManager = new BufferManager(logger);
			result = bufferManager.Update(dataSource, 0);
			Assert.IsTrue(result);
			Assert.AreEqual(0, logger.ErrorCount);
			Mock.Get(buffer1).Verify(m => m.Update(0), Times.Once);
			Mock.Get(buffer2).Verify(m => m.Update(0), Times.Once);
			Mock.Get(buffer3).Verify(m => m.Update(0), Times.Once);
		}


	}

}