namespace DalApi;

public interface IDal
{
    IWorker Worker { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
}
