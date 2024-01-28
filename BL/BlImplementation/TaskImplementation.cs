

using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddTask(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Task> ReadTasks(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void RemoveTask(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task TaskDetails(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateBeginTask(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public void UpdateTask(BO.Task task)
    {
        throw new NotImplementedException();
    }
}
