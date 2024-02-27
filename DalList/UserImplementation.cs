
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

namespace Dal;

internal class UserImplementation : IUser
{
    public void clear()
    {
        DataSource.Users.Clear();
    }

    public int Create(User item)
    {
        DataSource.Users.Add(item);
        return item.Password;
    }
}
