﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = DataSource.Config.NextTaskId;
        Task task = item with { Id = id };
        DataSource.Tasks.Add(task);
        return id;
    }

    public void Delete(int id)
    {
      if(Read(id) is null) 
        {
            throw new DalDoesNotExistException($"Task with ID={id} is not exists");
        }
        DataSource.Tasks.Remove(Read(id)!);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter=null)
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
    public void clear()
    {
        DataSource.Tasks.Clear();
    }
}
