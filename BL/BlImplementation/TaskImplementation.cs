using BlApi;
using BO;
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
        //IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll();
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        return from task in tasks
               //let depend = dependencies.Where(x => x!.IdDependentTask == task.Id)
               let worker = workers.First(x => x!.Id == task.WorkerId)
               let status = CalculateStatus(task)
               select new BO.Task()
               {
                   Id = task.Id,
                   Alias = task.Alias,
                   TaskDescription = task.TaskDescription,
                   Difficulty = (BO.Rank)task.Difficulty,
                   StatusTask = status,
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
                   DependencyTasks = getPriviousTask(task).ToList()//לבדוק אם מחזיר רשימה של משימות קודמות או של תלויות
                   //DependencyTasks = (from d in depend
                   //                   select new BO.TaskInList()
                   //                   {
                   //                       Id = task.Id,
                   //                       Alias = task.Alias,
                   //                       Description = task.TaskDescription,
                   //                       StatusTask = status
                   //                   }).ToList()
               };
    }

    public void RemoveTask(int id)
    {
        DO.Task? task=_dal.Task.Read(id);
        if(task == null) 
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} dosent exist");
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
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    public BO.Task Read(int id)
    {
        DO.Task? task=_dal.Task.Read(id);
        if (task==null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} dosent exist");
        }
        //IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll();
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        //var depend = dependencies.Where(x => x!.IdDependentTask == task!.Id);
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
            DependencyTasks = getPriviousTask(task).ToList()//לבדוק אם מחזיר רשימה של משימות קודמות או של תלויות
            //DependencyTasks = (from d in depend
            //                   select new BO.TaskInList()
            //                          {
            //                              Id = task.Id,
            //                              Alias = task.Alias,
            //                              Description = task.TaskDescription,
            //                              StatusTask = status
            //                          }).ToList()
       };
    }

    private void CheckBeginTask(int id, DateTime? date)
    {
        DO.Task task = _dal.Task.Read(id)!;
        var result = from BO.Task t in getPriviousTask(task)
                     where (t.BeginWork == null || date <= t.DeadLine)
                     select t;
        if (result.Count() > 0)
            throw new BO.BLcantUpdateTask("this date cant be update");
        if (getPriviousTask(task) == null && IBl.ProjectStartDate == null && date <= IBl.ProjectStartDate)
            throw new BO.BLcantUpdateTask("this date cant be update");
    }
    private IEnumerable<BO.TaskInList> getPriviousTask(DO.Task task)
    {
        return from DO.Dependency depend in _dal.Dependency.ReadAll(x => x.IdDependentTask == task.Id)
               select new BO.TaskInList
               {
                   Id = depend.IdPreviousTask,
                   Alias = _dal.Task.Read(depend.IdPreviousTask)!.Alias,
                   Description = _dal.Task.Read(depend.IdPreviousTask)!.TaskDescription,
                   StatusTask = CalculateStatus(_dal.Task.Read(depend.IdPreviousTask)!)
               };
    }
    private void CheckTaskForWorker(BO.Task task)
    {
        DO.Task oldTask=_dal.Task.Read(task.Id)!;
        if(oldTask != null && oldTask.WorkerId != task.WorkerId)
            throw new BO.BlcantAssignWorker("the task is already assigned to an worker");
        if (getPriviousTask(oldTask!).Where(x => x.StatusTask != BO.Status.Done).Any())
            throw new BO.BlcantAssignWorker("cant assign worker for this task");
        if(task.WorkerId is int idWorker)
        {
            DO.Worker worker = _dal.Worker.Read(idWorker)!;
            if (task.Difficulty > (BO.Rank)((int)worker.RankWorker))
                throw new BlcantAssignWorker("the worker cant do this task");
        }
    }
    public void UpdateTask(BO.Task task)
    {
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alias of task");
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            if (task.WorkerId != null)
                throw new BO.BLcantUpdateTask("Cant update worker on task in the planning level");
            if (task.BeginWork!=null)
                throw new BO.BLcantUpdateTask("Cant update planning startDate of task in the planning level");
        }
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
        {
            if (task.TimeTask != null)
                throw new BO.BLcantUpdateTask("Cant update during time of task in the planning level");
            if (task.BeginTask != null)
                throw new BO.BLcantUpdateTask("Cant update startDate of task in the planning level");
        }
        if (_dal.Worker.Read(x => x.Id == task.WorkerId) == null)
            throw new BO.BlDoesNotExistException($"Worker with ID={task.WorkerId} dosent exist");
        DO.Task newTask = new DO.Task(task.Id, (DO.Rank)task.Difficulty, task.WorkerId, task.TaskDescription, false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
        task.StatusTask = CalculateStatus(newTask);
        task.DependencyTasks = getPriviousTask(newTask).ToList(); 
        try
        {
            CheckTaskForWorker(task);
            CheckBeginTask(task.Id, task.BeginTask);
            _dal.Task.Update(newTask);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }
    private BO.Status CalculateStatus(DO.Task task)
    {
        var dateTimeNow=DateTime.Now;
        return task switch
        {
            DO.Task t when t.WorkerId is null => BO.Status.Unscheduled,
            DO.Task t when t.BeginTask > dateTimeNow => BO.Status.Scheduled,
            DO.Task t when t.EndWorkTime > dateTimeNow => BO.Status.OnTrack,
            _ => BO.Status.Done,
        };
        //return task.BeginWork is null? BO.Status.Unscheduled :
        //       task.BeginTask is null ? BO.Status.Scheduled :
        //       task.

    }
}
