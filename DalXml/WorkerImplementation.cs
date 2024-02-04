﻿
namespace Dal;
using DalApi;
using DO;

internal class WorkerImplementation:IWorker
{
    readonly string s_workers_xml = "workers";

    public int Create(Worker item)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistsException($"Worker with ID={item.Id} already exists");
        }
        workers.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Worker with ID={id} is not exists");
        }
        workers.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
    }

    public Worker? Read(int id)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        Worker? w = workers.FirstOrDefault(w => w.Id == id);
        return w;
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        return workers.FirstOrDefault(filter);
    }

    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter = null)
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        if (filter == null)
            return workers.Select(item => item);
        else
            return workers.Where(filter);
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Worker with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        workers.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
    }
    public void clear()
    {
        List<DO.Worker> workers = XMLTools.LoadListFromXMLSerializer<DO.Worker>(s_workers_xml);
        workers.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Worker>(workers, s_workers_xml);
    }
}
