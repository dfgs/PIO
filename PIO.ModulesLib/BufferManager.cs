using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;

namespace PIO.ModulesLib
{
	public class BufferManager : PIOModule, IBufferManager
	{

		public BufferManager(ILogger Logger, IDataSource DataSource) : base(Logger, DataSource)
		{
		}


		public IBuffer[]? GetBuffers()
		{
			IBuffer[]? buffers = null;

			LogEnter();
			Log(LogLevels.Debug, $"Trying to get buffers");
			if (!Try(() => DataSource.GetBuffers()).Then(result => buffers = result.ToArray()).OrAlert($"Failed to get buffers")) return null;
			return buffers;
		}
		public IBuffer? GetBuffer(BufferID BufferID)
		{
			IBuffer? buffer = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Buffer ID {BufferID}] Trying to get buffer");
			if (!Try(() => DataSource.GetBuffer(BufferID)).Then(result => buffer = result).OrAlert($"[Buffer ID {BufferID}] Failed to get buffer")) return null;
			return buffer;

		}
		public IBuffer? GetBuffer(ConnectorID ConnectorID)
		{
			IBuffer? buffer = null;

			LogEnter();
			Log(LogLevels.Debug, $"[Connector ID {ConnectorID}] Trying to get buffer");
			if (!Try(() => DataSource.GetBuffer(ConnectorID)).Then(result => buffer = result).OrAlert($"[Connector ID {ConnectorID}] Failed to get buffer")) return null;
			return buffer;

		}

		public bool IsBufferValid(IBuffer Buffer)
		{
			LogEnter();
			if (Buffer == null)
			{
				Log(LogLevels.Fatal, $"Parameter {nameof(Buffer)} cannot be null");
				return false;
			}

			return Buffer.IsValid;
		}
		public bool UpdateBuffer(IBuffer Buffer,float Cycle)
		{
			LogEnter();

			if (Buffer == null)
			{
				Log(LogLevels.Fatal, $"Parameter {nameof(Buffer)} cannot be null");
				return false;
			}

			if (!IsBufferValid(Buffer))
			{
				Log(LogLevels.Error, $"[Buffer ID {Buffer.ID}] Buffer has invalid state");
				return false;
			}

			if (Cycle < 0)
			{
				Log(LogLevels.Error, $"[Buffer ID {Buffer.ID}] Cycle value is invalid");
				return false;
			}

			if (Cycle > Buffer.GetETA())
			{
				Log(LogLevels.Error, $"[Buffer ID {Buffer.ID}] Cycle value is invalid");
				return false;
			}

			Log(LogLevels.Debug, $"[Buffer ID {Buffer.ID}] Trying to update buffer");
			return Try(() => Buffer.Update(Cycle)).OrAlert($"[Buffer ID {Buffer.ID}] Failed to update buffer");
		}


	}
}
