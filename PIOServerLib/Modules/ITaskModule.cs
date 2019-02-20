using PIOServerLib.Rows;

namespace PIOServerLib.Modules
{
	public interface ITaskModule:IDatabaseModule
	{
		TaskRow GetTask(int TaskID);
		//IEnumerable<Row> GetTasks(int FactoryID);
		//void SetTask(int FactoryID, int TaskID);
	}
}
