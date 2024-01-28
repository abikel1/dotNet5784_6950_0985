
namespace BlImplementation;
using BlApi;

internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal=DalApi.Factory.Get;
    public void AddWorker(BO.Worker worker)
    {
        if()
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Worker> ReadWorkers(Func<BO.Worker, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void RemoveWorker(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateWorker(BO.Worker worker)
    {
        throw new NotImplementedException();
    }

    public BO.Worker WorkerDetails(int id)
    {
        throw new NotImplementedException();
    }
}
