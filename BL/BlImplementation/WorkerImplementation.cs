
namespace BlImplementation;
using BlApi;
using BO;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal=DalApi.Factory.Get;
    public void AddWorker(BO.Worker worker)
    {
        if (worker.Id <= 0)//If the id isnt correct
            throw new BO.BlInValidInputException("Invalid ID of worker");
        if (worker.Name == "")//If the name is empty
            throw new BO.BlInValidInputException("Invalid name of worker");
        if (worker.HourPrice <= 0)//If the price is negetive
            throw new BO.BlInValidInputException("Invalid price of worker");
        if(!new EmailAddressAttribute().IsValid(worker.Email))//If the email is not correct
            throw new BO.BlInValidInputException("Invalid email of worker");
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning && worker.CurrentTask!=null)
            throw new BO.BLcantUpdateTask("You cant assign a task to an engineer during the planning project stage");
        try
        {
            DO.Worker newWorker = new DO.Worker(worker.Id,(DO.Rank)((int)worker.RankWorker),worker.HourPrice,worker.Name,worker.Email);//Building a worker with the data of the worker that the function got
            _dal.Worker.Create(newWorker);//Adding the worker to the data
        }
        catch(DO.DalAlreadyExistsException d) 
        {
            throw new BO.BlAlreadyExistsException(d.Message);
        } 
    }

    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null)//להוסיף filter
    {
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        IEnumerable<DO.Task?> tasks=_dal.Task.ReadAll();
        return from worker in workers
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
    }

    public void RemoveWorker(int id)
    {
        try
        {
            BO.Worker? worker = Read(id);
            if(worker==null)
                throw new BO.BlDoesNotExistException($"Worker with ID={id} dosent exist");
            IEnumerable<DO.Task> taskForWorker = _dal.Task.ReadAll(x => x.WorkerId == id);
            if (taskForWorker != null)
            {
                //We check if the worker in the middle of the task or that he finish the task, if so we cant delete the worker
                bool taskInTheMiddle = taskForWorker.Where(x => (x.BeginTask < DateTime.Now && x.EndWorkTime == null)).Any();
                if (taskInTheMiddle)
                    throw new BlCantRemoveObject("Cant delete worker that in the middle of a task");
                bool taskFinish = taskForWorker.Where(x => (x.EndWorkTime < DateTime.Now)).Any();
                if (taskFinish)
                    throw new BlCantRemoveObject("Cant delete worker that finish the task");
                if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Execution)
                    throw new BlCantRemoveObject("Cant delete worker in the execution stage");
            }
            _dal.Worker.Delete(id);
        }
        catch (DO.DalDoesNotExistException e) 
        {
            throw new BO.BlDoesNotExistException(e.Message);
        }
    }

    public void UpdateWorker(BO.Worker worker)//לבדוק אם אפשר לעדכן משימה מוקצית
    {
        DO.Worker? oldworker = _dal.Worker.Read(worker.Id);
        if (worker.Name == "")//If the name is not correct
            throw new BO.BlInValidInputException("Invalid name of worker");
        if (worker.HourPrice <= 0)//If the hourPrice is not correct
            throw new BO.BlInValidInputException("Invalid price of worker");
        if ((DO.Rank)worker.RankWorker > oldworker!.RankWorker)//If the rank of worker is not correct
            throw new BO.BlInValidInputException("Invalid rank of worker");
        if (!new EmailAddressAttribute().IsValid(worker.Email))//If the email is not correct
            throw new BO.BlInValidInputException("Invalid email of worker");
        try
        {
            DO.Worker newWorker = new DO.Worker(worker.Id, (DO.Rank)((int)worker.RankWorker), worker.HourPrice, worker.Name, worker.Email);
            _dal.Worker.Update(newWorker);
            if(worker.CurrentTask != null)
            {
                DO.Task? task = _dal.Task.Read(worker.CurrentTask!.Id);
                DO.Task newTask = new DO.Task(task!.Id, task.Difficulty, worker.Id, task.TaskDescription, task.MileStone, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
                _dal.Task.Update(newTask);
            }
        }
        catch (DO.DalAlreadyExistsException d)
        {
            throw new BO.BlAlreadyExistsException(d.Message);
        }
    }

    public BO.Worker Read(int id)
    {
        DO.Worker? worker = _dal.Worker.Read(id);
        if (worker == null)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={id} dosent exist");
        }
        TaskOnWorker? taskOnWorker = GetCurrentTaskOfWorker(id);
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
    private BO.TaskOnWorker? GetCurrentTaskOfWorker(int id)//function that return the current task of the worker with is that she got
    {
        DO.Task? task = _dal.Task.ReadAll(x => x.WorkerId == id).Where(x => x!.BeginTask != null&&x.EndWorkTime is null).FirstOrDefault();
        if (task == null)
            return null;
        return new BO.TaskOnWorker()
        {
            Id = task!.Id,
            Name = task.Alias
        };
    }
}
