
namespace BlImplementation;
using BlApi;
using BO;

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

    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void RemoveWorker(int id)
    {
        try
        {
            BO.Worker worker = Read(id);
            if (worker.CurrentTask != null)
                throw new BO.BlCantRemoveObject("Cant remove worker that in the middle of a task");
            _dal.Worker.Delete(id);
        }
        catch (BO.BlDoesNotExistException e) 
        {
            throw e;
        }
    }

    public void UpdateWorker(BO.Worker worker)
    {
        if (worker.Id <= 0)
            throw new BO.BlInValidInputException("Invalid ID of worker");
        if (worker.Name == "")
            throw new BO.BlInValidInputException("Invalid name of worker");
        if (worker.HourPrice <= 0)
            throw new BO.BlInValidInputException("Invalid price of worker");
        try
        {
            DO.Worker newWorker = new DO.Worker(worker.Id, (DO.Rank)((int)worker.RankWorker), worker.HourPrice, worker.Name, worker.Email);
            _dal.Worker.Update(newWorker);
            DO.Task? task=_dal.Task.Read(worker.CurrentTask!.Id);
            DO.Task newTask = new DO.Task(task.Id, task.Difficulty, worker.Id, task.TaskDescription, task.MileStone, task.Alias, task.CreateTask, task.BeginWork, task.BeginTask, task.TimeTask, task.DeadLine, task.EndWorkTime, task.Remarks, task.Product);
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
            throw new BO.BlDoesNotExistException($"Worker with ID={worker!.Id} is not exists");
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
    public WorkerOnTask GetCurrentTaskOfWorker(int id)
    {
    }
}
