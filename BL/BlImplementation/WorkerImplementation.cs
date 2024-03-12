
namespace BlImplementation;
using BlApi;
using BO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// WorkerImplementation class implements the IWorker interface and provides methods for CRUD operations on workers
internal class WorkerImplementation : IWorker
{

    private readonly IBl _bl;
    internal WorkerImplementation(IBl bl) => _bl = bl;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    // Private field for accessing data layer
    private DalApi.IDal _dal=DalApi.Factory.Get;
    // Method to add a new worker
    public int AddWorker(BO.Worker worker)
    {
        // Check for invalid input: ID
        if (worker.Id <= 0)//If the id isnt correct
            throw new BO.BlInValidInputException("Invalid ID of worker");
        if (worker.Name == "")//If the name is empty
            throw new BO.BlInValidInputException("Invalid name of worker");
        // Check for invalid input: HourPrice
        if (worker.HourPrice <= 0)//If the price is negetive
            throw new BO.BlInValidInputException("Invalid price of worker");
        // Check for invalid input: Email
        if (!new EmailAddressAttribute().IsValid(worker.Email))//If the email is not correct
            throw new BO.BlInValidInputException("Invalid email of worker");
        // Check if a task can be assigned during the planning project stage
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning && worker.CurrentTask!=null)
            throw new BO.BlplanningStatus("You cant assign a task to an engineer during the planning project stage");
        try
        {
            // Create a new worker object and add it to the data layer
            DO.Worker newWorker = new DO.Worker(worker.Id,(DO.Rank)((int)worker.RankWorker),worker.HourPrice,worker.Name,worker.Email);//Building a worker with the data of the worker that the function got
            int id=_dal.Worker.Create(newWorker);//Adding the worker to the data
            return id;
        }
        catch(DO.DalAlreadyExistsException d) 
        {
            throw new BO.BlAlreadyExistsException(d.Message);
        } 
    }

    // Method to read workers with optional filter
    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null)//להוסיף filter
    {
        // Retrieve workers and tasks from the data layer
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        IEnumerable<DO.Task?> tasks=_dal.Task.ReadAll();
        // Query to map DO.Worker to BO.Worker with optional filtering
        var result = from worker in workers
               let task = tasks.FirstOrDefault(x => x!.WorkerId == worker.Id)
               let w = new BO.Worker()
               {
                   Id = worker.Id,
                   Name = worker.Name,
                   HourPrice = worker.HourPrice,
                   RankWorker = (BO.Rank)worker.RankWorker,
                   Email = worker.Email,
                   CurrentTask = new BO.TaskOnWorker
                   {
                       Id = worker.Id,
                       Name = worker.Name,
                   }
               }
               where filter is null ? true : filter(w)
               select w;
        // Order the list of workers by their name
        var orederWorkers = result.OrderBy(x => x.Name);//order the list of the workers by their name
        return orederWorkers;
    }
    // Method to remove a worker
    public void RemoveWorker(int id)
    {
        try
        {
            // Read the worker from the data layer
            BO.Worker? worker = Read(id);
            if(worker==null)
                throw new BO.BlDoesNotExistException($"Worker with ID={id} dosent exist");
            // Check if the worker is involved in any tasks
            IEnumerable<DO.Task> taskForWorker = _dal.Task.ReadAll(x => x.WorkerId == id);
            if (taskForWorker != null)
            {
                //We checkInvalid if the worker in the middle of the task or that he finish the task, if so we cant delete the worker
                bool taskInTheMiddle = taskForWorker.Where(x => (x.BeginTask < DateTime.Now && x.EndWorkTime == null)).Any();
                if (taskInTheMiddle)
                    throw new BlCantRemoveObject("Cant delete worker that in the middle of a task");
                bool taskFinish = taskForWorker.Where(x => (x.EndWorkTime < DateTime.Now)).Any();
                if (taskFinish)
                    throw new BlCantRemoveObject("Cant delete worker that finish the task");
                // Check if the project is in the execution stage
                if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
                    throw new BlCantRemoveObject("Cant delete worker in the execution stage");
            }
            // Delete the worker from the data layer
            _dal.Worker.Delete(id);
            if (_dal.User.Read(id) is not null)
            {
                _dal.User.Delete(id);
            }
        }
        catch (DO.DalDoesNotExistException e) 
        {
            throw new BO.BlDoesNotExistException(e.Message);
        }
    }
    // Method to update a worker
    public void UpdateWorker(BO.Worker worker)//לבדוק אם אפשר לעדכן משימה מוקצית
    {
        // Read the old worker from the data layer
        DO.Worker? oldworker = _dal.Worker.Read(worker.Id);
        // Check for invalid input: Name
        if (worker.Name == "")//If the name is not correct
            throw new BO.BlInValidInputException("Invalid name of worker");
        // Check for invalid input: HourPrice
        if (worker.HourPrice <= 0)//If the hourPrice is not correct
            throw new BO.BlInValidInputException("Invalid price of worker");
        // Check if the rank of the worker is valid
        if ((DO.Rank)worker.RankWorker > oldworker!.RankWorker)//If the rank of worker is not correct
            throw new BO.BlInValidInputException("Invalid rank of worker");
        // Check for invalid input: Email
        if (!new EmailAddressAttribute().IsValid(worker.Email))//If the email is not correct
            throw new BO.BlInValidInputException("Invalid email of worker");
        try
        {
            // Create a new worker object with updated details
            DO.Worker newWorker = new DO.Worker(worker.Id, (DO.Rank)((int)worker.RankWorker), worker.HourPrice, worker.Name, worker.Email);
            // Update the worker in the data layer
            _dal.Worker.Update(newWorker);
            // If the worker has a current task, update the task details
            if (worker.CurrentTask != null)
            {
                DO.Task? task = _dal.Task.Read(worker.CurrentTask!.Id);
                DO.Task newTask = new DO.Task(task!.Id, task.Difficulty, worker.Id, task.TaskDescription, task.MileStone, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
                _dal.Task.Update(newTask);
            }
        }
        catch (DO.DalDoesNotExistException d)
        {
            throw new BO.BlDoesNotExistException(d.Message);
        }
    }

    // Method to read a worker by ID
    public BO.Worker Read(int id)
    {
        // Read the worker from the data layer
        DO.Worker? worker = _dal.Worker.Read(id);
        // Throw an exception if the worker doesn't exist
        if (worker == null)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={id} dosent exist");
        }
        // Get the current task of the worker
        TaskOnWorker? taskOnWorker = GetCurrentTaskOfWorker(id);
        // Return a BO.Worker object with the worker details
        return new BO.Worker()
        {
            Id = worker.Id,
            Name = worker.Name,
            Email = worker.Email,
            RankWorker = (BO.Rank)((int)worker.RankWorker),
            HourPrice = worker.HourPrice,
            CurrentTask = taskOnWorker
        };
    }
    // Helper method to get the current task of a worker
    public BO.TaskOnWorker? GetCurrentTaskOfWorker(int id)//function that return the current task of the worker with is that she got
    {
        // Retrieve the current task of the worker from the data layer
        DO.Task? task = _dal.Task.ReadAll(x => x.WorkerId == id).Where(x => x!.BeginTask != null&&x.EndWorkTime is null).FirstOrDefault();
        // Return null if the worker doesn't have a current task
        if (task == null)
            return null;
        // Return a BO.TaskOnWorker object with the current task details
        return new BO.TaskOnWorker()
        {
            Id = task!.Id,
            Name = task.Alias
        };
    }
    // Method to clear workers from the data layer
    public void clear()
    {
        _dal.Worker.clear();
    }

    // Method to get workers grouped by rank
    public IEnumerable<BO.Worker> RankGroups(int rank)
    {
        // Group workers by rank
        var groupWorker = _dal.Worker.ReadAll().GroupBy(w => (int)w.RankWorker);
        // Retrieve workers with the specified rank
        var groupw = groupWorker.FirstOrDefault(g => g.Key == rank);
        // Throw an exception if no workers with the specified rank are found
        if (groupw == null)
            throw new BO.BlDoesNotExistException("there is no such a worker");
        // Return a collection of BO.Worker objects with workers of the specified rank
        return from DO.Worker doworker in groupw
               select new BO.Worker()
               {
                   Id = doworker.Id,
                   Name = doworker.Name,
                   HourPrice = doworker.HourPrice,
                   Email = doworker.Email,
                   RankWorker = (BO.Rank)doworker.RankWorker,
                   CurrentTask = GetCurrentTaskOfWorker(doworker.Id)
               };
    }

    public IEnumerable<TaskInList> GetAssociatedTasksForWorker(int id)
    {
        IEnumerable<BO.TaskInList> tasks = s_bl.Task.ReadTaskInList();
        var result = from task in tasks
                     let t = s_bl.Task.Read(task.Id)
                     where t.Worker != null && t.Worker.Id == id && t.BeginTask == null
                     select task;
        return result;
    }

    //public bool CheckUser(User user)
    //{
    //    DO.User dUser = new()
    //    {
    //        UserName = user.UserName,
    //        Password=user.password
    //    };
    //    return _dal.Worker.Check(dUser);
    //}

    //public void AddUser(User user)
    //{
    //    DO.User dUser = new()
    //    {
    //        UserName = user.UserName,
    //        Password=user.password
    //    };
    //     _dal.Worker.AddUser(dUser);
    //}

    //public void RemoveUser(User user)
    //{
    //    DO.User dUser = new()
    //    {
    //        UserName = user.UserName,
    //        Password = user.password
    //    };
    //    _dal.Worker.RemoveUser(dUser);
    //}
}
