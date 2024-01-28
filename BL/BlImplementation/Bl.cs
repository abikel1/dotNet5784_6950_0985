using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IWorkerOnTask WorkerOnTask => new WorkerOnTaskImplementation();

    public ITaskInList taskInList => new TaskInListImplementation();
}
