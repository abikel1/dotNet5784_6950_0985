
namespace Dal;
using DalApi;
using DO;

internal class WorkerImplementation:IWorker
{
    readonly string s_workers_xml = "workers";

    public int Create(Worker item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Worker? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Worker?> ReadAll(Func<Worker, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Worker item)
    {
        throw new NotImplementedException();
    }
}
