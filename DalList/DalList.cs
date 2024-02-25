namespace Dal;
using DalApi;
using System;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();
    public IUser User => new UserImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public DateTime? GetStartProjectDate() { return DataSource.Config.startProject; }

    public void SetStartProjectDate(DateTime? startDate) => DataSource.Config.startProject=startDate;

    public DateTime? GetEndProjectDate() { return DataSource.Config.endProject; }

    public void SetEndProjectDate(DateTime? endDate) => DataSource.Config.startProject = endDate;
}
