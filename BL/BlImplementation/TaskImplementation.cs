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
        //לבדוק אם צריך לחשב תאריכים
        _dal.Task.Create(new DO.Task(task.Id, (DO.Rank)task.Difficulty, task.WorkerId, task.TaskDescription, false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product));
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
        DO.Task? task=_dal.Task.Read(id);
        if(task == null) 
        {
            throw new BO.BlDoesNotExistException($"Task with ID={task!.Id} dosent exist");
        }
        bool dependency = _dal.Dependency.ReadAll().Where(x=>x!.IdDependentTask==task.Id).Any();
        if(dependency)
        {
            throw new BO.BlCantRemoveObject("Cant remove task that with dependency tasks");
        }
        try
        {
            _dal.Worker.Delete(id);
        }
        catch(Exception ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    public BO.Task Read(int id)
    {
        DO.Task? task=_dal.Task.Read(id);
        if (task==null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={task!.Id} dosent exist");
        }
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll();
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        var depend = dependencies.Where(x => x!.IdDependentTask == task!.Id);
        var worker = workers.First(x => x!.Id == task!.WorkerId);
        var status = CalculateStatus(task!);
        return new BO.Task()
        {
            Id = task.Id,
            Alias = task.Alias,
            TaskDescription = task.TaskDescription,
            Difficulty = (BO.Rank)task.Difficulty,
            StatusTask = status,
            WorkerId = task.WorkerId,
            WorkerName = worker!.Name,
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

    public void UpdateBeginTask(int id, DateTime date)
    {
    }

    public void UpdateTask(BO.Task task)
    {
        if (task!.Id <= 0)
            throw new BO.BlInValidInputException("Invalid ID of task");
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alisa of task");
        try
        {
            DO.Task newTask=new DO.Task(task.Id, (DO.Rank) task.Difficulty, task.WorkerId, task.TaskDescription, false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
            _dal.Task.Update(newTask);
        }
        catch(Exception ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }

    }
    private BO.Status CalculateStatus(DO.Task task)
    {
        return task.BeginWork is null? BO.Status.Unscheduled :
               task.BeginTask is null ? BO.Status.Scheduled :
               task.

    }
}
