
using BlApi;
using BO;
using DO;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace BlImplementation;

internal class UserImplementation : IUser
{

    private readonly IBl _bl;
    internal UserImplementation(IBl bl) => _bl = bl;

    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public void clear()
    {
        throw new NotImplementedException();
    }

    public void Create(BO.User item)
    {
        if(item.password>=100000000||item.password<10000000)
        {
            throw new BlInValidInputException("Invalid password of user");
        }
        DO.User usern = _dal.User.Read(item.userName!)!;
        if (usern is not null)
        {
            throw new BO.BlAlreadyExistsException($"User with userName={item.userName} is already exist");
        }
        DO.User userId = _dal.User.Read(item.Id)!;
        if(userId is not null)
        {
            throw new BO.BlAlreadyExistsException($"User with Id={item.Id} is already exist");
        }
        DO.Worker worker = _dal.Worker.Read(item.Id)!;
        if(worker==null)
        {
            throw new BlInValidInputException("Cant create user to someone that not a worker");
        }
        try
        {
            // Create a new user object and add it to the data layer
            DO.User newUser = new DO.User(item.userName!, item.Id, item.password,item.isMennager);//Building a worker with the data of the worker that the function got
            _dal.User.Create(newUser);//Adding the user to the data
        }
        catch (DO.DalAlreadyExistsException d)
        {
            throw new BO.BlAlreadyExistsException(d.Message);
        }
    }
    public void Delete(int id)
    {
        try
        {
            BO.User? user = Read(id);
            if (user == null)
                throw new BO.BlDoesNotExistException($"user with ID={id} dosent exist");
            if (user.isMennager==true)
                throw new BO.BlCantRemoveObject("can't delete the mennager");
            _dal.User.Delete(id);
        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BO.BlDoesNotExistException(e.Message);
        }

    }
    public BO.User? Read(int id)
    {
        DO.User user=_dal.User.Read(id)!;
        if(user == null)
        {
            throw new BO.BlDoesNotExistException($"user with this id dosent exist");
        }
        return new BO.User()
        {
            userName = user.userName,
            Id = user.Id,
            password = user.Password
        };
    }
    public BO.User? Read(string name)
    {
        DO.User user = _dal.User.Read(name)!;
        //if (user == null)
        //{
        //    throw new BO.BlDoesNotExistException($"user with Name={name} dosent exist");
        //}
        if (user != null)
        {
            return new BO.User()
            {
                userName = user.userName,
                Id = user.Id,
                password = user.Password,
                isMennager = user.isMennager
            };
        }
        else
            return null;
    }
    //public BO.User? Read(Func<BO.User, bool> filter)
    //{
    //    throw new NotImplementedException();
    //}
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        IEnumerable<DO.User> users = _dal.User.ReadAll();
        var result=from DO.User user in users
                   let u=new BO.User()
                   {
                       userName = user.userName,
                       Id = user.Id,
                       password = user.Password
                   }
                   where filter is null ? true : filter(u)
                   select u;
        return result;
    }

    public void Update(BO.User item)
    {
        try
        {
            if (item.password >= 100000000 || item.password < 10000000)
            {
                throw new BlInValidInputException("Invalid password of user");
            }
            if (Read(item.Id)!.userName != item.userName)//if we want to update the user Name
            {
                if (Read(item.userName!) is not null)
                {
                    throw new BO.BlAlreadyExistsException($"User with userName={item.userName} is already exist");
                }
            }
            _dal.User.Update(new DO.User(item.userName!, item.Id, item.password,item.isMennager));
        }
        catch (DO.DalDoesNotExistException d)
        {
            throw new BO.BlDoesNotExistException(d.Message);
        }
    }

    public void checkInvalid(BO.User user)
    {
        DO.User dUser= _dal.User.Read(user.userName!)!;
        if(dUser == null )
        {
            throw new BO.BlDoesNotExistException($"user with Name={user.userName} dosent exist");
        }
        if(dUser.Password!=user.password) 
        {
            throw new BO.BlInValidInputException("the password is incorrect");
        }
    }

    //public void checkMennager(BO.User user)
    //{
    //    bool isMennager = s_bl.User.ReadAll(u => u.isMennager == true).Any();
    //    if(isMennager==true)
    //    {
    //        throw new BO.BlCantAddMenagger("There is already a mennager for the project");
    //    }
    //    user.isMennager = true;
    //    Create(user);
    //}

    //public bool checkWorkers(BO.User user)
    //{
    //    DO.Worker worker = _dal.Worker.Read(user.Id)!;
    //    if (worker == null)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
