

namespace BlApi;

public interface IBl
{
    public IWorker Worker { get;}
    public ITask Task { get;}
    public IWorkerOnTask WorkerOnTask { get;}
    public ITaskInList taskInList { get;}
}
