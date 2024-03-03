using BlApi;
using BO;
using DalApi;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace BlImplementation;

// Internal implementation of the ITask interface
internal class TaskImplementation : BlApi.ITask
{

    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // Private field for accessing the data access layer
    private DalApi.IDal _dal = DalApi.Factory.Get;
    // Method to add a new task
    public int AddTask(BO.Task task)
    {
        // Validation checks before adding a task
        if (string.IsNullOrEmpty( task.Alias))
            throw new BO.BlInValidInputException("Invalid alias of task");
        if(task.TimeTask==null)
        {
            throw new BO.BlInValidInputException("Invalid time of task");
        }
        // Check project status before adding a task
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            // Additional checks if project is in the planning phase
            if (task.Worker!=null&& task.Worker!.Id !=0)
                throw new BO.BlplanningStatus("Cant update worker on task in the planning level");
            if (task.BeginWork != null)
                throw new BO.BlplanningStatus("Cant update planning startDate of task in the planning level");
        }

        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
        {
            // Check project status before adding a task during execution phase
            throw new BO.BlexecutionStatus("You cant add a task during execution");
        }
        // Create a new DO.Task object and calculate status
        DO.Task newTask = (new DO.Task(task.Id, (DO.Rank)task.Difficulty,task.Worker!=null? task.Worker!.Id : 0, task.TaskDescription, false, task.Alias, _bl.Clock, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product));
        task.StatusTask = CalculateStatus(newTask);
        // Add the task to the database
        int id =_dal.Task.Create(newTask);
        // If the task has dependency tasks, create dependencies in the database
        if (task.DependencyTasks!=null)
        {
            var x = from BO.TaskInList t in task.DependencyTasks
                    select new DO.Dependency()
                    {
                        Id = 0,
                        IdDependentTask = id,
                        IdPreviousTask = t.Id,
                    };
            var y = (from t in x
                    select _dal.Dependency.Create(t)).ToList();
        }
        return id;
    }
    // Method to read tasks
    public IEnumerable<BO.TaskInList> ReadTaskInList(Func<BO.TaskInList, bool>? filter = null)
    {
        // Read all tasks from the database and apply filter
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        return from task in tasks
               let boTask = new BO.TaskInList()
               {
                   Id = task.Id,
                   Alias = task.Alias,
                   Description = task.TaskDescription,
                   StatusTask = CalculateStatus(task)
               }
               where filter is null ? true : filter(boTask)
               select boTask;
    }

    // Method to remove a task
    public void RemoveTask(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        // Check if the task exists
        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} dosent exist");
        }
        // Check if the task has dependencies
        bool dependency = _dal.Dependency.ReadAll().Where(x => x!.IdPreviousTask == task.Id).Any();
        if (dependency)
        {
            throw new BO.BlCantRemoveObject("Cant remove task that with dependency tasks");
        }
        // Remove the task from the database
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    // Method to read a single task by ID
    public BO.Task Read(int id)
    {
        // Read a task from the database and map it to a BO.Task object
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} dosent exist");
        }
        // Map DO.Task to BO.Task
        return new BO.Task()
        {// Map task properties
            Id = task.Id,
            Alias = task.Alias,
            TaskDescription = task.TaskDescription,
            Difficulty = (BO.Rank)task.Difficulty,
            StatusTask = CalculateStatus(task!),
            Worker = _dal.Worker.Read(x => x.Id == task!.WorkerId) is DO.Worker worker ?
            new WorkerOnTask
            {
                Id = worker.Id,
                Name = worker.Name,
            } : null,
            TimeTask = task.TimeTask,
            CreateTask = task!.CreateTask,
            BeginTask = task!.BeginTask,
            BeginWork = task!.BeginWork,
            DeadLine = task!.DeadLine,
            EndWorkTime = task!.EndWorkTime,
            Remarks = task.Remarks,
            Product = task.Product,
            DependencyTasks = getPriviousTask(id)
        };
    }

    private void CheckBeginTask(int id, DateTime? date)
    {
        // Read the task from the database
        DO.Task task = _dal.Task.Read(id)!;
        // Query previous tasks to checkInvalid constraints on begin task date
        IEnumerable<BO.TaskInList> tasks = getPriviousTask(id);
        var result = from BO.TaskInList t in tasks
                     let dTask = _dal.Task.Read(t.Id)
                     where (dTask.BeginWork == null || date <= dTask.DeadLine)
                     select t;
        // Check if any constraint violation exists and throw exception
        if (result.Any())
            throw new BO.BLcantUpdateTask("this date cant be update");
        // Check if project start date is defined and task has no previous tasks
        if (getPriviousTask(id) == null && BlApi.Factory.Get().GetStartProjectDate() == null && date <= BlApi.Factory.Get().GetStartProjectDate())
            throw new BO.BLcantUpdateTask("this date cant be update");
    }
    // Method to retrieve previous tasks based on dependencies
    public IEnumerable<BO.TaskInList>? getPriviousTask(int id)
    {
       DO.Task? task = _dal.Task.Read(id);
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll(x => x.IdDependentTask == task.Id);
           var result= from DO.Dependency depend in dependencies
                   let deptask= _dal.Task.Read(depend.IdPreviousTask)!
                   select new BO.TaskInList
                   {
                       Id = depend.IdPreviousTask,
                       Alias = deptask.Alias,
                       Description = deptask.TaskDescription,
                       StatusTask = CalculateStatus(_dal.Task.Read(depend.IdPreviousTask)!)
                   };
        return result;
    }
    // Method to checkInvalid constraints related to assigning a worker to a task
    private void CheckTaskForWorker(BO.Task task)
    {
        DO.Task oldTask = _dal.Task.Read(task.Id)!;
            if (oldTask != null && task.Worker!=null && oldTask.WorkerId != 0)
                throw new BO.BlCantAssignWorker("the task is already assigned to an worker");
        if (getPriviousTask(oldTask!.Id)!.Where(x => x.StatusTask != BO.Status.Done).Any())
            throw new BO.BlCantAssignWorker("cant assign worker for this task");
        if(task.Worker !=null)
        {
            if (task.Worker!.Id is int idWorker)
            {
                DO.Worker worker = _dal.Worker.Read(idWorker)!;
                if (task.Difficulty > (BO.Rank)((int)worker.RankWorker))
                    throw new BlCantAssignWorker("the worker cant do this task bacause his rank is lower");
            }
        }
    }

    // Method to update a task
    public void UpdateTask(BO.Task task)//לבדוק אם מעדכנים עובד במשימה האם צריך לעדכן גם אצל העובד
    {
        BO.Task oldTask = Read(task.Id);
        // Check for invalid alias
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alias of task");

        // Check project status for planning phase
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            if (task.Worker != oldTask.Worker)
                throw new BO.BlplanningStatus("Cant update worker on task in the planning level");
            if (task.BeginWork != oldTask.BeginWork)
                throw new BO.BlplanningStatus("Cant update planning startDate of task in the planning level");
        }

        // Check project status for execution phase
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
        {
            if (task.TimeTask != oldTask.TimeTask)
                throw new BO.BlexecutionStatus("Cant update during time of task in the excution level");
            if (task.BeginTask != oldTask.BeginTask)
                throw new BO.BlexecutionStatus("Cant update startDate of task in the excution level");
        }

        // Check if assigned worker exists
        if (task.Worker is not null && _dal.Worker.Read(x => x.Id == task.Worker.Id) == null)
            throw new BO.BlDoesNotExistException($"Worker with ID={task.Worker.Id} dosent exist");
        // Create a new DO.Task object based on the updated task
        DO.Task newTask;
        if (task.Worker is null)
        {
            newTask = new DO.Task(task.Id, (DO.Rank)task.Difficulty,null, task.TaskDescription,
            false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask,
            task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
        }
        else
        {
           newTask = new DO.Task(task.Id, (DO.Rank)task.Difficulty, null, task.TaskDescription,
           false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask,
           task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
        }

        // Calculate and set the status of the updated task
        task.StatusTask = CalculateStatus(newTask);
        task.DependencyTasks = getPriviousTask(newTask.Id)!.ToList();

        try
        {
            // Check constraints before updating the task
            CheckTaskForWorker(task);
            if(task.BeginTask!=null)
            {
                CheckBeginTask(task.Id, task.BeginTask);
            }
            _dal.Task.Update(newTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    private BO.Status CalculateStatus(DO.Task task)
    {
        var dateTimeNow = _bl.Clock;
        return task switch
        {
            DO.Task t when t.BeginWork is null => BO.Status.Unscheduled,
            DO.Task t when t.WorkerId is 0 => BO.Status.Scheduled,
            DO.Task t when t.BeginTask is null=> BO.Status.OnTrack,
            DO.Task t when t.EndWorkTime is null=>BO.Status.Started,
            _ => BO.Status.Done,
        };
        //return task switch
        //{
        //    DO.Task t when t.WorkerId is null => BO.Status.Unscheduled,
        //    DO.Task t when t.BeginTask > dateTimeNow => BO.Status.Scheduled,
        //    DO.Task t when t.EndWorkTime > dateTimeNow => BO.Status.OnTrack,
        //    _ => BO.Status.Done,
        //};
    }

    // Method to update the start dates of a task
    public void UpdateStartDates(int id, DateTime? startDate)
    {
        // Read the task from the database
        DO.Task? dotask = _dal.Task.Read(id);
        if(dotask==null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        // Retrieve previous tasks
        IEnumerable<TaskInList> previousTasks = getPriviousTask(id)!;
        // Check constraints related to updating start dates
        if ((startDate < BlApi.Factory.Get().GetStartProjectDate()))
            throw new BO.BlcanotUpdateStartdate("cant update start date because the start date is before the planning date of starting the project");
            if (previousTasks.Count()!=0)
            {
                IEnumerable<TaskInList>? result = null;
                result = from p in previousTasks
                         let t = _dal.Task.Read(p.Id)
                         where t != null && t.BeginWork == null
                         select p;
                if (result.Count()!=0)
                    throw new BO.BlcanotUpdateStartdate("cant update the start date of the task because the task depent on tasks that dont have start date ");
                var result2 = from p in previousTasks
                              let t = _dal.Task.Read(p.Id)
                              where (t != null) && (startDate < t.EndWorkTime)
                              select p;
                if (result2.Count()!=0)
                    throw new BO.BlcanotUpdateStartdate("cant update start date because the task depent on tasks that finish after the start date");
            }
        // Update the start date of the task
        try
        {
            DO.Task updateTask = dotask with { BeginWork = startDate, DeadLine=startDate+dotask.TimeTask };
            _dal.Task.Update(updateTask);
        }
        catch(DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        }
    }

    // Method to clear tasks and dependencies
    public void clear()
    {
        _dal.Dependency.clear();
        _dal.Task.clear();
    }

    public IEnumerable<BO.Task> ReadTasks(Func<BO.Task, bool>? filter = null)
    {
        IEnumerable<BO.TaskInList> tasksI = ReadTaskInList();
        IEnumerable<BO.Task> tasks=from BO.TaskInList task in tasksI
                                   let t=Read(task.Id)
                                   where filter is null ? true : filter(t)
                                   select t;
        return tasks;
    }

    public IEnumerable<TaskInList> relevantTask(BO.Worker worker)
    {
        IEnumerable<BO.Task> tasks = ReadTasks();
        var result = from BO.Task task in tasks
                     where task.Worker == null && task.BeginWork != null && worker.RankWorker == task.Difficulty
                     select new BO.TaskInList()
                     {
                         Id = task.Id,
                         Alias = task.Alias,
                         Description = task.TaskDescription,
                         StatusTask = task.StatusTask
                     };
        return result;
    }

    public void autoSchedule()
    {
        List<BO.TaskInList> allNoStartDate = new List<BO.TaskInList>();
        IEnumerable<DO.Task> allTasks = _dal.Task.ReadAll();
        foreach (var task in allTasks)
        {
            if ((getPriviousTask(task.Id))!.Count() == 0)
            {
                UpdateStartDates(task.Id, _dal.GetStartProjectDate());
            }
        }
        allTasks = _dal.Task.ReadAll();
        while (allTasks.Any(t => t.BeginWork == null))
        {
            foreach (DO.Task task in allTasks)
            {
                IEnumerable<BO.TaskInList> tasksDependencies = getPriviousTask(task.Id)!;
                List<BO.TaskInList> noStartDate = new List<BO.TaskInList>();
                List<DO.Task> thereIsStartDate = new List<DO.Task>();
                foreach (BO.TaskInList taskList in tasksDependencies)
                {
                    DO.Task dep = _dal.Task.Read(taskList.Id)!;
                    if (dep.BeginWork == null)
                    {
                        noStartDate.Add(taskList);
                        allNoStartDate.Add(taskList);
                    }
                    else
                    {
                        thereIsStartDate.Add(dep);
                    }
                }
                if (noStartDate.Count() == 0 && task.BeginWork == null)
                {
                    var scheduledDate = thereIsStartDate.Max(dep => dep.BeginWork + dep.TimeTask);
                    UpdateStartDates(task.Id, (DateTime)scheduledDate!);
                    allTasks = _dal.Task.ReadAll();
                }
            }
        }
        ////Selects all tasks that have no previous tasks and updates the task's scheduled date to the project's start date
        //IEnumerable<DO.Task> tasks = _dal.Task.ReadAll();
        //foreach(var task in tasks)
        //{
        //    if(getPriviousTask(task.Id)==null)
        //    {
        //        UpdateStartDates(task.Id, BlApi.Factory.Get().GetStartProjectDate());
        //    }
        //}
        ////List<BO.Task> tasks = (from DO.Task task in _dal.Task.ReadAll()
        ////                              where getPriviousTask(task.Id) == null
        ////                              let t = task with { BeginWork = BlApi.Factory.Get().GetStartProjectDate() }
        ////                              select Read(t.Id)).ToList();
        ////Selects all tasks that do not have a scheduled date and sends to the function
        //List<BO.Task> tasks1 = (from DO.Task task in _dal.Task.ReadAll()
        //     where task.BeginWork== null
        //     select Read(task.Id)).ToList();
        //taskWithDepend(tasks1);
    }
    private void taskWithDepend(List<BO.Task> tasks)
    {
        if (tasks == null)
            return;
        else
        {
            List<BO.Task>? tasksWithScheduleDate = new List<BO.Task>();
            foreach(BO.Task task in tasks)
            {
                if(task.DependencyTasks!=null)
                {
                    if(!(from d in task.DependencyTasks//chack if all the dependency task have schedule date
                       let t=Read(d.Id)
                       where t.BeginWork == null
                       select t).Any())
                    {
                        DateTime? possibleDate = PossibleDate(task);
                        if(possibleDate!=null)
                        {
                            UpdateStartDates(task.Id, possibleDate);
                        }
                        tasksWithScheduleDate!.Add(task);
                    }
                }
            }
            //foreach(BO.Task ta in tasksWithScheduleDate!)
            //{
            //    if (tasks.FirstOrDefault(t => t.Id == ta.Id) != null)
            //        tasks.Remove(ta);
            //}
            //taskWithDepend(tasks);
        }
    }
    private DateTime? PossibleDate(BO.Task task)
    {
        var result = from BO.TaskInList d in task.DependencyTasks!
                     select Read(d.Id);
        return result.Max(t => (t.BeginWork+t.TimeTask));
    }
    public void AddTaskForWorker(int idWorker, int idTask)
    {
        BO.Worker worker = s_bl.Worker.Read(idWorker);
        BO.Task taskB =Read(idTask);
        CheckTaskForWorker(taskB);
        BO.Task task1 = new BO.Task()
        {
            Id = taskB.Id,
            Alias = taskB.Alias,
            TaskDescription = taskB.TaskDescription,
            Difficulty = (BO.Rank)taskB.Difficulty,
            StatusTask = taskB.StatusTask,
            Worker = new BO.WorkerOnTask()
            {
                Id = worker.Id,
                Name = worker.Name
            },
            TimeTask = taskB.TimeTask,
            CreateTask = taskB.CreateTask,
            BeginTask = taskB.BeginTask,
            BeginWork = taskB.BeginWork,
            DeadLine = taskB.DeadLine,
            EndWorkTime = taskB.EndWorkTime,
            Remarks = taskB.Remarks,
            Product = taskB.Product,
            DependencyTasks = taskB.DependencyTasks
        };
        UpdateTask(task1);
    }

    public IEnumerable<TaskInList> ReadPossibleDependencies(int id)
    {
        var result=from BO.TaskInList task in ReadTaskInList()
                   where _dal.Dependency.ReadAll(d=>d.IdDependentTask==id&&d.IdPreviousTask==task.Id).Count() ==0
                   select task;
        return result;
    }
}
