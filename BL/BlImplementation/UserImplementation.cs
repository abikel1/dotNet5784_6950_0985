
using BlApi;
using BO;
using DO;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void clear()
    {
        throw new NotImplementedException();
    }

    public void Create(BO.User item)
    {
        try
        {
            // Create a new user object and add it to the data layer
            DO.User newUser = new DO.User(item.userName!, item.Id, item.password);//Building a worker with the data of the worker that the function got
            _dal.User.Create(newUser);//Adding the user to the data
        }
        catch (DO.DalAlreadyExistsException d)
        {
            throw new BO.BlAlreadyExistsException(d.Message);
        }
    }
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
    public BO.User? Read(int id)
    {
        throw new NotImplementedException();
    }
    public User? Read(string name)
    {
        throw new NotImplementedException();
    }
    public User? Read(Func<User, bool> filter)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(User item)
    {
        throw new NotImplementedException();
    }
}
