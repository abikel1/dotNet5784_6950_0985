namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class WorkerImplementation : IWorker
{
    public void AddWorker(Worker worker)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Worker> ReadWorkers(Func<Worker, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void RemoveWorker(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateWorker(Worker worker)
    {
        throw new NotImplementedException();
    }

    public Worker WorkerDetails(int id)
    {
        throw new NotImplementedException();
    }
}
