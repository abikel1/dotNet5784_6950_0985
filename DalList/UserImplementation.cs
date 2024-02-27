
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

namespace Dal;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void clear()
    {
       DataSource.Users.Clear();
    }

    public int Create(User item)
    {
        Worker worker = _dal.Worker.Read(item.Id)!;
        if (worker == null)
        {
            throw new DalDoesNotExistException($"Worker with ID={item.Id} is not exists");
        }
        if(Read(item.userName) is not null)
        {
            throw new DalAlreadyExistsException($"User with userName={item.userName} is already exist");
        }
        DataSource.Users.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"User with ID={id} is not exists");
        }
        DataSource.Users.Remove(Read(id)!);
    }

    public User? Read(int id)
    {
        User? w = DataSource.Users.FirstOrDefault(w => w.Id == id);
        return w;
    }
    public User? Read(string name)
    {
        User? w = DataSource.Users.FirstOrDefault(w => w.userName == name);
        return w;
    }

    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter);
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Users.Select(item => item);
        else
            return DataSource.Users.Where(filter);
    }

    public void Update(User item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"User with ID={item.Id} is not exists");
        }
        if(Read(item.Id)!.userName!=item.userName)//if we want to update the user Name
        {
            if (Read(item.userName) is not null)
            {
                throw new DalAlreadyExistsException($"User with userName={item.userName} is already exist");
            }
        }
        Delete(item.Id);
        DataSource.Users.Add(item);
    }
}
