using BlApi;
using System.Security.Cryptography;

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
        task.DependencyTasks!.ForEach(t => _dal.Dependency.Create(new DO.Dependency()
        {
            IdDependentTask = t.Id,
            IdPreviousTask = task.Id,
        }));
 //       _dal.Task.Create(new DO.Task(task.Id,task.))
    }

    public IEnumerable<BO.Task> ReadTasks(Func<BO.Task, bool>? filter = null)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll();
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        return from task in tasks
               let depend = dependencies.Where(x => x!.IdDependentTask == task.Id)
               let worker = workers.First(x => x!.Id == task.WorkerId)
               let status = CalculateStatus(task)
               select new BO.Task()
               {
                   Id = task.Id,
                   Alias = task.Alias,
                   TaskDescription = task.TaskDescription,
                   Difficulty = (BO.Rank)task.Difficulty,
                   StatusTask=status,
                   WorkerId = task.WorkerId,
                   WorkerName = worker.Name,
                   TimeTask = task.TimeTask,
                   CreateTask = task.CreateTask,
                   BeginTask = task.BeginTask,
                   BeginWork = task.BeginWork,
                   DeadLine = task.DeadLine,
                   EndWorkTime = task.EndWorkTime,
                   Remarks = task.Remarks,
                   Product = task.Product,
                   DependencyTasks = (from d in depend
                                      select new BO.TaskInList()
                                      {
                                          Id = task.Id,
                                          Alias = task.Alias,
                                          Description = task.TaskDescription,
                                          StatusTask = status
                                      }).ToList()
               };
    }

    public void RemoveTask(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task Read(int id)
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
    private BO.Status CalculateStatus(BO.Task task)
    {
        return task.BeginWork is null? BO.Status.Unscheduled :
               task.BeginTask is null ? BO.Status.Scheduled :
               task.

    }
}
