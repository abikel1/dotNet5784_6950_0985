
using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;

internal class UserImplementation:IUser
{
    readonly string s_userss_xml = "users";

    public int Create(User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_userss_xml);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_userss_xml);
        return item.Password;
    }
    public void clear()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_userss_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_userss_xml);
    }
}
