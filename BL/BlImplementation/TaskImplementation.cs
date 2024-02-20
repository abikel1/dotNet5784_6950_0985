using BlApi;
using BO;
using System.Xml.Linq;

namespace BlImplementation;

// Internal implementation of the ITask interface
internal class TaskImplementation : ITask
{
    // Private field for accessing the data access layer
    private DalApi.IDal _dal = DalApi.Factory.Get;
    // Method to add a new task
    public void AddTask(BO.Task task)
    {
        // Validation checks before adding a task
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alisa of task");
        // Check project status before adding a task
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            // Additional checks if project is in the planning phase
            if (task.Worker != null)
                if (task.Worker != null)
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
        DO.Task newTask = (new DO.Task(task.Id, (DO.Rank)task.Difficulty, 0, task.TaskDescription, false, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product));
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
    }
    // Method to read tasks
    public IEnumerable<BO.TaskInList> ReadTasks(Func<BO.TaskInList, bool>? filter = null)
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
            DependencyTasks = getPriviousTask(task)
        };
    }

    private void CheckBeginTask(int id, DateTime? date)
    {
        // Read the task from the database
        DO.Task task = _dal.Task.Read(id)!;
        // Query previous tasks to check constraints on begin task date
        var result = from BO.Task t in getPriviousTask(task)
                     where (t.BeginWork == null || date <= t.DeadLine)
                     select t;
        // Check if any constraint violation exists and throw exception
        if (result.Count() > 0)
            throw new BO.BLcantUpdateTask("this date cant be update");
        // Check if project start date is defined and task has no previous tasks
        if (getPriviousTask(task) == null && BlApi.Factory.Get().GetStartProjectDate() == null && date <= BlApi.Factory.Get().GetStartProjectDate())
            throw new BO.BLcantUpdateTask("this date cant be update");
    }
    // Method to retrieve previous tasks based on dependencies
    private IEnumerable<BO.TaskInList> getPriviousTask(DO.Task task)
    {
           var result= from DO.Dependency depend in _dal.Dependency.ReadAll(x => x.IdDependentTask == task.Id)
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
    // Method to check constraints related to assigning a worker to a task
    private void CheckTaskForWorker(BO.Task task)
    {
        DO.Task oldTask = _dal.Task.Read(task.Id)!;
            if (oldTask != null && task.Worker!=null && oldTask.WorkerId != task.Worker!.Id)
                throw new BO.BlCantAssignWorker("the task is already assigned to an worker");
        if (getPriviousTask(oldTask!).Where(x => x.StatusTask != BO.Status.Done).Any())
            throw new BO.BlCantAssignWorker("cant assign worker for this task");
        if(task.Worker !=null)
        {
            if (task.Worker!.Id is int idWorker)
            {
                DO.Worker worker = _dal.Worker.Read(idWorker)!;
                if (task.Difficulty > (BO.Rank)((int)worker.RankWorker))
                    throw new BlCantAssignWorker("the worker cant do this task");
            }
        }
    }

    // Method to update a task
    public void UpdateTask(BO.Task task)//לבדוק אם מעדכנים עובד במשימה האם צריך לעדכן גם אצל העובד
    {
        // Check for invalid alias
        if (task.Alias == "")
            throw new BO.BlInValidInputException("Invalid alias of task");

        // Check project status for planning phase
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            if (task.Worker != null)
                throw new BO.BlplanningStatus("Cant update worker on task in the planning level");
            if (task.BeginWork != null)
                throw new BO.BlplanningStatus("Cant update planning startDate of task in the planning level");
        }

        // Check project status for execution phase
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
        {
            if (task.TimeTask != null)
                throw new BO.BlexecutionStatus("Cant update during time of task in the excution level");
            if (task.BeginTask != null)
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
        task.DependencyTasks = getPriviousTask(newTask).ToList();

        try
        {
            // Check constraints before updating the task
            CheckTaskForWorker(task);
            CheckBeginTask(task.Id, task.BeginTask);
            _dal.Task.Update(newTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    private BO.Status CalculateStatus(DO.Task task)
    {
        var dateTimeNow = DateTime.Now;
        return task switch
        {
            DO.Task t when t.WorkerId is null => BO.Status.Unscheduled,
            DO.Task t when t.BeginTask > dateTimeNow => BO.Status.Scheduled,
            DO.Task t when t.EndWorkTime > dateTimeNow => BO.Status.OnTrack,
            _ => BO.Status.Done,
        };
    }

    // Method to update the start dates of a task
    public void UpdateStartDates(int id, DateTime? startDate)
    {
        // Read the task from the database
        DO.Task? dotask = _dal.Task.Read(id);
        if(dotask==null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        // Retrieve previous tasks
        IEnumerable<TaskInList> previousTasks = getPriviousTask(dotask);
        // Check constraints related to updating start dates
        if ((previousTasks == null) && (startDate < BlApi.Factory.Get().GetStartProjectDate()))
            throw new BO.BlcanotUpdateStartdate("cant update start date because the start date is before the planning date of starting the project");
        else
        {
            var result = from p in previousTasks
                         let t = _dal.Task.Read(p.Id)
                         where t != null && t.BeginWork == null
                         select p;
            if (result != null)
                throw new BO.BlcanotUpdateStartdate("cant update the start date of the task because the task depent on tasks that dont have start date ");
            var result2 = from p in previousTasks
                          let t = _dal.Task.Read(p.Id)
                          where (t != null) && (startDate < t.EndWorkTime)
                          select p;
            if (result2 != null)
                throw new BO.BlcanotUpdateStartdate("cant update start date because the task depent on tasks that finish after the satart date");
        }
        // Update the start date of the task
        try
        {
            DO.Task updateTask = dotask with { BeginTask = startDate };
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
}
