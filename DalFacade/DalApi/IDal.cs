namespace DalApi;

public interface IDal
{
    IWorker Worker { get; }
    ITask Task { get; }
    IDependency Dependency { get; }

    public DateTime? GetStartProjectDate();
    public void SetStartProjectDate(DateTime? startDate);
    public DateTime? GetEndProjectDate();
    public void SetEndProjectDate(DateTime? endDate);
}
