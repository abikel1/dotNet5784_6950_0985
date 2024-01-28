

using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void AddTask(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlInValidInputException("Invalid ID of task");
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alisa of task");

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
