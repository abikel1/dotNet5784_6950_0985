using DO;

namespace DalApi;

public interface IUser:ICrud<User>
{
    public User? Read(string name);
}
