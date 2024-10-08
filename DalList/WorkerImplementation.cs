﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
internal class WorkerImplementation : IWorker
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Worker item)
    {
        if (Read(item.Id) is not null)
        {
            throw new DalAlreadyExistsException($"Worker with ID={item.Id} already exists");
        }
        DataSource.Workers.Add(item);
        //DataSource.Users.Add(new User()
        //{
        //    UserName = item.Name,
        //    Password = item.Id
        //}); 
        return item.Id;
    }
    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Worker with ID={id} is not exists");
        }
        DataSource.Workers.Remove(Read(id)!);
    }

    public Worker? Read(int id)
    {
        Worker? w=DataSource.Workers.FirstOrDefault(w => w.Id == id);
        return w;
    }

    public Worker? Read(Func<Worker, bool> filter)
    {
        return DataSource.Workers.FirstOrDefault(filter);
    }

    public IEnumerable<Worker> ReadAll(Func<Worker, bool>? filter=null)
    {
        if (filter == null)
            return DataSource.Workers.Select(item => item);
        else
            return DataSource.Workers.Where(filter);
    }

    public void Update(Worker item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Worker with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Workers.Add(item);
    }
    public void clear()
    {
        DataSource.Workers.Clear();
    }

    //public bool Check(User user)=> DataSource.Users.Contains(user);

    //public void AddUser(User user)=>DataSource.Users.Add(user);

    //public void RemoveUser(User user) => DataSource.Users.RemoveAll(x => x.Password == user.Password && x.UserName == user.UserName);
}
