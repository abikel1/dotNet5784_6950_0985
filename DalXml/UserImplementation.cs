
using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;

internal class UserImplementation:IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    readonly string s_userss_xml = "users";

    public void Create(User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_userss_xml);
        Worker worker = _dal.Worker.Read(item.Id)!;
        if (worker == null)
        {
            throw new DalDoesNotExistException($"Worker with ID={item.Id} is not exists");
        }
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_userss_xml);
    }
    public void clear()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_userss_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_userss_xml);
    }
}
