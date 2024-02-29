
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;
namespace Dal;

internal class UserImplementation:IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    readonly string s_users_xml = "users";

    public User? Read(string name)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        User? u = users.FirstOrDefault(u => u.userName == name);
        return u;
    }

    public int Create(User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
        return item.Id;
    }

    public User? Read(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        User? u = users.FirstOrDefault(u => u.Id == id);
        return u;
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        if (filter == null)
            return users.Select(item => item);
        else
            return users.Where(filter);
    }

    public void Update(User item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"User with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }

    public void Delete(int id)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"User with ID={id} is not exists");
        }
        users.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }

    public User? Read(Func<User, bool> filter)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        return users.FirstOrDefault(filter);
    }

    public void clear()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }
}
