namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
public class TaskImplementation : ITask
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
            throw new Exception($"Task with ID={id} is not exists");
        }
        DataSource.Tasks.Remove(Read(id)!);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(item => item.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) is null)
        {
            throw new Exception($"Task with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Tasks.Add(item);
    }
}
