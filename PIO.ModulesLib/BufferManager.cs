using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;

namespace PIO.ModulesLib
{
	public class BufferManager : PIOModule,IBufferManager
	{

		public BufferManager(ILogger Logger,IDataSource DataSource) : base(Logger,DataSource)
		{
		}

		public bool Update( float Cycle)
		{
			IBuffer[] buffers=[];

			LogEnter();

			Log(LogLevels.Information, "Retrieving all buffers");
			if (!Try(() => DataSource.GetBuffers()).Then(items => buffers = items.ToArray()).OrAlert("Failed to retrieve buffers")) return false;
			

			Log(LogLevels.Information, "Updating all buffers");
			foreach(IBuffer buffer in buffers)
			{
				Log(LogLevels.Debug, $"[Buffer ID {buffer.ID}] Processing buffer");
				if (!buffer.IsValid)
				{
					Log(LogLevels.Error, $"[Buffer ID {buffer.ID}] Buffer has invalid state");
					continue;
				}
				// update buffer
				if (!Try(()=>buffer.Update(Cycle)).OrAlert($"[Buffer ID {buffer.ID}] Failed to update buffer")) continue;

				// clear rates
				Log(LogLevels.Debug, $"[Buffer ID {buffer.ID}] Clearing rates of buffer");
				buffer.InRate = 0;buffer.OutRate = 0;

			}

			return true;

		}


	}
}
