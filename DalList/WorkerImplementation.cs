namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
public class WorkerImplementation : IWorker
{
    public int Create(Worker item)
    {
        if (Read(item.Id) is not null)
        {
            throw new Exception($"Worker with ID={item.Id} already exists");
        }

        DataSource.Workers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new Exception($"Worker with ID={id} is not exists");
        }
        DataSource.Workers.Remove(Read(id)!);
    }

    public Worker? Read(int id)
    {
        Worker? w=DataSource.Workers.FirstOrDefault(w => w.Id == id);
        if(w is not null)
        return w;
        throw new Exception($"Workers with ID={id} does not exist");
        //       return DataSource.Workers.Find(item => item.Id == id) ?? throw new Exception($"Workers with ID={id} does not exist");
    }

    public List<Worker?> ReadAll()
    {
        return new List<Worker?>(DataSource.Workers);
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) is null)
        {
            throw new Exception($"Worker with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Workers.Add(item);
    }
}
