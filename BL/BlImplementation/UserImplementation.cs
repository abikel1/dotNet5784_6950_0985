
using BlApi;
using BO;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void create(User user)
    {
        _dal.User.Create(new DO.User(user.Id,user.password,user.IsMennager));
    }
}
