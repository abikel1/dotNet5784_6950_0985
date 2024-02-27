
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

    public void Create(User item)
    {
        Worker worker =_dal.Worker.Read(item.Id)!;
        if(worker == null) 
        {
            throw new DalDoesNotExistException($"Worker with ID={item.Id} is not exists");
        }
        DataSource.Users.Add(item);
    }
}
