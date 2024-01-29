
namespace BlImplementation;
using BlApi;
using BO;
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
        try
        {
            DO.Worker newWorker = new DO.Worker(worker.Id,(DO.Rank)((int)worker.RankWorker),worker.HourPrice,worker.Name,worker.Email);//Building a worker with the data of the worker that the function got
            _dal.Worker.Create(newWorker);//Adding the worker to the data
        }
        catch(DO.DalAlreadyExistsException d) 
        {
            throw new BO.BlAlreadyExistsException($"Worker with ID={worker.Id} already exists");
        } 
    }

    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null)//להוסיף filter
    {
        IEnumerable<DO.Worker?> workers = _dal.Worker.ReadAll();
        IEnumerable<DO.Task?> tasks=_dal.Task.ReadAll();
        return (from worker in workers
                let task = tasks.FirstOrDefault(x => x!.WorkerId == worker.Id)
                select new BO.Worker()
                {
                    Id = worker.Id,
                    Name = worker.Name,
                    HourPrice = worker.HourPrice,
                    RankWorker = (BO.Rank)worker.RankWorker,
                    Email = worker.Email,
                    CurrentTask = new BO.WorkerOnTask
                    {
                        Id = worker.Id,
                        Name = worker.Name,
                    }
                });
    }

    public void RemoveWorker(int id)
    {
        try
        {
            BO.Worker worker = Read(id);
            bool x=_dal.Task.ReadAll(x=> x.WorkerId==id).Where(x=> x!.BeginTask is not null ).Any();
            if (x)
                throw new BO.BlCantRemoveObject("Cant remove worker that in the middle of a task");
            _dal.Worker.Delete(id);
        }
        catch (Exception e) 
        {
            throw new BO.BlDoesNotExistException(e.Message);
        }
    }

    public void UpdateWorker(BO.Worker worker)
    {
        DO.Worker? oldworker = _dal.Worker.Read(worker.Id);
        if (worker.Id <= 0)
            throw new BO.BlInValidInputException("Invalid ID of worker");
        if (worker.Name == "")
            throw new BO.BlInValidInputException("Invalid name of worker");
        if (worker.HourPrice <= 0)
            throw new BO.BlInValidInputException("Invalid price of worker");
        if ((DO.Rank)worker.RankWorker > oldworker!.RankWorker)
            throw new BO.BlInValidInputException("Invalid rank of worker");
        try
        {
            DO.Worker newWorker = new DO.Worker(worker.Id, (DO.Rank)((int)worker.RankWorker), worker.HourPrice, worker.Name, worker.Email);
            _dal.Worker.Update(newWorker);
            DO.Task? task=_dal.Task.Read(worker.CurrentTask!.Id);
            DO.Task newTask = new DO.Task(task!.Id, task.Difficulty, worker.Id, task.TaskDescription, task.MileStone, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
            _dal.Task.Update(newTask);
        }
        catch (DO.DalAlreadyExistsException d)
        {
            throw new BO.BlAlreadyExistsException($"Worker with ID={worker.Id} dosent exist");
        }
    }

    public BO.Worker Read(int id)
    {
        DO.Worker? worker = _dal.Worker.Read(id);
        if (worker == null)
        {
            throw new BO.BlDoesNotExistException($"Worker with ID={worker!.Id} dosent exist");
        }
        WorkerOnTask workerOnTask = GetCurrentTaskOfWorker(id);
        return new BO.Worker()
        {
            Id = worker.Id,
            Name = worker.Name,
            Email = worker.Email,
            RankWorker = (BO.Rank)((int)worker.RankWorker),
            HourPrice = worker.HourPrice,
            CurrentTask = workerOnTask
        };
    }
    private BO.WorkerOnTask GetCurrentTaskOfWorker(int id)
    {
        DO.Task? task = _dal.Task.ReadAll(x => x.WorkerId == id).Where(x => x!.BeginTask != null&&x.EndWorkTime is null).FirstOrDefault();

        return new BO.WorkerOnTask()
        {
            Id = task!.Id,
            Name = task.Alias
        };
    }
}
