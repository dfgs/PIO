using LogLib;
using ModuleLib;
using PIO.CoreLib;
using PIO.CoreLib.Exceptions;

namespace PIO.ModulesLib
{
	public class BufferManager : Module,IBufferManager
	{

		public BufferManager(ILogger Logger) : base(Logger)
		{
		}

		public bool Update(IDataSource DataSource, float Cycle)
		{
			IBuffer[] buffers=[];

			LogEnter();

			Log(LogLevels.Information, "Retrieving all buffers");
			if (!Try(() => DataSource.GetBuffers()).Then(items => buffers = items.ToArray()).OrAlert("Failed to retrieve buffers")) return false;
			

			Log(LogLevels.Information, "Updating all buffers");
			foreach(IBuffer buffer in buffers)
			{
				if (!buffer.IsValid)
				{
					Log(LogLevels.Error, $"Buffer with ID {buffer.ID} has invalid state");
					continue;
				}
				// update buffer
				if (!Try(()=>buffer.Update(Cycle)).OrAlert($"Failed to update buffer with ID {buffer.ID}")) continue;

				// clear rates
				buffer.InRate = 0;buffer.OutRate = 0;

			}

			return true;

		}


	}
}
