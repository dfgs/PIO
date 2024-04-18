using LogLib;
using ModuleLib;
using PIO.CoreLib;

namespace PIO.ModulesLib
{
	public class BufferManager : Module,IBufferManager
	{
		public BufferManager(ILogger Logger) : base(Logger)
		{
		}

		public void Update(IDataSource DataSource, float Cycle)
		{
			IBuffer[] buffers=[];

			LogEnter();

			Log(LogLevels.Information, "Retrieving all buffers");
			if (!Try(() => DataSource.GetBuffers()).Then(items => buffers = items.ToArray()).OrAlert("Failed to retrieve buffers")) return;
			

			Log(LogLevels.Information, "Updating all buffers");
			foreach(IBuffer buffer in buffers)
			{
				if (!buffer.IsValid)
				{
					Log(LogLevels.Error, $"Buffer with ID {buffer.ID} has invalid state");
					continue;
				}

				if (!Try(()=>buffer.Update(Cycle)).OrAlert($"Failed to update buffer with ID {buffer.ID}")) return;

			}

		}

	}
}
